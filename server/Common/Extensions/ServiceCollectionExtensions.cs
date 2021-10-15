using AutoComplete.Common.Interfaces;
using AutoComplete.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutoComplete.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register MySQL interface
        /// </summary>
        public static IServiceCollection AddMySqlDal(this IServiceCollection services, string connectionString)
        {
            // const string connectionString = "Server=localhost:3307;Database=test_db;Uid=root;Pwd=root;";
            services.AddSingleton<IMySqlDal>(provider => new MySqlDal(connectionString));
            return services;
        }
    }
}