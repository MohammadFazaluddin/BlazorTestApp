using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Data.Interfaces
{
    public interface IRoles
    {
        public abstract Task<IdentityResult> CreateRole(string roleName);

        public abstract Task<IdentityResult> DeleteRole(string roleName);

    }
}
