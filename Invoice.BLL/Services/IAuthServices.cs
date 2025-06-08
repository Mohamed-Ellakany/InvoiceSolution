using Invoice.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Services
{
    public interface IAuthServices
    {
        Task<(bool, string)> Register(RegisterDto dto);

        Task<(bool, LoginResponse)> Login(LoginDto dto);

    }
}
