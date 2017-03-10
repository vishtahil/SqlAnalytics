using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using SqlAnalyticsManager.Models;
using SqlAnalyticsDomain.Domain;

namespace SqlAnalyticsTest
{
    [TestClass]
    public class ParseSqlOverviewStatsTest
    {
        public SqlStatsParser _parser;

        public ParseSqlOverviewStatsTest()
        {
            _parser = new SqlStatsParser();
        }

        [TestMethod]
        public void TestSqlIoStats()
        {
            List<SqlOverviewMessages> messages = new List<SqlOverviewMessages>()
            {
                new SqlOverviewMessages {  Description=@"Table 'Worktable'. Scan count 0, logical reads 2000, physical reads 10, read-ahead reads 0, lob logical reads 20.23, lob physical reads 0, lob read-ahead reads 0."},
                new SqlOverviewMessages {  Description=@"Table 'Employee'. Scan count 0, logical reads 2000, physical reads 1000, read-ahead reads 0, lob logical reads 21.0, lob physical reads 0, lob read-ahead reads 0."}

            };

            messages=_parser.ParseSqlOverviewStats(messages);

            Assert.AreEqual(messages[0].LogicalReads==2000,true);
            Assert.AreEqual(messages[0].LobLogicalReads == 20.23m, true);
            Assert.AreEqual(messages[0].LogicalReads == 2000, true);
            Assert.AreEqual(messages[1].LobLogicalReads == 21.0m, true);
            Assert.AreEqual(messages[0].TableName == "Worktable", true);
            Assert.AreEqual(messages[1].TableName == "Employee", true);

        }
    }
}
