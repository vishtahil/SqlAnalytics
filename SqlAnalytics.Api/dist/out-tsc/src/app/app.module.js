"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
//our root app component
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var app_component_1 = require("./app.component");
var sql_analytics_component_1 = require("./sql-analytics/sql-analytics-component");
var sql_analytics_overview_component_1 = require("./sql-analytics/sql-analytics-overview-component");
var sql_analytics_plan_component_1 = require("./sql-analytics/sql-analytics-plan-component");
var sql_analytics_optimize_component_1 = require("./sql-analytics/sql-analytics-optimize-component");
var windows_wrapper_1 = require("./shared/windows-wrapper");
var local_storage_wrapper_1 = require("./shared/local-storage-wrapper");
var ngx_bootstrap_1 = require("ngx-bootstrap");
var sanitize_html_pipe_1 = require("./shared/sanitize-html.pipe");
var error_notify_service_1 = require("./shared/error-notify-service");
var global_error_handler_1 = require("./shared/global-error-handler");
var sql_help_dialog_component_1 = require("./shared/sql-help-dialog/sql-help-dialog.component");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [platform_browser_1.BrowserModule, forms_1.FormsModule, http_1.HttpModule,
            ngx_bootstrap_1.TabsModule.forRoot(), ngx_bootstrap_1.AlertModule.forRoot(),
            ngx_bootstrap_1.ModalModule.forRoot(), ngx_bootstrap_1.TooltipModule.forRoot()],
        declarations: [app_component_1.AppComponent, sanitize_html_pipe_1.SanitizeHtml, sql_analytics_component_1.SqlAnalyticsComponent, sql_analytics_plan_component_1.SqlAnalyticsPlanComponent,
            sql_analytics_overview_component_1.SqlAnalyticsOverviewComponent, sql_analytics_optimize_component_1.SqlAnalyticsOptimizeComponent, sql_help_dialog_component_1.SqlHelpDialogComponent],
        bootstrap: [app_component_1.AppComponent],
        providers: [windows_wrapper_1.WindowRef,
            local_storage_wrapper_1.StorageRef,
            error_notify_service_1.ErrorNotifyService,
            { provide: core_1.ErrorHandler, useClass: global_error_handler_1.default }]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map