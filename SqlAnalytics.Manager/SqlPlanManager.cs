using SqlAnalytics.Domain;
using SqlAnalytics.Models;
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

        /// <summary>
        /// get sql statistics execution plan mode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SqlStatisticsSummary GetSqlStatistcisSqlMode(string connectionString, string sql)
        {
            var dynamicSql = _sqlStatsParser.InjectSqlStats(sql);
            var planOverViewModel = GetSqlOverviewModel(connectionString, dynamicSql);
            var sqlPlanStatsModel = _sqlPlanParser.GetPlanStats(planOverViewModel.SqlExecutionPlan);
            sqlPlanStatsModel.SqlPlanStats = sortExecutionPlanStats(sqlPlanStatsModel);
            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlPlanOverviewModel = planOverViewModel,
                SqlOptimizationHints = sqlOptimizationHints,
                SqlPlanStatisticsModel = sqlPlanStatsModel
            };
        }

       public bool IsCorrelatedSqlQueryCodes(string code)
        {
           return _sqlHintsEvaluator.IsCorrelatedSqlQueryCodes(code);
        }

        /// <summary>
        /// get sql statistics execution plan mode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SqlStatisticsSummary GetSqlStatistcisExecutionPlanMode(string executionPlan)
        {
            var sql = _sqlPlanParser.GetSqlFromPlan(executionPlan);
            var sqlPlanStatsModel = _sqlPlanParser.GetPlanStats(executionPlan);
            sqlPlanStatsModel.SqlPlanStats = sortExecutionPlanStats(sqlPlanStatsModel);
            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlOptimizationHints = sqlOptimizationHints,
                SqlPlanStatisticsModel = sqlPlanStatsModel,
                SqlStatement = sql
            };
        }

        /// <summary>
        /// get sql statistics lint mode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SqlStatisticsSummary GetSqlStatistcisLintMode(string sql)
        {
            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlOptimizationHints = sqlOptimizationHints
            };
        }
        private SqlPlanStatisticsModel GetSqlStats(string sqlExecutionPlan)
        {
            SqlPlanStatisticsModel sqlPlanModel = _sqlPlanParser.GetPlanStats(sqlExecutionPlan);
            sqlPlanModel.SqlPlanStats = sortExecutionPlanStats(sqlPlanModel);
            return sqlPlanModel;
        }

        private  List<SqlPlanStats> sortExecutionPlanStats(SqlPlanStatisticsModel sqlPlanModel)
        {
            return sqlPlanModel.SqlPlanStats.OrderByDescending(x => x.TotalNodeCost)
                                .ThenByDescending(x => x.EstimateRows)
                                .ThenByDescending(x => x.EstimateCPU)
                                .ThenByDescending(x => x.EstimateIO).ToList();
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
