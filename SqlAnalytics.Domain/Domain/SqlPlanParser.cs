using Newtonsoft.Json;
using SqlAnalyticsManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SqlAnalyticsManager.Domain
{
    public class SqlPlanParser
    {
        public  ShowPlanXML ParseSqlPlan(string sqlPlan)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ShowPlanXML));
            var plan = (ShowPlanXML)ser.Deserialize(new StringReader(sqlPlan));
            return plan;
        }
        
        /// <summary>
        /// get sql from Sql Plan
        /// </summary>
        /// <param name="sqlPlan"></param>
        /// <returns></returns>
        public string GetSqlFromPlan(string sqlPlan)
        {
            var sqlXmlPlan = ParseSqlPlan(sqlPlan);
            return sqlXmlPlan?.BatchSequence?
                    .SelectMany(x => x)?
                     .SelectMany(x => x.Items)?
                     .OfType<StmtSimpleType>()?
                     .Select(x => x.StatementText)?.ToList()?[0]; ;
        }

        /// <summary>
        /// Get plan Statistics
        /// </summary>
        /// <param name="sqlPlan"></param>
        /// <returns></returns>
        public List<SqlPlanStatisticsModel> GetPlanStats(string sqlPlan)
        {
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(sqlPlan);

            XmlNamespaceManager nameSpaceManager = new XmlNamespaceManager(xmldoc.NameTable);
            nameSpaceManager.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/2004/07/showplan");
            var nodeList = xmldoc.SelectNodes("//ns:RelOp", nameSpaceManager);




            var sqlXmlPlan = ParseSqlPlan(sqlPlan);
            var relOp = ((StmtSimpleType)sqlXmlPlan?.BatchSequence?[0]?[0]?.Items?[0])?
                .QueryPlan?.RelOp;
            var sqlPlanList = new List<SqlPlanStatisticsModel>();
            parseStatistics(relOp,sqlPlanList);
            var result = JsonConvert.SerializeObject(sqlXmlPlan );

            return new List<SqlPlanStatisticsModel>();
        }

        /// <summary>
        /// parse statistics 
        /// </summary>
        /// <param name="relOp"></param>
        private  void parseStatistics(RelOpType relOp,  List<SqlPlanStatisticsModel> sqlPlanList)
        {
            while (relOp?.Item != null)
            {
                var relop =((SortType) relOp.Item)?.RelOp;
                if (relop != null)
                {
                   var sqlPlanStats = new SqlPlanStatisticsModel();
                    sqlPlanStats.EstimateCPU = relop.EstimateCPU;
                    sqlPlanStats.EstimateIO= relop.EstimateIO;
                    sqlPlanStats.EstimateRows = relop.EstimateRows;
                    sqlPlanStats.EstimateTotalSubTreeCost = relop.EstimatedTotalSubtreeCost;
                    sqlPlanStats.LogicalOperation= relop.LogicalOp.ToString();
                    sqlPlanStats.PhysicalOperation = relop.PhysicalOp.ToString();
                    sqlPlanStats.NodeId = relop.NodeId;
                    //relop.tabl

                }
               
            }
        }
    }
}
