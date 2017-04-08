**where function clause
SELECT * FROM Members WHERE Phone= @Phone;
SELECT * FROM Members WHERE Phone= dbo.FormatPhone(@Phone);
SELECT * FROM Members WHERE 1=1 and dbo.FormatPhone(Phone)=@Phone;
SELECT [SalesOrderID], [SalesOrderDetailID], [ModifiedDate] FROM [Sales].[SalesOrderDetail] WHERE DATEDIFF(YEAR,ModifiedDate,GETDATE()) < 0;
SELECT EmailAddress 
FROM person.contact 
WHERE left(EmailAddress,2) = 'As'
