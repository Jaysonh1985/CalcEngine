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
         $scope.getFunction($scope.function);
     }

     $scope.getClickedRow = function () {  //function that sets the value of selectedRow to current index
        
         return $scope.selectedRow;
     }

     $scope.getParameter = function () {

        $scope.parameter = configService.getParameters();
        return $scope.parameter

     }

     $scope.updateParameter = function (index) {
                  
         $scope.updateRow = $scope.getClickedRow();
         $scope.parameters = $scope.getParameter()

         $scope.config[$scope.updateRow].push = {

             Function: "Test",
             Variable: "Variable",
             Parameter: $scope.parameters,
             Type: "Type",
             Output: "Output"
         }
     }
  

     $scope.getFunction = function (func) {
         if (func !== null & angular.isDefined(func)) {
             $location.path(func);
         }
         else {
             $location.path('/');
         }
     }
     $scope.setFunction = function (index) {  //function that sets the value of selectedRow to current index

         $scope.function = this.rows.Function;
         $scope.disableSelect = true;
         $scope.getFunction($scope.function);

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
