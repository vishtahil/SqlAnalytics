"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
//our root app component
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
// import ng2-bootstrap pagination module
var ng2_bootstrap_1 = require('ng2-bootstrap');
var AppComponent = (function () {
    function AppComponent() {
        this.totalPagerItems = 64;
        this.currentPagerPage = 3;
        this.smallnumPagerPages = 0;
    }
    AppComponent.prototype.pageChanged = function (event) {
        console.log('Page changed to: ' + event.page);
        console.log('Number items per page: ' + event.itemsPerPage);
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: 'my-app',
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <h4>Pager</h4>\n    <div style=\"margin: 20px 0;\">\n      <pagination [directionLinks]=\"false\" [totalItems]=\"totalPagerItems\" [(ngModel)]=\"currentPagerPage\" (numPages)=\"smallnumPagerPages = $event\"></pagination>\n    </div>\n\n    <div>\n      <pager [totalItems]=\"totalPagerItems\" [(ngModel)]=\"currentPagerPage\" (pageChanged)=\"pageChanged($event)\" pageBtnClass=\"btn\"\n        itemsPerPage=\"10\"></pager>\n    </div>\n  ",
        })
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, ng2_bootstrap_1.PaginationModule.forRoot(), forms_1.FormsModule],
            declarations: [AppComponent],
            bootstrap: [AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
