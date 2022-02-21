using DemoSecurity.AuthProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
namespace DemoSecurity.Pages.Auth
{
    public partial class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }

        HttpClient _client { get; set; }

        [Inject]
        public IJSRuntime js{ get; set; }

        [Inject]
        public IServiceProvider _serviceProvider { get; set; }

        

        public void SubmitLogin()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44338/api/");

            string jsonContent = JsonSerializer.Serialize(new { Email = Email, Password = Password });
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using(HttpResponseMessage response = _client.PostAsync("auth", content).Result)
            {
                if(response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;

                    js.InvokeVoidAsync("localStorage.setItem", "currentToken", jsonResponse);

                    // MyProperty.NotifyUserConnected();
                    ((DemoAuthStateProvider)_serviceProvider.GetService<AuthenticationStateProvider>()).token = jsonResponse;
                     ((DemoAuthStateProvider)_serviceProvider.GetService<AuthenticationStateProvider>()).NotifyUserConnected();
                }
            }
        }
    }
}
