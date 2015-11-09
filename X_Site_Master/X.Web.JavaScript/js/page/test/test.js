define(function(require, exports) {
    exports.init = function () {
        var $ = require("jQuery");
        var rsa = require("rsa");
        var en = rsa("010001", "9db77de673280b9f0e97695331ef5598ef0b7431a3bfd1c43a37216775ea6c06bb9235c2bf94cd14236314c622f04edeaa1c154592ea0e7a9d19ad197d8776709cb6f865a53cc2febb3bf5286c8872e63b51f0b6d5d4cf1ab27e7285b64751d38106b1ffe4fdbc9354ab2552eafa9df444ddb857fc29c5010d7838afc2095f1d", 1024, "01234567890");
        console.log(en);
        var plugins = require("plugins");
        console.log(plugins.url());
        //$.ajax({
        //    url: "http://172.16.73.48:8000/api/workday/GetCurrentDateTime",
        //    type: "Get",
        //    timeout: 30000,
        //    dataType: "json",
        //    success: function(m) {
        //        console.log(m);
        //    }
        //});
    }
});
