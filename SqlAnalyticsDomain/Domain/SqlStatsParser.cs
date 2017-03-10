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
        private const string _ioStatsPattern = @"^Table\s'(?<TableName>\w+)'.*logical reads\s(?<LogicalReads>\d*\.?\d*),\sphysical.*\blob logical reads\s(?<LobLogicalReads>\d*\.?\d*)";
        
        private const string _cpuTimePattern = "";
        private Regex _ioStatsRegex = null;
        private Regex _cpuStatsRegex = null;

        public  SqlStatsParser()
        {
            _ioStatsRegex = new Regex(_ioStatsPattern);
            _cpuStatsRegex= new Regex(_cpuTimePattern);
        }

        /// <summary>
        /// parse sql statss
        /// </summary>
        /// <returns></returns>
        public List<SqlOverviewMessages> ParseSqlOverviewStats(List<SqlOverviewMessages> sqlMessages)
        {
            foreach( var sqlMessage in sqlMessages)
            {
                var match = _ioStatsRegex.Match(sqlMessage.Description);
                sqlMessage.TableName = match.Groups["TableName"].Value; 
                sqlMessage.LogicalReads =Convert.ToDecimal( match.Groups["LogicalReads"].Value); 
                sqlMessage.LobLogicalReads = Convert.ToDecimal(match.Groups["LobLogicalReads"].Value);
             }

            return sqlMessages;
        }
    }
}
