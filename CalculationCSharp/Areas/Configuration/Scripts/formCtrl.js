sulhome.kanbanBoardApp.controller('formCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService, $filter) {

function init() {
        var id = 2;
        configService.initialize().then(function (data) {
            $scope.isLoading = true;
            var id = 1  ;
             configService.getCalc(id)
               .then(function (data) {
                   $scope.isLoading = false;
                   $scope.config = data;
                   $scope.getFormFields();
               });
 
        });
    };
    
    $scope.getFormFields = function getFormFields() {  //function that sets the parameters available under the different variable types
    var counter = 0;
    var scopeid = 0;
    var functionID = 0;
    $scope.fields = [];
    $scope.fieldset = [];
    angular.forEach($scope.config, function (groups) {
    functionID = 0;
    $scope.fields = $filter('filter')($scope.config[scopeid].Functions, { Function: 'Input' });
    angular.forEach($scope.fields, function (functions) {
            $scope.fieldset.push($scope.fields[functionID].Parameter[0]);
            functionID = functionID + 1
        });
        scopeid = scopeid + 1
    });
}
$scope.data = {

};
        
init();
});