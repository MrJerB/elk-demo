using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsDemo
{
    public class StudentReader : BackgroundService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<StudentReader> _logger;

        public StudentReader(ILogger<StudentReader> logger, IElasticClient elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoSearch(stoppingToken);
                await AggregateSearch(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            }
        }

        private async Task AggregateSearch(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregate search, bucket by awesome boolean");

            var searchResponse = await _elasticClient.SearchAsync<Student>(s =>

                s.Aggregations(a =>
                    a.Terms("is_awesome", ta => ta.Field(f => f.Awesome))
                )
            );

            var termsAggregation = searchResponse.Aggregations.Terms("is_awesome");

            var buckets = termsAggregation.Buckets;
            foreach (var bucket in buckets)
            {
                _logger.LogInformation("Bucket awesome value: {bucketName}, count {count}", bucket.KeyAsString, bucket.DocCount);
            }
        }

        private async Task DoSearch(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Attempting to read all the awesome students");

            var searchResponse = await _elasticClient.SearchAsync<Student>(s =>
                s.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.Awesome)
                            .Query("true")
                    )
                ), stoppingToken);

            var students = searchResponse.Documents;

            foreach (var student in students)
            {
                _logger.LogInformation("Found awesome student {name}", student.Name);
            }

        }
    }
}
