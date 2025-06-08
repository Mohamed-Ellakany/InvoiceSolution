using Invoice.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsServices productService) : ControllerBase
    {
        private readonly IProductsServices _productService = productService;


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
                return NotFound("No products found.");
            return Ok(products); 
        }

    }
}
