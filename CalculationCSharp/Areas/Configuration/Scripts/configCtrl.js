sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService) {
    // Model
    $scope.config = [];
    $scope.isLoading = true;



    function init() {
        var id = $location.absUrl();
        $scope.isLoading = true;
        configService.initialize().then(function (data) {
            $scope.isLoading = true;
            $scope.refreshConfig();


        }, onError);
    };

     $scope.refreshConfig = function refreshBoard() {        
         configService.getConfig()
           .then(function (data) {               
               $scope.isLoading = true;
               $scope.config = data;
            }, onError);
     };


     if ($scope.config !== null) {
         $scope.config = ($scope.config);
     }

     $scope.btn_add = function () {
         $scope.config.push({
                 
             });
    }

     $scope.remItem = function ($index) {
         $scope.config.splice($index, 1);
     }

     $scope.selectedRow = null;  // initialize our variable to null
     $scope.function = null;  // initialize our variable to null
     $scope.setClickedRow = function (index) {  //function that sets the value of selectedRow to current index
         $scope.selectedRow = index;
         $scope.function = this.rows.Function;
         ;
         if ($scope.function !== null & angular.isDefined($scope.function)) {
             $location.path($scope.function);
         }
         else
         {
             $location.path('/');
         }
     }
     $scope.setFunction = function (index) {  //function that sets the value of selectedRow to current index
         $scope.disableSelect = index;
         $scope.function = $scope.Function;
     }

    
    // Listen to the 'refreshBoard' event and refresh the board as a result
    $scope.$parent.$on("refreshBoard", function (e) {
        $scope.refreshBoard();
        toastr.success("Board updated successfully", "Success");
    });

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
