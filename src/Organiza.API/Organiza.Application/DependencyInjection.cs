// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Organiza.Application.Features.Users.Users.Commands.InsertUsers;
using Organiza.Application.Services.UserServices;
using Organiza.Infrastructure.BaseClients;
using Organiza.Infrastructure.Behaviours;
using Organiza.Infrastructure.Persistence;
using System.Net;
using System.Reflection;

namespace Organiza.Application
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEF<Context>(configuration);
            services.AddHttpContextAccessor().TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            services.AddHttpClient();

            services.AddValidatorsFromAssemblyContaining<InsertUserCommandValidator>(lifetime: ServiceLifetime.Transient);

            ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("pt-BR");

            services.RegisterHttpClients(configuration);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseClientService, BaseClientService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterEF<T>(this IServiceCollection services, IConfiguration configuration, string schema = "dbo") where T : DbContext
        {
            IConfiguration configuration2 = configuration;
            string schema2 = schema;
            return services.AddDbContextPool<T>(delegate (IServiceProvider servicesProvider, DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(configuration2.GetConnectionString(typeof(T).Name), delegate (SqlServerDbContextOptionsBuilder option)
                {
                    option.MigrationsHistoryTable("__EFMigrationsHistory", schema2);
                    option.EnableRetryOnFailure();
                    option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            }, 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicesProvider"></param>
        /// <param name="minutesTimeout"></param>
        /// <returns></returns>
        public static async Task MigrateDatabaseAsync<T>(this IServiceProvider servicesProvider, int minutesTimeout = 5) where T : DbContext
        {
            using IServiceScope scope = servicesProvider.CreateScope();
            T requiredService = scope.ServiceProvider.GetRequiredService<T>();
            requiredService.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(minutesTimeout).TotalMilliseconds);
            await requiredService.Database.MigrateAsync();
        }

        /// <summary>
        /// Registrando HTTP Clients
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var externalApplications = configuration.GetSection("ExternalApplications").AsEnumerable();
            foreach (var application in externalApplications)
                services.AddHttpClient(application.Key, c =>
                {
                    c.BaseAddress = new Uri(application.Value);
                    c.Timeout = Timeout.InfiniteTimeSpan;

                }).ConfigurePrimaryHttpMessageHandler(c => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
        }
    }
}
