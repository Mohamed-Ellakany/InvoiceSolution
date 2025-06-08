using Invoice.BLL.Dtos;
using Invoice.BLL.Hubs;
using Invoice.DAL.Data;
using Invoice.DAL.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using InvoiceEntity = Invoice.DAL.Entities.Invoice;

namespace Invoice.BLL.Services
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<InvoiceHub> _hub;

        public InvoiceServices(ApplicationDbContext context, IHubContext<InvoiceHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .OrderByDescending(i => i.Date)
                .ToListAsync();

            return invoices.Select(MapToDto);
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            var i = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            return i == null ? null : MapToDto(i);
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            if (await _context.Invoices.AnyAsync(i => i.Serial == dto.Serial))
                return null;

            if (!await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId) ||
                !await _context.Stores.AnyAsync(s => s.Id == dto.StoreId))
                return null;

            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            if (products.Count != productIds.Count)
                return null;

            var invoiceItems = dto.Items.Select(item =>
            {
                var unitPrice = products[item.ProductId].UnitPrice;
                return new InvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = unitPrice,
                    DiscountPercentage = item.DiscountPercentage,
                    TaxPercentage = item.TaxPercentage,
                    Total = CalculateItemTotal(unitPrice, item.DiscountPercentage, item.TaxPercentage, item.Quantity)
                };
            }).ToList();

            var total = invoiceItems.Sum(item => item.Total);

            var invoice = new InvoiceEntity
            {
                Serial = dto.Serial,
                Date = dto.Date,
                Note = dto.Note,
                StoreId = dto.StoreId,
                CustomerId = dto.CustomerId,
                Items = invoiceItems,
                Total = total
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("InvoiceUpdated");

            var createdInvoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(i => i.Id == invoice.Id);

            return MapToDto(createdInvoice);
        }

        public async Task<InvoiceDto> UpdateInvoiceAsync(int id, CreateInvoiceDto dto)
        {
            var invoice = await _context.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null || await _context.Invoices.AnyAsync(i => i.Serial == dto.Serial && i.Id != id))
                return null;

            if (!await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId) ||
                !await _context.Stores.AnyAsync(s => s.Id == dto.StoreId))
                return null;

            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            if (products.Count != productIds.Count)
                return null;

            invoice.Serial = dto.Serial;
            invoice.Date = dto.Date;
            invoice.Note = dto.Note;
            invoice.StoreId = dto.StoreId;
            invoice.CustomerId = dto.CustomerId;

            invoice.Items.Clear();
            foreach (var item in dto.Items)
            {
                var unitPrice = products[item.ProductId].UnitPrice;
                invoice.Items.Add(new InvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = unitPrice,
                    DiscountPercentage = item.DiscountPercentage,
                    TaxPercentage = item.TaxPercentage,
                    Total = CalculateItemTotal(unitPrice, item.DiscountPercentage, item.TaxPercentage, item.Quantity)
                });
            }

            invoice.Total = invoice.Items.Sum(i => i.Total);

            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("InvoiceUpdated");

            var updatedInvoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Store)
                .Include(i => i.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            return MapToDto(updatedInvoice);
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return false;
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("InvoiceUpdated");
            return true;
        }

        private InvoiceDto MapToDto(InvoiceEntity i)
        {
            return new InvoiceDto
            {
                Id = i.Id,
                Serial = i.Serial,
                Date = i.Date,
                Note = i.Note,
                StoreId = i.StoreId,
                StoreName = i.Store?.Name,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer?.Name,
                Total = i.Total,
                Items = i.Items.Select(item => new InvoiceItemDto
                {
                    Id = item.Id,
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    DiscountPercentage = item.DiscountPercentage,
                    TaxPercentage = item.TaxPercentage,
                    Total = item.Total
                }).ToList()
            };
        }

        private decimal CalculateItemTotal(decimal price, float discountPercentage, float taxPercentage, int quantity)
        {
            var allPrice = price * quantity;
            var discount = allPrice * (decimal)discountPercentage / 100;
            var sub = allPrice - discount;
            var tax = sub * (decimal)taxPercentage / 100;
            var total = sub + tax;
            return total;
        }
    }
}
