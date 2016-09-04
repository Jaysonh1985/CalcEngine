sulhome.kanbanBoardApp.controller('arrayFunctionsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.array = [];

    if (Functions.length > 0) {
        $scope.array.LookupType = Functions[0].LookupType;
        $scope.array.LookupValue = Functions[0].LookupValue;
        $scope.array.Function = Functions[0].Function;
    }
    else {
        $scope.array = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            LookupType: $scope.array.LookupType,
            LookupValue: $scope.array.LookupValue,
            Function: $scope.array.Function,
        })
    },



    $scope.removeMathsItem = function (index) {
        $scope.array.splice(index, 1);
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
