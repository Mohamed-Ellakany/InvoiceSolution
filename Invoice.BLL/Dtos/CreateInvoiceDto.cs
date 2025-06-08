using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Dtos
{
    public class CreateInvoiceDto
    {
        public string Serial { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public List<CreateInvoiceItemDto> Items { get; set; }
    }
}
