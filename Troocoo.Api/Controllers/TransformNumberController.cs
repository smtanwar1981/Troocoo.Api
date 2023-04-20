using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Troocoo.Api.CQRS.Query;

namespace Troocoo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransformNumberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransformNumberController> _logger;

        public TransformNumberController(IMediator mediator, ILogger<TransformNumberController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("Transform")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransformNumberToWords([FromQuery] string numberToTransform, CancellationToken cancellationToken)
        {
            string result = string.Empty;
            try
            {
                _logger.LogInformation($"Request received to transform number: {numberToTransform}");

                decimal number0 = 0;
                var canConvert = decimal.TryParse(numberToTransform, out number0);
                if (!canConvert)
                {
                    _logger.LogInformation($"Not a valid number: {numberToTransform}.");
                    return BadRequest($"Not a valid number: {numberToTransform}");
                }
                
                decimal number = Convert.ToDecimal(numberToTransform);
                if (number < 0)
                {
                    _logger.LogInformation($"Number {numberToTransform} cannot be zero or negative.");
                    return BadRequest("Number can not be zero or negative");
                }
                result = await _mediator.Send(new TransformNumberToWordsQuery { NumberToConvert = number }, cancellationToken);
                _logger.LogInformation($"Finished processing request with : {result}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}
