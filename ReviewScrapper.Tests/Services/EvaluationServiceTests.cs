using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReviewScrapper.Tests.Services
{
    [TestClass]
    public class EvaluationServiceTests
    {
        private Mock<IEvaluationService> _evaluationService { get; set; }
        private Mock<IFileService> _fileService { get; set; }
        private IConfigurationRoot _configuration { get; set; }
        private EvaluationService evaluationService { get; set; }
        private Review _review { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            // Method to instantiate EvaluationService
            _evaluationService = new Mock<IEvaluationService>();
            _fileService = new Mock<IFileService>();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var expressionsText = System.IO.File.ReadAllText(_configuration["ExpressionsFile"]);
            var scoresText = System.IO.File.ReadAllText(_configuration["ScoresFile"]);

            _fileService.SetupSequence
                (t => t.GetStringFromFile(It.IsAny<string>())).
                Returns(expressionsText).
                Returns(scoresText);

            evaluationService = new EvaluationService(_fileService.Object, _configuration);
            
        }

        // created for brevity
        private Review CreateBaseReview()
        {
            return new Review()
            {
                Author = "Stanley",
                Date = new DateTime(2021, 11, 21),
                Score = 5,
                Title = "What a great experience!",
            };
        }

        [TestMethod]
        public void ShouldCheckIfUserIsInsideListAndReturn2()
        {
            //Arrange
            List<Review> reviews = new List<Review>();

            var review = new Review()
            {
                Author = "Stanley",
                Date = new DateTime(2021, 11, 21),
                Score = 5,
                Title = "What a great experience!",
                ReviewText = "Best experience of my life!"
            };

            var review2 = new Review()
            {
                Author = "Stanley",
                Date = new DateTime(2021, 11, 19),
                Score = 5,
                Title = "The Best!",
                ReviewText = "Best experience of my life!"
            };

            var review3 = new Review()
            {
                Author = "John",
                Date = new DateTime(2021, 11, 19),
                Score = 5,
                Title = "The Best!",
                ReviewText = "Best experience of my life!"
            };

            reviews.Add(review);
            reviews.Add(review2);
            reviews.Add(review3);

            _evaluationService.Setup(t => t.EvaluateUser(review, reviews));

            //Action
            double result = evaluationService.EvaluateUser(review, reviews);


            //Assert
            Assert.AreEqual(result, 2);

        }

        [TestMethod]
        public void ShouldCheckIfUserIsInsideListAndReturn1()
        {
            //Arrange
            List<Review> reviews = new List<Review>();

            var review = new Review()
            {
                Author = "Stanley",
                Date = new DateTime(2021, 11, 21),
                Score = 5,
                Title = "What a great experience!",
                ReviewText = "Best experience of my life!"
            };

            var review2 = new Review()
            {
                Author = "Jonas",
                Date = new DateTime(2021, 11, 19),
                Score = 5,
                Title = "The Best!",
                ReviewText = "Best experience of my life!"
            };

            var review3 = new Review()
            {
                Author = "John",
                Date = new DateTime(2021, 11, 19),
                Score = 5,
                Title = "The Best!",
                ReviewText = "Best experience of my life!"
            };

            reviews.Add(review);
            reviews.Add(review2);
            reviews.Add(review3);

            _evaluationService.Setup(t => t.EvaluateUser(review, reviews));

            //Action
            double result = evaluationService.EvaluateUser(review, reviews);


            //Assert
            Assert.AreEqual(result, 1);

        }

        #region Test Words

        [TestMethod]
        public void ShouldReturn0_NoWords()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Normal experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 0); ;

        }

        [TestMethod]
        public void ShouldReturn1_OneNormalWord()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Great experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);

            //Assert
            Assert.AreEqual(review.TotalScore, 1);

        }

        [TestMethod]
        public void ShouldReturn2_OneHappyWord()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Great! experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 2);

        }

        [TestMethod]
        public void ShouldReturn3_OneExcitedWord()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Great!! experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 3);

        }

        [TestMethod]
        public void ShouldReturn4_OneOverlyExcitedWord()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Great!!! experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 4);

        }

        [TestMethod]
        public void ShouldReturn5_OneExtraWord()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "I Had a Great!!!! experience";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 5);

        }

        #endregion

        #region Test Sentences
        [TestMethod]
        public void ShouldReturn2_OneNormalSentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "This is the Best dealership";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 2);
        }

        [TestMethod]
        public void ShouldReturn3_OneHappySentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "This is the Best dealership!";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 3);
        }

        [TestMethod]
        public void ShouldReturn4_OneExcitedSentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "This is the Best dealership!!";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 4);
        }

        [TestMethod]
        public void ShouldReturn5_OneOverlyExcitedSentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "This is the Best dealership!!!";

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 5);
        }

        #endregion 

        #region Test Words Contained In Sentence
        [TestMethod]
        public void ShouldReturn2_5_OneWordContainedInSentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "That was my best experience. This is the Best dealership ever."; // best + best dealership

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 2.5);
        }

        [TestMethod] //TODO
        public void ShouldReturn3_TwoWordsContainedInSentence()
        {
            //Arrange
            Review review = CreateBaseReview();
            review.ReviewText = "That was my best experience. Best. This is the Best dealership ever."; // best (0.5) + best (0.5) + best dealership (2)

            _evaluationService.Setup(t => t.EvaluateReviewBody(review));

            //Action
            evaluationService.EvaluateReviewBody(review);


            //Assert
            Assert.AreEqual(review.TotalScore, 3);
        }

        #endregion
    }
}