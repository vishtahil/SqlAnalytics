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
var core_1 = require("@angular/core");
var sql_hints_regex_1 = require("./sql-hints-regex");
var SqlAnalyticsOptimizeComponent = (function () {
    function SqlAnalyticsOptimizeComponent(_sqlHintRegexService) {
        this._sqlHintRegexService = _sqlHintRegexService;
        this.onSqlHint = new core_1.EventEmitter();
        this.size = 'md';
    }
    SqlAnalyticsOptimizeComponent.prototype.ngOnInit = function () {
    };
    SqlAnalyticsOptimizeComponent.prototype.ngOnChanges = function (changes) {
        this.originalSql = this.sqlStmt;
    };
    SqlAnalyticsOptimizeComponent.prototype.matchPattern = function (sqlHintModel) {
        this.sqlStmt = this.originalSql; //restore with original
        var regex = new RegExp(sqlHintModel.MatchedExpression, 'gi');
        var matches = this.sqlStmt.match(regex);
        for (var _i = 0, matches_1 = matches; _i < matches_1.length; _i++) {
            var match = matches_1[_i];
            this.sqlStmt = this.sqlStmt.replace(match, "<span style='color:red;font-weight:bold'>" + match + "</span>");
        }
        this.onSqlHint.emit(this.sqlStmt);
    };
    return SqlAnalyticsOptimizeComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], SqlAnalyticsOptimizeComponent.prototype, "sqlHintsModel", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], SqlAnalyticsOptimizeComponent.prototype, "sqlStmt", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], SqlAnalyticsOptimizeComponent.prototype, "onSqlHint", void 0);
__decorate([
    core_1.Input('title'),
    __metadata("design:type", Object)
], SqlAnalyticsOptimizeComponent.prototype, "title", void 0);
__decorate([
    core_1.Input('size'),
    __metadata("design:type", Object)
], SqlAnalyticsOptimizeComponent.prototype, "size", void 0);
__decorate([
    core_1.ViewChild('modal'),
    __metadata("design:type", Object)
], SqlAnalyticsOptimizeComponent.prototype, "modal", void 0);
SqlAnalyticsOptimizeComponent = __decorate([
    core_1.Component({
        selector: 'sql-analytics-optimize',
        templateUrl: './sql-analytics-optimize-component.html',
        styles: ["\n      .alert {cursor:pointer;}\n  "],
        providers: [sql_hints_regex_1.SqlHintRegexService],
        changeDetection: core_1.ChangeDetectionStrategy.OnPush
    }),
    __metadata("design:paramtypes", [sql_hints_regex_1.SqlHintRegexService])
], SqlAnalyticsOptimizeComponent);
exports.SqlAnalyticsOptimizeComponent = SqlAnalyticsOptimizeComponent;
//# sourceMappingURL=sql-analytics-optimize-component.js.map