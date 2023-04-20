using MediatR;
using Troocoo.Api.Services;

namespace Troocoo.Api.CQRS.Query.Handler
{
    public class TransformNumberToWordsQueryHandler : IRequestHandler<TransformNumberToWordsQuery, string>
    {
        private readonly ITransformNumberService _transformNumberService;
        private readonly ILogger<TransformNumberToWordsQueryHandler> _logger;
        public TransformNumberToWordsQueryHandler(ITransformNumberService transformNumberService, ILogger<TransformNumberToWordsQueryHandler> logger)
        {
            _transformNumberService = transformNumberService;
            _logger = logger;
        }

        public async Task<string> Handle(TransformNumberToWordsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Executing TransformNumberToWordsQuery handler.");

            var result = await _transformNumberService.TransformNumberToWords(request.NumberToConvert, cancellationToken);

            _logger.LogInformation($"Finished executing TransformNumberToWordsQuery handler with result: {result}");

            return result;
        }
    }
}
