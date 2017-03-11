using SqlAnalytics.Repo;
using SqlAnalyticsDomain.Domain;
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
        private SqlStatsParser _sqlStatsParser;

        public SqlPlanManager(string connectionString)
        {
            _optimizerRepo = new OptimizerRepo();
            _sqlStatsParser = new SqlStatsParser();
        }

        public SqlStatisticsSummary GetSqlStatistcis(string connectionString, string sql)
        {
            var overViewModel = _optimizerRepo.GetSqlExecutionPlan(connectionString, sql);
            overViewModel = _sqlStatsParser.ParseSqlOverviewStats(overViewModel);
            var sqlStatistics = _optimizerRepo.GetSqlPlanStatistics(connectionString, overViewModel.SqlExecutionPlan);
            return new SqlStatisticsSummary() { SqlPlanOverviewModel=overViewModel};
        }
    }
}
