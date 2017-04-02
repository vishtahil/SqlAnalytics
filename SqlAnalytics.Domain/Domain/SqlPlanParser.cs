using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string GetSqlFromPlan(ShowPlanXML plan)
        {
            return plan?.BatchSequence?
                    .SelectMany(x => x)?
                     .SelectMany(x => x.Items)?
                     .OfType<StmtSimpleType>()?
                     .Select(x => x.StatementText)?.ToList()?[0];
        }
    }
}
