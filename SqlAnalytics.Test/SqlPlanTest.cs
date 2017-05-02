using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using SqlAnalyticsManager.Models;
using SqlAnalyticsDomain.Domain;
using SqlAnalyticsManager.Domain;
using SqlAnalytics.Repo;
using System.Linq;

namespace SqlAnalyticsTest
{
    [TestClass]
    public class SqlPlanTest
    {
        public SqlPlanParser _parser;
        private string _testDataLocation = "./TestData";
        private OptimizerRepo _optimizerRepo;
        private string _connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = AdventureWorks2012; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public SqlPlanTest()
        {
            _parser = new SqlPlanParser();
            _optimizerRepo = new OptimizerRepo();
        }

        [TestMethod]
        public void TestSqlPlanWithUnMtachedIndexWarnings()
        {
            //string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlPlan.xml");
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/UnmatchedIndex.xml");
            var sqlXmlPlan = _parser.GetPlanStats(sqlText);

            Assert.AreEqual(sqlXmlPlan.SqlPlanStats?.Count > 0, true);
            Assert.AreEqual(sqlXmlPlan.Warnings?.Any(x => x.Key == Warnings.UNMATCHED_INDEX.ToString()), true);
        }
        [TestMethod]
        public void TestSqlPlanWithWarnings()
        {
            //string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlPlan.xml");
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/Convert_Implicit.xml"); 
            var sqlXmlPlan = _parser.GetPlanStats(sqlText);

            Assert.AreEqual(sqlXmlPlan.SqlPlanStats?.Count > 0, true);
            Assert.AreEqual(sqlXmlPlan.Warnings ?.Count > 0, true);
        }

        [TestMethod]
        public void TestGetSqlStatement()
        {
            string sqlText = System.IO.File.ReadAllText($"{_testDataLocation}/RandomSqlPlan.xml");
            var sql = _parser.GetSqlFromPlan(sqlText);
            Assert.AreEqual(sql.Contains("SELECT"), true);

        }




    }
}
