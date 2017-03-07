using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAnalyticsManager.Domain;
using System.Linq;
using SqlAnalytics.Repo;
using System.Data;

namespace SqlAnalyticsTest
{
    [TestClass]
    public class SqlPlanTest
    {
        private  string _testDataLocation = "./TestData";
        private string _connectionString = @"Data Source =(LocalDB)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security = True;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlPlanParser _sqlPlanParser;
        private OptimizerRepo _optimizerRepo;

        
        public SqlPlanTest()
        {
            _sqlPlanParser = new SqlPlanParser();
            _optimizerRepo = new OptimizerRepo();
        }

        [TestMethod]
        public void GetExecutionPlan()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlQueryWithStats.sql");
            var sqlPlan = _optimizerRepo.GetSqlExecutionPlan(_connectionString, sqlText);
            Assert.AreEqual(!string.IsNullOrEmpty(sqlPlan),true);
            Assert.AreEqual(sqlPlan.Contains("http://schemas.microsoft.com/sqlserver/2004/07/showplan"), true);
        }

        [TestMethod]
        public void GetExecutionPlanStatistics()
        {
            string sqlPlan = System.IO.File.ReadAllText($"{ _testDataLocation}/RandomSqlPlan.xml");
            var dataTable = _optimizerRepo.GetSqlPlanStatistics(_connectionString, sqlPlan);
            Assert.AreEqual(dataTable != null, true);
        }

        [TestMethod]
        public void GetExecutionPlanMessage()
        {
            string sqlPlan = System.IO.File.ReadAllText($"{ _testDataLocation}/RandomSqlQueryWithStats.sql");
           var a = _optimizerRepo.GetSqlServerMessages(_connectionString, sqlPlan);
            //Assert.AreEqual(dataTable != null, true);
        }

        [TestMethod]
        public void DesrializeSqlPlan()
        {
            string text = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlPlan.xml");
            var plan = _sqlPlanParser.ParseSqlPlan(text);
            Assert.AreEqual(plan != null, true);

            string sqlFromPlan = _sqlPlanParser.GetSqlFromPlan(plan);
            Assert.AreEqual(sqlFromPlan != null, true);
            Assert.AreEqual(sqlFromPlan.GetType().ToString(), "System.String");
        }

        
    }
}
