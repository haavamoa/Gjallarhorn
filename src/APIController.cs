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
                package.Version = await m_nugetService.GetLatestVersionAsync(package.Name, package.Url);
                package.CompareVersion = await m_nugetService.GetLatestVersionAsync(package.Name, package.CompareUrl);

                return Ok(package);

            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }

    public class Package
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }
        public string CompareUrl { get; set; }
        public string CompareVersion { get; set; }
    }

    
}