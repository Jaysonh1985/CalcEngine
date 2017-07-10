// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('tableCtrl', function ($scope, $uibModal, $uibModalInstance, $log, $http, $location, $filter, parameters) {
    $scope.rows = [];
    $scope.topRow = [];
    $scope.parameters = [];
    rows = [];
    $scope.array = [];
    $scope.rows.Row = [];
    function init() {
        $scope.parameters = parameters;
        angular.forEach(parameters, function (value, key, obj) {
            var Split = value.Column.split('~');
            angular.forEach(Split, function (valueS, keyS,objS) {
                rows.push({
                    Name: valueS,
                    RowNum: keyS
                });
            });
            $scope.topRow.push(rows);
            $scope.parameters[key].Rows = rows;
            rows = [];
        });
        $scope.parametersrows = [];
        var i, pivoted, prop, rec;
        pivoted = {};
        if ($scope.topRow !== null) {
            i = 0;
            while (i < $scope.topRow.length) {
                rec = $scope.topRow[i];
                pivoted[rec.RowNum] = pivoted[rec.RowNum] || {};
                for (prop in rec) {
                    pivoted[rec.RowNum][prop] = pivoted[rec.RowNum][prop] || [];
                    if (!pivoted[rec.RowNum][prop].date) {
                        pivoted[rec.RowNum][prop].push(rec[prop]);
                    }

                }
                i++;
            }
            $scope.parametersrows = pivoted.undefined;
        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    init();

})
