using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlAnalytics.Domain
{
    public class SqlNormalizer
    {
        private static readonly Regex trimmer = new Regex(@"\s\s+");
        public SqlNormalizer()
        {
        }
        
        /// <summary>
        /// Normalize
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Normalize(string sql)
        {
            //remove new line characters
            sql = Regex.Replace(sql, @"\t|\n|\r", "");
            
            //remove extra white spaces
            sql = trimmer.Replace(sql, " ");

            //converting into upper case
            return sql;
        }
    }
}
