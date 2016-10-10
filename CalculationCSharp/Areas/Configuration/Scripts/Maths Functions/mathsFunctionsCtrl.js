// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('mathsFunctionsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];
    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.period.Type = Functions[0].Type;
        $scope.period.Number1 = Functions[0].Number1;
        $scope.period.Number2 = Functions[0].Number2;
        $scope.period.Rounding = Functions[0].Rounding;
        $scope.period.RoundingType = Functions[0].RoundingType;
    }
    else {
        $scope.period = Functions
    };
    //Set up new object
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            Type: $scope.period.Type,
            Number1: $scope.period.Number1,
            Number2: $scope.period.Number2,
            Rounding: $scope.period.Rounding,
            RoundingType: $scope.period.RoundingType,
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
