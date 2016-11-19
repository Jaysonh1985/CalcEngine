// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('arrayFunctionsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.array = [];
    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.array.LookupType = Functions[0].LookupType;
        $scope.array.LookupValue = Functions[0].LookupValue;
        $scope.array.Function = Functions[0].Function;
        $scope.array.PeriodType = Functions[0].PeriodType;
    }
    else {
        $scope.array = Functions
    };
    //Set up new object  
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            LookupType: $scope.array.LookupType,
            LookupValue: $scope.array.LookupValue,
            Function: $scope.array.Function,
            PeriodType: $scope.array.PeriodType,
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
