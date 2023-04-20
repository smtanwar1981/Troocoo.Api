using Troocoo.Api.Helpers;

namespace Troocoo.Api.Services
{
    public class TransformNumberService : ITransformNumberService
    {
        private readonly ILogger<TransformNumberService> _logger;

        public TransformNumberService(ILogger<TransformNumberService> logger)
        {
            _logger = logger;
        }
        /// <inheritdoc />
        public async Task<string> TransformNumberToWords(decimal numberToTransform, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Executing TransformNumberToWords() method.");

            string result = string.Empty;
            if (Utility.IsNumberLessPositiveOrGreaterThanZero(numberToTransform))
            {
                _logger.LogInformation("Number found to be positive.");

                int fractionalNumber = Utility.GetFractionalPart(numberToTransform);

                _logger.LogInformation($"Fractional part of the number is : {fractionalNumber}");

                int decimalNumber = Utility.GetDecimalPart(numberToTransform);

                if (decimalNumber.ToString().Length > 4)
                {
                    _logger.LogInformation($"{Constants.NumberTooBigWarningMsg} : {decimalNumber}");
                    result = Constants.NumberTooBigWarningMsg;
                }
                else
                {
                    _logger.LogInformation($"Decimal part of the number is : {decimalNumber}");
                    result = TransformNumber(decimalNumber) + (fractionalNumber > 0 ? " AND " + TransformNumber(fractionalNumber) + " CENTS" : string.Empty);
                }
            }
            else
            {
                _logger.LogInformation($"Number: {numberToTransform} found to be either negative or zero.");
                result = Constants.LessThanZeroWarningMsg;
            }

            _logger.LogInformation($"Finished executing TransformNumberToWords() method with result : {result}");

            return await Task.Run(() => result);
        }

        private string TransformNumber(int numberToTransform)
        {
            if (numberToTransform < 20)
            {
                return Constants.Ones[numberToTransform];
            }
            else if (numberToTransform > 19 && numberToTransform < 100)
            {
                return GetWordsLessThanHundreds(numberToTransform);
            }
            else if (numberToTransform > 99 && numberToTransform < 1000)
            {
                return GetWordsLessThanThousand(numberToTransform);
            }
            else if (numberToTransform > 999 && numberToTransform < 10000)
            {
                int thousandsDigit = numberToTransform;
                do
                {
                    thousandsDigit = thousandsDigit / 10;
                } while (thousandsDigit > 9);
                int onesDigit = numberToTransform % 1000;
                return Constants.Ones[thousandsDigit] + " THOUSANDS " + (onesDigit < 10 ? Constants.Ones[onesDigit] : onesDigit < 100 ? GetWordsLessThanHundreds(onesDigit) : GetWordsLessThanThousand(onesDigit));
            }

            return string.Empty;
        }

        private string GetWordsLessThanHundreds(int theNumber)
        {
            int tensDigit = theNumber / 10;
            int onesDigit = theNumber % 10;
            return Constants.Tens[tensDigit] + "-" + (onesDigit > 0 ? Constants.Ones[onesDigit] : "");
        }

        private string GetWordsLessThanThousand(int theNumber)
        {
            int hundredsDigit = theNumber;
            int onesDigit = theNumber % 100;
            if (onesDigit > 9)
            {
                do
                {
                    hundredsDigit = hundredsDigit / 10;
                } while (hundredsDigit > 10);
            }
            else
            {
                do
                {
                    hundredsDigit = hundredsDigit / 10;
                } while (hundredsDigit > 9);
            }

            return Constants.Ones[hundredsDigit] + " HUNDRED " + (onesDigit > 0 ? "AND " + (onesDigit < 20 ? Constants.Ones[onesDigit] : GetWordsLessThanHundreds(onesDigit)) : "");
        }
    }
}
