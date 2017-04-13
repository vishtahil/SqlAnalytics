"use strict";
var SqlModel = (function () {
    function SqlModel(connectstring, sql) {
        this.ConnectionString = connectstring;
        this.Sql = sql;
    }
    return SqlModel;
}());
exports.SqlModel = SqlModel;
var SqlPlanStatisticsModel = (function () {
    function SqlPlanStatisticsModel() {
    }
    return SqlPlanStatisticsModel;
}());
exports.SqlPlanStatisticsModel = SqlPlanStatisticsModel;
var SqlPlanOveriviewModel = (function () {
    function SqlPlanOveriviewModel() {
    }
    return SqlPlanOveriviewModel;
}());
exports.SqlPlanOveriviewModel = SqlPlanOveriviewModel;
var SqlOptimizationHint = (function () {
    function SqlOptimizationHint() {
    }
    return SqlOptimizationHint;
}());
exports.SqlOptimizationHint = SqlOptimizationHint;
var SqlOverviewMessages = (function () {
    function SqlOverviewMessages() {
    }
    return SqlOverviewMessages;
}());
exports.SqlOverviewMessages = SqlOverviewMessages;
var SqlStatisticsSummary = (function () {
    function SqlStatisticsSummary() {
    }
    return SqlStatisticsSummary;
}());
exports.SqlStatisticsSummary = SqlStatisticsSummary;
//# sourceMappingURL=sql-analytics-model.js.map