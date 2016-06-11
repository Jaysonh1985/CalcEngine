﻿sulhome.kanbanBoardApp.controller('logicCtrl', function ($scope, $uibModalInstance, Logic) {
    
    $scope.Logic = Logic;
    $scope.addItem = function () {
        $scope.Logic.push({
            ID: this.Logic.length
        });
    },

    $scope.removeItem = function (index) {
        $scope.selected.Logic.splice(index, 1);
    },

    //Click OK
    $scope.ok = function () {
        $uibModalInstance.close($scope.Logic);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});