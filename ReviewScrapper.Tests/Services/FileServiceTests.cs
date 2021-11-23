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
    public class FileServiceTests
    {
        private Mock<IDataFetcherService> _dataFetcherService { get; set; }
        private Mock<IEvaluationService> _evaluationService { get; set; }
        private Mock<IFileService> _fileService { get; set; }
        private IConfigurationRoot _configuration { get; set; }
        private FileService fileService { get; set; }
        private Review _review { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            _fileService = new Mock<IFileService>();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            fileService = new FileService();

        }

        [TestMethod]
        public void Should_Return_True_Reading_Expressions_File()
        {
            //Arrange

            //Action
            var result = fileService.GetStringFromFile(_configuration["ExpressionsFile"]);

            //Assert
            Assert.IsTrue(result.Contains("Word"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Return_False_Sending_A_Empty_Path()
        {
            //Arrange

            //Action
            var result = fileService.GetStringFromFile("");

            //Assert
            //Expects Exception
        }

    }
}
