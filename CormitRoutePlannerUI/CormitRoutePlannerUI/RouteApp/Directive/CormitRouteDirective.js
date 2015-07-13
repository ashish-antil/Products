//CormitRouteApp.directive("menu", function () {
//    return {
//        restrict: "E",
//        template: "<div ng-class='{ show: visible, left: alignment === \"left\", right: alignment === \"right\" }' ng-transclude></div>",
//        transclude: true,
//        scope: {
//            visible: "=",
//            alignment: "@"
//        }
//    };
//});

//CormitRouteApp.directive("menuItem", function () {
//    return {
//        restrict: "E",
//        template: "<div ng-click='navigate()' ng-transclude></div>",
//        transclude: true,
//        scope: {
//            hash: "@"
//        },
//        link: function($scope) {
//            $scope.navigate = function() {
//                window.location.hash = $scope.hash;
//            };
//        }
//    };
//});


//CormitRouteApp.run(function ($rootScope) {
//    document.addEventListener("keyup", function (e) {
//        if (e.keyCode === 27)
//            $rootScope.$broadcast("escapePressed", e.target);
//    });

//    document.addEventListener("click", function (e) {
//        $rootScope.$broadcast("documentClicked", e.target);
//    });
//});