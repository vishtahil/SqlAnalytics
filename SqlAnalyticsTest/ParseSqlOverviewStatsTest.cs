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
            var overViewModel = new SqlPlanOveriviewModel();

            overViewModel.SqlOverviewMessages = new List<SqlOverviewMessages>()
            {
                new SqlOverviewMessages {  Description=@"Table 'Worktable'. Scan count 0, logical reads 2000, physical reads 10, read-ahead reads 0, lob logical reads 20.23, lob physical reads 0, lob read-ahead reads 0."},
                new SqlOverviewMessages {  Description=@"Table 'Employee'. Scan count 0, logical reads 2000, physical reads 1000, read-ahead reads 0, lob logical reads 21.0, lob physical reads 0, lob read-ahead reads 0."},
                new SqlOverviewMessages {  Description=@"SQL Server Execution Times: CPU time = 1.0 ms,  elapsed time = 14 ms."},
                new SqlOverviewMessages {  Description=@"SQL Server Execution Times: CPU time = 0 ms,  elapsed time = 0 ms."}
            };

            overViewModel = _parser.ParseSqlOverviewStats(overViewModel);
          
            Assert.AreEqual(overViewModel.SqlOverviewMessages[0].LogicalReads == 2000, true);
            Assert.AreEqual(overViewModel.SqlOverviewMessages[0].LobLogicalReads == 20.23m, true);
            Assert.AreEqual(overViewModel.SqlOverviewMessages[0].LogicalReads == 2000, true);
            Assert.AreEqual(overViewModel.SqlOverviewMessages[1].LobLogicalReads == 21.0m, true);
            Assert.AreEqual(overViewModel.SqlOverviewMessages[0].TableName == "Worktable", true);
            Assert.AreEqual(overViewModel.SqlOverviewMessages[1].TableName == "Employee", true);
            Assert.AreEqual(overViewModel.TotalCpuTime != 0, true);
            Assert.AreEqual(overViewModel.TotalElapsedTime != 0, true);
            Assert.AreEqual(overViewModel.TotalLogicReads != 0, true);
        }
    }
}
