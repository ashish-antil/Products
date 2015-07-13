var CormitRouteController = CormitRouteApp.controller("CormitRouteController",
    function ($scope, $rootScope) {
     
        $scope.map = {
            center: {
                latitude: 45.4,
                longitude: -71.9
            },

            showTraffic: true,
            show: true,
            zoom: 15,
            options: {
                panControl: true,
                zoomControl: true,
                zoomControlOptions:{
                    position: { ControlPosition :'BOTTOM_LEFT' },
                    style: 'SMALL',
                }
            }
        };
        $scope.visible = false;
        console.log($scope.map, "MAPS");
    });