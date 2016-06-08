sulhome.kanbanBoardApp.controller('logicCtrl', function ($scope, $uibModalInstance, logic) {
    
    $scope.logic = logic;
    $scope.addItem = function () {
        $scope.logic.push({
            ID: this.logic.length
        });
    },

    $scope.removeItem = function (index) {
        $scope.selected.logic.splice(index, 1);
    },

    //Click OK
    $scope.ok = function () {
        $uibModalInstance.close($scope.logic);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});