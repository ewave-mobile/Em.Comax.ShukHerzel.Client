namespace EM.Comax.ShukHerzl.Common;

public sealed class ApiResponseCleanupResult
{
    public bool Skipped { get; init; }
    public string Message { get; init; } = string.Empty;
    public int DeletedCount { get; init; }
    public IReadOnlyList<(string FilePath, Exception Exception)> FileErrors { get; init; }
        = Array.Empty<(string, Exception)>();
}

public static class ApiResponseCleanupHelper
{
    public static ApiResponseCleanupResult Cleanup(string? directoryPath, int retentionDays)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return new ApiResponseCleanupResult
            {
                Skipped = true,
                Message = "ApiResponseCleanup skipped: ApiResponseDirectory is not configured."
            };
        }

        if (retentionDays <= 0)
        {
            return new ApiResponseCleanupResult
            {
                Skipped = true,
                Message = "ApiResponseCleanup skipped: ApiResponseRetentionDays must be greater than 0."
            };
        }

        if (!Directory.Exists(directoryPath))
        {
            return new ApiResponseCleanupResult
            {
                Skipped = true,
                Message = $"ApiResponseCleanup skipped: directory does not exist ({directoryPath})."
            };
        }

        var cutoff = DateTime.Now.AddDays(-retentionDays);
        var deletedCount = 0;
        var fileErrors = new List<(string, Exception)>();

        foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
        {
            try
            {
                if (File.GetCreationTime(filePath) < cutoff)
                {
                    File.Delete(filePath);
                    deletedCount++;
                }
            }
            catch (Exception ex)
            {
                fileErrors.Add((filePath, ex));
            }
        }

        DeleteEmptyDirectories(directoryPath);

        return new ApiResponseCleanupResult
        {
            Skipped = false,
            DeletedCount = deletedCount,
            FileErrors = fileErrors,
            Message = $"ApiResponseCleanup completed. Deleted {deletedCount} file(s) older than {retentionDays} days from {directoryPath}."
        };
    }

    private static void DeleteEmptyDirectories(string rootPath)
    {
        foreach (var directory in Directory.EnumerateDirectories(rootPath, "*", SearchOption.AllDirectories)
                     .OrderByDescending(d => d.Length))
        {
            if (!Directory.EnumerateFileSystemEntries(directory).Any())
            {
                Directory.Delete(directory);
            }
        }
    }
}
