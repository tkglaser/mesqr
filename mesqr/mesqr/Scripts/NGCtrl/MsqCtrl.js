function MsqCtrl($scope, $window, $http) {
    $scope.supportsGeo = $window.navigator;
    $scope.Location = null;
    $scope.IsError = false;
    $scope.Msqs = [];
    $scope.Tables = [];
    $scope.Loading = false;
    $scope.FriendlyPosition = '';
    $scope.geocoder = new google.maps.Geocoder();
    $scope.Message = '';
    $scope.SubscribedTables = [];

    $scope.Distance = function (lat1, lon1, lat2, lon2) {
        var dist = new LatLon(lat1, lon1).distanceTo(new LatLon(lat2, lon2));
        if (dist < 1)
            return dist * 1000 + ' m';
        return dist + ' km';
    }

    $scope.ToDate = function (d) {
        var parts = d.match(/(\d+)/g);
        var result = new Date(parts[0], parts[1] - 1, parts[2], parts[3], parts[4], parts[5]);
        return result;
    }

    $scope.getAddress = function (position) {
        var latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
        $scope.geocoder.geocode({
            'latLng': latlng
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    $scope.$apply(function () {
                        $scope.FriendlyPosition = results[1].formatted_address;
                    });
                } else {
                    alert('No results found');
                }
            } else {
                alert('Geocoder failed due to: ' + status);
            }
        });
    }

    $scope.FetchMsqs = function () {
        $http.get('api/nearbymsq', {
            params: {
                Latitude: $scope.Location.coords.latitude,
                Longitude: $scope.Location.coords.longitude,
                Accuracy: $scope.Location.coords.accuracy,
                Radius: 20000,
                Skip: 0,
                Take: 10
            }
        }).success(function (data, status) {
            $scope.Msqs = data;
            $scope.Loading = false;
        });
    };

    $scope.FetchSubscribedTables = function () {
        $http.get('api/table').success(function (data, status) {
            $scope.SubscribedTables = data;
            $scope.Loading = false;
        });
    };

    $scope.FetchTables = function () {
        $http.get('api/table', {
            params: {
                Latitude: $scope.Location.coords.latitude,
                Longitude: $scope.Location.coords.longitude,
                Accuracy: $scope.Location.coords.accuracy
            }
        }).success(function (data, status) {
            $scope.Tables = data;
            $scope.Loading = false;
        });
    };

    $scope.RefreshLocation = function () {
        $window.navigator.geolocation.getCurrentPosition(
            function (position) {
                $scope.$apply(function () {
                    $scope.Location = position;
                    $scope.Loading = true;
                });
                $scope.getAddress(position);
                $scope.FetchMsqs();
                $scope.FetchTables();
                $scope.FetchSubscribedTables();
            },
            function (error) {
                $scope.$apply(function () {
                    $scope.IsError = true;
                });
            }
        );
    }

    $scope.Subscribe = function (tableid) {
        $http.post('api/tablesubscription', {
            TableId: tableid
        }).
          success(function (data, status, headers, config) {
              $scope.RefreshLocation();
          });
    };

    $scope.Delete = function (tableid) {
        $http.delete('api/tablesubscription', {
            params: {
                TableId: tableid
            }
        }).
          success(function (data, status, headers, config) {
              $scope.RefreshLocation();
          });
    };

    $scope.SaveMessage = function () {
        $http.post('api/msq', {
            Message : $scope.Message,
            FriendlyPosition: $scope.FriendlyPosition,
            Latitude : $scope.Location.coords.latitude,
            Longitude : $scope.Location.coords.longitude,
            Accuracy: $scope.Location.coords.accuracy,
        }).
          success(function (data, status, headers, config) {
              $scope.Msqs.unshift(data);
              $scope.Message = '';
          }).
          error(function (data, status, headers, config) {
              alert('BOOM');
          });
    };

    $scope.RefreshLocation();

    $scope.AutoRefresh = function () {
        if ($scope.Location && !$scope.Loading) {
            $scope.FetchMsqs();
        }
        setTimeout($scope.AutoRefresh, 5000);
    };

    $scope.AutoRefresh();
}
