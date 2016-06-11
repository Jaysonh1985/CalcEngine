sulhome.kanbanBoardApp.controller('mathsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, $window, $routeParams, Functions) {
    // Model
    $scope.maths = Functions;
    $scope.addItem = function () {
        $scope.maths.push({
            ID: this.maths.length
        });
    },

    $scope.removeItem = function (index) {
        $scope.selected.maths.splice(index, 1);
    },

    //Click OK
    $scope.ok = function () {
        $uibModalInstance.close($scope.maths);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
