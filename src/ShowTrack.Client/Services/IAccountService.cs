using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Services;

public interface IAccountService
{
    Task<UserInfo> GetCurrentUser();
    Task<bool> Login(LoginDto login);
    Task Logout();
}