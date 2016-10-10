// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('regressionOutputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Output, $filter) {
    

    function init() {
        $scope.output = angular.fromJson(Output);
    };

    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.output, function (value, key, obj) {
            $scope.openIndexRegression[key] = true;
        })
    }

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndexRegression, function (value, key, obj) {
            $scope.openIndexRegression[key] = false;
        })
    }

    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     
        $uibModalInstance.close();
    }

    init();

})
