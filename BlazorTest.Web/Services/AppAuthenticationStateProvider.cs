using BlazorTest.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlazorTest.Web.Services
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private HttpContextAccessor _httpContextAccessor;
        public AppAuthenticationStateProvider(HttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        }

        public async Task SignIn(UserModel model)
        {
            var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role),
                new Claim(ClaimTypes.Name, model.Username)
            }, "AppAuth"));

            await _httpContextAccessor.HttpContext.SignInAsync("AppAuth", claimsPrinciple);
        }

    }
}
