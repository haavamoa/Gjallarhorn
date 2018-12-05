using Client;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class NuGetService : INuGetService
{
    private ILogger<NuGetService> m_logger;

    public NuGetService(ILogger<NuGetService> logger)
    {
        m_logger = logger;
    }
    public async Task<string> GetLatestVersionAsync(string packageName, string packageUrl)
    {
        var prefix = string.Empty;
        if (packageUrl.Contains("api.nuget.org/v3/"))
        {
            if (packageUrl.Last().Equals('/'))
            {
                prefix += "index.json";
            }
            else
            {
                prefix += "/index.json";
            }

        }

        NuGet.Common.ILogger logger = new Logger(m_logger);

        var sourceRepository = Repository.Factory.GetCoreV3(packageUrl + prefix);
        var feed = await sourceRepository.GetResourceAsync<ListResource>();

        var allPackages = new List<IEnumerableAsync<IPackageSearchMetadata>>();
        var packagesFromSource =
            await feed.ListAsync(packageName, true, false, false, logger, CancellationToken.None);
        allPackages.Add(packagesFromSource);

        ComparePackageSearchMetadata comparer = new ComparePackageSearchMetadata();
        var asyncEnumerator = new AggregateEnumerableAsync<IPackageSearchMetadata>(allPackages, comparer, comparer).GetEnumeratorAsync();
        if (asyncEnumerator != null)
        {
            while (await asyncEnumerator.MoveNextAsync())
            {
                var p = asyncEnumerator.Current;
                if (p.Identity.Id == packageName)
                {
                    return p.Identity.Version.ToString();
                }
            }
        }
        return "N/A";
    }
}

public interface INuGetService
{
    Task<string> GetLatestVersionAsync(string packageName, string packageUrl);
}

public class Logger : NuGet.Common.ILogger
{
    private ILogger<INuGetService> m_logger;

    public Logger(ILogger<INuGetService> m_logger)
    {
        this.m_logger = m_logger;
    }

    public void LogDebug(string data) => m_logger.LogDebug(message: data);
    public void LogVerbose(string data) => m_logger.LogTrace(message: data);
    public void LogInformation(string data) => m_logger.LogInformation(message: data);
    public void LogMinimal(string data) => m_logger.LogInformation(message: data);
    public void LogWarning(string data) => m_logger.LogWarning(message: data);
    public void LogErrorSummary(string data) => m_logger.LogError(message: data);
    public void LogInformationSummary(string data) => m_logger.LogInformation(message: data);

    public void LogError(string data) => m_logger.LogError(message: data);
}