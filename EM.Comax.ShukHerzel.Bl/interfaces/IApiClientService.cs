using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IApiClientService
    {
        Task SendItemsToEslAsync(bool logRequests, IProgress<string> progress, CancellationToken cancellationToken = default);
    }
}
