using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gjallarhorn.Blazor.Server.Services;
using Gjallarhorn.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gjallarhorn.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class ComparePackageController : Controller
    {
        private ILogger<ComparePackageController> m_logger;
        private INuGetService m_nugetService;

        public ComparePackageController(ILogger<ComparePackageController> logger, INuGetService nugetService)
        {
            m_logger = logger;
            m_nugetService = nugetService;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<Package> ComparePackage([FromBody] Package package)
        {
            try
            {
                package.SourceAVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceA, package.ComparePreRelease);
                package.SourceBVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceB, package.ComparePreRelease);

                return package;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Package>> ComparePackages([FromBody] string body)
        {
            var requestData = body;
            try
            {
                UserConfiguration userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(body);
                foreach (var sourceComparer in userConfiguration.SourceComparers)
                {
                    foreach (var package in sourceComparer.Packages)
                    {
                        package.SourceA = sourceComparer.SourceA;
                        package.SourceB = sourceComparer.SourceB;
                        package.SourceAVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceA, package.ComparePreRelease);
                        package.SourceBVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceB, package.ComparePreRelease);
                    }
                }

                var packages = MergePackages(userConfiguration.SourceComparers);
                return packages.OrderBy(p => p.SourceAVersion != p.SourceBVersion);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private static List<Package> MergePackages(List<SourceComparer> sourceComparers)
        {
            var packages = new List<Package>();
            foreach (var sourceComparer in sourceComparers)
            {
                foreach (var package in sourceComparer.Packages)
                {
                    packages.Add(package);
                }
            }
            return packages;
        }
    }
}