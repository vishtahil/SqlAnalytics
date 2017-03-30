using SqlAnalyticsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlAnalyticsDomain.Domain
{
    /// <summary>
    /// http://stackoverflow.com/questions/9436381/c-sharp-regex-string-extraction
    /// </summary>
    public class SqlStatsParser
    {
        private const string _ioStatsPattern = @"^Table\s'(?<TableName>[\w\s]+)'.*logical reads\s(?<LogicalReads>\d*\.?\d*),\sphysical.*\blob logical reads\s(?<LobLogicalReads>\d*\.?\d*)";
        private const string _cpuTimePattern = @"CPU time\s=\s(?<CPUTime>\d*\.?\d*).*elapsed time\s=\s(?<ElapsedTime>\d*\.?\d*)";

        private Regex _ioStatsRegex = null;
        private Regex _cpuStatsRegex = null;

        public SqlStatsParser()
        {
            _ioStatsRegex = new Regex(_ioStatsPattern);
            _cpuStatsRegex = new Regex(_cpuTimePattern);
        }

        /// <summary>
        /// parse sql statss
        /// </summary>
        /// <returns></returns>
        public string InjectSqlStats(string dynamicSql)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("SET STATISTICS TIME ON;");
            stringBuilder.AppendLine("SET STATISTICS IO ON;");
            stringBuilder.Append(dynamicSql);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// parse sql statss
        /// </summary>
        /// <returns></returns>
        public SqlPlanOveriviewModel ParseSqlOverviewStats(SqlPlanOveriviewModel sqlPlanOveriviewModel)
        {
            SetCPUStats(sqlPlanOveriviewModel);
            SetIOStats(sqlPlanOveriviewModel);
            return sqlPlanOveriviewModel;
        }

        /// <summary>
        /// Set IO Statistics
        /// </summary>
        /// <param name="sqlMessages"></param>
        private void SetIOStats(SqlPlanOveriviewModel sqlPlanOveriviewModel)
        {
            var sqlMessages = sqlPlanOveriviewModel.SqlOverviewMessages.Where(x => x.Description.Contains("Table")).ToList();
            foreach (var sqlMessage in sqlMessages)
            {
                var match = _ioStatsRegex.Match(sqlMessage.Description);
                sqlMessage.TableName = match.Groups["TableName"].Value;
                sqlMessage.LogicalReads = Convert.ToDecimal(match.Groups["LogicalReads"].Value);
                sqlMessage.LobLogicalReads = Convert.ToDecimal(match.Groups["LobLogicalReads"].Value);
            }
            sqlPlanOveriviewModel.SqlOverviewMessages = sqlMessages.OrderByDescending(x=>x.LogicalReads).ThenByDescending(x=>x.LogicalReads).ToList();
            sqlPlanOveriviewModel.TotalLogicReads = sqlMessages.Sum(x => x.LogicalReads);
        }

        /// <summary>
        /// Set CPU Statistics
        /// </summary>
        /// <param name="sqlMessages"></param>
        /// <returns></returns>
        private void SetCPUStats(SqlPlanOveriviewModel sqlPlanOveriviewModel)
        {
             var sqlMessages = sqlPlanOveriviewModel.SqlOverviewMessages.Where(x => x.Description.Contains("CPU time")).ToList();
             foreach (var sqlMessage in sqlMessages)
            {
                var cpuMatch = _cpuStatsRegex.Match(sqlMessage.Description);
                var elapsedTime = Convert.ToDecimal(cpuMatch.Groups["ElapsedTime"].Value);
                if (elapsedTime == 0) continue;
                sqlPlanOveriviewModel.TotalElapsedTime = elapsedTime;
                sqlPlanOveriviewModel.TotalCpuTime = Convert.ToDecimal(cpuMatch.Groups["CPUTime"].Value);
            }
        }
    }
}
