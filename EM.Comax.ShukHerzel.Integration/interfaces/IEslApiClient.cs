// IEslApiClient.cs
using EM.Comax.ShukHerzel.Models.DtoModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Integration.interfaces
{
    public interface IEslApiClient
    {
        /// <summary>
        /// Sends a batch of items to the ESL API for a specific store.
        /// </summary>
        /// <param name="storeId">The ID of the store.</param>
        /// <param name="items">The list of EslDto items to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendItemsToEslAsync(string storeId, IEnumerable<EslDto> items, CancellationToken cancellationToken = default);
    }
}
