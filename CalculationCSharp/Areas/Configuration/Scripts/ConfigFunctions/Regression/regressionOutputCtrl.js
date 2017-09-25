// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('regressionOutputCtrl', function ($scope, $uibModalInstance,$uibModal, $log, $http, $location, Output, $filter, Header) {  

    function init() {
        $scope.output = angular.fromJson(Output);
        $scope.Header = Header;
    };

    $scope.OutputButtonClick = function (size, colIndex, type, SubOutput) {
        $scope.Output = angular.toJson(SubOutput);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/ConfigFunctions/Regression/RegressionOutputModal.html',
            scope: $scope,
            controller: 'regressionOutputCtrl',
            size: size,
            resolve: {
                Output: function () { return $scope.Output },
                Header: function () { return "Output" }
            }
        });
        modalInstance.result.then(function (selectedItem) {
        }, function () {

        });
    };

    $scope.OpenAllButton = function () {
        angular.forEach($scope.output, function (value, key, obj) {
            $scope.openIndexRegression[key] = true;
        })
    };

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndexRegression, function (value, key, obj) {
            $scope.openIndexRegression[key] = false;
        })
    };

    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     
        $uibModalInstance.close();
    };
    $scope.cancel = function () {
        $uibModalInstance.close();
    };

    init();
})
