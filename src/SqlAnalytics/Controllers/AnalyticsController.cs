using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using SqlAnalytics.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlAnalytics.Controllers
{
    [Route("api/[controller]")]
    public class AnalyticsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public class TodoItem
        {
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }
        
        [HttpPost("Analytics")]
        public IActionResult GetSqlAnalytics( [FromBody]AnalyticsModel analytics)
        {
            return Ok(analytics);
        }
    }
}
