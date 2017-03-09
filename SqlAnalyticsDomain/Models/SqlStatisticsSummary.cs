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
            this.SqlPlanOverviewModel = new List<SqlPlanOveriviewModel>();
            this.SqlOptimizationHint= new List<SqlOptimizationHint>();
        }

        public IList<SqlPlanStatisticsModel> SqlPlanStatisticsModel { get; set; }

        public IList<SqlPlanOveriviewModel> SqlPlanOverviewModel { get; set; }

        public IList<SqlOptimizationHint> SqlOptimizationHint { get; set; }
    }

    

    
   
}
