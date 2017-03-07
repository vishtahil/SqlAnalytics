SET STATISTICS TIME ON;
SET STATISTICS IO ON;
SELECT CrossProd.ProductID, 
 CrossProd.EmployeeID, 

 ISNULL(UnitsSold, 0) as Unitsold, isnull(TotalSales, $0) as TotalSales
 FROM (SELECT Employees.EmployeeID, Products.ProductID 
 FROM Employees, Products) AS CrossProd 
 LEFT JOIN ( 
 SELECT ProductID, EmployeeID, 
 SUM(Quantity) AS UnitsSold, 
 SUM(Quantity * UnitPrice) AS TotalSales 
 FROM Orders 
 JOIN [Order Details]
 ON Orders.OrderID = [Order Details].OrderID 
 
 GROUP BY ProductID, EmployeeID ) AS AnnualSales 
 ON CrossProd.EmployeeID = AnnualSales.EmployeeID 
 AND CrossProd.ProductID = AnnualSales.ProductID 
 UNION 
SELECT 0 AS ProductID, Employees.EmployeeID, 
 
 CAST(isnull(SUM(Quantity),0) as numeric(12)) AS UnitsSold, 
 isnull(SUM(Quantity * UnitPrice), $0) AS TotalSales 
 FROM Orders 
 JOIN [Order Details] 
 ON Orders.OrderID = [Order Details].OrderID 
  
 RIGHT JOIN Employees 
 ON Orders.EmployeeID = Employees.EmployeeID 
 GROUP BY Employees.EmployeeID 
ORDER BY 2, 1;