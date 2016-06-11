sulhome.kanbanBoardApp.controller('mathsCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService) {
    // Model
    $scope.maths = [];
    $scope.rowid = 0;
    $scope.isLoading = true;
    $scope.cssClass = 'Maths';

    function init() {

        if (configService.getParameters() == null)
        {
            $scope.maths = [];

        }
        else
        {
            $scope.maths = configService.getParameters();
        }

        $scope.rowid = configService.getRowid();
        $scope.colid = configService.getColid();
        $scope.decimal = configService.getDecimalList();


     };

    $scope.refreshConfig = function refreshBoard() {
        configService.getConfig()
          .then(function (data) {
              $scope.isLoading = true;
              $scope.config = data;
          }, onError);
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
        $scope.rowid = configService.getRowid();
        $scope.colid = configService.getColid();
        $scope.decimal = configService.getDecimalList();
                  
    }
    $scope.save = function () {
        
        configService.setParameters($scope.maths, $scope.colid, $scope.rowid,$scope.decimal);
        $location.path('/');
     
    }


    init();
})
