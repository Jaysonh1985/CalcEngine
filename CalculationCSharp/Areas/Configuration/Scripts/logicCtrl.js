sulhome.kanbanBoardApp.controller('logicCtrl', function ($scope, $uibModalInstance, logic) {
    
    $scope.logic = logic;
    $scope.addItem = function () {
        $scope.logic.push({
            Bracket1: "",
            Input1: "",
            Input2: "",
            LogicInd: "",
            Bracket2: "",
            Operator: ""
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