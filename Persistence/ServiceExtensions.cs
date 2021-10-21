using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services
                .AddSqlClient(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services

                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IAccountRepository, AccountRepository>()
                .AddSingleton<ITransactionRepository, TransactionRepository>();
            
                
        }

        private static IServiceCollection AddSqlClient(this IServiceCollection services, IConfiguration configuration)
        {
            /*var fluentConnectionStringBuilder = new FluentConnectionStringBuilder();*/

            /*var connectionString = configuration.GetSection("ConnectionStrings")["SqlConnectionString"];*/ //pirmas budas
            /*var connectionString = configuration.GetSection("ConnectionStrings").
                                                    GetSection("SqlConnectionString").Value;*/ // antras budas
            var connectionString = configuration.GetSection("SqlConnectionSettings:SqlConnectionString").Value; // trecias budas

            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }

    }
}
