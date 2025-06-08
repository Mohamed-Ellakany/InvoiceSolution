using Invoice.BLL.Common.Authentication;
using Invoice.BLL.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Invoice.BLL.Services
{
    public class AuthServices(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager , IJwtProvider jwtProvider) : IAuthServices
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<(bool, string)> Register(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
                return (false, "Email already in use.");

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return (true, "User created");

            foreach (var error in result.Errors)
            {
                return (false, error.Description);
            }

            return (false, "An error occurred while creating the user.");
        }

   

        public async Task<(bool, LoginResponse)> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return (false, new LoginResponse(dto.Email, string.Empty, 0));

            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            if (!result.Succeeded)
            {
                return (false, new LoginResponse(string.Empty, string.Empty, 0));
            }

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
                return (true, new LoginResponse(dto.Email, token, expiresIn));

        }
    }
}
    
            