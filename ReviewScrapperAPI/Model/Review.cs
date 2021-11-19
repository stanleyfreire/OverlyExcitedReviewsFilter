using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewScrapperAPI.Model
{
    public class Review
    {
        public string Author { get; set; }
        public DateTime Date{ get; set; }
        public double Score { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
    }
}
