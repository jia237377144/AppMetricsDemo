using App.Metrics;
using App.Metrics.Formatters.Json;
using App.Metrics.Health;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Metrics监控配置
            /*
            var uri = new Uri("http://127.0.0.1:8086");
            var metrics = AppMetrics.CreateDefaultBuilder()
            .Configuration.Configure(
            options =>
            {
                options.AddAppTag("TestDev");
                options.AddEnvTag("Developer");
            })
            .Report.ToInfluxDb(
            options =>
            {
                options.InfluxDb.BaseUri = uri;
                options.InfluxDb.Database = "TestDev";
                options.InfluxDb.UserName = "admin";
                options.InfluxDb.Password = "jialipeng";
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.FlushInterval = TimeSpan.FromSeconds(5);
            })
            .Build();

            services.AddMetrics(metrics);
            */
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 注入Metrics
            /*
            app.UseMetricsActiveRequestMiddleware();
            app.UseMetricsAllMiddleware();
            */
            #endregion
            //Run();
            app.UseMvc();
        }

    }

    public class OKHealthCheck : HealthCheck
    {
        public OKHealthCheck() : base("正常的检查(OKHealthCheck)") { }

        protected override async ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(HealthCheckResult.Healthy("OK"));
        }

    }
}
