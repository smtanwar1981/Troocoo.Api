using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Troocoo.Api.Helpers;
using Troocoo.Api.Services;
using Xunit;

namespace Troocoo.Api.Tests
{
    public class TransformNumberServiceTests
    {
        private readonly string Cents = "CENTS";
        private readonly string Hundred = "HUNDRED";
        private readonly string Thousand = "THOUSAND";
        private readonly string ThreeTwentyThree = "THREE HUNDRED AND TWENTY-THREE";
        private readonly string OneTwoThreeFourtyFive = "ONE HUNDRED AND TWENTY-THREE AND FOURTY-FIVE CENTS";
        private readonly Mock<ILogger<TransformNumberService>> loggerMock;
        public TransformNumberServiceTests()
        {
            loggerMock = new Mock<ILogger<TransformNumberService>>();
        }

        [Fact]
        public void TransformNumberToWords_should_return_response_warning_message_for_more_than_thousand()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(32432), cancellationToken);

            //assert
            Assert.Contains(Constants.NumberTooBigWarningMsg, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_return_response_having_thousand_if_there_is_4_digit_number()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(3243), cancellationToken);

            //assert
            Assert.Contains(Thousand, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_return_response_having_hundreds_if_there_is_3_digit_number()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(323), cancellationToken);

            //assert
            Assert.Contains(Hundred, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_not_return_response_that_has_cents_if_there_is_no_fractional_values()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(323), cancellationToken);

            //assert
            Assert.DoesNotContain(Cents, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_have_cents_if_there_is_fractional_values()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(323.13), cancellationToken);

            //assert
            Assert.Contains(Cents, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_ignore_zeros_after_decimal()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(323.00), cancellationToken);

            //assert
            Assert.Equal(ThreeTwentyThree, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_ignore_leading_zeros()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(00323), cancellationToken);

            //assert
            Assert.Equal(ThreeTwentyThree, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_return_greater_than_0_warning_when_0_is_passed()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(-1), cancellationToken);

            //assert
            Assert.Equal(Constants.LessThanZeroWarningMsg, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_return_greater_than_0_warning_when_negative_number_passed()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(-1), cancellationToken);

            //assert
            Assert.Equal(Constants.LessThanZeroWarningMsg, result.Result);
        }

        [Fact]
        public void TransformNumberToWords_should_return_valid_response()
        {
            //arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;

            //setup
            var transformNumberService = new TransformNumberService(loggerMock.Object);

            //act
            var result = transformNumberService.TransformNumberToWords(Convert.ToDecimal(123.45), cancellationToken);

            //assert
            Assert.Equal(OneTwoThreeFourtyFive, result.Result);
        }
    }
}
