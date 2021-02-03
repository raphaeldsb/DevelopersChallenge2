using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Parsers.Interfaces;
using BankTransactionConciliationAPI.Repository.Interfaces;
using BankTransactionConciliationAPI.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BankTransactionConciliationAPI.Tests.Services
{
    public class BankTransactionServiceTests
    {
        /// <summary>
        /// simple test with mock sample
        /// </summary>
        [Fact]        
        public void CreateMany_Should_Parses_And_Returns_BankTransactionsList()
        {
            // arrange
            Mock<IBankTransactionRepository> repositoryMock = new Mock<IBankTransactionRepository>();
            repositoryMock
                .Setup(m => m.Upsert(It.IsAny<BankTransaction>()));

            Mock<ICsvParser> csvParserMock = new Mock<ICsvParser>(); ;
            
            Mock<IOfxParser> ofxParserMock = new Mock<IOfxParser>();

            ofxParserMock
                .Setup(m => m.Convert(It.IsAny<string>()))
                .Returns(new List<BankTransaction>
                {
                    new BankTransaction(),
                    new BankTransaction(),
                    new BankTransaction()
                });

            var service = new BankTransactionService(
                repositoryMock.Object, 
                csvParserMock.Object, 
                ofxParserMock.Object);

            var ofx = "<OFX>....</OFX>";

            // act
            var result = service.CreateMany(ofx);

            // assert
            Assert.Equal(3, result.Count);
            ofxParserMock.Verify(m => m.Convert(It.IsAny<string>()), Times.Once);
            repositoryMock.Verify(m => m.Upsert(It.IsAny<BankTransaction>()), Times.Exactly(3));
        }
    }
}
