using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlAnalytics.Repo
{
    public class OptimizerRepo
    {
        public OptimizerRepo()
        {
        }

        public bool IsPrime(int candidate)
        {
            throw new NotImplementedException("Please create a test first");
        }

        /// <summary>
        /// get sql execution plan
        /// </summary>
        /// <param name="dynamicSql"></param>
        /// <returns></returns>
        public string GetSqlExecutionPlan(string connectionString,string dynamicSql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand command = new SqlCommand(dynamicSql, connection))
                {
                    using (var sqlExecutionPanCommand = connection.CreateCommand())
                    {
                        sqlExecutionPanCommand.CommandText = "SET STATISTICS XML ON";
                        sqlExecutionPanCommand.ExecuteNonQuery();
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.NextResult())
                        {
                            if (reader.GetName(0) == "Microsoft SQL Server 2005 XML Showplan")
                            {
                                reader.Read();
                                return reader.GetString(0);
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
