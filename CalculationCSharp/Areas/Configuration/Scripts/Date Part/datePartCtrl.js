﻿// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('datePartCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];
    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.period.Part = Functions[0].Part;
        $scope.period.Date1 = Functions[0].Date1;
    }
    else {
        $scope.period = Functions
    };
    //Set up new object
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            Part: $scope.period.Part,
            Date1: $scope.period.Date1,
        })
    };
    //Click OK moves back to modal instantiation
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };
    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
