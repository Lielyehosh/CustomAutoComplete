using System;
using System.Collections.Generic;
using AutoComplete.Common.Interfaces;
using AutoComplete.Common.Models;
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
            services.AddSingleton<IMySqlDal>(provider => new MySqlDal(connectionString));
            return services;
        }
        
        /// <summary>
        /// Register Query service - set all the tables scheme here
        /// </summary>
        public static IQueryService AddQueryService(this IServiceCollection services, List<Type> types)
        {
            var queryService = new QueryService();
            foreach (var type in types)
            {
                queryService.RegisterNewTableScheme(type);
            }
            services.AddSingleton<IQueryService>(provider => queryService);
            return queryService;
        }
    }
}