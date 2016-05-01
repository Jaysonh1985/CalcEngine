sulhome.kanbanBoardApp.controller('mathsCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService) {
    // Model
    $scope.maths = [];
    $scope.isLoading = true;



    function init() {

       $scope.maths = configService.getParameters();

    };


    if ($scope.maths !== null) {
        $scope.maths = ($scope.maths);
    }

    $scope.btn_add = function () {
        $scope.maths.push({
                 
        });
    }

    $scope.remItem = function ($index) {
        $scope.maths.splice($index, 1);
    }


    $scope.get = function () {

        $scope.maths = configService.getParameters();
                  
    }
    $scope.save = function () {
        
        configService.setParameters($scope.maths);
      
    }

    $scope.btn_get = function () {
        
        
        parameter = configService.getParameters();

    }
})
