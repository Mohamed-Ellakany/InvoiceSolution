using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Common.Authentication
{
    public interface IJwtProvider
    {
        (string token , int expiresIn) GenerateToken(IdentityUser user);

    }
}
