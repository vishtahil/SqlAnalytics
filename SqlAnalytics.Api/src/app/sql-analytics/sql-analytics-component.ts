//our root app component
import {Component,OnInit, OnChanges,SimpleChanges} from '@angular/core';
import {Http} from '@angular/http';
import {SqlAnalyticsService} from './sql-analytics-service';
import {SqlModel} from './sql-analytics-model';
import {SqlStatisticsSummary,SqlPlanStatisticsModel} from './sql-analytics-model';
import { WindowRef } from './../shared/windows-wrapper';
import {Observable} from 'rxjs/Rx';
import {NgForm} from '@angular/forms';

// Import RxJs required methods
import 'rxjs/add/operator/map';

/*
forms: example:
https://plnkr.co/edit/fYxjHY5xALoLuqaQze6u?p=preview
*/
@Component({
    selector: 'sql-analytics',
    templateUrl:'./sql-analytics-component.html',
    styles: [``],
    providers: [SqlAnalyticsService]
})

export class SqlAnalyticsComponent implements OnInit, OnChanges{
    showResults:boolean=false;
    buttonText:string="Submit";
    _analyticsModel:SqlStatisticsSummary;
    sqlModel:SqlModel;
    sqlStmt:string='';
   
    ngOnInit() {
      console.log("on init");
    }

    ngOnChanges(changes: SimpleChanges) {
      console.log("on changes");
    }
        

    constructor(private _sqlAnalyticsService:SqlAnalyticsService,
    private winRef: WindowRef, private http: Http
    ) {
      let connectionString=`Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AdventureWorks2012;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False`;
      this._analyticsModel= new SqlStatisticsSummary();
      this.sqlModel= new SqlModel(connectionString,'');
    }
    
    onSqlHint(sql:string){
      this.sqlStmt=sql;
    }
    
    onSubmitBackButtonClick(form:NgForm){
      this.sqlStmt=this.sqlModel.Sql;
      this._sqlAnalyticsService.getSqlSqlStats({
        "Base64ConnectionString":this.winRef.nativeWindow.btoa(this.sqlModel.ConnectionString),
        "Base64Sql":this.winRef.nativeWindow.btoa(this.sqlModel.Sql)
      }).subscribe((response:SqlStatisticsSummary)=>{
                                this._analyticsModel=response;
                                this.showResults=true;
                                 console.log(this._analyticsModel);
                                 }
                               );
    }
}