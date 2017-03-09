using SqlAnalytics.Repo;
using SqlAnalyticsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager
{
   public  class SqlPlanManager //dependency inversion later on
    {
        private OptimizerRepo _optimizerRepo;
        
        public SqlPlanManager(string connectionString)
        {
            _optimizerRepo = new OptimizerRepo();
        }

        public SqlStatisticsSummary GetSqlStatistcis(string connectionString, string sql)
        {
            var overViewModel = _optimizerRepo.GetSqlExecutionPlan(connectionString, sql);
            var sqlStatistics = _optimizerRepo.GetSqlPlanStatistics(connectionString, overViewModel.SqlExecutionPlan);
            return new SqlStatisticsSummary();
        }
    }
}
