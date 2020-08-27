using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gjallarhorn.Server.Controllers
{
    public class TestController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
