sulhome.kanbanBoardApp.controller('regressionDifferenceCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Difference, $filter, $sce) {
    

    function init() {
        $scope.diffValue = $sce.trustAsHtml(Difference);
    };


    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     
        $uibModalInstance.close();
    }

    init();

})
