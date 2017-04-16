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
      analytics.ConnectionString = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64ConnectionString));
      analytics.Sql = Encoding.UTF8.GetString(Convert.FromBase64String(analytics.Base64Sql));
      var analyticsSummary = _sqlManager.GetSqlStatistcis(analytics.ConnectionString,
          analytics.Sql);
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