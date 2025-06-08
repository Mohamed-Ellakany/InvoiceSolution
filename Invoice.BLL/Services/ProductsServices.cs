using Invoice.DAL.Data;
using Invoice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Services
{
    public class ProductsServices(ApplicationDbContext context) : IProductsServices
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

    }


}
