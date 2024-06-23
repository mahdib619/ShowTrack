using Microsoft.AspNetCore.Components.Authorization;
using ShowTrack.Client.Services;
using ShowTrack.Contracts.Dtos;
using System.Security.Claims;

namespace ShowTrack.Client;

public class AppAuthenticationStateProvider(IAccountService accountService) : AuthenticationStateProvider
{
    private UserInfo? _currentUserInfo;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        try
        {
            var userInfo = await GetCurrentUser();
            if (userInfo.IsAuthenticated)
            {
                var claims = _currentUserInfo!.Claims!.Select(c => new Claim(c.Key, c.Value));
                identity = new(claims, "Server authentication");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request failed:" + ex);
        }

        return new(new(identity));
    }

    private async Task<UserInfo> GetCurrentUser()
    {
        if (_currentUserInfo is not { IsAuthenticated: true })
        {
            _currentUserInfo = await accountService.GetCurrentUser();
        }

        return _currentUserInfo;
    }

    public async Task<bool> Login(LoginDto loginParameters)
    {
        if (await accountService.Login(loginParameters) == false)
        {
            return false;
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        return true;
    }

    public async Task Logout()
    {
        await accountService.Logout();
        _currentUserInfo = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}