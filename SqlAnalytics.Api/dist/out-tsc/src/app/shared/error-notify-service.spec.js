"use strict";
var testing_1 = require("@angular/core/testing");
var error_notify_service_1 = require("./error-notify-service");
describe('ErrorNotifyServiceService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [error_notify_service_1.ErrorNotifyService]
        });
    });
    it('should ...', testing_1.inject([error_notify_service_1.ErrorNotifyService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=error-notify-service.spec.js.map