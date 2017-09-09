using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAnalyticsManager;

namespace SqlAnalyticsTest.IntegrationTests
{
    [TestClass]
    public class SqlIntegrationTests
    {
        private SqlPlanManager _sqlPlanManager;
        private string _testDataLocation = "./TestData";
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2012;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public SqlIntegrationTests()
        {
            _sqlPlanManager = new SqlPlanManager(_connectionString);
        }

        [TestMethod]
        public void Test_SqlPlanSqlMode()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/Convert_Implicit.sql");
            var summaryModel = _sqlPlanManager.GetSqlStatistcisSqlMode(_connectionString, sqlText);
            Assert.AreEqual(summaryModel != null, true);
            Assert.AreEqual(summaryModel.SqlOptimizationHints != null, true);
            Assert.AreEqual(summaryModel.SqlPlanOverviewModel != null, true);
            Assert.AreEqual(summaryModel.SqlPlanStatisticsModels != null, true);

        }

        [TestMethod]
        public void Test_SqlPlanMultipleSqlMode()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/MultipleSqlStatement.sql");
            var summaryModel = _sqlPlanManager.GetSqlStatistcisSqlMode(_connectionString, sqlText);
            Assert.AreEqual(summaryModel != null, true);
            Assert.AreEqual(summaryModel.SqlOptimizationHints != null, true);
            Assert.AreEqual(summaryModel.SqlPlanOverviewModel != null, true);
            Assert.AreEqual(summaryModel.SqlPlanStatisticsModels != null, true);

        }

        [TestMethod]
        public void Test_SqlPlanLintMode()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlQueryWithStats.sql");
            var summaryModel = _sqlPlanManager.GetSqlStatistcisLintMode(sqlText);
            Assert.AreEqual(summaryModel != null, true);
            Assert.AreEqual(summaryModel.SqlOptimizationHints != null, true);
            Assert.AreEqual(summaryModel.SqlPlanOverviewModel == null, true);
            Assert.AreEqual(summaryModel.SqlPlanStatisticsModels == null, true);
        }

        [TestMethod]
        public void Test_SqlExecutionPlanMode()
        {
            string sqlExecutionPlan = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlPlan.xml");
            var summaryModel = _sqlPlanManager.GetSqlStatistcisExecutionPlanMode( sqlExecutionPlan);
            Assert.AreEqual(summaryModel != null, true);
            Assert.AreEqual(summaryModel.SqlOptimizationHints != null, true);
            Assert.AreEqual(summaryModel.SqlPlanOverviewModel == null, true);
            Assert.AreEqual(summaryModel.SqlPlanStatisticsModels != null, true);
        }
    }
}
