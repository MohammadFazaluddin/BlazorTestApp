using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorTest.Web.Services
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        public AppAuthenticationStateProvider()
        {
            
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
