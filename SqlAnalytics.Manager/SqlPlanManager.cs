using SqlAnalytics.Domain;
using SqlAnalytics.Models;
using SqlAnalytics.Repo;
using SqlAnalyticsDomain.Domain;
using SqlAnalyticsManager.Domain;
using SqlAnalyticsManager.Models;
using System;
using System.Collections.Concurrent;
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
            var sqlPlanStatsModels =GetSqlStats(planOverViewModel.SqlExecutionPlans);
            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);

            return new SqlStatisticsSummary()
            {
                SqlPlanOverviewModel = planOverViewModel,
                SqlOptimizationHints = sqlOptimizationHints,
                SqlPlanStatisticsModels = sqlPlanStatsModels
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
            var sqlOptimizationHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);
            var sqlPlanModels = new List<SqlPlanStatisticsModel>();
            sqlPlanModels.Add(sqlPlanStatsModel);
            return new SqlStatisticsSummary()
            {
                SqlOptimizationHints = sqlOptimizationHints,
                SqlPlanStatisticsModels = sqlPlanModels,
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

        private List<SqlPlanStatisticsModel> GetSqlStats(List<string> sqlExecutionPlans)
        {
            var sqlPlanModels = new ConcurrentBag<SqlPlanStatisticsModel>();
            Parallel.ForEach(sqlExecutionPlans, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (sqlPlan) =>
            {
                var sqlPlanModel = _sqlPlanParser.GetPlanStats(sqlPlan);
                sqlPlanModel.SqlFromPlan = _sqlPlanParser.GetSqlFromPlan(sqlPlan);
                sqlPlanModels.Add(sqlPlanModel);
            });
            return sqlPlanModels.ToList();
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
