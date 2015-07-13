
var CormitRouteApp = angular.module("CormitRouteApp", [
    "ngCookies",
    "ngResource",
    "ngSanitize",
    "ngRoute",
    "uiGmapgoogle-maps"])
    .config(function (uiGmapGoogleMapApiProvider) {
        uiGmapGoogleMapApiProvider.configure({
            //    key: 'your api key',
            v: '3.17',
            libraries: 'weather,geometry,visualization'
        });
    })
    ;