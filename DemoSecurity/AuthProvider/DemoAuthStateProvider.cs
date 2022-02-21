using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoSecurity.AuthProvider
{
    public class DemoAuthStateProvider : AuthenticationStateProvider
    {
        [Inject]
        public IJSRuntime js { get; set; }
        public string token { get; set; }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(2000);

            List<Claim> claims = new List<Claim>();


            //claims.Add(new Claim(ClaimTypes.Name, "Arthur"));
            //claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            //string token = await js.InvokeAsync<string>("localStorage.getItem", "currentToken");

            ClaimsIdentity currentUser;
            if (string.IsNullOrWhiteSpace(token))
            {
                currentUser = new ClaimsIdentity();
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(currentUser)));
            }
            
            JwtSecurityToken jwt = new JwtSecurityToken(token);

            foreach (Claim item in jwt.Claims)
            {
                claims.Add(item);
            }

            currentUser = new ClaimsIdentity(claims, "demoAuth");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(currentUser)));
        }

        public void NotifyUserConnected()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
