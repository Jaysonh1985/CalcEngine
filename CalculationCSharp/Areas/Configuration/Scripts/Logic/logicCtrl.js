// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('logicCtrl', function ($scope, $uibModalInstance, Logic) {   
    $scope.Logic = Logic;
    $scope.addItem = function (index) {
        var item = null;
        item = {
            ID: index + 1,
        };
        $scope.Logic.splice(index + 1, 0, item);
    },

    $scope.removeLogicItem = function (index) {
        $scope.Logic.splice(index, 1);
    },

    $scope.OperatorClauseValidations = function OperatorClauseValidations(form) {

        if ($scope.Logic.length > 1)
        {
            key = $scope.Logic.length - 1;
            var AttName = 'Operator_' + key;
            form[AttName].$setValidity("clauseblank", true);
            if ($scope.Logic[key].Operator == "" || $scope.Logic[key].Operator == null) {
                if ($scope.Logic[key - 1] != null) {
                    form[AttName].$setValidity("clauseblank", false);
                }
            }
        }

    }

    $scope.BracketsValidations = function BracketsValidations(form) {
        var LBcounter = 0;
        var RBcounter = 0;
        angular.forEach($scope.Logic, function (value, key, obj) {
            if ($scope.Logic[key].Bracket1 == "(") {
                var AttName = 'BracketLeft_' + key;
                form[AttName].$setValidity("bracketnotclosed", true);
                LBcounter = LBcounter + 1;
            }

            if ($scope.Logic[key].Bracket2 == ")") {
                var AttName = 'BracketRight_' + key;
                form[AttName].$setValidity("bracketnotopen", true);
                RBcounter = RBcounter + 1;
            }
        })

        if (LBcounter > RBcounter) {
            angular.forEach($scope.Logic, function (value, key, obj) {
                var AttName = 'BracketLeft_' + key;
                form[AttName].$setValidity("bracketnotclosed", true);
                if ($scope.Logic[key].Bracket1 = "(") {
                    form[AttName].$setValidity("bracketnotclosed", false);
                }
            })
        }
        else if (LBcounter < RBcounter) {
            angular.forEach($scope.Logic, function (value, key, obj) {
                var AttName = 'BracketRight_' + key;
                form[AttName].$setValidity("bracketnotopen", true);
                if ($scope.Logic[key].Bracket2 = ")") {
                    form[AttName].$setValidity("bracketnotopen", false);
                }
            })
        }
        else
        {
            angular.forEach($scope.Logic, function (value, key, obj) {
                var AttName = 'BracketLeft_' + key;
                form[AttName].$setValidity("bracketnotclosed", true);
                var AttName = 'BracketRight_' + key;
                form[AttName].$setValidity("bracketnotopen", true);
            })
        }
    }

    //Click OK
    $scope.ok = function (form) {
        $scope.OperatorClauseValidations(form);
        $scope.BracketsValidations(form);
        if (form.$valid == true) {
            $uibModalInstance.close($scope.Logic);
        };

    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});