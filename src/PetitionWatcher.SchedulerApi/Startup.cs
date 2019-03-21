using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetitionWatcher.SchedulerApi.Publishers;

namespace PetitionWatcher.SchedulerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("hangfire")));
        }        
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration.GetConnectionString("hangfire"));

            app.UseMvc();           
            app.UseHangfireServer();

            if (Configuration.GetValue<bool>("Hangfire:EnableDashboard"))
            {
                app.UseHangfireDashboard();
            }

            //TODO-move all this to config, use ID & CRON format
            RecurringJob.AddOrUpdate("petition-load-241584", () => DataLoadDuePublisher.Publish(241584), Cron.Minutely);
        }
    }
}
