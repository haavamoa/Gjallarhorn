using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Common;
using Microsoft.Extensions.Logging;
using Client.Models;

namespace Client
{
    [Produces("application/json")]
    public class APIController : ControllerBase
    {
        private ILogger<APIController> m_logger;
        private INuGetService m_nugetService;

        public APIController(ILogger<APIController> logger, INuGetService nugetService)
        {
            m_logger = logger;
            m_nugetService = nugetService;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ComparePackage([FromBody] string body)
        {
            var requestData = body;
            try
            {
                var package = JsonConvert.DeserializeObject<Package>(body);
                package.SourceAVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceA, package.ComparePrerelease);
                package.SourceBVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceB, package.ComparePrerelease);

                return Ok(package);

            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        public async Task<IActionResult> ComparePackages([FromBody] string body)
        {
            var requestData = body;
            try
            {
                CompareRequest compareRequest = JsonConvert.DeserializeObject<CompareRequest>(body);
                foreach (var sourceComparer in compareRequest.SourceComparers)
                {
                    foreach (var package in sourceComparer.Packages)
                    {
                        package.SourceA = sourceComparer.SourceA;
                        package.SourceB = sourceComparer.SourceB;
                        package.SourceAVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceA, package.ComparePrerelease);
                        package.SourceBVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.SourceB, package.ComparePrerelease);
                    }
                }

                var packages = MergePackages(compareRequest.SourceComparers);
                var orderedpackages = packages.OrderBy(p => p.SourceAVersion != p.SourceBVersion);
                return Ok(orderedpackages);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
        private List<Package> MergePackages(List<SourceComparer> sourceComparers)
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