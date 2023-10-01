using BlazorTest.Web.Models;
using BlazorTest.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorTest.Web.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        public UsersService UserService { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; }

        public LoginModel EditModel { get; set; } = new LoginModel();

        string loginMessage = "";

        public async void LoginToSystem()
        {
            var loggedIn = await UserService.LoginUser(EditModel);

            if (loggedIn != null)
            {
                var auth = (AppAuthenticationStateProvider)AuthStateProvider;

                await auth.SignIn(loggedIn);
                NavManager.NavigateTo("/index");
            }
            else
            {
                loginMessage = "Login failed";
            }
        }

    }
}
