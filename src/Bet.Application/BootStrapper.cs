using Bet.Application.Services.Email;
using Bet.Application.Services.Hash;
using Bet.Application.Services.LoggedUser;
using Bet.Application.Services.Token;
using Bet.Application.UseCases.Bet.DefineWinner;
using Bet.Application.UseCases.Bet.GetAllFromUser;
using Bet.Application.UseCases.Bet.Pay;
using Bet.Application.UseCases.Bet.Register;
using Bet.Application.UseCases.User.JoinBet;
using Bet.Application.UseCases.User.Login;
using Bet.Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using EAC.Application.Email;
using Bet.Application.UseCases.Bet.PayById;
using Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
using Bet.Application.UseCases.User.ChangePassword;

namespace Bet.Application;
public static class BootStrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddJwtToken(services, configuration);
        AddEmailService(services, configuration);
        AddOptionalPassworKey(services, configuration);
        AddUseCases(services);
        AddLoggedUser(services);
    }
    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddEmailService(IServiceCollection services, IConfiguration configuration) 
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddSingleton<SmtpClient>(provider =>
        {
            var emailSettings = provider.GetRequiredService<IOptions<EmailSettings>>().Value;

            return new SmtpClient(emailSettings.SmtpServer)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
                EnableSsl = true,
            };
        });
        services.AddScoped<IEmailService, EmailService>();
    }

    private static void AddOptionalPassworKey(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configurations:Key");

        services.AddScoped(option => new PasswordHasher(section.Value));
    }

    private static void AddJwtToken(IServiceCollection services, IConfiguration configuration)
    {
        var sectionKey = configuration.GetRequiredSection("Configurations:TokenKey");
        var sectionTimeExpired = configuration.GetRequiredSection("Configurations:TokenTimeExpired");

        services.AddScoped(options => new TokenController(int.Parse(sectionTimeExpired.Value), sectionKey.Value));
    }
    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterBetUseCase, RegisterBetUseCase>();
        services.AddScoped<IJoinBetUseCase, JoinBetUseCase>();
        services.AddScoped<IDefineWinner, DefineWinner>();
        services.AddScoped<IPayBetsUseCase, PayBetsUseCase>();
        services.AddScoped<IGetAllFromUser, GetAllFromUser>();
        services.AddScoped<IPayBetsByIdUseCase, PayBetsByIdUseCase>();
        services.AddScoped<IGetAllOpenWithOdd, GetAllOpenWithOdd>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
}
