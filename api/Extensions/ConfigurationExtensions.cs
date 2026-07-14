using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace APIKros.Extensions;

public static class ConfigurationExtensions
{
    public static string GetSqlServerConnectionString(this IConfiguration configuration)
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = $"{configuration["DB_HOST"]},{configuration["DB_PORT"]}",
            InitialCatalog = configuration["DB_NAME"],
            UserID = configuration["DB_USER"],
            Password = configuration["DB_PASSWORD"],
            TrustServerCertificate = bool.Parse(configuration["DB_TRUST_CERTIFICATE"] ?? "true")
        }.ConnectionString;
    }
}