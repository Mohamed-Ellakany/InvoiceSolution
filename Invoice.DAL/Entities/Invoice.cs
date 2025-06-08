using Microsoft.EntityFrameworkCore;

namespace Invoice.DAL.Entities
{
    [Index(nameof(Serial), IsUnique =true)]
    public class Invoice
    {
        public int Id { get; set; }

        public string Serial { get; set; }

        public DateTime Date { get; set; }

        public string Note { get; set; }

        public decimal Total { get; set; }
        
        public int StoreId { get; set; }
        public Store Store { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
       
        public List<InvoiceItem> Items { get; set; }
    }
}
