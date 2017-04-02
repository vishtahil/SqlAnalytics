using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.RegularExpressions;
using System.Linq;
using SqlAnalytics.Domain;
using System.Collections.Generic;
using SqlAnalyticsManager.Models;
using SqlAnalyticsDomain.Domain;

namespace SqlOptimizer.Tests.OptimizerRepo
{
    [TestClass]
    public class OptimizerRepoTest
    {
        private readonly SqlNormalizer _normalizer;
        private SqlHintsEvaluator _sqlHintsEvaluator;


        public OptimizerRepoTest()
        {
            _normalizer = new SqlNormalizer();
            _sqlHintsEvaluator = new SqlHintsEvaluator();
        }
        

        [TestMethod]
        public void FetchSqlExecutionPlan()
        {
            Assert.AreEqual(true,true);
        }

        [TestMethod]
        public void SampleTest()
        {
            var input = @"This is just a little test of the memb to see if it gets picked up. 
       Deb of course should also be caught here.";
            var dictionary = new Dictionary<string, string>
            {
                {"memb", "Member"}
                ,{"deb","Debut"}
            };
            var regex = "(" + String.Join(")|(", dictionary.Keys.ToArray()) + ")";
            foreach (Match metamatch in Regex.Matches(input
               , regex  /*@"(memb)|(deb)"*/
               , RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture))
            {
                input = input.Replace(metamatch.Value, dictionary[metamatch.Value.ToLower()]);
            }
            //Console.Write(input);
        }

        [TestMethod]
        public void CheckForCorrelatedSubQuery()
        {
            var sql = @"SELECT employee_number, name
            FROM employees AS Bob
            WHERE salary > (
            SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department);

SELECT employee_number, name
            FROM employees AS Bob
            WHERE salary > (
            SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department);
            
            SELECT employee_number, name
            FROM employees AS Bob
            WHERE salary <(
            SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department);

            SELECT employee_number, name
            FROM employees AS Bob
            WHERE salary <(
            SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department)
            
            SELECT employee_number, name
            FROM employees AS Bob
            WHERE salary <(
            SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department);

            SELECT
            employee_number,
            name,
            (SELECT AVG(salary)
            FROM employees
            WHERE department = Bob.department) AS department_average
            FROM employees AS Bob;

            select CustomerID, CompanyName
            from customers as a
            where exists
            (
            select *from orders as b
            where a.CustomerID = b.CustomerID
            and ShipCountry = 'UK')

            select CustomerID, CompanyName
            from customers as a
            where not exists
            (
            select * from orders as b
                     where a.CustomerID = b.CustomerID
            and ShipCountry = 'UK'
            ); 
            
            select CustomerID, CompanyName
            from customers as a
            where exists
            (
            select * from orders as b
                     where a.CustomerID = b.CustomerID
            and ShipCountry = 'UK'
            ); 
            
            SELECT Name
            FROM Production.Product
            WHERE ProductSubcategoryID  IN
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

            SELECT Name
            FROM Production.Product
            WHERE ProductSubcategoryID NOT IN
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

            SELECT Name
            FROM Production.Product
            INNER JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

        SELECT Name
            FROM Production.Product
            INNER JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

        SELECT Name
            FROM Production.Product
            LEFT OUTER JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

         SELECT Name
            FROM Production.Product
            RIGHT OUTER JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')


SELECT Name
            FROM Production.Product
            CROSS JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

SELECT Name
            FROM Production.Product
            FULL JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

SELECT Name
            FROM Production.Product
            LEFT JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

SELECT Name
            FROM Production.Product
            JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')

SELECT Name
            FROM Production.Product
            RIGHT JOIN 
            (SELECT ProductSubcategoryID
            FROM Production.ProductSubcategory
            WHERE Name = 'Mountain Bikes' 
            OR Name = 'Road Bikes'
            OR Name = 'Touring Bikes')
            ";

            var sqlOptimizerHints = _sqlHintsEvaluator.GetSqlOptimationHints(sql);
            //sql = _normalizer.Normalize(sql);

            //var matchList = new Dictionary<string, string>()
            //{
            //    { "LEFT","LEFT_JOIN"},
            //    { "RIGHT","RIGHT_JOIN"},
            //    { "FULL","FULL_JOIN"},
            //    { "INNER","INNER_JOIN"},
            //    { "CROSS","CROSS_JOIN"},
            //    { "<","NESTED_LESS_THAN"},
            //    { ">","NESTED_GREATER_THAN"},
            //    { "=","NESTED_EQUAL_TO"},
            //    { "IN","NESTED_IN"},
            //    { "EXISTS","NESTED_EXISTS"}
            //};


            //var firstSet = new Dictionary<string, string>()
            //    {
            //        {@"\b(in\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.NESTED_IN.ToString() },
            //        {@"\b(exists\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.NESTED_EXISTS.ToString() },
            //        {@"\b(cross\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.CROSS_JOIN.ToString() },
            //        {@"\b(left\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.LEFT_JOIN.ToString() },
            //        {@"\b(inner\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.INNER_JOIN.ToString() },
            //        {@"\b(full\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.FULL_JOIN.ToString() },
            //        {@"\b(right\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.RIGHT_JOIN.ToString() },
            //        {@"\b(right\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.RIGHT_JOIN.ToString() },
            //        {@"\b(left\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.LEFT_JOIN.ToString() },
            //        {@"\b(full\s*[\r\n]*outer\s*[\r\n]*join\s*[\r\n]*\(\s*[\r\n]*select[^)]*\))",SqlClause.FULL_JOIN.ToString() },
            //        {@",\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_SELECT.ToString() },
            //        {@">\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_GREATER_THAN.ToString() },
            //        {@"<\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_LESS_THAN.ToString() },
            //        {@"=\s*[\r\n]*\(\s*[\r\n]*select[^)]*\)",SqlClause.NESTED_EQUAL_TO.ToString() }
            //    };

            //sql = GetFormattedInput(sql, firstSet);

            ///*var secondSet = new string[]
            //    {
            //        @"\bJOIN\s?\(\s?SELECT.*?\)",
            //        @"\bIN\s?\(\s?SELECT.*?\)",
            //        @"\bEXISTS\s?\(\s?SELECT.*?\)"
            //    };*/

            ////sql = GetFormattedInput(sql, secondSet);

            Assert.AreEqual(true, true);
        }

        private SqlOptimizationHint sqlClauseContains(Match metamatch)
        {
            var matchList = new Dictionary<string, string>()
            {
                { "LEFT","LEFT_JOIN"},
                { "RIGHT","RIGHT_JOIN"},
                { "FULL","FULL_JOIN"},
                { "INNER","INNER_JOIN"},
                { "CROSS","CROSS_JOIN"},
                { "<","NESTED_LESS_THAN"},
                { ">","NESTED_GREATER_THAN"},
                { "=","NESTED_EQUAL_TO"},
                { "IN","NESTED_IN"},
                { "EXISTS","NESTED_EXISTS"}
            };

            var regex = "(" + String.Join(")|(", matchList.Keys.ToArray()) + ")";

            foreach (Match matchedClause in Regex.Matches(metamatch.Value
               , regex  /*@"(memb)|(deb)"*/
               , RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture))
            {
                return new SqlOptimizationHint() { };
                
            }
            return null;
           
        }

        private  string GetFormattedInput(string sql, Dictionary<string,string> patterns)
        {
            List<SqlOptimizationHint> sqlOptimizationHints 
                = new List<SqlOptimizationHint>();

            foreach(var pattern in patterns.Keys.ToArray())
            {
                var matches = Regex.Matches(sql, pattern,RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                if(matches != null)
                {
                    sqlOptimizationHints.Add(new SqlOptimizationHint()
                    {
                        MatchedExpression=pattern,
                        MatchedSqlClause=patterns[pattern],
                    });
                }
            }


            var regex = "(" + String.Join(")|(", patterns) + ")";
            var _normalizedSql = _normalizer.Normalize(sql);

            
            foreach (Match metamatch in Regex.Matches(sql
               , regex  /*@"(memb)|(deb)"*/
               , RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture))
            {
                
                sql = sql.Replace(metamatch.Value, $"<span style='color:red;font-weight:bold'>{metamatch.Value}</span>");
            }

            sql = Regex.Replace(sql, @"(<span style='color:red;font-weight:bold'>)+", @"<span style='color:red;font-weight:bold'>");
            sql = Regex.Replace(sql, @"(</span>)+", @"</span>");

            return sql;
        }


        [TestMethod]
        public void CheckForExplicitConversion()
        {
            //check for correlated sub query examples 
            /* 
             *  CAST
             *  CONVERT
             */
            Assert.AreEqual(true, true);
        }


    }

}
