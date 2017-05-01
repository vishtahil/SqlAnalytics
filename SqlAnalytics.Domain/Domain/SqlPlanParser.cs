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
        public ShowPlanXML ParseSqlPlan(string sqlPlan)
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
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(sqlPlan);

            XmlNamespaceManager nameSpaceManager = new XmlNamespaceManager(xmldoc.NameTable);
            nameSpaceManager.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/2004/07/showplan");
            var statementNode = xmldoc.SelectSingleNode("//ns:StmtSimple[1]", nameSpaceManager);
            return GetNodeAttributeValue<string>(statementNode, "StatementText");
        }

        /// <summary>
        /// get statement subtree cost
        /// </summary>
        /// <param name="sqlPlan"></param>
        /// <returns></returns>
        public decimal GetStatemtentSubTreeCost(XmlDocument xmlDoc, XmlNamespaceManager nameSpaceManager)
        {
            var statementNode = xmlDoc.SelectSingleNode("//ns:StmtSimple[1]", nameSpaceManager);
            return GetNodeAttributeValue<decimal>(statementNode, "StatementSubTreeCost");
        }
        

        /// <summary>
        /// Get plan Statistics
        /// </summary>
        /// <param name="sqlPlan"></param>
        /// <returns></returns>
        public SqlPlanStatisticsModel GetPlanStats(string sqlPlan)
        {
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(sqlPlan);

            XmlNamespaceManager nameSpaceManager = new XmlNamespaceManager(xmldoc.NameTable);
            nameSpaceManager.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/2004/07/showplan");
            var nodeList = xmldoc.SelectNodes("//ns:RelOp", nameSpaceManager);
            var sqlPlanModel = new SqlPlanStatisticsModel();
            sqlPlanModel.StatementSubTreeCost = GetStatemtentSubTreeCost(xmldoc, nameSpaceManager);

            sqlPlanModel.SqlPlanStats = new List<SqlPlanStats>();

            foreach (XmlNode node in nodeList)
            {
                int counter = 1;

                //get parent node
                populatePlannStats(node, sqlPlanModel.SqlPlanStats, counter, nameSpaceManager);
                counter++;
            }

            //calculate node cost and percentages
            calculateTotalNodeCost(sqlPlanModel);
            var jsonString= JsonConvert.SerializeObject(sqlPlanModel.SqlPlanStats);

            return sqlPlanModel;

        }

        /// <summary>
        /// https://dba.stackexchange.com/questions/143548/estimated-operator-cost-calculation
        /// </summary>
        /// <param name="sqlPlanModel"></param>
        private void calculateTotalNodeCost(SqlPlanStatisticsModel sqlPlanModel)
        {
            decimal total = 0;
            ///sql plan stats
            foreach (var planStats in sqlPlanModel.SqlPlanStats)
            {
                var totalCost = planStats.EstimateTotalSubTreeCost - sqlPlanModel.SqlPlanStats
                                                     .Where(x => x.ParentNodeId == planStats.NodeId)
                                                     .Sum(x => x.EstimateTotalSubTreeCost);
                if (totalCost > 0)
                {
                    planStats.TotalNodeCost = totalCost;
                    planStats.TotalNodeCostPercentage =Math.Round( totalCost /sqlPlanModel.StatementSubTreeCost * 100,2);
                    total += planStats.TotalNodeCostPercentage;
                }

            }
        }


        /// <summary>
        /// populate plan stats
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentNode"></param>
        /// <param name="planStats"></param>
        private  void populatePlannStats(XmlNode node, List<SqlPlanStats> planStats, int counter, XmlNamespaceManager nameSpaceManager)
        {
            var planStat = new SqlPlanStats ();
            planStat.EstimateCPU = GetNodeAttributeValue<decimal>(node, "EstimateCPU");
            planStat.EstimateIO = GetNodeAttributeValue<decimal>(node, "EstimateIO");
            planStat.EstimateRows = GetNodeAttributeValue<decimal>(node, "EstimateRows");
            planStat.EstimateTotalSubTreeCost = GetNodeAttributeValue<decimal>(node, "EstimatedTotalSubtreeCost");
            planStat.PhysicalOperation = GetNodeAttributeValue<string>(node, "PhysicalOp");
            planStat.LogicalOperation = GetNodeAttributeValue<string>(node, "LogicalOp");
            planStat.NodeId = GetNodeAttributeValue<int>(node, "NodeId");

            var parentNode = getParentRelNode(node);
          

            if (parentNode != null)
            {
                planStat.ParentNodeId = GetNodeAttributeValue<int>(parentNode, "NodeId");
            }

            //get index node
            var indexNode = getIndexInfo(node, nameSpaceManager);

            if (indexNode != null)
            {
                planStat.DatabaseName = GetNodeAttributeValue<string>(indexNode, "Database");
                planStat.SchemaName = GetNodeAttributeValue<string>(indexNode, "Schema");
                planStat.TableName = GetNodeAttributeValue<string>(indexNode, "Table");
                planStat.IndexName = GetNodeAttributeValue<string>(indexNode, "Index");
                planStat.DailyQueryID = counter;
            }
            planStats.Add(planStat);

           
                        
                                 
        }
        
        
        

        /// <summary>
        /// get parent rel node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private XmlNode getParentRelNode(XmlNode node)
        {
            var parentNode = node.SelectSingleNode("../..");
            if (parentNode?.LocalName == "RelOp")
            {
                return parentNode;
            }
            return null;
        }

        /// <summary>
        /// get parent rel node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private XmlNode getIndexInfo(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            return node.SelectSingleNode("./ns:IndexScan/ns:Object", nameSpaceManager);
        }

        /// <summary>
        /// get attribute value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static T GetNodeAttributeValue<T>(XmlNode node, string name)
        {
            if (typeof(T).FullName == "System.Decimal")
            {
                return (T)Convert.ChangeType(Convert.ToDouble(node.Attributes[name].Value), typeof(T));
            }

            return (T)Convert.ChangeType(node.Attributes[name].Value, typeof(T));
        }

    }
}
