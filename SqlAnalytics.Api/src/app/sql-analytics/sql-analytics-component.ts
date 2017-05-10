//our root app component
import { Component, OnInit, Input, OnChanges, SimpleChanges, ChangeDetectionStrategy } from '@angular/core';
import { Http } from '@angular/http';
import { SqlAnalyticsService } from './sql-analytics-service';
import { SqlModel } from './sql-analytics-model';
import { SqlStatisticsSummary, SqlPlanStatisticsModel } from './sql-analytics-model';
import { WindowRef } from './../shared/windows-wrapper';
import { StorageRef } from './../shared/local-storage-wrapper'
import { Observable } from 'rxjs/Rx';
import { NgForm } from '@angular/forms';
import Utilities from './../shared/utilities';
import { ErrorNotifyService } from './../shared/error-notify-service';

// Import RxJs required methods
import 'rxjs/add/operator/map';

/*
forms: example:
https://plnkr.co/edit/fYxjHY5xALoLuqaQze6u?p=preview
*/
@Component({
  selector: 'sql-analytics',
  templateUrl: './sql-analytics-component.html',
  styles: [`
   .lint-badge{background-color:rgb(235, 204, 209);color:rgb(169, 68, 66);}
  `],
  providers: [SqlAnalyticsService]
})


export class SqlAnalyticsComponent implements OnInit, OnChanges {
  showResults: boolean = false;
  loading: boolean = false;
  buttonText: string = "Submit";
  _analyticsModel: SqlStatisticsSummary;
  sqlModel: SqlModel;
  sqlStmt: string = '';
  selectedSqlMode: string = '';
  sqlStatement:string='';

  @Input()
  sqlMode: string;

  ngOnInit() {

  }

  ngOnChanges(changes: SimpleChanges) {
    this.selectedSqlMode = this.sqlMode;
  }

  constructor(private _sqlAnalyticsService: SqlAnalyticsService,
    private errorNotifyService: ErrorNotifyService,
    private winRef: WindowRef, private http: Http,
    private storage: StorageRef
  ) {
    let connectionString = this.storage.getItem(Utilities.Constants.CONNECTION_KEY)
      || `Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AdventureWorks2012;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False`;
    this._analyticsModel = new SqlStatisticsSummary();
    let sqlText = this.storage.getItem(Utilities.Constants.SQL_KEY) || '';
    this.sqlModel = new SqlModel(connectionString, sqlText);
  }

  onSqlHint(sql: string) {
    this.sqlStmt = sql;
  }

  getSqlModelOnSubmit(){
    var sql=this.winRef.nativeWindow.btoa(this.sqlModel.Sql);

    var sqlObject:any={
      "Base64ConnectionString": this.winRef.nativeWindow.btoa(this.sqlModel.ConnectionString),
      "Base64Sql": sql,
      "Base64ExecutionPlan":sql,
      "SqlMode":Utilities.GetSqlMode(this.selectedSqlMode)
    };

    if(this.selectedSqlMode==Utilities.Constants.SQL_LINT_MODE){
       sqlObject.Base64ConnectionString=undefined;
       sqlObject.Base64ExecutionPlan=undefined;
    }else if (this.selectedSqlMode==Utilities.Constants.EXECUTION_PLAN_MODE){
      sqlObject.Base64Sql=undefined;
      sqlObject.Base64ConnectionString=undefined;
    }else{
       sqlObject.Base64ExecutionPlan=undefined;
    }
    return sqlObject;
  }

  onSubmitBackButtonClick(form: NgForm) {
    this.loading = true;
    this.sqlStmt = this.sqlModel.Sql;
    this.sqlStatement=this.sqlModel.Sql;
    
    
    this.storage.setItem(Utilities.Constants.CONNECTION_KEY, this.sqlModel.ConnectionString);
    this.storage.setItem(Utilities.Constants.SQL_KEY, this.sqlModel.Sql);
   
    var sqlObject:any=this.getSqlModelOnSubmit();
    this._sqlAnalyticsService.getSqlSqlStats(sqlObject)
        .subscribe((response: SqlStatisticsSummary) => {
      this._analyticsModel = response;
      this.showResults = true;
      this.loading = false;
      console.log(this._analyticsModel);
      if(this.selectedSqlMode==Utilities.Constants.EXECUTION_PLAN_MODE){
        this.sqlStmt=this._analyticsModel.SqlStatement;
        this.sqlStatement=this._analyticsModel.SqlStatement;
        this.loading=false;
      }
    },
      (error: any) => {
        this.errorNotifyService.error(error);
        this.loading=false;
      });
  }
}