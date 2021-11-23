using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReviewScrapper.Tests.Services
{
    [TestClass]
    public class DataFetcherServiceTests
    {
        private Mock<IDataFetcherService> _dataFetcherService { get; set; }
        private Mock<IEvaluationService> _evaluationService { get; set; }
        private Mock<IFileService> _fileService { get; set; }
        private IConfigurationRoot _configuration { get; set; }
        private DataFetcherService dataFetcherService { get; set; }
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

            dataFetcherService = new DataFetcherService(_configuration);

        }
        
        [TestMethod]
        public void Should_Return_50_Reviews()
        {
            //Arrange
            
            //Action
            var result = dataFetcherService.FetchData(5);

            //Assert
            Assert.AreEqual(result.Count, 50);
        }

        [TestMethod]
        public void Should_Return_Null_If_Number_Of_Pages_Is_Zero()
        {
            //Arrange

            //Action
            var result = dataFetcherService.FetchData(0);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void Should_Expect_Null_If_Website_URL_Is_Null()
        {
            //Arrange
            _configuration["WebsiteURL"] = null;

            //Action
            var result = dataFetcherService.FetchData(1);

            //Assert
            Assert.IsTrue(result == null);
        }
    }
}
