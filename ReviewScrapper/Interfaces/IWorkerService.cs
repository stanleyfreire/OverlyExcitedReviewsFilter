using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewScrapper.Interfaces
{
    public interface IWorkerService
    {
        public Task<List<Review>> RunEvaluation(string endpoint, int pages);
    }
}
