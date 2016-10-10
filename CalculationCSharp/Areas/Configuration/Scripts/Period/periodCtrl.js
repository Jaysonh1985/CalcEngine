// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('periodCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];

    if (Functions.length > 0) {
        
        $scope.period.Date1 = Functions[0].Date1;
        $scope.period.DateAdjustmentType = Functions[0].DateAdjustmentType;
        $scope.period.Date2 = Functions[0].Date2;
        $scope.period.Inclusive = Functions[0].Inclusive;
        $scope.period.DaysinYear = Functions[0].DaysinYear;
    }
    else {
        $scope.period = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            DateAdjustmentType: $scope.period.DateAdjustmentType,
            Date1: $scope.period.Date1,
            Date2: $scope.period.Date2,
            Inclusive: $scope.period.Inclusive,
            DaysinYear: $scope.period.DaysinYear,
        })
    },



    $scope.removeMathsItem = function (index) {
        $scope.period.splice(index, 1);
    },

    //Click OK
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
