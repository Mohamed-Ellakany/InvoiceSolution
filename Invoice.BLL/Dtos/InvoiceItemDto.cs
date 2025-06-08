using Invoice.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Dtos
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public float DiscountPercentage { get; set; }

        public float TaxPercentage { get; set; }

        public decimal Total { get; set; }
    }
}
