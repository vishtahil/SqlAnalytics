using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlStatisticsSummary
    {
        public List<SqlPlanStatisticsModel> SqlPlanStatisticsModel { get; set; } = new List<Models.SqlPlanStatisticsModel>();

        public SqlPlanOveriviewModel SqlPlanOverviewModel { get; set; } = new SqlPlanOveriviewModel();

        public List<SqlOptimizationHint> SqlOptimizationHint { get; set; } = new List<Models.SqlOptimizationHint>();

        public string NormalizedSql { get; set; }
    }

    

    
   
}
