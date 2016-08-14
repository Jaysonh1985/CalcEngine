sulhome.kanbanBoardApp.controller('regressionOutputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Output, $filter) {
    

    function init() {

        $scope.output = angular.fromJson(Output);

    };


    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     

        $uibModalInstance.close();
    }

    init();

})
