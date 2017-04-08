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
                {@"\b(right\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.RIGHT_OUTER_JOIN.ToString() },
                {@"\b(left\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.LEFT_OUTER_JOIN.ToString() },
                {@"\b(full\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.FULL_OUTER_JOIN.ToString() },
                {@",\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_SELECT.ToString() },
                {@">\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_GREATER_THAN.ToString() },
                {@"<\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_LESS_THAN.ToString() },
                {@"=\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_EQUAL_TO.ToString() },
                {@"\b(FROM\s*[\r\n]*\(\s*[\r\n]*SELECT[^)]*\))",SqlClause.NESTED_SELECT_FROM.ToString() },
                {@"select\s*[\r\n]*[a-zA-Z0-9,\.\s]*\*",SqlClause.BLOCK_SELECT.ToString() },
                {@"order\s+[\r\n]*by",SqlClause.ORDER_BY.ToString() },
                {@"like\s*[\r\n]*\'%",SqlClause.LIKE_BEGIN.ToString() },
                {@"where[^=<>\)]*\(|and[^=<>\)]*\(",SqlClause.WHERE_FUNCTION_PRECEEDING.ToString() },
            };

        public static Dictionary<string, string> OperatorPatternSet = new Dictionary<string, string>()
            {
                {SqlClause.NESTED_IN.ToString() ,"Contains Nested Sub Query with IN clause."},
                {SqlClause.NESTED_EXISTS.ToString(),"Contains Nested Sub Query  with EXISTS clause." },
                {SqlClause.CROSS_JOIN.ToString(),"Contains CROSS JOIN Operator" },
                {SqlClause.LEFT_JOIN.ToString(),"Contains Nested Sub Query  with LEFT JOIN clause." },
                {SqlClause.INNER_JOIN.ToString(),"Contains Nested Sub Query  with INNER JOIN clause."},
                {SqlClause.NESTED_JOIN.ToString(),"Contains Nested Sub Query swith JOIN clause."},
                {SqlClause.FULL_JOIN.ToString(),"Contains Nested Sub Query  with FULL JOIN clause."},
                {SqlClause.RIGHT_JOIN.ToString(),"Contains Nested Sub Query  with RIGHT JOIN clause."},
                {SqlClause.LEFT_OUTER_JOIN.ToString(),"Contains Sub Query  Select with LEFT OUTER JOIN clause."},
                {SqlClause.FULL_OUTER_JOIN.ToString(),"Contains Sub Query  Select with FULL OUTER JOIN clause."},
                {SqlClause.NESTED_SELECT.ToString(),"Contains Nested Sub Query  Statement." },
                {SqlClause.NESTED_GREATER_THAN.ToString(),"Contains Nested Sub Query  with GREATER THAN Clause" },
                {SqlClause.NESTED_LESS_THAN.ToString(),"Contains Nested Sub Query  with LESS THAN Clause" },
                {SqlClause.NESTED_EQUAL_TO.ToString(),"Contains Nested Sub Query  with EQUAL TO Clause" },
                {SqlClause.NESTED_SELECT_FROM.ToString(),"Contains Nested Sub Query with SELECT Clause" },
                { SqlClause.BLOCK_SELECT.ToString(),"Contains SELECT * statement" },
                { SqlClause.ORDER_BY.ToString(),"Contains ORDER BY Clause" },
                { SqlClause.LIKE_BEGIN.ToString(),"Contains LIKE expression with leading wildcards" },
                { SqlClause.WHERE_FUNCTION_PRECEEDING.ToString(),"Conatains function on table column after where  where clause" },
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
                    //store all the matched values
                    foreach(Match match in matches)
                    {
                        sqlOptimizationHints.Add(new SqlOptimizationHint()
                        {
                            MatchedExpression = pattern,
                            MatchedSqlClause = sqlPatterns[pattern],
                            MatchedSqlText = OperatorPatternSet[sqlPatterns[pattern]]

                        });
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
                        MatchedSqlClause = SqlClause.NESTED_JOIN.ToString(),
                        MatchedSqlText= OperatorPatternSet[SqlClause.NESTED_JOIN.ToString()]
                    });
                }
            }

            return sqlOptimizationHints;
        }

    }


}
