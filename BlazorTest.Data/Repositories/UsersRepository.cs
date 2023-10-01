using BlazorTest.Data.Interfaces;
using BlazorTest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Data.Repositories
{
    public class UsersRepository : IUsers
    {
        private DatabaseContext Database;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UsersRepository(DatabaseContext database, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> role)
        {

            Database = database;
            _userManager = userManager;
            _roleManager = role;

        }

        public async Task<IdentityResult> AddUser(UserModel user)
        {
            var result = await _userManager.CreateAsync(new()
            {
                Email = user.Email,
                UserName = user.Username,
            }, user.Password);

            if (!result.Succeeded)
            {
                return result!;
            }

            if (await _roleManager.RoleExistsAsync(user.Role))
            {
                var identityUser = await _userManager.FindByEmailAsync(user.Email);
                var addRole = await _userManager.AddToRoleAsync(identityUser, user.Role);

                if (!addRole.Succeeded)
                {
                    return addRole;
                }
            }

            return result;
        }
        


        public async Task<IEnumerable<IdentityUser>> GetAllUsers(int pageNumber, int pageSize)
        {
            var skipCount = (pageNumber - 1) * pageSize;
            var result = await Database.Users.Skip(skipCount).Take(pageSize).ToListAsync();
            return result;
        }   

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            return result;
        }

        public async Task<List<string>> GetUserRolesByEmail(string email, IdentityUser? user = null)
        {
            user ??= await _userManager.FindByEmailAsync(email);

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        public async Task<bool> ValidateUser(LoginModel loginDetails)
        {
            var user = await _userManager.FindByEmailAsync(loginDetails.Email);

            var validUser = await _userManager.CheckPasswordAsync(user, loginDetails.Passoword);

            return validUser;
        }
    }
}
