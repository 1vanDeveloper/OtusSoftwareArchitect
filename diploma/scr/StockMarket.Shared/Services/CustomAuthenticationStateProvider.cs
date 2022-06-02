using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using StockMarket.Shared.Models;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.Services;

/// <inheritdoc cref="AuthenticationStateProvider"/>
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILoginViewModel _loginViewModel;
    private readonly IAccessTokenService _accessTokenService;

    public CustomAuthenticationStateProvider(ILoginViewModel loginViewModel,
        IAccessTokenService accessTokenService)
    {
        _loginViewModel = loginViewModel;
        _accessTokenService = accessTokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetUserByJwtAsync();

        if (currentUser?.Email != null)
        {
            //create claimsPrincipal
            var claimsPrincipal = GetClaimsPrinciple(currentUser);
            return new AuthenticationState(claimsPrincipal);
        }
        else
        {
            await _accessTokenService.RemoveAccessTokenAsync("jwt_token");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated()
    {
        var user = await GetUserByJwtAsync();
        var claimsPrincipal = GetClaimsPrinciple(user);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _accessTokenService.RemoveAccessTokenAsync("jwt_token");

        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task<User> GetUserByJwtAsync()
    {
        //pulling the token from localStorage
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        if (jwtToken == null || jwtToken.Length < 5)
        {
            return null;
        }

        //jwtToken = $@"""{jwtToken}""";
        return _loginViewModel.GetUser(jwtToken);
    }

    private static ClaimsPrincipal GetClaimsPrinciple(User currentUser)
    {
        //create a claims
        var claimEmailAddress = new Claim(ClaimTypes.Name, currentUser.Email);
        var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, currentUser.Email);
        
        //create claimsIdentity
        var claimsIdentity =
            new ClaimsIdentity(new[] {claimEmailAddress, claimNameIdentifier}, "serverAuth");
        //create claimsPrincipal
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}