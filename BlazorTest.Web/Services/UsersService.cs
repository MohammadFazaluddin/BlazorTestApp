using BlazorTest.Web.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace BlazorTest.Web.Services
{
    public class UsersService
    {
        private HttpClient _httpClient;
        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserModel> LoginUser(LoginModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync("users/login", content);

            if(result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(response)!;
            }

            return null!;
        }
    }
}
