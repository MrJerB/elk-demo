using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace EsDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Random>();

                    // NEST high level client
                    services.AddSingleton<IElasticClient>(s =>
                    {
                        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                            .DefaultIndex("students");

                        return new ElasticClient(settings);
                    });

                    services.AddHostedService<Lumberjack>();
                    services.AddHostedService<StudentWriter>();
                    services.AddHostedService<StudentReader>();
                })
                .UseSerilog((hostContext, configuration) =>
                {
                    configuration.WriteTo.Console();
                    configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        BatchAction = ElasticOpType.Create,
                        IndexFormat = "serilog-index-{0:yyyy.MM.dd}",
                        TypeName = null
                    });
                });

    }
}
