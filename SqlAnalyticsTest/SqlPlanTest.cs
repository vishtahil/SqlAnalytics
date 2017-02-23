using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAnalyticsManager.Domain;
using System.Linq;

namespace SqlAnalyticsTest
{
    [TestClass]
    public class SqlPlanTest
    {
        public string testDataLocation = "./TestData/RandomSqlPlan.xml";
        private SqlPlanParser _sqlPlanParser;
        public SqlPlanTest()
        {
            _sqlPlanParser = new SqlPlanParser();
        }

        [TestMethod]
        public void DesrializeSqlPlan()
        {
            string text = System.IO.File.ReadAllText(testDataLocation);
            var plan = _sqlPlanParser.ParseSqlPlan(text);
            Assert.AreEqual(plan != null, true);

            string sqlFromPlan = _sqlPlanParser.GetSqlFromPlan(plan);
            Assert.AreEqual(sqlFromPlan != null, true);
            Assert.AreEqual(sqlFromPlan.GetType().ToString(), "System.String");
        }

        
    }
}
