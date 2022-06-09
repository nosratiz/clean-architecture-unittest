using Hangfire;
using Hastnama.Solico.Api.Installer;
using Hastnama.Solico.Api.Middleware;
using Hastnama.Solico.Application;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common;
using Hastnama.Solico.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using Hastnama.Solico.Api.Filter;
using Hastnama.Solico.Common.Environment;

namespace Hastnama.Solico.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesAssembly(Configuration);

            services.AddApplication(Configuration);

            services.AddCommon(Configuration);

            services.AddPersistence(Configuration);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApplicationBootstrapper applicationBootstrapper, ISolicoJobService solicoJobService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            applicationBootstrapper.Initial();

            #region Folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Documents)),
                RequestPath = ApplicationStaticPath.Clients.Document
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Others)),
                RequestPath = ApplicationStaticPath.Clients.Other
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Images)),
                RequestPath = ApplicationStaticPath.Clients.Image
            });

            #endregion Folder

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hastnama.kale.Api v1"));

            app.UseHttpsRedirection();
            
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXContentTypeOptions();
            app.UseXfo(options => { options.Deny(); });

            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseMultiLingual();
            app.UseMiddleware<ApplicationMetaMiddleware>();
            app.UseMiddleware<MembershipMiddleware>();

            app.UseAuthorization();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hang-fire", new  DashboardOptions
            {
                Authorization = new []{new CustomAuthorization()}
            });

          
            BackgroundJob.Enqueue(() => solicoJobService.SyncCustomerPriceAsync());
             //BackGroundJob(solicoJobService);


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }


        #region Method

        private static void BackGroundJob(ISolicoJobService solicoJobService)
        {
            BackgroundJob.Enqueue(() => solicoJobService.SyncMaterialAsync());
            RecurringJob.AddOrUpdate("SyncMaterial", () => solicoJobService.SyncMaterialAsync(), Cron.Daily);
            RecurringJob.AddOrUpdate("SyncCustomer", () => solicoJobService.SyncCustomerAsync(), Cron.Daily);
            RecurringJob.AddOrUpdate("SyncNewCustomer", () => solicoJobService.SyncNewCustomer(), Cron.Daily);
            RecurringJob.AddOrUpdate("SyncCredit", () => solicoJobService.SyncCreditAsync(), Cron.Daily);
            RecurringJob.AddOrUpdate("SyncProforma", () => solicoJobService.TrackProformaAsync(), Cron.Hourly);
            RecurringJob.AddOrUpdate("SyncSailedOrder", () => solicoJobService.TrackSailedOrderAsync(), Cron.Hourly);
            RecurringJob.AddOrUpdate("SyncQuotation", () => solicoJobService.SyncQuotation(), Cron.Hourly);
            RecurringJob.AddOrUpdate("OpenItems",()=> solicoJobService.SyncOpenItems(),Cron.Daily);
            RecurringJob.AddOrUpdate("SyncDeliverOrder",()=> solicoJobService.SyncDeliverOrderAsync(),Cron.Daily);
            RecurringJob.AddOrUpdate("DeleteOldTokens",()=> solicoJobService.DeleteExpiredUserTokenAsync(),Cron.Weekly);
            RecurringJob.AddOrUpdate("SyncMaterialPrice",()=> solicoJobService.SyncCustomerPriceAsync(),Cron.Daily);
        }

        #endregion
     
    }
}