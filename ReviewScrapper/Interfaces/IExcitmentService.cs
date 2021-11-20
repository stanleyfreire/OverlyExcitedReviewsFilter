using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Interfaces
{
    public interface IExcitmentService
    {
        public void EvaluateUser(Review review, List<Review> reviews);
        public void EvaluateSentences(Review review);
        public void EvaluateWords(Review review);
    }
}
