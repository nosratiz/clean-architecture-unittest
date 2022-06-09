using Hastnama.Solico.Common.Email;
using Hastnama.Solico.Common.FileProcessor;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Helper.Claims.User;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Common.Sms;
using Hastnama.Solico.Common.TemplateNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hastnama.Solico.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileExtensions>(configuration.GetSection("FileExtensions"));
            services.Configure<HostAddress>(configuration.GetSection("HostAddress"));
            services.Configure<MultiLingualOptions>(configuration.GetSection("MultiLingual"));
            services.Configure<SolicoWebServiceSetting>(configuration.GetSection("SolicoWebServiceSetting"));
            services.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));
            services.Configure<Payamak>(configuration.GetSection("Payamak"));
            services.Configure<ImageSetting>(configuration.GetSection("ImageSetting"));

            services.AddSingleton<ILanguageInfo, LanguageInfo>();
            services.AddSingleton<IRequestMeta, RequestMeta>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentCustomerService, CurrentCustomerService>();
            services.AddScoped<ImageProcessor, ImageProcessor>();

            services.AddTransient<ILocalization, Localization.Localization>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddTransient<INotificationTemplateGenerator, NotificationTemplateGenerator>();
            services.AddTransient<IPayamakService, PayamakService>();
            services.AddTransient<IKaveNegarService, KaveNegarService>();
            
            
            
            #region Redis

            var redisConfiguration = new RedisConfiguration();
            configuration.Bind(nameof(RedisConfiguration), redisConfiguration);
            services.AddSingleton(redisConfiguration);

            services.AddDistributedRedisCache(options => { options.Configuration = redisConfiguration.Connection; });

            #endregion Redis


            return services;
        }
    }
}