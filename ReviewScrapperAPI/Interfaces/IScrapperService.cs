using ReviewScrapperAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewScrapperAPI.Interfaces
{
    public interface IScrapperService
    {
        public Task<List<Review>> FetchReviews(int numberOfPages);
    }
}
