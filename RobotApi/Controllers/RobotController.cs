using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RobotApi.Controllers
{
    public class Response
    {
        public string Output { get; set; }
    }

    public class RobotController : Controller
    {
        private readonly ILogger<RobotController> _logger;

        Response resp = new Response();

        public RobotController(ILogger<RobotController> logger)
        {
            _logger = logger;
        }

        [HttpPost]

        public ActionResult<string> ExecuteCommands([FromBody] IEnumerable<string> inputCommands)
        {
            Robot rb = new Robot(5);
            string[] line = inputCommands.ToArray();
            if (line != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    rb.performAction(line[i]);
                    if (line[i].Equals("REPORT"))
                    {
                        resp.Output = rb.cmdReport();
                    }
                }
            }
            return resp.Output;
        }

    }
}
