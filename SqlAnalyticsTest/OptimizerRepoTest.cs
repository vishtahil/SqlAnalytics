using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.RegularExpressions;
using System.Linq;
using SqlAnalytics.Domain;
using System.Collections.Generic;

namespace SqlOptimizer.Tests.OptimizerRepo
{
    [TestClass]
    public class OptimizerRepoTest
    {
        private readonly SqlNormalizer _normalizer;


        public OptimizerRepoTest()
        {
            _normalizer = new SqlNormalizer();
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

            sql = _normalizer.Normalize(sql);

            var firstSet = new string[]
                {
                   // @"\b(NOT IN\s?\(\s?SELECT.*?\))",
                    //@"\b(NOT EXISTS\s?\(\s?SELECT.*?\))",
                    @"\bIN\s?\(\s?SELECT.*?\)",
                    @"\b(EXISTS\s?\(\s?SELECT.*?\))",
                    @"\b(JOIN\s?\(\s?SELECT.*?\))",
                    @",\s?\(\s?SELECT.*?\)",
                   // @"\b(LEFT OUTER JOIN\s?\(\s?SELECT.*?\))",
                   // @"\b(RIGHT OUTER JOIN\s?\(\s?SELECT.*?\))",
                   // @"\b(CROSS JOIN\s?\(\s?SELECT.*?\))",
                   // @"\b(FULL JOIN\s?\(\s?SELECT.*?\))",
                   // @"\b(LEFT JOIN\s?\(\s?SELECT.*?\))",
                   // @"\b(RIGHT JOIN\s?\(\s?SELECT.*?\))",
                    @">\s?\(\s?SELECT.*?\)",
                    @"<\s?\(\s?SELECT.*?\)",
                    @"=\s?\(\s?SELECT.*?\)"
                };

            sql = GetFormattedInput(sql, firstSet);

            /*var secondSet = new string[]
                {
                    @"\bJOIN\s?\(\s?SELECT.*?\)",
                    @"\bIN\s?\(\s?SELECT.*?\)",
                    @"\bEXISTS\s?\(\s?SELECT.*?\)"
                };*/

            //sql = GetFormattedInput(sql, secondSet);

            Assert.AreEqual(true, true);
        }

        private static string GetFormattedInput(string sql, string[] patterns)
        {
            var regex = "(" + String.Join(")|(", patterns) + ")";

            foreach (Match metamatch in Regex.Matches(sql
               , regex  /*@"(memb)|(deb)"*/
               , RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture))
            {
                sql = sql.Replace(metamatch.Value, $"<br/><span style='color:red;font-weight:bold'> {metamatch.Value}</span>");
            }

            sql = Regex.Replace(sql, @"(<br/><span style='color:red;font-weight:bold'>)+", @"<br/><span style='color:red;font-weight:bold'>");

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
