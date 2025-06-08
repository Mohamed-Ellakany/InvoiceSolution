using Invoice.BLL.Dtos;
using Invoice.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;


        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterDto dto)
        {
            var result = await _authServices.Register(dto);

            if (result.Item1 is false)
                return BadRequest(result.Item2);

            return Ok(result.Item2);

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authServices.Login(dto);
            
            if (result.Item1 is false)
                return BadRequest("Invalid Credintials");

            return Ok(result.Item2);
        }
    }
}
