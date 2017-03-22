(function () {
    'use strict';

    angular
        .module('appParkingLot')
        .factory('parkingLotFactory', parkingLotFactory);

    parkingLotFactory.$inject = ['$http', '$q'];

    function parkingLotFactory($http, $q) {
        var baseUrl = "http://localhost:5000/api/parkingLot";
        var _parkingLots = [];
        var _isInit = false;

        var _isReady = function () {
            return _isInit;
        }

        var _getAllParkingLots = function () {

            var deferred = $q.defer();
            $http.get(baseUrl)
            .then(function (result) {
                angular.copy(result.data, _parkingLots);
                _isInit = true;
                deferred.resolve();
            },
            function (error) {
                deferred.reject();
                console.error(error);
            });

            return deferred.promise;
        }

        var _addParkingLot = function (parkingLot) {

            var deferred = $q.defer();
            $http.post(baseUrl, parkingLot)
            .then(function (result) {
                var newlyCreatedParkingLot = result.data;

                _parkingLots.splice(0, 0, newlyCreatedParkingLot);
                deferred.resolve(newlyCreatedParkingLot);
            },
            function (error) {
                deferred.reject();
                console.error(error);
            })

            return deferred.promise;
        };

        function _findParkingLot(id) {
            var found = null;

            $.each(_parkingLots, function (i, item) {
                if (item.id == id) {
                    found = item;
                    return false;
                }
            });

            return found;
        }

        var _getParkingLotById = function (id) {

            var deferred = $q.defer();

            if (_isReady()) {
                var parkingLot = _findParkingLot(id);
                if (parkingLot) {
                    deferred.resolve(parkingLot);
                } else {
                    deferred.reject();
                }
            } else {
                $http.get(baseUrl + "/" + id)
                    .then(function (result) {
                        deferred.resolve(parkingLot);
                    },
                    function (error) {
                        deferred.reject(error);
                        console.error(error);
                    });
            }

            return deferred.promise;
        };

        var _updateParkingLot = function (parkingLot) {

            var deferred = $q.defer();

            $http.put(baseUrl, parkingLot)
            .then(function (result) {
                deferred.resolve(result.data);
                _getAllParkingLots();
            },
            function (error) {
                deferred.reject();
                console.error(error);
            })

            return deferred.promise;
        }

        var _delteParkingLot = function (id) {

            var deferred = $q.defer();

            $http.delete(baseUrl + "/" + id)
            .then(function (result) {
                deferred.resolve(result);
                _getAllParkingLots();
            },
            function (error) {
                deferred.reject();
                console.error(error);
            })

            return deferred.promise;
        }

        return {
            parkingLots: _parkingLots,
            getAllParkingLots: _getAllParkingLots,
            addParkingLot: _addParkingLot,
            isReady: _isReady,
            getParkingLotById: _getParkingLotById,
            updateParkingLot: _updateParkingLot,
            delteParkingLot: _delteParkingLot
        };
    }
})();