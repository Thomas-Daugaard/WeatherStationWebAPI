// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44301/WeatherHub").build();

connection.on("WeatherUpdate",
    function (message) {
        //console.log(JSON.stringify(message));
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        //var encodedMsg = msg;
        var li = document.createElement("li");
        li.textContent = msg;
        document.getElementById("WeatherLog").appendChild(li);
    });

connection.start().then(function() {
    console.log("Connection started")
}).catch(function(err) {
    return console.error(err.toString());
});
