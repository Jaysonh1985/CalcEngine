// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('dateAdjustmentCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];

    if (Functions.length > 0) {
        $scope.period.Type = Functions[0].Type;
        $scope.period.Date1 = Functions[0].Date1;
        $scope.period.Date2 = Functions[0].Date2;
        $scope.period.PeriodType = Functions[0].PeriodType;
        $scope.period.Period = Functions[0].Period;
        $scope.period.Adjustment = Functions[0].Adjustment;
        $scope.period.Day = Functions[0].Day;
        $scope.period.Month = Functions[0].Month;
    }
    else {
        $scope.period = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            Type: $scope.period.Type,
            Date1: $scope.period.Date1,
            Date2: $scope.period.Date2,
            PeriodType: $scope.period.PeriodType,
            Period: $scope.period.Period,
            Adjustment: $scope.period.Adjustment,
            Day: $scope.period.Day,
            Month: $scope.period.Month
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
