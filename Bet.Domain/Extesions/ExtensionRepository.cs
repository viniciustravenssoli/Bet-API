using Microsoft.Extensions.Configuration;

namespace Bet.Domain.Extesions;
public static class ExtensionRepository
{
    public static string GetDataBaseName(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetConnectionString("DataBaseName");

        return databaseName;
    }

    public static string GetConnection(this IConfiguration configurationManager)
    {
        var connection = configurationManager.GetConnectionString("Connection");

        return connection;
    }
    public static string GetConnection2(this IConfiguration configurationManager)
    {
        var connection = configurationManager.GetConnectionString("Connection2");

        return connection;
    }

    public static string GetFullConnection(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetDataBaseName();
        var connection = configurationManager.GetConnection();

        return $"{connection}Database={databaseName}";
    }
}
