using Bet.Domain.Extesions;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.User;
using Bet.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bet.Infra;
public static class Bootstrapper
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddRepositories(services);
        AddUnitOfWork(services);
        AddContext(services, configurationManager);
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {

        var connectionString = configurationManager.GetConnection2();

        services.AddDbContext<BetContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IBetReadOnlyRepository, BetRepository>();
        services.AddScoped<IBetUpdateOnlyRepository, BetRepository>();
        services.AddScoped<IBetWriteOnlyRepository, BetRepository>();
    }
}
