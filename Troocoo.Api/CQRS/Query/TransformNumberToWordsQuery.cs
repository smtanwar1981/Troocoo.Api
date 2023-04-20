using MediatR;

namespace Troocoo.Api.CQRS.Query
{
    /// <summary>
    /// Transform number to words query request.
    /// </summary>
    public class TransformNumberToWordsQuery : IRequest<string>
    {
        /// <summary>
        /// Gets or sets NumberToConvert.
        /// </summary>
        public decimal NumberToConvert { get; set; }
    }
}
