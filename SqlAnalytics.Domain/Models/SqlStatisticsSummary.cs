using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlStatisticsSummary
    {
        public SqlPlanStatisticsModel SqlPlanStatisticsModel { get; set; } 

        public SqlPlanOveriviewModel SqlPlanOverviewModel { get; set; } 

        public List<SqlOptimizationHint> SqlOptimizationHints { get; set; } 
        
    }

    

    
   
}
