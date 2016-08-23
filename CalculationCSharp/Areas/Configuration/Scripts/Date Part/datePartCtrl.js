sulhome.kanbanBoardApp.controller('datePartCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.period = [];

    if (Functions.length > 0) {
        $scope.period.Part = Functions[0].Part;
        $scope.period.Date1 = Functions[0].Date1;
    }
    else {
        $scope.period = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            Part: $scope.period.Part,
            Date1: $scope.period.Date1,
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
