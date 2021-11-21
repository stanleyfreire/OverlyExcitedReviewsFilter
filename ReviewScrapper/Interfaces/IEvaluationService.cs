using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Interfaces
{
    public interface IEvaluationService
    {
        public void EvaluateUser(Review review, List<Review> reviews);
        public void EvaluateReviewBody(Review review);
    }
}
