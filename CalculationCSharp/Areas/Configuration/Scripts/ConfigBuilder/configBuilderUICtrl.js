// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configBuilderUICtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, $mdSidenav, configService, configFunctionFactory, configModalFactory, configTypeaheadFactory, configValidationFactory, $filter, $timeout) {

    $scope.OpenAllButton = function () {
        angular.forEach($scope.config, function (value, key, obj) {
            $scope.openIndex[key] = true;
        });
    };

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndex, function (value, key, obj) {
            $scope.openIndex[key] = false;
        });
    };

    $scope.RemoveExpectedResultsButton = function () {
        var cf = confirm("Are you sure you wish remove all Expected Results?");
        if (cf == true) {
            angular.forEach($scope.config, function (value, key, obj) {
                angular.forEach(value.Functions, function (valueF, keyF, objF) {
                    $scope.config[key].Functions[keyF].ExpectedResult = null;
                })
            });
        };
    };

    $scope.RemoveInputsButton = function () {
        var cf = confirm("Are you sure you wish remove all Input values?");
        if (cf == true) {
            angular.forEach($scope.config[0].Functions, function (value, key, obj) {
                $scope.config[0].Functions[key].Output = null;
            });
        };
    };

    $scope.ExitButton = function () {
        if (!$scope.form.$dirty) {
            var ID = configFunctionFactory.getConfigID();
            if ($scope.Function == true) {
                $window.location.assign('/Configuration/Function/Exit/' + ID);
            }
            else {
                $window.location.assign('/Configuration/Config/Exit/' + ID);
            };
        }
        else {
            var cf = confirm("Are you sure you wish to exit unsaved changes will be lost?");
            if (cf == true) {
                var ID = configFunctionFactory.getConfigID();
                if ($scope.Function == true) {
                    $window.location.assign('/Configuration/Function/Exit/' + ID);
                }
                else {
                    $window.location.assign('/Configuration/Config/Exit/' + ID);
                };
            };
        };
    };

    $scope.HistoryButtonClick = function (size) {
        $scope.ID = configFunctionFactory.getConfigID();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/History/HistoryModal.html',
            scope: $scope,
            controller: 'historyCtrl',
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
                isFunction: function () { return $scope.Function }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.config = JSON.parse(selectedItem);
            toastr.success("Reverted successfully", "Success");
        }, function () {
        });
    };

    $scope.RegressionButtonClick = function (size, form) {
        $scope.ID = configFunctionFactory.getConfigID();
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionModal.html',
            scope: $scope,
            controller: 'regressionCtrl',
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.CalcButtonClick(form);
        }, function () {
        });
    };

    $scope.ImpactAssessmentButtonClick = function () {
        $scope.ID = configFunctionFactory.getConfigID();
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Impact Assessment/ImpactAssessmentModal.html',
            scope: $scope,
            controller: 'impactAssessmentCtrl',
            size: 'lg',
            resolve: {
                ID: function () { return $scope.ID },
            }
        });
        modalInstance.result.then(function (selectedItem) {
        }, function () {
        });
    };

    $scope.SpecButtonClick = function (form) {
        var id = configFunctionFactory.getConfigID();
        $scope.isLoading = true;
        var promise = configService.specBuilder(id, $scope.config).then(function (data) {
            $scope.isLoading = false;
            $scope.SpecOutput = data;
            toastr.success("Specification Produced", "Success");
            return data;
        }, onError);
        return promise;
    };

    var onError = function (errorMessage) {
        $scope.viewOnly = false;
        toastr.error(errorMessage, "Error");
    };
});
