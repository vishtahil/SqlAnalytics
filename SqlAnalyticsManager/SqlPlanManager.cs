using SqlAnalytics.Domain;
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
        private SqlNormalizer _sqlNormalizer;
        private SqlHintsEvaluator _sqlHintsEvaluator;

        public SqlPlanManager(string connectionString)
        {
            _optimizerRepo = new OptimizerRepo();
            _sqlStatsParser = new SqlStatsParser();
            _sqlNormalizer = new SqlNormalizer();
            _sqlHintsEvaluator = new SqlHintsEvaluator();
        }

        public SqlStatisticsSummary GetSqlStatistcis(string connectionString, string sql)
        {
            //get information about logical reads and cpu time
            var overViewModel = GetSqlOverviewModel(connectionString, sql);

            //get statistics about sql plan
            var sqlStatistics = _optimizerRepo.GetSqlPlanStatistics(connectionString, overViewModel.SqlExecutionPlan);
            
            //normalize aql
            var normalizedSql = _sqlNormalizer.Normalize(sql);

            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);
            
            return new SqlStatisticsSummary()
            {
                SqlPlanOverviewModel = overViewModel,
                SqlPlanStatisticsModel = sqlStatistics,
                NormalizedSql=normalizedSql
            };
        }

        /// <summary>
        /// Get Logical Reads and CPU Time stats
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        private SqlPlanOveriviewModel GetSqlOverviewModel(string connectionString, string sql)
        {
            var overViewModel = _optimizerRepo.GetSqlExecutionPlan(connectionString, sql);
            overViewModel = _sqlStatsParser.ParseSqlOverviewStats(overViewModel);
            return overViewModel;
        }
    }
}
