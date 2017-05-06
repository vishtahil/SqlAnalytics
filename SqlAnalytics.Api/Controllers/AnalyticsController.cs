using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using SqlAnalyticsManager;
using System.Text;
using SqlAnalytics.Models;
using Microsoft.Extensions.Options;
using SqlAnalytics.Api.Configuration;
using SqlAnalyticsManager.Models;

namespace SqlAnalytics.Controllers
{
  [Route("api/[controller]")]
  public class AnalyticsController : Controller
  {
    private readonly ContentConfiguration _contentConfig;
    private SqlPlanManager _sqlManager = null;

    public AnalyticsController(IOptionsSnapshot<ContentConfiguration> contentAccessor)
    {
      _sqlManager = new SqlPlanManager();
      _contentConfig = contentAccessor.Value;
    }


    [HttpPost("Analytics")]
    public IActionResult GetSqlAnalytics([FromBody]AnalyticsModel analytics)
    {
      var analyticsSummary = new SqlStatisticsSummary();
      var sqlExecutionPlan = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64ExecutionPlan));
      var sql = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64Sql));
      var connectionString = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64ConnectionString));

      if (analytics.SqlMode == SqlAnalyticsMode.SQL_LINT_MODE)
      {
        analyticsSummary = _sqlManager.GetSqlStatistcisLintMode(sql);
      }
      else if (analytics.SqlMode == SqlAnalyticsMode.SQL_EXECUTION_PLAN_MODE)
      {
        analyticsSummary = _sqlManager.GetSqlStatistcisExecutionPlanMode(sqlExecutionPlan);
      }
      else
      {
        analyticsSummary = _sqlManager.GetSqlStatistcisSqlMode(connectionString,sql);
      }

      return Ok(analyticsSummary);
    }

    [HttpGet("help/{code}")]
    public IActionResult GetSqlLintHelp(string code)
    {
      var helpContentList = _contentConfig.Content.Where(x => x.Name.Equals(code, StringComparison.OrdinalIgnoreCase));

      if (helpContentList.Count() > 0)
      {
        return Ok(helpContentList.ToList());
      }

      throw new Exception("Sql Lint help code not found");
    }
  }
}