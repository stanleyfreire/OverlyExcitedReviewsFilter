using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ReviewScrapper
{
    class Program
    {
        public static IConfigurationRoot _configuration;
        public static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            try
            {
                SetupConfiguration();
                SetupInjections();

                //do work here
                var worker = serviceProvider.GetService<IWorkerService>();
                var reviews = worker.Run(_configuration["API:Address"].ToString().ToString(),
                    Convert.ToInt32(_configuration["API:Pages"].ToString())).Result;

                if (reviews != null)
                {
                    Console.WriteLine("### Overly Excited reviews: ###\n\n\n");

                    //print overly excited reviews
                    reviews.ForEach(t => Console.WriteLine($"Author: {t.Author} \n" +
                        $"Date: {t.Date} \n" +
                        $"Review: {t.ReviewText}\n" +
                        $"TotalScore: {t.TotalScore}\n\n"));
                }
                else
                    Console.WriteLine("API returned null.");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erros Running The Solution: {e}");
                Console.ReadLine();
            }
        }

        private static void SetupConfiguration()
        {
            //setup configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private static void SetupInjections()
        {
            //setup DI
            serviceProvider = new ServiceCollection()
                .AddSingleton<IDataFetcherService, DataFetcherService>()
                .AddSingleton<IWorkerService, WorkerService>()
                .AddSingleton<IEvaluationService, EvaluationService>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton(_configuration)
                .BuildServiceProvider();
        }
    }
}
