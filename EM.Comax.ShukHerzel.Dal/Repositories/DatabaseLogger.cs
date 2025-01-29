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
            //delete all logs older than the given days from the database
            var date = DateTime.Now.AddDays(-days);
           await _context.BulkDeleteAsync(_context.ServiceLogs.Where(x => x.TimeStamp < date));
           await _context.BulkDeleteAsync(_context.ErrorLogs.Where(x => x.CreateDate < date));
           await _context.BulkDeleteAsync(_context.TraceLogs.Where(x => x.CreateDate < date));
            
        }
    }
}
