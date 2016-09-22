sulhome.kanbanBoardApp.controller('commentsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];

    if (Functions.length > 0) {
        $scope.period.String1 = Functions[0].String1;
    }
    else {
        $scope.period = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            String1: $scope.period.String1,
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
