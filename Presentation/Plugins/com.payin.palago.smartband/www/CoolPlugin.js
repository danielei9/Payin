var exec = require('cordova/exec');

function CoolPlugin() { 
    console.log("CoolPlugin.js: is created");
}

CoolPlugin.prototype.connect = function(success, error, address, name) {
    console.log("CoolPlugin.js: connect");

    exec(
        success,
        error,
        "CoolPlugin",
        "connect",
        [address, name]
    );
}
CoolPlugin.prototype.scanDevices = function(success, error, callback) {
    console.log("CoolPlugin.js: scanDevices");
    
    document.removeEventListener("connect-detection", callback, false);
    document.addEventListener("connect-detection", callback, false);

    exec(
        success,
        error,
        "CoolPlugin",
        "scanDevices",
        []
    );
}
CoolPlugin.prototype.stopScan = function(success, error) {
    console.log("CoolPlugin.js: stopScan");

    exec(
        success,
        error,
        "CoolPlugin",
        "stopScan",
        []
    );
}
CoolPlugin.prototype.getAllCards = function(success, error, callback, address, name) {
    console.log("CoolPlugin.js: getAllCards");
    
    document.removeEventListener("card-detection", callback, false);
    document.addEventListener("card-detection", callback, false);

    exec(
        success,
        error,
        "CoolPlugin",
        "getAllCards",
        [address, name]
    );
}
CoolPlugin.prototype.activate = function(success, error, callback, id) {
    console.log("CoolPlugin.js: activate");

    document.removeEventListener("card-activation", callback, false);
    document.addEventListener("card-activation", callback, false);

    exec(
        success,
        error,
        "CoolPlugin",
        "activate",
        [id]
    );
}
CoolPlugin.prototype.createCard = function(success, error, address, name) {
    console.log("CoolPlugin.js: createCard");

    exec(
        success,
        error,
        "CoolPlugin",
        "createCard",
        [address, name]
    );
}
CoolPlugin.prototype.deleteCard = function(success, error, entry) {
    console.log("CoolPlugin.js: deleteCard");

    exec(
        success,
        error,
        "CoolPlugin",
        "deleteCard",
        [entry]
    );
}
var coolPlugin = new CoolPlugin();

module.exports = coolPlugin;