using Newtonsoft.Json;
using SqlAnalyticsManager.Models;
using System;
using System.Collections.Concurrent;
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
            sqlPlanModel.Warnings = GetSqlWarnings(xmldoc, nameSpaceManager);
           
            sqlPlanModel.SqlPlanStats = new List<SqlPlanStats>();

            int counter = 1;
            foreach (XmlNode node in nodeList)
            {
                //get parent node
                populatePlannStats(node, sqlPlanModel, counter, nameSpaceManager);
                counter++;
            }

            //calculate node cost and percentages
            calculateTotalNodeCost(sqlPlanModel);
            //order by descending
            sqlPlanModel.SqlPlanStats = sortExecutionPlanStats(sqlPlanModel);
            return sqlPlanModel;
        }

        private List<SqlPlanStats> sortExecutionPlanStats(SqlPlanStatisticsModel sqlPlanModel)
        {
            return sqlPlanModel.SqlPlanStats.OrderByDescending(x => x.TotalNodeCost)
                                .ThenByDescending(x => x.EstimateRows)
                                .ThenByDescending(x => x.EstimateCPU)
                                .ThenByDescending(x => x.EstimateIO).ToList();
        }
        /// <summary>
        /// https://dba.stackexchange.com/questions/143548/estimated-operator-cost-calculation
        /// </summary>
        /// <param name="sqlPlanModel"></param>
        private void calculateTotalNodeCost(SqlPlanStatisticsModel sqlPlanModel)
        {
            decimal total = 0;
            
            foreach (var planStats in sqlPlanModel.SqlPlanStats)///sql plan stats
            {
                var totalCost = planStats.EstimateTotalSubTreeCost - sqlPlanModel.SqlPlanStats
                                                     .Where(x => x.ParentNodeId == planStats.NodeId)
                                                     .Sum(x => x.EstimateTotalSubTreeCost);
                if (totalCost > 0)
                {
                    planStats.TotalNodeCost = totalCost;
                    planStats.TotalNodeCostPercentage =Math.Round(totalCost /sqlPlanModel.StatementSubTreeCost * 100,2);
                    total += planStats.TotalNodeCostPercentage;
                }

            }
        }

        /// <summary>
        /// get sql warnings
        /// http://sqlblogcasts.com/blogs/sqlandthelike/archive/2011/11/29/execution-plan-warnings-the-final-chapter.aspx
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="nameSpaceManager"></param>
        /// <returns></returns>
        private List<KeyValuePair<string,string>> GetSqlWarnings(XmlDocument xmlDoc, XmlNamespaceManager nameSpaceManager)
        {
            var listKvp = new List<KeyValuePair<string, string>>();
            var warningNode = xmlDoc.SelectSingleNode("//ns:Warnings[1]", nameSpaceManager);

            if (warningNode == null)
            {
                return listKvp;
            }

            var unMatchedIndexExists = GetNodeAttributeValue<string>(warningNode, "UnmatchedIndexes");

            //https://sahlean.wordpress.com/2013/12/01/filtered-indexes-and-parameters/*
            if (!string.IsNullOrEmpty(unMatchedIndexExists))
            {
                listKvp.Add(new KeyValuePair<string, string>(Warnings.UNMATCHED_INDEX.ToString(), Warnings.UNMATCHED_INDEX.ToString()));
            }

            foreach(XmlNode  node in warningNode?.ChildNodes)
            {
                //cardinality estimate :https://dba.stackexchange.com/questions/33528/warning-in-query-plan-cardinality-estimate#answer-34331
                if (node.Name == "PlanAffectingConvert")
                {
                    var issue = GetNodeAttributeValue<string>(node, "ConvertIssue");
                    var issueExpression = GetNodeAttributeValue<string>(node, "Expression");
                    listKvp.Add(new KeyValuePair<string, string>(issue, issueExpression));
                }
         
            }
            return listKvp;

        }

        

        /// <summary>
        /// populate plan stats
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentNode"></param>
        /// <param name="planStats"></param>
        private  void populatePlannStats(XmlNode node, SqlPlanStatisticsModel sqlPlanModel, 
            int counter, XmlNamespaceManager nameSpaceManager)
        {
            var planStat = new SqlPlanStats ();
            planStat.DailyQueryID = counter;
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

            planStat.NodeWarning = GetNodeWarningInfo(node, nameSpaceManager, sqlPlanModel.Warnings);

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
            sqlPlanModel.SqlPlanStats.Add(planStat);
                     
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
        /// get warning info
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nameSpaceManager"></param>
        /// <param name="kvpWarnings"></param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetNodeWarningInfo(XmlNode node, XmlNamespaceManager nameSpaceManager,
            List<KeyValuePair<string, string>> kvpWarnings)
        {
            var warningInfo = node.SelectSingleNode("./ns:IndexScan/ns:Predicate/ns:ScalarOperator/@ScalarString", nameSpaceManager)?.Value;
            if (!string.IsNullOrEmpty(warningInfo))
            {
                return kvpWarnings.Where(x => x.Value == warningInfo || x.Value.Contains(warningInfo)).FirstOrDefault();
            }else
            {
                return new KeyValuePair<string, string>();
            }
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
                return (T)Convert.ChangeType(Convert.ToDouble(node.Attributes[name]?.Value), typeof(T));
            }

            return (T)Convert.ChangeType(node.Attributes[name]?.Value, typeof(T));
        }

    }
}
