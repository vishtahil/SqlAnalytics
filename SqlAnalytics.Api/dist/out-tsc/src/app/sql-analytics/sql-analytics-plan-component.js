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
var SqlAnalyticsPlanComponent = (function () {
    function SqlAnalyticsPlanComponent() {
        this.operationWarnings = ["Table Scan", "Clustered Index Scan", "Key Lookup"];
    }
    SqlAnalyticsPlanComponent.prototype.ngOnInit = function () {
    };
    SqlAnalyticsPlanComponent.prototype.ngOnChanges = function (changes) {
    };
    SqlAnalyticsPlanComponent.prototype.showAlert = function (colValue) {
        if (this.operationWarnings.indexOf(colValue.PhysicalOperation) > -1 ||
            this.operationWarnings.indexOf(colValue.LogicalOperation) > -1) {
            return true;
        }
        return false;
    };
    SqlAnalyticsPlanComponent.prototype.setClasses = function (colValue) {
        var alertShow = this.showAlert(colValue);
        var classes = {
            'alert': alertShow,
            'alert-danger': alertShow,
        };
        return classes;
    };
    return SqlAnalyticsPlanComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], SqlAnalyticsPlanComponent.prototype, "sqlPlanModel", void 0);
SqlAnalyticsPlanComponent = __decorate([
    core_1.Component({
        selector: 'sql-analytics-plan',
        templateUrl: './sql-analytics-plan-component.html',
        styles: [""],
        changeDetection: core_1.ChangeDetectionStrategy.OnPush
    }),
    __metadata("design:paramtypes", [])
], SqlAnalyticsPlanComponent);
exports.SqlAnalyticsPlanComponent = SqlAnalyticsPlanComponent;
//# sourceMappingURL=sql-analytics-plan-component.js.map