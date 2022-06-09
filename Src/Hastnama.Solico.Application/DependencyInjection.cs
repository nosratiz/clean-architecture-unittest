using Hangfire;
using Hangfire.SqlServer;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Interfaces.Statistic;
using Hastnama.Solico.Application.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Hastnama.Solico.Common.Environment;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Domain.Models.Statistic;
using Polly;

namespace Hastnama.Solico.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IDailyStatistic, DailyStatisticService>();
            services.AddTransient<ISolicoWebService, SolicoWebService>();
            services.AddTransient<ISolicoEventService, SolicoEventService>();
            services.AddTransient<IProductCategoryServices, ProductCategoryServices>();
            services.AddTransient<ICustomerProductViewService, CustomerProductViewService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IBankTransactionService, BankTransactionService>();


            services.AddSingleton<IApplicationBootstrapper, ApplicationBootstrapper>();

            services.AddScoped<ISolicoJobService, SolicoJobService>();

            services.Configure<SolicoWebServiceSetting>(configuration.GetSection("SolicoWebServiceSetting"));

            var solicoWebServiceSetting = new SolicoWebServiceSetting();
            configuration.Bind(nameof(SolicoWebServiceSetting), solicoWebServiceSetting);
            services.AddSingleton(solicoWebServiceSetting);

            services.AddHttpClient("Solico", c =>
            {
                c.BaseAddress = new Uri(solicoWebServiceSetting.BaseUrl);
                c.Timeout = TimeSpan.FromMinutes(5);

                c.DefaultRequestHeaders.Add("Authorization",
                    $"Basic {Utils.SetHashBasicAuth(solicoWebServiceSetting.UserName, solicoWebServiceSetting.Password)}");
            }).AddTransientHttpErrorPolicy(x
                => x.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            

            #region Api Behavior

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = new
                    {
                        message =
                            actionContext.ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage.ToString())
                                .FirstOrDefault()
                    };
                    return new BadRequestObjectResult(errors);
                };
            });

            #endregion Api Behavior

            #region HangFire

            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangFireConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            #endregion HangFire

            return services;
        }
    }
}