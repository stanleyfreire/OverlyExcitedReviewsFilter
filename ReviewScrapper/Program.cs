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

        static void Main(string[] args)
        {
            try
            {
                //setup configuration
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json", false)
                    .Build();

                //setup our DI
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IDataFetcherService, DataFetcherService>()
                    .AddSingleton<IWorkerService, WorkerService>()
                    .AddSingleton<IExcitmentService, ExcitmentService>()
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton(_configuration)
                    .BuildServiceProvider();

                //do the work here
                var worker = serviceProvider.GetService<IWorkerService>();
                var reviews = worker.RunEvaluation(_configuration["API:Address"].ToString().ToString(),
                    Convert.ToInt32(_configuration["API:Pages"].ToString())).Result;
                

                //print overly excited reviews
                reviews.ForEach(t => Console.WriteLine($"Author: {t.Author} \nDate: {t.Date.ToString()} \nReview: {t.ReviewText}\n\n"));

                Console.ReadLine();
            }
            catch (Exception e)
            {
                throw e;
            }
            

        }
    }
}
