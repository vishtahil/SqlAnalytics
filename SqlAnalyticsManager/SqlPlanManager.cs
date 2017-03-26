using SqlAnalytics.Domain;
using SqlAnalytics.Repo;
using SqlAnalyticsDomain.Domain;
using SqlAnalyticsManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public SqlPlanManager(string connectionString):this()
        {
           
        }

        public SqlPlanManager()
        {
            _optimizerRepo = new OptimizerRepo();
            _sqlStatsParser = new SqlStatsParser();
            _sqlNormalizer = new SqlNormalizer();
            _sqlHintsEvaluator = new SqlHintsEvaluator();
        }

        public SqlStatisticsSummary GetSqlStatistcis(string connectionString, string sql)
        {
            var dynamicSql = _sqlStatsParser.InjectSqlStats(sql);
            var overViewModel = GetSqlOverviewModel(connectionString, dynamicSql);
            var planStats = _optimizerRepo.GetSqlPlanStatistics(connectionString, overViewModel.SqlExecutionPlan);
            var optimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlPlanOverviewModel = overViewModel,
                SqlPlanStatisticsModel = planStats,
                SqlOptimizationHints = optimizationHints
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
