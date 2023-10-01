using BlazorTest.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Data.Repositories
{
    public class RolesRepository : IRoles
    {
        private RoleManager<IdentityRole> _roleManager;

        public RolesRepository(RoleManager<IdentityRole> roles)
        {
            _roleManager = roles;
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            return result;
        }

        public async Task<IdentityResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            
            if (role == null)
            {
                return null!;
            }

            var result = await _roleManager.DeleteAsync(role);

            return result;
        }
    }
}
