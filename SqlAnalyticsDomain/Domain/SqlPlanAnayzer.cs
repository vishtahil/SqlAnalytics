using System;
using System.Collections.Generic;
using System.Text;

namespace SqlAnalytics.Domain
{
    public class SqlPlanAnayzer
    {
        public SqlPlanAnayzer() { }

        /// <summary>
        /// analyze sql
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Analyze(string connectionString, string sql)
        {
            var sqlNormaizer = new SqlNormalizer();
            return sqlNormaizer.Normalize(sql);
        }

    }
}