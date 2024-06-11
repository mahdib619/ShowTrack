namespace ShowTrack.Web.Services;

public interface IEmailService
{
    Task<bool> Send(string receiver, string subject, string body);
}