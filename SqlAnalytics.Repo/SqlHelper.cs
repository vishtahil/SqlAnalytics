using System;
using System.Collections.Generic;
using System.Text;

namespace SqlAnalytics.Repo
{
   public class SqlHelper
    {
        /// <summary>
        /// check valid sql plan
        /// </summary>
        public static bool CheckValidSqlPlan(string sqlPlan)
        {
            if (sqlPlan.Contains("http://schemas.microsoft.com/sqlserver"))
                return true;
            return false;
        }
    }
}
