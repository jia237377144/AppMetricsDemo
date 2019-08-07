using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Health;
using App.Metrics.AspNetCore.Health;
using System.Diagnostics;
using App.Metrics.Reporting.InfluxDB;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var uri = new Uri("http://127.0.0.1:8086");
            //InfluxDb 配置
            var metricsReportingInfluxDbOptions = new MetricsReportingInfluxDbOptions
            {
                InfluxDb = new InfluxDbOptions
                {
                    BaseUri = uri,
                    Database = "TestDev",
                    UserName = "admin",
                    Password = "jialipeng",
                },
                FlushInterval = TimeSpan.FromSeconds(5),
                HttpPolicy = new App.Metrics.Reporting.InfluxDB.Client.HttpPolicy
                {
                    BackoffPeriod = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10),
                    FailuresBeforeBackoff = 5
                }
            };
            //创建指标
            var metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
            {
                options.AddEnvTag("developer");
                options.AddAppTag("Demo");
            })
            .Report.ToInfluxDb(metricsReportingInfluxDbOptions).Build();

            return WebHost.CreateDefaultBuilder(args)
                 .ConfigureMetricsWithDefaults(
                builder =>
                {
                    builder.Report.ToInfluxDb(metricsReportingInfluxDbOptions);
                })
               .ConfigureHealthWithDefaults((context, builder) =>
               {
                   builder.HealthChecks.AddPingCheck("ping baidu", "www.baidu.com", TimeSpan.FromSeconds(3));
                   builder.HealthChecks.AddPingCheck("ping google", "www.google.com", TimeSpan.FromSeconds(3));
                   builder.Report.ToMetrics(metrics, new App.Metrics.Health.Reporting.Metrics.HealthAsMetricsOptions { Enabled = true, ReportInterval = TimeSpan.FromSeconds(3) });
               })
               .UseHealth()
               .UseMetrics()
               .UseStartup<Startup>()
               .Build();

        }
    }
}
