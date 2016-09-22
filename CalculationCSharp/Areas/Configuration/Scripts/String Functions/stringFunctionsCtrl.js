sulhome.kanbanBoardApp.controller('stringFunctionsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];

    if (Functions.length > 0) {
        $scope.period.Type = Functions[0].Type;
        $scope.period.Number1 = Functions[0].Number1;
        $scope.period.String1 = Functions[0].String1;
        $scope.period.String2 = Functions[0].String2;
    }
    else {
        $scope.period = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            Type: $scope.period.Type,
            Number1: $scope.period.Number1,
            String1: $scope.period.String1,
            String2: $scope.period.String2,

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
