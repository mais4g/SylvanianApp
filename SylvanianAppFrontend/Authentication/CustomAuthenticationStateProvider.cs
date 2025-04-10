using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SylvanianAppFrontend.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isLoggedIn = await _localStorage.GetItemAsync<bool>("isLoggedIn");
            var isAdmin = await _localStorage.GetItemAsync<bool>("isAdmin");

            ClaimsIdentity identity;
            if (isLoggedIn)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "UsuárioDemo")
        };

                if (isAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                identity = new ClaimsIdentity(claims, "FakeAuthType");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }


        public async Task LoginAsync()
        {
            var isAdmin = await _localStorage.GetItemAsync<bool>("isAdmin");
            await _localStorage.SetItemAsync("isLoggedIn", true);
            await _localStorage.SetItemAsync("isAdmin", isAdmin);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("isLoggedIn");
            await _localStorage.RemoveItemAsync("isAdmin");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
