sulhome.kanbanBoardApp.controller('mathsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.maths = Functions;


    $scope.addItem = function () {
        $scope.maths.push({
            ID: this.maths.length,
            
        });
    },

    $scope.removeMathsItem = function (index) {
        $scope.maths.splice(index, 1);
    },

    $scope.OperatorClauseValidations = function OperatorClauseValidations(form) {

        angular.forEach($scope.maths, function (value, key, obj) {

            var AttName = 'Operator_' + key;

            form[AttName].$setValidity("clause", true);
            form[AttName].$setValidity("clauseblank", true);

            if ($scope.maths[key].Logic2 != "" && $scope.maths[key].Logic2 != null) {

                if ($scope.maths[key + 1] == null) {

                    form[AttName].$setValidity("clause", false);

                }

            }
            else if ($scope.maths[key].Logic2 == "" || $scope.maths[key].Logic2 == null)
            {
                if ($scope.maths[key + 1] != null) {

                    form[AttName].$setValidity("clauseblank", false);

                }
            }
        })
    }

    $scope.BracketsValidations = function BracketsValidations(form) {

        var LBcounter = 0;
        var RBcounter = 0;

        angular.forEach($scope.maths, function (value, key, obj) {

            if ($scope.maths[key].Bracket1 == "(") {

                var AttName = 'BracketLeft_' + key;

                form[AttName].$setValidity("bracketnotclosed", true);

                LBcounter = LBcounter + 1;
            }

            if ($scope.maths[key].Bracket2 == ")") {

                var AttName = 'BracketRight_' + key;

                form[AttName].$setValidity("bracketnotopen", true);

                RBcounter = RBcounter + 1;
            }
        })

        if (LBcounter > RBcounter) {

            angular.forEach($scope.maths, function (value, key, obj) {

                var AttName = 'BracketLeft_' + key;

                form[AttName].$setValidity("bracketnotclosed", true);

                if ($scope.maths[key].Bracket1 = "(") {

                    form[AttName].$setValidity("bracketnotclosed", false);

                }

            })

        }
        else if (LBcounter < RBcounter) {

            angular.forEach($scope.maths, function (value, key, obj) {

                var AttName = 'BracketRight_' + key;

                form[AttName].$setValidity("bracketnotopen", true);

                if ($scope.maths[key].Bracket2 = ")") {

                    form[AttName].$setValidity("bracketnotopen", false);

                }

            })

        }
        else {
            angular.forEach($scope.maths, function (value, key, obj) {

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
            $uibModalInstance.close($scope.maths);
        };
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
