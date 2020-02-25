using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Gjallarhorn.Client.UWP.Services
{
    public class NuGetService : INuGetService
    {
        private readonly ILogger m_logger;

        public NuGetService()
        {
            m_logger = new Logger();
        }

        public async Task<string> GetLatestVersionAsync(string packageName, string packageUrl, bool comparePrerelease)
        {
            ArgumentGuard.StringMissing(packageName, nameof(packageName));
            ArgumentGuard.StringMissing(packageUrl, nameof(packageUrl));

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

            var sourceRepository = Repository.Factory.GetCoreV3(packageUrl + prefix);
            var feed = await sourceRepository.GetResourceAsync<ListResource>();

            var allPackages = new List<IEnumerableAsync<IPackageSearchMetadata>>();
            var packagesFromSource = await feed.ListAsync(packageName, comparePrerelease, false, false, m_logger, CancellationToken.None);
            allPackages.Add(packagesFromSource);

            var comparer = new ComparePackageSearchMetadata();
            var asyncEnumerator = new AggregateEnumerableAsync<IPackageSearchMetadata>(allPackages, comparer, comparer).GetEnumeratorAsync();
            if (asyncEnumerator != null)
            {
                while (await asyncEnumerator.MoveNextAsync())
                {
                    var p = asyncEnumerator.Current;
                    if (p.Identity.Id.Equals(packageName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return p.Identity.Version.ToString();
                    }
                }
            }

            return "N/A";
        }
    }

    public static class ArgumentGuard
    {
        public static void StringMissing(string value, string nameOfValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Missing argument : {nameOfValue}");
            }
        }
    }

    public interface INuGetService
    {
        Task<string> GetLatestVersionAsync(string packageName, string packageUrl, bool comparePrerelease);
    }

    public class Logger : NuGet.Common.ILogger
    {
        public void LogDebug(string data)
        {
            
        }

        public void LogVerbose(string data)
        {
            
        }

        public void LogInformation(string data)
        {
            ;
        }

        public void LogMinimal(string data)
        {
            
        }

        public void LogWarning(string data)
        {
            
        }

        public void LogError(string data)
        {
            
        }

        public void LogInformationSummary(string data)
        {
            
        }

        public void Log(LogLevel level, string data)
        {
            
        }

        public Task LogAsync(LogLevel level, string data)
        {
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message)
        {
            
        }

        public Task LogAsync(ILogMessage message)
        {
            return Task.CompletedTask;
        }
    }
}