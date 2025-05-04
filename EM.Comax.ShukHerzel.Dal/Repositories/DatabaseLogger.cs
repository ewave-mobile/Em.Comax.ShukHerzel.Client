using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class DatabaseLogger : IDatabaseLogger
    {
        private readonly ShukHerzelEntities _context;

        public DatabaseLogger(ShukHerzelEntities dbContext)
        {
            _context = dbContext;
        }
        public async Task LogServiceActionAsync(string? message)
        {
            var serviceLog = new ServiceLog
            {
                Message = message,
                TimeStamp = DateTime.Now
            };

            _context.ServiceLogs.Add(serviceLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogErrorAsync(string? sourceName, string? request, Exception? exception)
        {
            var errorLog = new ErrorLog
            {
                SourceName = sourceName,
                Request = request,
                ExceptionMessage = exception?.Message ?? "",
                InnerException = exception?.InnerException?.Message ?? "",
                CreateDate = DateTime.Now
            };

            _context.ErrorLogs.Add(errorLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogTraceAsync(string? url, string? request, string? response, string? status)
        {
            var traceLog = new TraceLog
            {
                Url = url,
                Request = request,
                Response = response,
                ResponseStatus = status,
                CreateDate = DateTime.Now
            };

            _context.TraceLogs.Add(traceLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllLogsOlderThan(int days)
        {
            const int batchSize = 1000; // Define batch size
            var cutoffDate = DateTime.UtcNow.AddDays(-days); // Use UtcNow for consistency
            int totalServiceLogsDeleted = 0;
            int totalErrorLogsDeleted = 0;
            int totalTraceLogsDeleted = 0;

            // Log start (using a simple Console.WriteLine or similar if direct logging is complex here)
            Console.WriteLine($"Starting batch deletion of logs older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");
            // Alternatively, could try logging via a separate mechanism if available

            try
            {
                // --- Batch delete ServiceLogs ---
                while (true)
                {
                    var idsToDelete = await _context.ServiceLogs
                        .Where(log => log.TimeStamp < cutoffDate)
                        .Select(log => log.Id)
                        .Take(batchSize)
                        .ToListAsync();

                    if (!idsToDelete.Any()) break;

                    int deletedInBatch = await _context.ServiceLogs
                                               .Where(log => idsToDelete.Contains(log.Id))
                                               .ExecuteDeleteAsync();
                    totalServiceLogsDeleted += deletedInBatch;
                    if (deletedInBatch == 0 || deletedInBatch < batchSize) break;
                    // Optional: await Task.Delay(50);
                }
                Console.WriteLine($"Deleted {totalServiceLogsDeleted} old ServiceLog entries.");

                // --- Batch delete ErrorLogs ---
                while (true)
                {
                    var idsToDelete = await _context.ErrorLogs
                        .Where(log => log.CreateDate < cutoffDate)
                        .Select(log => log.LogId) // Use LogId instead of Id
                        .Take(batchSize)
                        .ToListAsync();

                    if (!idsToDelete.Any()) break;

                    int deletedInBatch = await _context.ErrorLogs
                                               .Where(log => idsToDelete.Contains(log.LogId)) // Use LogId instead of Id
                                               .ExecuteDeleteAsync();
                    totalErrorLogsDeleted += deletedInBatch;
                    if (deletedInBatch == 0 || deletedInBatch < batchSize) break;
                     // Optional: await Task.Delay(50);
                }
                 Console.WriteLine($"Deleted {totalErrorLogsDeleted} old ErrorLog entries.");

                // --- Batch delete TraceLogs ---
                 while (true)
                 {
                     var idsToDelete = await _context.TraceLogs
                         .Where(log => log.CreateDate < cutoffDate)
                         .Select(log => log.Id)
                         .Take(batchSize)
                         .ToListAsync();

                     if (!idsToDelete.Any()) break;

                     int deletedInBatch = await _context.TraceLogs
                                                .Where(log => idsToDelete.Contains(log.Id))
                                                .ExecuteDeleteAsync();
                     totalTraceLogsDeleted += deletedInBatch;
                     if (deletedInBatch == 0 || deletedInBatch < batchSize) break;
                      // Optional: await Task.Delay(50);
                 }
                 Console.WriteLine($"Deleted {totalTraceLogsDeleted} old TraceLog entries.");

                 Console.WriteLine($"Finished batch deletion of logs. Total deleted: Service={totalServiceLogsDeleted}, Error={totalErrorLogsDeleted}, Trace={totalTraceLogsDeleted}.");

            }
            catch (Exception ex)
            {
                 // Log error using a simple mechanism as direct logging might be problematic
                 Console.WriteLine($"Error during batch deletion of logs: {ex.Message}");
                 // Consider logging to a file or event log if Console isn't appropriate
                 throw; // Re-throw
            }
        }
    }
}
