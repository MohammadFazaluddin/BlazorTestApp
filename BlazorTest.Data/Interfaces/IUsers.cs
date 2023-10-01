using BlazorTest.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTest.Data.Interfaces
{
    public interface IUsers
    {
        public abstract Task<IdentityResult> AddUser(UserModel user);

        public abstract Task<IEnumerable<IdentityUser>> GetAllUsers(int pageNumber, int pageSize);

        public abstract Task<IdentityUser> GetUserByEmail(string email);

        public abstract Task<bool> ValidateUser(LoginModel loginDetails);

        public abstract Task<List<string>> GetUserRolesByEmail(string email, IdentityUser? user = null);
    }
}
