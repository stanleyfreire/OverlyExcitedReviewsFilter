using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReviewScrapper.Services
{
    public class DataFetcherService : IDataFetcherService
    {
        private readonly IConfigurationRoot _configuration;
        public DataFetcherService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public List<Review> FetchData(int pages)
        {
            if (pages <= 0)
                return null;

            int page = 1;
            var fullList = new List<Review>();

            try
            {
                while (page <= pages)
                {
                    fullList.AddRange(FetchItensByPage(page));
                    page++;
                }

                return fullList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error scrapping website. Exception: {e}");
                return null;
            }

        }

        private List<Review> FetchItensByPage(int page)
        {
            try
            {
                var url = String.Format(_configuration["WebsiteURL"].ToString(), page.ToString());

                var client = new HttpClient();
                var doc = new HtmlDocument();
                List<Review> reviews = new List<Review>();

                var html = client.GetStringAsync(url).Result;
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
                            score = (Convert.ToDouble(score.Split('-')[1]) / 10).ToString(); // rating-50 -> 50 -> 50/10 = 5 -> 5 stars
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
                        StarScore = Convert.ToDouble(score),
                        Title = title,
                        ReviewText = text
                    });

                }
                return reviews;

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
