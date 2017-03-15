using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlPlanOveriviewModel
    {

        public List<SqlOverviewMessages> SqlOverviewMessages { get; set; } = new List<Models.SqlOverviewMessages>();

        public string SqlExecutionPlan { get; set; }

        public decimal TotalCpuTime { get; set; }

        public decimal TotalElapsedTime { get; set; }

        public decimal TotalLogicReads { get; set; } 
    }

    public class SqlOverviewMessages
    {
        [JsonIgnore]
        public string Description { get; set; }

        [JsonIgnore]
        public decimal ToalView { get; set; }

        public string TableName { get; set; }

        public decimal LogicalReads { get; set; }

        public decimal LobLogicalReads { get; set; }
    }
}
