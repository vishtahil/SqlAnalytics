"use strict";
var testing_1 = require("@angular/core/testing");
var sql_help_dialog_component_1 = require("./sql-help-dialog.component");
describe('SqlHelpDialogComponent', function () {
    var component;
    var fixture;
    beforeEach(testing_1.async(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [sql_help_dialog_component_1.SqlHelpDialogComponent]
        })
            .compileComponents();
    }));
    beforeEach(function () {
        fixture = testing_1.TestBed.createComponent(sql_help_dialog_component_1.SqlHelpDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=sql-help-dialog.component.spec.js.map