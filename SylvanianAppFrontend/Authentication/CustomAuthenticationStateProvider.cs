using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
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

            ClaimsIdentity identity;
            if (isLoggedIn)
            {
                // Aqui você pode adicionar claims reais do usuário (ex: nome, email, roles, etc)
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "UsuárioDemo")
                }, "FakeAuthType");
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
            await _localStorage.SetItemAsync("isLoggedIn", true);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("isLoggedIn");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
