using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.BLL.Dtos
{
    public record LoginResponse
    (
        string Email , string Token, int ExpiresIn
        );

    
}
