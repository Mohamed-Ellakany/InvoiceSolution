using Invoice.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomersServices customersServices) : ControllerBase
    {
        private readonly ICustomersServices _customersServices = customersServices;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customersServices.GetAllCustomersAsync();

            if (customers is null || !customers.Any())
            {
                return NotFound("No customers found.");
            }
        
            return Ok(customers);
     
        }
    }
}
