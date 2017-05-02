using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlPlanStatisticsModel
    {
        public List<SqlPlanStats> SqlPlanStats { get; set; }
        public List<KeyValuePair<string, string>> Warnings { get; set; } = new List<KeyValuePair<string, string>>();
        public decimal StatementSubTreeCost { get; set; }
    }

    public class SqlPlanStats
    {
        public int DailyQueryID { get; set; }
        public DateTime PullDate { get; set; }
        public string PhysicalOperation { get; set; }
        public string LogicalOperation { get; set; }
        public string IndexName { get; set; }
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public decimal EstimateIO { get; set; }
        public decimal EstimateCPU { get; set; }
        public decimal EstimateRows { get; set; }
        public decimal EstimateTotalSubTreeCost { get; set; }
        public string StatementText { get; set; }
        public int NodeId { get; set; }
        public int ParentNodeId { get; set; }
        public decimal TotalNodeCost { get; set; } = 0;
        public decimal TotalNodeCostPercentage { get; set; } = 0;
        public KeyValuePair<string,string> NodeWarning { get; set; }
    }

    public enum Warnings
    {
        CONVERT_IMPLICIT=1,
        UNMATCHED_INDEX=2,


    }
}
