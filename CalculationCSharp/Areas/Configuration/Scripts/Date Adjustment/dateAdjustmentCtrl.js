// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('dateAdjustmentCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];
    //Map Back the input values if they exist
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
    //Set up new object
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
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
