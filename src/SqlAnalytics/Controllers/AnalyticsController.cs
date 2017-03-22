using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using SqlAnalytics.Models;
using SqlAnalyticsManager;

namespace SqlAnalytics.Controllers
{
    [Route("api/[controller]")]
    public class AnalyticsController : Controller
    {
        private SqlPlanManager _sqlManager = null;

        public AnalyticsController()
        {
            _sqlManager = new SqlPlanManager();
        }

        
        [HttpPost("Analytics")]
        public IActionResult GetSqlAnalytics( [FromBody]AnalyticsModel analytics)
        {
            var analyticsSummary = _sqlManager.GetSqlStatistcis(analytics.ConnectionString,
                analytics.Sql);
            return Ok(analyticsSummary);
        }
    }
}
