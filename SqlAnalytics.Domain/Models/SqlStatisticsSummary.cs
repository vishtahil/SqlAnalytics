using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlStatisticsSummary
    {
        public List<SqlPlanStatisticsModel> SqlPlanStatisticsModels { get; set; } 

        public SqlPlanOveriviewModel SqlPlanOverviewModel { get; set; } 

        public List<SqlOptimizationHint> SqlOptimizationHints { get; set; }

        public string SqlStatement  { get; set; }


    }

    

    
   
}
