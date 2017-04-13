"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
//our root app component
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var sql_analytics_service_1 = require("./sql-analytics-service");
var sql_analytics_model_1 = require("./sql-analytics-model");
var sql_analytics_model_2 = require("./sql-analytics-model");
var windows_wrapper_1 = require("./../shared/windows-wrapper");
var local_storage_wrapper_1 = require("./../shared/local-storage-wrapper");
var utilities_1 = require("./../shared/utilities");
var error_notify_service_1 = require("./../shared/error-notify-service");
// Import RxJs required methods
require("rxjs/add/operator/map");
/*
forms: example:
https://plnkr.co/edit/fYxjHY5xALoLuqaQze6u?p=preview
*/
var SqlAnalyticsComponent = (function () {
    function SqlAnalyticsComponent(_sqlAnalyticsService, errorNotifyService, winRef, http, storage) {
        this._sqlAnalyticsService = _sqlAnalyticsService;
        this.errorNotifyService = errorNotifyService;
        this.winRef = winRef;
        this.http = http;
        this.storage = storage;
        this.showResults = false;
        this.loading = false;
        this.buttonText = "Submit";
        this.sqlStmt = '';
        var connectionString = this.storage.getItem(utilities_1.default.Constants.CONNECTION_KEY)
            || "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AdventureWorks2012;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        this._analyticsModel = new sql_analytics_model_2.SqlStatisticsSummary();
        var sqlText = this.storage.getItem(utilities_1.default.Constants.SQL_KEY) || '';
        this.sqlModel = new sql_analytics_model_1.SqlModel(connectionString, sqlText);
    }
    SqlAnalyticsComponent.prototype.ngOnInit = function () {
        console.log("on init");
    };
    SqlAnalyticsComponent.prototype.ngOnChanges = function (changes) {
        console.log("on changes");
    };
    SqlAnalyticsComponent.prototype.onSqlHint = function (sql) {
        this.sqlStmt = sql;
    };
    SqlAnalyticsComponent.prototype.onSubmitBackButtonClick = function (form) {
        var _this = this;
        this.loading = true;
        this.sqlStmt = this.sqlModel.Sql;
        this.storage.setItem(utilities_1.default.Constants.CONNECTION_KEY, this.sqlModel.ConnectionString);
        this.storage.setItem(utilities_1.default.Constants.SQL_KEY, this.sqlModel.Sql);
        this._sqlAnalyticsService.getSqlSqlStats({
            "Base64ConnectionString": this.winRef.nativeWindow.btoa(this.sqlModel.ConnectionString),
            "Base64Sql": this.winRef.nativeWindow.btoa(this.sqlModel.Sql)
        }).subscribe(function (response) {
            _this._analyticsModel = response;
            _this.showResults = true;
            _this.loading = false;
            console.log(_this._analyticsModel);
        }, function (error) {
            _this.errorNotifyService.error(error);
        });
    };
    return SqlAnalyticsComponent;
}());
SqlAnalyticsComponent = __decorate([
    core_1.Component({
        selector: 'sql-analytics',
        templateUrl: './sql-analytics-component.html',
        styles: ["\n   .lint-badge{background-color:rgb(235, 204, 209);color:rgb(169, 68, 66);}\n  "],
        providers: [sql_analytics_service_1.SqlAnalyticsService]
    }),
    __metadata("design:paramtypes", [sql_analytics_service_1.SqlAnalyticsService,
        error_notify_service_1.ErrorNotifyService,
        windows_wrapper_1.WindowRef, http_1.Http,
        local_storage_wrapper_1.StorageRef])
], SqlAnalyticsComponent);
exports.SqlAnalyticsComponent = SqlAnalyticsComponent;
//# sourceMappingURL=sql-analytics-component.js.map