using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlStatisticsSummary
    {
        public SqlPlanStatisticsModel SqlPlanStatisticsModel { get; set; } = new Models.SqlPlanStatisticsModel();

        public SqlPlanOveriviewModel SqlPlanOverviewModel { get; set; } = new SqlPlanOveriviewModel();

        public List<SqlOptimizationHint> SqlOptimizationHints { get; set; } = new List<Models.SqlOptimizationHint>();
        
    }

    

    
   
}
