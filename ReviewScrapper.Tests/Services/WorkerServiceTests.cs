using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReviewScrapper.Tests.Services
{
    [TestClass]
    public class WorkerServiceTests
    {

        private Mock<IDataFetcherService> _dataFetcherService { get; set; }
        private Mock<IEvaluationService> _evaluationService { get; set; }
        private Mock<IFileService> _fileService { get; set; }
        private IConfigurationRoot _configuration { get; set; }
        private WorkerService workerService { get; set; }
        private Review _review { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            _evaluationService = new Mock<IEvaluationService>();
            _fileService = new Mock<IFileService>();
            _dataFetcherService = new Mock<IDataFetcherService>();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            workerService = new WorkerService(_dataFetcherService.Object, _evaluationService.Object, _configuration);

        }
        private List<Review> CreateListReview()
        {
            return new List<Review>() //Carlos, Stanley, 
            {
                new Review() //Total 10
                {
                    Author = "Stanley",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 5,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() //Total 10
                {
                    Author = "Carlos",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 7,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 8
                {
                    Author = "Diego",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 3,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 6
                {
                    Author = "Bruno",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 3
                {
                    Author = "Panta",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 1
                {
                    Author = "Danilo",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 0,
                    SentenceScore = 0,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
            };

        }

        private List<Review> CreateDuplicateListReview()
        {
            return new List<Review>() //Carlos, Stanley, 
            {
                new Review() //Total 10
                {
                    Author = "Stanley",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 7,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() //Total 10
                {
                    Author = "Stanley",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 7,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 8
                {
                    Author = "Diego",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 3,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                },
                new Review() // Total 6
                {
                    Author = "Bruno",
                    Date = new DateTime(2021, 11, 21),
                    UserScore = 1,
                    WordScore = 2,
                    SentenceScore = 4,
                    StarScore = 5,
                    Title = "What a great experience!",
                    ReviewText = "kkkk",

                }
            };

        }


        #region Test Worker Service

        [TestMethod]
        public void Should_Return_3_Reviews()
        {
            //Arrange
            List<Review> reviews = CreateListReview();
            _dataFetcherService.Setup(t => t.FetchData(It.IsAny<int>())).Returns(reviews);

            //Action
            var result = workerService.Run(5);

            //Assert
            Assert.AreEqual(result.Count, 3);
        }

        [TestMethod]
        public void Tie_Break_Should_Return_Carlos_As_First()
        {
            //Arrange
            List<Review> reviews = CreateListReview();
            _dataFetcherService.Setup(t => t.FetchData(It.IsAny<int>())).Returns(reviews);

            //Action
            var result = workerService.Run(5);

            //Assert
            Assert.IsTrue(result.FirstOrDefault().Author == "Carlos");
        }

        [TestMethod]
        public void Tie_Break_Should_Return_Stanley_As_Second()
        {
            //Arrange
            List<Review> reviews = CreateListReview();
            _dataFetcherService.Setup(t => t.FetchData(It.IsAny<int>())).Returns(reviews);

            //Action
            var result = workerService.Run(5);

            //Assert
            Assert.IsTrue(result[1].Author == "Stanley");
        }

        [TestMethod]
        public void Tie_Break_Should_Return_Diego_As_Third()
        {
            //Arrange
            List<Review> reviews = CreateListReview();
            _dataFetcherService.Setup(t => t.FetchData(It.IsAny<int>())).Returns(reviews);

            //Action
            var result = workerService.Run(5);

            //Assert
            Assert.IsTrue(result[2].Author == "Diego");
        }

        [TestMethod]
        public void Should_Remove_Duplicate_From_List()
        {
            //Arrange
            List<Review> reviews = CreateDuplicateListReview(); //sends a list with 4 reviews, but 1 is a duplicate.
            _dataFetcherService.Setup(t => t.FetchData(It.IsAny<int>())).Returns(reviews);

            //Action
            var result = workerService.Run(It.IsAny<int>());

            //Assert
            Assert.IsTrue(result.Count == 3);
        }

        #endregion
    }
}
