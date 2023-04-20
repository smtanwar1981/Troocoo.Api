
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Troocoo.Api.Controllers;
using Troocoo.Api.CQRS.Query;
using Xunit;

namespace Troocoo.Api.Tests
{
    public class TransformNumberControllerTests
    {
        private readonly Mock<IMediator> mediatorMock ;
        private readonly Mock<ILogger<TransformNumberController>> loggerMock;
        public TransformNumberControllerTests()
        {
            mediatorMock = new Mock<IMediator>();
            loggerMock = new Mock<ILogger<TransformNumberController>>();
        }

        [Theory]
        [InlineData("-1")]
        public async Task TransformNumberToWords_should_return_bad_request_for_negative_integer(string value)
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            mediatorMock.Setup(m => m.Send(It.IsAny<TransformNumberToWordsQuery>(), default)).ReturnsAsync(string.Empty);
            var controllerMock = new TransformNumberController(mediatorMock.Object, loggerMock.Object);

            //act
            var result = await controllerMock.TransformNumberToWords(value, cancellationToken);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("testdata")]
        public async Task TransformNumberToWords_should_return_bad_request_for_a_string(string value)
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            mediatorMock.Setup(m => m.Send(It.IsAny<TransformNumberToWordsQuery>(), default)).ReturnsAsync(string.Empty);
            var controllerMock = new TransformNumberController(mediatorMock.Object, loggerMock.Object);

            //act
            var result = await controllerMock.TransformNumberToWords(value, cancellationToken);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("123.45")]
        public async Task TransformNumberToWords_should_return_valid_response(string value)
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            mediatorMock.Setup(m => m.Send(new TransformNumberToWordsQuery { NumberToConvert = Convert.ToDecimal(value) }, cancellationToken)).ReturnsAsync("ONE HUNDRED AND TWENTY-THREE AND FOURTY-FIVE");
            var controllerMock = new TransformNumberController(mediatorMock.Object, loggerMock.Object);

            //act
            var result = await controllerMock.TransformNumberToWords(value, cancellationToken);
            var statusCodeResult = (IStatusCodeActionResult)result;

            // assert
            Assert.NotNull(result);
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
    }
}
