sulhome.kanbanBoardApp.controller('logicCtrl', function ($scope, $uibModalInstance, Logic) {
    
    $scope.Logic = Logic;
    $scope.addItem = function () {
        $scope.Logic.push({
            ID: this.Logic.length
        });
    },

    $scope.removeLogicItem = function (index) {
        $scope.Logic.splice(index, 1);
    },

    $scope.OperatorClauseValidations = function OperatorClauseValidations(form) {

        $scope.validations = [];

        

        angular.forEach($scope.Logic, function (value, key, obj) {

            var AttName = 'Operator_' + key;

            form[AttName].$setValidity("integer", true);

            if ($scope.Logic[key].Operator != "" && $scope.Logic[key].Operator != null) {

                if ($scope.Logic[key + 1] == null) {

                    form[AttName].$setValidity("integer", false);

                }

            }
        })
    }


    //Click OK
    $scope.ok = function (form) {

        $scope.OperatorClauseValidations(form);

        if (form.$valid == true) {
            $uibModalInstance.close($scope.Logic);
        };

    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});