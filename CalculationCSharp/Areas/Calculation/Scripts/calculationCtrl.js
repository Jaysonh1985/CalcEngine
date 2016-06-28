﻿sulhome.kanbanBoardApp.controller('calculationCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, calculationService, $filter) {

    $scope.output = [];
    $scope.formset = null;
    $scope.fieldset = [];
    function init() {
        calculationService.initialize().then(function (data) {
                $scope.isLoading = true;
                var id = $scope.getConfigID();
                 calculationService.getCalc(id)
                   .then(function (data) {
                       $scope.isLoading = false;
                       $scope.config = data;
                       $scope.getFormFields();
                   });
            });
    };


    $scope.getConfigID = function getConfigID() {
        var url = location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);
        id = parseInt(id, 10);
        if (angular.isNumber(id) == false) {
            id = null;
        }
        return id;
    }
    
    $scope.getFormFields = function getFormFields() {  //function that sets the parameters available under the different variable types
        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        $scope.fields = [];
        $scope.fieldset = [];
        angular.forEach($scope.config, function (groups) {
            functionID = 0;
            $scope.fields = $filter('filter')($scope.config[scopeid].Functions, { Function: 'Input' });
            angular.forEach($scope.fields, function (functions) {
                    $scope.fieldset.push($scope.fields[functionID].Parameter[0]);
                    functionID = functionID + 1
                });
                scopeid = scopeid + 1
    });
    }


    function getIndexOf(arr, val, prop) {
        var l = arr.length,
          k = 0;
        for (k = 0; k < l; k = k + 1) {
            if (arr[k][prop] === val) {
                return k;
            }
        }
        return false;
    };

    function createArray(arr) {



    };


    $scope.CalcButtonClick = function CalcButtonClick() {
     
        $scope.isLoading = true;
        angular.forEach($scope.formset, function (value, key) {
            var index = getIndexOf($scope.config[0].Functions, key, 'Name');
            $scope.config[0].Functions[index].Value = value;
        });
        calculationService.postCalc(1, $scope.config).then(function (data) {
            $scope.isLoading = false;
            $scope.output = data;
        });

    };


   

  
        
    init();
});