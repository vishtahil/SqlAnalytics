"use strict";
var protractor_1 = require("protractor");
var DemoAppPage = (function () {
    function DemoAppPage() {
    }
    DemoAppPage.prototype.navigateTo = function () {
        return protractor_1.browser.get('/');
    };
    DemoAppPage.prototype.getParagraphText = function () {
        return protractor_1.element(protractor_1.by.css('app-root h1')).getText();
    };
    return DemoAppPage;
}());
exports.DemoAppPage = DemoAppPage;
//# sourceMappingURL=app.po.js.map