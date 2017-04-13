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
var sql_analytics_model_1 = require("./sql-analytics-model");
var windows_wrapper_1 = require("./../shared/windows-wrapper");
var SqlAnalyticsOverviewComponent = (function () {
    function SqlAnalyticsOverviewComponent(winRef) {
        this.winRef = winRef;
    }
    SqlAnalyticsOverviewComponent.prototype.ngOnInit = function () {
    };
    SqlAnalyticsOverviewComponent.prototype.ngOnChanges = function (changes) {
    };
    return SqlAnalyticsOverviewComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", sql_analytics_model_1.SqlPlanOveriviewModel)
], SqlAnalyticsOverviewComponent.prototype, "sqlOverViewModel", void 0);
SqlAnalyticsOverviewComponent = __decorate([
    core_1.Component({
        selector: 'sql-analytics-overview',
        templateUrl: './sql-analytics-overview-component.html',
        styles: [""],
        changeDetection: core_1.ChangeDetectionStrategy.OnPush
    }),
    __metadata("design:paramtypes", [windows_wrapper_1.WindowRef])
], SqlAnalyticsOverviewComponent);
exports.SqlAnalyticsOverviewComponent = SqlAnalyticsOverviewComponent;
//# sourceMappingURL=sql-analytics-overview-component.js.map