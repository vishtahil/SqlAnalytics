using SqlAnalytics.Domain;
using SqlAnalytics.Repo;
using SqlAnalyticsDomain.Domain;
using SqlAnalyticsManager.Domain;
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
        private SqlPlanParser _sqlPlanParser;

        public SqlPlanManager(string connectionString):this()
        {
           
        }

        public SqlPlanManager()
        {
            _optimizerRepo = new OptimizerRepo();
            _sqlStatsParser = new SqlStatsParser();
            _sqlNormalizer = new SqlNormalizer();
            _sqlHintsEvaluator = new SqlHintsEvaluator();
            _sqlPlanParser = new SqlPlanParser();
        }

        public SqlStatisticsSummary GetSqlStatistcis(string connectionString, string sql)
        {
            var dynamicSql = _sqlStatsParser.InjectSqlStats(sql);
            var overViewModel = GetSqlOverviewModel(connectionString, dynamicSql);
            var  sqlPlanMpdel = GetSqlPlanStats(connectionString, overViewModel);
            var optimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlPlanOverviewModel = overViewModel,
                SqlPlanStatisticsModel = sqlPlanMpdel,
                SqlOptimizationHints = optimizationHints
            };
        }
        /// <summary>
        /// Get Sql Execution Plan Statistics 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="overViewModel"></param>
        /// <returns></returns>
        private SqlPlanStatisticsModel GetSqlPlanStats(string connectionString, SqlPlanOveriviewModel overViewModel)
        {
            var sqlPlanModel = new SqlPlanStatisticsModel();
            sqlPlanModel = _sqlPlanParser.GetPlanStats(overViewModel.SqlExecutionPlan);
            sqlPlanModel.SqlPlanStats.OrderByDescending(x=>x.TotalNodeCost)
                                        .ThenByDescending(x => x.EstimateRows)
                                        .ThenByDescending(x => x.EstimateCPU)
                                        .ThenByDescending(x => x.EstimateIO).ToList();
            return sqlPlanModel;
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
