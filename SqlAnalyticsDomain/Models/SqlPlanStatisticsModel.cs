﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlPlanStatisticsModel
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
        public long EstimateRows { get; set; }
        public decimal EstimateTotalSubTreeCost { get; set; }
        public string StatementText { get; set; }
        public int NodeId { get; set; }
        public int ParentNodeId { get; set; }
        public decimal TotalNodeCost { get; set; }
    }
}
