// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('ReturnCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.return = [];
    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.return.Variable = Functions[0].Variable;
        $scope.return.Datatype = Functions[0].Datatype;
    }
    else {
        $scope.return = Functions
    };
    //Set up new object
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            Variable: $scope.return.Variable,
            Datatype: $scope.return.Datatype,
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
