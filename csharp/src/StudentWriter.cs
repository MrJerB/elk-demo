using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EsDemo
{
    public class StudentWriter : BackgroundService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<StudentWriter> _logger;
        private readonly Random _random;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public StudentWriter(IElasticClient elasticClient, ILogger<StudentWriter> logger, Random random)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _random = random;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Random students
                var name = new string(Enumerable.Repeat(chars, 8).Select(s => s[_random.Next(s.Length)]).ToArray());
                var isAwesome = _random.NextDouble() >= 0.5;
                var dobYear = DateTime.Today.AddYears(_random.Next(-17, -6)).Year;
                var dob = new DateTime(dobYear, _random.Next(1, 12), _random.Next(1, 28));

                var student = new Student(name, isAwesome, dob);

                var response = await _elasticClient.IndexDocumentAsync(student, stoppingToken);
                _logger.LogInformation("Indexed student name {name} response ID: {id}", student.Name, response.Id);

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
