using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IDatabaseLogger
    {
        Task LogServiceActionAsync(string message);
        Task LogErrorAsync(string sourceName, string request, Exception exception);
        Task LogToErrorLogAsync(string sourceName, string request, string message);
        Task LogTraceAsync(string? url, string? request, string? response, string? status);
        Task DeleteAllLogsOlderThan(int days);
    }
}
