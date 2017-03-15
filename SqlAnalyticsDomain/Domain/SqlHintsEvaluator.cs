using SqlAnalyticsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlAnalyticsDomain.Domain
{
    public class SqlHintsEvaluator
    {
        public static Dictionary<string, string> FirstSqlClausePatternSet = new Dictionary<string, string>()
            {
                {@"\b(in\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.NESTED_IN.ToString() },
                {@"\b(exists\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.NESTED_EXISTS.ToString() },
                {@"\b(cross\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.CROSS_JOIN.ToString() },
                {@"\b(left\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.LEFT_JOIN.ToString() },
                {@"\b(inner\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.INNER_JOIN.ToString() },
                {@"\b(full\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.FULL_JOIN.ToString() },
                {@"\b(right\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.RIGHT_JOIN.ToString() },
                {@"\b(right\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.RIGHT_JOIN.ToString() },
                {@"\b(left\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.LEFT_JOIN.ToString() },
                {@"\b(full\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.FULL_JOIN.ToString() },
                {@",\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_SELECT.ToString() },
                {@">\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_GREATER_THAN.ToString() },
                {@"<\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_LESS_THAN.ToString() },
                {@"=\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_EQUAL_TO.ToString() },
            };

         public static string NESTED_JOIN_PATTERN = @"\b(join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))";
        


        /// <summary>
        /// Public get Sql Optimation Hints
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlPatterns"></param>
        /// <returns></returns>
        public List<SqlOptimizationHint> GetSqlOptimationHints(string sql, Dictionary<string, string> sqlPatterns = null)
        {
            sqlPatterns = sqlPatterns ?? FirstSqlClausePatternSet;
            List<SqlOptimizationHint> sqlOptimizationHints
                = new List<SqlOptimizationHint>();
            List<string> matchedValues = new List<string>();

            //match agaist first set of sql patterns
            foreach (var pattern in sqlPatterns.Keys.ToArray())
            {
                var matches = Regex.Matches(sql, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                if (matches != null)
                {
                    sqlOptimizationHints.Add(new SqlOptimizationHint()
                    {
                        MatchedExpression = pattern,
                        MatchedSqlClause = sqlPatterns[pattern]
                    });
                    
                    //store all the matched values
                    foreach(Match match in matches)
                    {
                        matchedValues.Add(match.Value);
                    }
                }
            }

            //match against second set of sql patterns
            var nestedJoinMatches = Regex.Matches(sql, NESTED_JOIN_PATTERN, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            foreach(Match nestedMatch in nestedJoinMatches)
            {
                bool partialMatch = matchedValues.Any(x => x.Contains(nestedMatch.Value));
                if (!partialMatch)
                {
                    sqlOptimizationHints.Add(new SqlOptimizationHint()
                    {
                        MatchedExpression = NESTED_JOIN_PATTERN,
                        MatchedSqlClause = SqlClause.NESTED_JOIN.ToString()
                    });
                }
            }

            return sqlOptimizationHints;
        }

    }


}
