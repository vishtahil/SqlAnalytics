"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var SplitWordPipe = (function () {
    function SplitWordPipe() {
    }
    SplitWordPipe.prototype.transform = function (value, args) {
        if (!value)
            return value;
        return value.replace(/([a-z])([A-Z])/g, '$1 $2');
    };
    return SplitWordPipe;
}());
SplitWordPipe = __decorate([
    core_1.Pipe({ name: 'splitword' })
], SplitWordPipe);
exports.SplitWordPipe = SplitWordPipe;
//# sourceMappingURL=split-word.pipe.js.map