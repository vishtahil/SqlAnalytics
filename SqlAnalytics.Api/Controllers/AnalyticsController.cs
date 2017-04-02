using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using SqlAnalyticsManager;
using System.Text;
using SqlAnalytics.Models;

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
        public IActionResult GetSqlAnalytics([FromBody]AnalyticsModel analytics)
        {
            analytics.ConnectionString = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64ConnectionString));
            analytics.Sql = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64Sql));
            var analyticsSummary = _sqlManager.GetSqlStatistcis(analytics.ConnectionString,
                analytics.Sql);
            return Ok(analyticsSummary);
        }
    }
}