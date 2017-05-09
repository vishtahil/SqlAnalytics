export default class Utilities {
    public static Constants = class Inner {
        public static CONNECTION_KEY: string = "ConnectionKey";
        public static SQL_KEY: string = "SqlKey";
        public static SQL_MODE_KEY: string = "SqlModeKey";
        public static EXECUTION_PLAN_MODE: String = "Execution Plan Mode";
        public static SQL_MODE: string = "Sql Mode";
        public static SQL_LINT_MODE: String = "Sql Lint Mode";
    };

    public static GetSqlMode(sqlMode:string):number{
        if(sqlMode==Utilities.Constants.SQL_MODE) {
            return 1;
        }else if (sqlMode==Utilities.Constants.EXECUTION_PLAN_MODE){
            return 2;
        }else{
            return 3;
        }
    }
}