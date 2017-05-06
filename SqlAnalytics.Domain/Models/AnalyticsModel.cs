using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlAnalytics.Models
{
    public class AnalyticsModel
    {
        public string Base64ConnectionString { get; set; } = string.Empty;
        public string Base64Sql { get; set; } = string.Empty;
        public string Base64ExecutionPlan { get; set; } = string.Empty;
        public SqlAnalyticsMode SqlMode { get; set; } = SqlAnalyticsMode.SQL_MODE;
    }

    public enum SqlAnalyticsMode
    {
        SQL_MODE = 1,
        SQL_EXECUTION_PLAN_MODE = 2,
        SQL_LINT_MODE = 3
    }
}

