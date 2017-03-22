(function () {
    'use strict';

    angular
        .module('appParkingLot')
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when("/", {
                    controller: "parkingLotController",
                    templateUrl: "../views/parkingLots.html"
                })
                .when("/parkingLot", {
                    controller: "parkingLotController",
                    templateUrl: "../views/parkingLots.html"
                })
                .when("/editParkingLot/:id", {
                    controller: "singleparkingLotController",
                    templateUrl: "/views/editParkingLot.html"
                })
                .when("/addParkingLot", {
                    controller: "singleparkingLotController",
                    templateUrl: "/views/editParkingLot.html"
                })
                .otherwise(
                {
                    redirectTo: "/"
                });
        }])
        .controller('parkingLotController', parkingLotController)
        .controller('singleparkingLotController', singleparkingLotController);

    parkingLotController.$inject = ['$scope', 'parkingLotFactory', '$location'];
    singleparkingLotController.$inject = ['$scope', '$location', '$routeParams', 'parkingLotFactory'];

    function parkingLotController($scope, parkingLotFactory, $location) {
        $scope.parkingLots = [];
        $scope.isBusy = false;
        getParkingLots();

        $scope.delete = function (id) {
            parkingLotFactory.delteParkingLot(id)
            .then(function (result) {
                getParkingLots();
            },
            function (error) {
                alert("Could not delte the parking lot " + error);
            });
        };

        $scope.add = function () {
            $location.path('/addParkingLot');
        };

        function getParkingLots() {
            $scope.isBusy = true;

            parkingLotFactory.getAllParkingLots()
                .then(function (response) {
                    $scope.parkingLots = parkingLotFactory.parkingLots;
                },
                function () {
                    alert("Could not load parking Lots");
                })
                .then(function () {
                    $scope.isBusy = false;
                });
        };
    };

    function singleparkingLotController($scope, $location, $routeParams, parkingLotFactory) {
        $scope.parkingLot = {};
        $scope.existsParkingLot = false;

        if ($routeParams.id) {
            parkingLotFactory.getParkingLotById($routeParams.id)
               .then(function (response) {
                   $scope.parkingLot = response;
                   $scope.existsParkingLot = true;
               },
               function (error) {
                   alert("Error retrieving parking lot" + error);
                   console.log(error);
                   $scope.back();
               });
        }

        $scope.back = function () {
            $location.path('/');
        };

        $scope.save = function () {
            if ($routeParams.id && $scope.existsParkingLot) {
                parkingLotFactory.updateParkingLot($scope.parkingLot)
                    .then(function (result) {
                        angular.copy(result, $scope.parkingLot);
                        $scope.back();
                    },
                    function (error) {
                        alert("Could not save the parking lot " + error);
                    });
            } else {
                parkingLotFactory.addParkingLot($scope.parkingLot)
                    .then(function (result) {
                        angular.copy(result, $scope.parkingLot);
                        $scope.back();
                    },
                    function (error) {
                        alert("Could not save the parking lot " + error);
                    });
            }
        };
    };
})();