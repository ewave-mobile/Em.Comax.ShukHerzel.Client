namespace EM.Comax.ShukHerzl.Common;

public static class ApiResponsePathHelper
{
    /// <summary>
    /// Builds and ensures: {baseDirectory}\{companyId}\{yyyy}\{MM}\{dd}
    /// </summary>
    public static string GetOrCreateCompanyDateDirectory(string baseDirectory, long? companyId, DateTime? date = null)
    {
        var now = date ?? DateTime.Now;
        var companyFolder = companyId?.ToString() ?? "Unknown";
        var directoryPath = Path.Combine(
            baseDirectory,
            companyFolder,
            now.ToString("yyyy"),
            now.ToString("MM"),
            now.ToString("dd"));

        Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }
}
