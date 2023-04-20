namespace Troocoo.Api.Services
{
    public interface ITransformNumberService
    {
        /// <summary>
        /// This method will transform a number to its words form.
        /// </summary>
        /// <param name="NumberToTransform">The number to transform in decimal</param>
        /// <param name="cancellationToken">A cancellation token for the ongoing request.</param>
        /// <returns></returns>
        public Task<string> TransformNumberToWords(decimal NumberToTransform, CancellationToken cancellationToken);
    }
}
