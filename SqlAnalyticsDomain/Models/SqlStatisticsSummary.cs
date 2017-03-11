using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlStatisticsSummary
    {
        public SqlStatisticsSummary()
        {
            this.SqlPlanStatisticsModel = new List<SqlPlanStatisticsModel>();
            this.SqlPlanOverviewModel = new SqlPlanOveriviewModel();
            this.SqlOptimizationHint= new List<SqlOptimizationHint>();
        }

        public List<SqlPlanStatisticsModel> SqlPlanStatisticsModel { get; set; }

        public SqlPlanOveriviewModel SqlPlanOverviewModel { get; set; }

        public List<SqlOptimizationHint> SqlOptimizationHint { get; set; }
    }

    

    
   
}
