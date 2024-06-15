using Microsoft.Extensions.Options;
using ShowTrack.Web.Models;
using System.Net;
using System.Net.Mail;

namespace ShowTrack.Web.Services;

public sealed class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;
    private readonly string _sender;

    public EmailService(IOptions<EmailClient> emailClientOptions, ILogger<EmailService> logger)
    {
        _logger = logger;

        var clientConfig = emailClientOptions.Value;

        _sender = clientConfig.UserName;
        _smtpClient = new()
        {
            Host = clientConfig.Host,
            Credentials = new NetworkCredential(clientConfig.UserName, clientConfig.Password),
            UseDefaultCredentials = false,
            EnableSsl = true
        };
    }

    public async Task<bool> Send(string receiver, string subject, string body)
    {
        try
        {
            await _smtpClient.SendMailAsync(_sender, receiver, subject, body);
            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while sending email, Receiver:{Receiver}, Subject:{Subject}", receiver, subject);
            return false;
        }
    }
}
