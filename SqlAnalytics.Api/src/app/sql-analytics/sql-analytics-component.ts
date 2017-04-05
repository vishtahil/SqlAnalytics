//our root app component
import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Http } from '@angular/http';
import { SqlAnalyticsService } from './sql-analytics-service';
import { SqlModel } from './sql-analytics-model';
import { SqlStatisticsSummary, SqlPlanStatisticsModel } from './sql-analytics-model';
import { WindowRef } from './../shared/windows-wrapper';
import { StorageRef } from './../shared/local-storage-wrapper'
import { Observable } from 'rxjs/Rx';
import { NgForm } from '@angular/forms';
import Utilities from './../shared/utilities';
import {ErrorNotifyService} from './../shared/error-notify-service';

// Import RxJs required methods
import 'rxjs/add/operator/map';

/*
forms: example:
https://plnkr.co/edit/fYxjHY5xALoLuqaQze6u?p=preview
*/
@Component({
  selector: 'sql-analytics',
  templateUrl: './sql-analytics-component.html',
  styles: [``],
  providers: [SqlAnalyticsService,ErrorNotifyService]
})


export class SqlAnalyticsComponent implements OnInit, OnChanges {

  showResults: boolean = false;
  loading: boolean = false;
  buttonText: string = "Submit";
  _analyticsModel: SqlStatisticsSummary;
  sqlModel: SqlModel;
  sqlStmt: string = '';

  ngOnInit() {
    console.log("on init");
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log("on changes");
  }


  constructor(private _sqlAnalyticsService: SqlAnalyticsService,
    private errorNotifyService:ErrorNotifyService,
    private winRef: WindowRef, private http: Http,
    private storage: StorageRef
  ) {
    let connectionString = this.storage.getItem(Utilities.Constants.CONNECTION_KEY)
      || `Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AdventureWorks2012;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False`;
    this._analyticsModel = new SqlStatisticsSummary();
    this.sqlModel = new SqlModel(connectionString, '');
  }

  onSqlHint(sql: string) {
    this.sqlStmt = sql;
  }

  onSubmitBackButtonClick(form: NgForm) {
    this.loading = true;
    this.sqlStmt = this.sqlModel.Sql;
    this.storage.setItem(Utilities.Constants.CONNECTION_KEY, this.sqlModel.ConnectionString);
    this._sqlAnalyticsService.getSqlSqlStats({
      "Base64ConnectionString": this.winRef.nativeWindow.btoa(this.sqlModel.ConnectionString),
      "Base64Sql": this.winRef.nativeWindow.btoa(this.sqlModel.Sql)
    }).subscribe((response: SqlStatisticsSummary) => {
      this._analyticsModel = response;
      this.showResults = true;
      this.loading = false;
      console.log(this._analyticsModel);
    },
      (error: any) => {
        this.errorNotifyService.error(error);
      }
      );
  }
}