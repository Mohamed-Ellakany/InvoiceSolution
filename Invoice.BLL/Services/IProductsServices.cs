using Invoice.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Services
{
    public interface IProductsServices
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
