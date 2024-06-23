using ShowTrack.Client.Models;
using ShowTrack.Contracts.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace ShowTrack.Client.Services;

public sealed class AccountService : IAccountService
{
    public const string HTTP_CLIENT_NAME = "Login";

    private readonly HttpClient _loginHttpClient;
    private readonly HttpClient _apiHttpClient;

    public AccountService(IServiceProvider serviceProvider, HttpClient apiHttpClient)
    {
        _apiHttpClient = apiHttpClient;
        _loginHttpClient = serviceProvider.GetRequiredKeyedService<HttpClient>(HTTP_CLIENT_NAME);
    }

    public async Task<UserInfo> GetCurrentUser()
    {
        return await _apiHttpClient.GetFromJsonAsync<UserInfo>("Account/UserInfo") ?? new();
    }

    public async Task<bool> Login(LoginDto login)
    {
        using var result = await _loginHttpClient.PostAsJsonAsync("login?useCookies=true&useSessionCookies=true", login);

        return result switch
        {
            { IsSuccessStatusCode: true } => true,
            { StatusCode: HttpStatusCode.Unauthorized } => false,
            _ => throw ClientException.UnknownError
        };
    }

    public async Task Logout()
    {
        await _loginHttpClient.PostAsync("/logout", null);
    }
}
