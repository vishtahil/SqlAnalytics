using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlAnalytics.Models
{
    public class AnalyticsModel
    {
        public string Base64ConnectionString { get; set; }
        public string Base64Sql { get; set; }

        public string ConnectionString { get; set; }
        public string Sql { get; set; }
    }
}

