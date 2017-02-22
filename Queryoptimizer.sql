
DECLARE @xmlDocument XML;
DECLARE @Segment VARCHAR(MAX);
DECLARE @queryText VARCHAR(MAX);
DECLARE @AverageLogicalReads DECIMAL(14,2);
DECLARE @MinLogicalReads BIGINT;
DECLARE @MaxLogicalReads BIGINT;
DECLARE @AverageWorkerTime DECIMAL(14,2);
DECLARE @MinWorkerTime DECIMAL(14,2);
DECLARE @MaxWorkerTime DECIMAL(14,2);
DECLARE @ExecutionCount BIGINT;
DECLARE @CachedTime DATETIME;
DECLARE @counter INT = 1;
DECLARE @parentCounter INT = 1;
DECLARE @listSize INT;
DECLARE @segmentSize INT;
DECLARE @StartDate DATE = CAST(GETDATE()-1 AS DATE);
DECLARE @EndDate DATE = CAST(GETDATE() AS DATE);



/* PULL QUERY PLAN DATA */
SELECT ROW_NUMBER() OVER(ORDER BY p.cached_time) AS rowNum
               , t.*
               , p.query_plan
               , p.execution_count
               , p.total_logical_reads / p.execution_count AS averageLogicalReads
               , p.min_logical_reads
               , p.max_logical_reads
               , ROUND((CAST(p.total_worker_time AS MONEY) / p.execution_count) / 1000000, 2) AS averageWorkerTimeSeconds
               , ROUND(CAST(p.min_worker_time AS MONEY) / 1000000 , 2) AS min_worker_time
               , ROUND(CAST(p.max_worker_time AS MONEY) / 1000000 , 2) AS max_worker_time
INTO #QueryList
FROM ---table--
JOIN #Temp t
               ON t.text = p.text
               AND t.maxCachedTime = p.cached_time
WHERE ((p.total_worker_time / p.execution_count) > 15000000
               OR p.max_logical_reads > 1000000)

               
SELECT @listSize = COUNT(*)
FROM #QueryList q

CREATE TABLE #Segments (
               rowNum INT IDENTITY(1,1)
               , xmlSegment XML
               , statementText varchar(MAX)
               , childStatementText VARCHAR(MAX)
               , ExecutionCount BIGINT
               , AverageLogicalReads DECIMAL(14,2)
               , MinLogicalReads BIGINT
               , MaxLogicalReads BIGINT
               , AverageWorkerTime DECIMAL(14,2)
               , MinWorkerTime DECIMAL(14,2)
               , MaxWorkerTime DECIMAL(14,2)
               , CachedTime DATETIME
               , QueryCounter INT
)

/* ITERATE THROUGH THE TOP LEVEL OF EACH QUERY TO GET LIST OF EACH SUB-SEGMENT FROM WITHIN THEM */
WHILE (@counter <= @listSize)
BEGIN
               SELECT @xmlDocument = q.query_plan
                              , @queryText = q.text
                              , @AverageLogicalReads = q.averageLogicalReads
                              , @CachedTime = q.maxCachedTime
                              , @MinLogicalReads = q.min_logical_reads
                              , @MaxLogicalReads = q.max_logical_reads
                              , @ExecutionCount = q.execution_count
                              , @AverageWorkerTime = q.averageWorkerTimeSeconds
                              , @MinWorkerTime = q.min_worker_time
                              , @MaxWorkerTime = q.max_worker_time
               FROM #QueryList q
               WHERE q.rowNum = @counter

               ;WITH xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/sqlserver/2004/07/showplan')
               INSERT INTO #Segments
                       ( xmlSegment ,
                         statementText ,
                                               childStatementText ,
                         ExecutionCount ,
                         AverageLogicalReads ,
                        MinLogicalReads ,
                         MaxLogicalReads ,
                                               AverageWorkerTime ,
                                               MinWorkerTime ,
                                               MaxWorkerTime ,
                         CachedTime ,
                         QueryCounter
                       )
               SELECT x2.x.query('.') AS xmlSegment
                              , x.value('(@StatementText)[1]', 'varchar(max)') AS statementText
                              , NULL AS childStatementText
                              , @ExecutionCount AS ExecutionCount
                              , @AverageLogicalReads AS AverageLogicalReads
                              , @MinLogicalReads AS MinLogicalReads
                              , @MaxLogicalReads AS MaxLogicalReads
                              , @AverageWorkerTime AS AverageWorkerTime
                              , @MinWorkerTime AS MinWorkerTime
                              , @MaxWorkerTime AS MaxWorkerTime
                              , @CachedTime AS CachedTime
                              , @counter AS QueryCounter
               FROM @xmlDocument.nodes('//StmtSimple') x2(x)

               SET @counter = @counter + 1;

END 


/* SEGMENTS NEEDS TO HAVE A STRAIGHT UP 1-606 OR WHATEVER ROW NUMBER TO ITERATE THROUGH EVERYTHING */
SELECT @segmentSize = COUNT(*) 
FROM #Segments s


CREATE TABLE #Final(
               IndexName varchar(256)
               , DatabaseName VARCHAR(256)
               , SchemaName VARCHAR(256)
               , TableName VARCHAR(256)
               , PhysicalOperation VARCHAR(MAX)
               , LogicalOperation VARCHAR(MAX)            
               , EstimateIO DECIMAL(18,7)
               , EstimateCPU DECIMAL(18,7)
               , EstimateRows DECIMAL
               , EstimateTotalSubtreeCost DECIMAL(18,7)
               , StatementText VARCHAR(MAX)
               , QueryNumber INT
               , NodeId INT
               , ParentNodeId INT
               , TotalNodeCost DECIMAL(18,7)
)


SET @counter = 1

/* ITERATE THROUGH THE SUB-SEGMENTS AND PULL ALL OF THE PROCESS INFORMATION FROM THOSE NODES TO BE PERSISTED */
WHILE(@Counter <= @segmentSize)
BEGIN

               SELECT @xmlDocument = t.xmlSegment
                              , @Segment = t.statementText
                              , @parentCounter = t.QueryCounter
               FROM #Segments t
               WHERE t.rowNum = @Counter

               ;WITH xmlnamespaces ('http://schemas.microsoft.com/sqlserver/2004/07/showplan' AS p1)
               INSERT INTO #Final
                       ( IndexName ,
                         DatabaseName ,
                         SchemaName ,
                         TableName ,
                                               PhysicalOperation ,
                         LogicalOperation ,
                         EstimateIO ,
                         EstimateCPU ,
                         EstimateRows ,
                         EstimateTotalSubtreeCost ,
                         StatementText ,
                                               QueryNumber ,
                                               NodeId ,
                                               ParentNodeId ,
                                               TotalNodeCost
                       )
               SELECT c.value('(@Index)[1]', 'varchar(256)') AS IndexName
                              , c.value('(@Database)[1]', 'varchar(256)') AS DatabaseName
                              , c.value('(@Schema)[1]', 'varchar(256)') AS SchemaName
                              , c.value('(@Table)[1]', 'varchar(256)') AS TableName
                              , x.value('(@PhysicalOp)[1]', 'varchar(max)') AS PhysicalOperation
                              , x.value('(@LogicalOp)[1]', 'varchar(max)') AS LogicalOperation
                              , x.value('(@EstimateIO)[1]', 'DECIMAL(18,7)') AS EstimateIO
                              , CONVERT(DECIMAL(18,7), CONVERT(REAL, x.value('(@EstimateCPU)[1]', 'varchar(max)'))) AS EstimateCPU
                              , CONVERT(DECIMAL, CONVERT(REAL, x.value('(@EstimateRows)[1]', 'varchar(max)'))) AS EstimateRows
                              , CONVERT(DECIMAL(18,7), CONVERT(REAL, x.value('(@EstimatedTotalSubtreeCost)[1]', 'varchar(max)'))) AS EstimateTotalSubtreeCost
                              , @Segment AS StatementText
                              , @parentCounter
                              , x.value('(@NodeId)[1]', 'INT') AS Node
                              , p.value('(@NodeId)[1]', 'INT') AS ParentNode
                              , NULL AS TotalNodeCost
               FROM @xmlDocument.nodes('//p1:RelOp') x2(x)
               OUTER APPLY x2.x.nodes('p1:IndexScan/p1:Object') AS child(c)
               OUTER APPLY x2.x.nodes('../..') AS parent(p)

               SET @Counter = @Counter + 1;

END

/* FIND THE INDIVIDUAL NODE COST, USING THE OVERALL TOTALSUBTREECOST */
;WITH parentOpCost AS (
               SELECT f1.NodeId AS Node
                              , f1.StatementText
                              , f1.EstimateTotalSubtreeCost - SUM(f.EstimateTotalSubtreeCost) AS costOfNode
               FROM #Final f
               JOIN #Final f1
                              ON f1.NodeId = f.ParentNodeId
                              AND f1.StatementText = f.StatementText
                              AND f1.QueryNumber = f.QueryNumber
               GROUP BY f1.NodeId, f1.EstimateTotalSubtreeCost, f1.StatementText
), bottomOpCost AS (
               SELECT f.NodeId
                              , f.StatementText
                              , f.EstimateTotalSubtreeCost AS costOfNode
               FROM #Final f
               LEFT JOIN parentOpCost p
                              ON p.Node = f.NodeId
                              AND p.StatementText = f.StatementText
               WHERE p.Node IS NULL
)

SELECT *
INTO #IndividualNodeCosts
FROM bottomOpCost b

UNION ALL

SELECT *
FROM parentOpCost p
ORDER BY b.StatementText

UPDATE #Final
SET TotalNodeCost = i.costOfNode
FROM #Final f
JOIN #IndividualNodeCosts i
               ON i.NodeId = f.NodeId
               AND i.StatementText = f.StatementText


INSERT INTO SQLMaintenance.dbo.dba_QueryPlanMaster
        ( DailyQueryID ,
          PullDate ,
          QueryText ,
          CachedTime ,
          QueryPlan ,
          ExecutionCount ,
          AverageLogicalReads ,
          MinLogicalReads ,
          MaxLogicalReads ,
                                AverageWorkerTimeSeconds ,
                                MinWorkerTimeSeconds ,
                                MaxWorkerTimeSeconds
        )
SELECT q.rowNum AS DailyQueryID
               , @StartDate AS PullDate
               , q.text 
               , q.maxCachedTime 
               , q.query_plan 
               , q.execution_count 
               , q.averageLogicalReads 
               , q.min_logical_reads 
               , q.max_logical_reads
               , q.averageWorkerTimeSeconds
               , q.min_worker_time
               , q.max_worker_time
FROM #QueryList q


INSERT INTO SQLMaintenance.dbo.dba_QueryPlanDetail
        ( DailyQueryID ,
          PullDate ,
                                PhysicalOperation ,
          LogicalOperation ,
          IndexName ,
          DatabaseName ,
          SchemaName ,
          TableName ,
          EstimateIO ,
          EstimateCPU ,
          EstimateRows ,
          EstimateTotalSubtreeCost ,
          StatementText,
                                NodeId,
                                ParentNodeId,
                                TotalNodeCost
        )
SELECT f.QueryNumber AS DailyQueryID
               , @StartDate AS PullDate
               , f.PhysicalOperation
               , f.LogicalOperation 
               , f.IndexName 
               , f.DatabaseName 
               , f.SchemaName 
               , f.TableName 
               , f.EstimateIO 
               , f.EstimateCPU 
               , f.EstimateRows 
               , f.EstimateTotalSubtreeCost 
               , f.StatementText 
               , f.NodeId
               , f.ParentNodeId
               , f.TotalNodeCost
FROM #Final f

