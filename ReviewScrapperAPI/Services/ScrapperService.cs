using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ReviewScrapperAPI.Interfaces;
using ReviewScrapperAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReviewScrapperAPI.Services
{
    public class ScrapperService : IScrapperService
    {
        [HttpGet]
        public async Task<List<Review>> FetchReviews(int numberOfPages)
        {

            int page = 1;
            List<Review> fullList = new List<Review>();

            while (page <= numberOfPages)
            {
                fullList.AddRange(await FetchItensByPage(page));
                page++;
            }


            return fullList;
        }

        private static async Task<List<Review>> FetchItensByPage(int page)
        {
            var url = String.Format("https://www.dealerrater.com/dealer/McKaig-Chevrolet-Buick-A-Dealer-For-The-People-dealer-reviews-23685/page{0}/?filter=#link", page.ToString()); // TODO: Configuration 

            var client = new HttpClient();
            var doc = new HtmlDocument();
            List<Review> reviews = new List<Review>();

            var html = await client.GetStringAsync(url);
            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div").Where(t => t.GetAttributeValue("class", "").Equals("review-entry col-xs-12 text-left pad-none pad-top-lg  border-bottom-teal-lt"));

            foreach (var div in divs)
            {
                // Two divs store the main data I wanna use.
                var ratingDiv = div.Descendants("div").
                    FirstOrDefault(t => t.GetAttributeValue("class", "").Contains("review-date"));

                var reviewDiv = div.Descendants("div").
                    FirstOrDefault(t => t.GetAttributeValue("class", "").Contains("review-wrapper"));

                // Scrapping the divs to fetch data
                var author = reviewDiv.Descendants("span").FirstOrDefault().InnerText;

                var date = ratingDiv.Descendants("div").FirstOrDefault().InnerText;

                var scoreDivs = ratingDiv.Descendants("div");
                string score = string.Empty;

                foreach (var scoreDiv in scoreDivs)
                {
                    string scoreClass = scoreDiv.GetAttributeValue("class", "");
                    // Checking if div has class attribute named "rating-50" or "rating-40"
                    // which tells CSS how to render the rating stars on web.
                    score = Regex.Match(scoreClass, @"rating-\d+").Value; 

                    if (!string.IsNullOrEmpty(score))
                    {
                        score = (Convert.ToInt32(score.Split('-')[1]) / 10).ToString(); // rating-50 -> 50 -> 50/10 = 5 -> 5 stars
                        break;
                    }
                }

                var title = reviewDiv.Descendants("h3").FirstOrDefault().InnerText;

                var text = reviewDiv.Descendants("div").
                    Where(t => t.GetAttributeValue("class", "").Equals("tr margin-top-md")).FirstOrDefault().
                    Descendants("p").FirstOrDefault().InnerText;

                reviews.Add(new Review()
                {
                    Author = author.Substring(2, author.Length - 2), // removes the - from author's name
                    Date = Convert.ToDateTime(date),
                    Score = Convert.ToDouble(score),
                    Title = title,
                    ReviewText = text
                });
            }

            return reviews;

        }
    }
}
