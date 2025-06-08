using Invoice.DAL.Data;
using Invoice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoice.BLL.Services
{
    public class CustomersServices(ApplicationDbContext context):ICustomersServices
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
