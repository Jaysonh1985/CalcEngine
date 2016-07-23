sulhome.kanbanBoardApp.controller('regressionDifferenceCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions, Input, $filter) {
    

    function init() {



    };


    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     

        $uibModalInstance.close($scope.formset.fields);
    }

    init();

})
