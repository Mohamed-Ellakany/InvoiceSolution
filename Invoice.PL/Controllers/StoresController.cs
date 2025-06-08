using Invoice.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController(IStoresServices storesServices) : ControllerBase
    {
        private readonly IStoresServices _storesServices = storesServices;


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStores()
        {
            var stores = await _storesServices.GetAllStoresAsync();
            if (stores is null || !stores.Any())
                return NotFound("No stores found");

            return Ok(stores);


        }
    }
}
