using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Services;
using System;
using System.Threading.Tasks;

namespace ReviewScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //using IHost host = CreateHostBuilder(args).Build();

            try
            {
                //setup our DI
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IDataFetcherService, DataFetcherService>()
                    .AddSingleton<IWorkerService, WorkerService>()
                    .AddSingleton<IExcitmentService, ExcitmentService>()
                    .BuildServiceProvider();

                //do the actual work here
                var worker = serviceProvider.GetService<IWorkerService>();
                var reviews = worker.RunEvaluation("https://localhost:44321/Reviews", 5).Result;

                reviews.ForEach(t => Console.WriteLine($"Author: {t.Author} \nDate: {t.Date.ToString()} \nReview: {t.ReviewText}\n\n"));

                Console.ReadLine();
            }
            catch (Exception e)
            {

                throw e;
            }
            

        }

        //static IHostBuilder CreateHostBuilder(string[] args) =>
        //   Host.CreateDefaultBuilder(args)
        //       .ConfigureServices((_, services) =>
        //           services.AddSingleton<IDataFetcherService, DataFetcherService>()
        //           .AddSingleton<IWorkerService, WorkerService>());

        //static async Task RunProgram(IServiceProvider services)
        //{
        //    using IServiceScope serviceScope = services.CreateScope();
        //    IServiceProvider provider = serviceScope.ServiceProvider;

        //    try
        //    {
        //        WorkerService worker = provider.GetRequiredService<WorkerService>();
        //        var reviews = await worker.RunEvaluation("https://localhost:44321/Reviews", 5);

        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }


        //    Console.ReadLine();
        //}


    }
}
