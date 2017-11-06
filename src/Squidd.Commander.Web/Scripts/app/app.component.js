"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var AppComponent = (function () {
    function AppComponent() {
        this.location = "UK South";
        this.runner = {
            id: 1,
            friendlyName: "Primary Web Server",
            computerName: "WebServer01",
            ip: "12.34.456.78:13000",
            os: "Windows Server 2012 R2"
        };
    }
    AppComponent = __decorate([
        core_1.Component({
            selector: "all-runners",
            template: "    \n<h4>{{location}}</h4>\n<div class=\"row\">\n    <div class=\"col-md-6\">\n        <div class=\"card text-center runner-card\">\n            <div class=\"card-header\">\n                {{runner.friendlyName}}\n            </div>\n            <div class=\"card-block\">\n                <i class=\"fa fa-globe\"></i>\n                <h5 class=\"card-title\">{{runner.computerName}}</h5>\n                <ul class=\"card-text list-unstyled\">\n                    <li>{{runner.os}}</li>\n                    <li>{{runner.ip}}</li>\n                </ul>\n            </div>\n            <div class=\"card-footer text-muted\">\n                Last contact: 2 days ago\n            </div>\n        </div>\n    </div>\n</div>\n  "
        })
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
var Runner = (function () {
    function Runner() {
    }
    return Runner;
}());
exports.Runner = Runner;
//# sourceMappingURL=app.component.js.map