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
        private string _connectionString = @"Data Source =(LocalDB)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security = True;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public SqlIntegrationTests()
        {
            _sqlPlanManager = new SqlPlanManager(_connectionString);
        }

        [TestMethod]
        public void SqlPlan()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlQueryWithStats.sql");
            var summaryModel = _sqlPlanManager.GetSqlStatistcis(_connectionString, sqlText);
            Assert.AreEqual(summaryModel != null, true);
        }
    }
}
