using bitify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitify.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
