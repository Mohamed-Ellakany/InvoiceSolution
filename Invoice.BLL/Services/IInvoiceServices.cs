using Invoice.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Services
{
    public interface IInvoiceServices
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();

        Task<InvoiceDto> GetInvoiceByIdAsync(int id);

        Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto);

        Task<InvoiceDto> UpdateInvoiceAsync(int id, CreateInvoiceDto dto);

        Task<bool> DeleteInvoiceAsync(int id);
    }
}
