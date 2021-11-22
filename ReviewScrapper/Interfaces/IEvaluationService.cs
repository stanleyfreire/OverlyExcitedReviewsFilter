using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewScrapper.Interfaces
{
    public interface IEvaluationService
    {
        public double EvaluateUser(Review review, List<Review> reviews);
        public void EvaluateReviewBody(Review review);
    }
}
