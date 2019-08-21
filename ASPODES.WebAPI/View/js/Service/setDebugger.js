var setDebuggerApp = angular.module("setDebuggerService", []);
setDebuggerApp.service('$setDebugger', function () {
    var self = this;

    self.debugger = false;

    self.setDebuggerFn = function (flag) {
        self.debugger = flag;
    }

    self.getDebuggerFn = function () {
        return self.debugger;
    }
});