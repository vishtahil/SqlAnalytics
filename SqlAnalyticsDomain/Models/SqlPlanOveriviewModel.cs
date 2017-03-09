using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlPlanOveriviewModel
    {
        public IList<SqlOverviewMessages> SqlOverviewMessages { get; set; }

        public string SqlExecutionPlan { get; set; }
    }

    public class SqlOverviewMessages
    {
        public string Description { get; set; }
        public decimal ToalView { get; set; }
    }
}
