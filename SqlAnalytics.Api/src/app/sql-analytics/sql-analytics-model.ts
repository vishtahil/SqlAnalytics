    export class SqlModel {
        
        constructor(connectstring:string, sql:string){
          this.ConnectionString=connectstring;
          this.Sql=sql;
       
       }
      
        public ConnectionString: string;
        public Sql: string;
        public SqlExecutionPlan: string;
        public SqlMode:string;
    }
    export class SqlPlanStatisticsModel {
        public SqlPlanStats: SqlPlanStats[];
        public Warnings: NodeWarning[];
        public StatementSubTreeCost: string;
    }

    export class SqlPlanStats {
        public DailyQueryID: number;
        public PullDate: Date;
        public PhysicalOperation: string;
        public LogicalOperation: string;
        public IndexName: string;
        public DatabaseName: string;
        public SchemaName: string;
        public TableName: string;
        public EstimateIO: number;
        public EstimateCPU: number;
        public EstimateRows: number;
        public EstimateTotalSubTreeCost: number;
        public StatementText: string;
        public NodeId: number;
        public ParentNodeId: number;
        public TotalNodeCost: number;
        public NodeWarning:NodeWarning;

    }

    export class NodeWarning{
        public Key: string;
        public Value: string;
    }

    export class SqlPlanOveriviewModel {
        public SqlOverviewMessages: SqlOverviewMessages[];
        public SqlExecutionPlan: string;
        public TotalCpuTime: number;
        public TotalElapsedTime: number;
        public TotalLogicReads: number;
    }

    export class SqlOptimizationHint {
        public MatchedValue: string;
        public MatchedExpression: string;
        public MatchedSqlClause: string;
        public MatchedSqlText: string;
    }

    export class SqlOverviewMessages {
        public TableName: string;
        public LogicalReads: number;
        public LobLogicalReads: number;
    }

    export class SqlStatisticsSummary {
        public SqlPlanStatisticsModel: SqlPlanStatisticsModel;
        public SqlPlanOverviewModel: SqlPlanOveriviewModel;
        public SqlOptimizationHints: SqlOptimizationHint[];
        public SqlStatement:string;
    }
