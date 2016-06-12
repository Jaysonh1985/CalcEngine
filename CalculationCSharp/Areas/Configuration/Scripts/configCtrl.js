sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService, $filter) {
    // Model
    $scope.config = [];
    $scope.DecimalNames = [];
    $scope.isLoading = true;
    $scope.oneAtATime = false;
    $scope.status = {
        isCustomHeaderOpen: false,
        isFirstOpen: true,
        isFirstDisabled: false
    };

    function init() {
        var id = $location.absUrl();
        $scope.isLoading = true;
        configService.initialize().then(function (data) {
            $scope.isLoading = true;
            var id = $scope.getConfigID();
             configService.getCalc(id)
               .then(function (data) {
                   $scope.isLoading = true;
                   $scope.config = data;
               }, onError);
 
        }, onError);
    };

    $scope.getConfigID = function getConfigID() {
        var url = location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);
        id = parseInt(id, 10);
        if (angular.isNumber(id) == false) {
            id = null;
        }
        return id;
    }

     $scope.AddFunction = function (colIndex, index) {
         $scope.Functions = [];
         $scope.Functions = $scope.config[colIndex].Functions;
         $scope.config[colIndex].Functions.push({
             ID: this.config[colIndex].length,
             Logic: [],
             Parameter: []

         });
     }

     $scope.AddCategory = function (colIndex) {
         $scope.config.push({
             ID: this.config.length,
             Name: "New Category",
             Functions: []
         });
     }

     $scope.SaveButtonClick = function SaveBoard() {
         $scope.isLoading = true;
         var id = $scope.getConfigID();
         configService.putCalc(id, $scope.config).then(function (data) {
             $scope.isLoading = false;
         }, onError);
     };

     $scope.CalcButtonClick = function CalcBoard() {
         $scope.isLoading = true;
         var id = $scope.getConfigID();
         configService.postCalc(id, $scope.config).then(function (data) {
             $scope.isLoading = false;
             $scope.config = data;
         }, onError);
     };

     $scope.remItem = function (colIndex, $index) {
         $scope.config[colIndex].Functions.splice($index, 1);
     }

     $scope.selectedRow = null;  // initialize our variable to null
     $scope.function = null;  // initialize our variable to null

     $scope.setClickedRow = function (index) {  //function that sets the value of selectedRow to current index
         if (angular.isDefined($scope.selectedRow) & index == $scope.selectedRow)
         {
             $scope.selectedRow = null;
             $scope.getFunction('/');
         }
         else
         {
             $scope.selectedRow = index;
             $scope.function = this.rows.Function;
         }
     }

     $scope.getClickedRow = function () {  //function that sets the value of selectedRow to current index
         return $scope.selectedRow;
     }

     $scope.getParameter = function () {
        $scope.parameter = configService.getParameters();
        return $scope.parameter
     }

     $scope.getColid = function () {
         $scope.ColID = configService.getColid();
         return $scope.ColID
     }

     $scope.getRowid = function () {
         $scope.RowID = configService.getRowid();
         return $scope.RowID
     }

     $scope.$on('parameterupdated', function (event, data) {
         console.log(data);
     });

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
     }

     $scope.getVariableTypes = function getVariableTypes() {  //function that sets the parameters available under the different variable types
         var counter = 0;
         var scopeid = 0;
         var functionID = 0;

         angular.forEach($scope.config, function (groups) {
             $scope.Decimal = $filter('filter')($scope.config[scopeid].Functions, { Type: 'Decimal' });
             angular.forEach($scope.Decimal, function (functions) {
                 $scope.DecimalNames.push($scope.Decimal[functionID].Name);
                 functionID = functionID + 1
             });
             scopeid = scopeid + 1
         });
     }

     $scope.getFunctionCtrl = function getFunctionCtrl(Function) {
         if (Function == 'Maths') {
             return 'mathsCtrl';
         }
     }
    
     $scope.getFunctionTempURL = function getFunctionTempURL(Function) {
         if (Function == 'Maths') {
             return '/Areas/Configuration/Scripts/MathsModal.html';
         }
     }
    
     $scope.FunctionButtonClick = function (size, colIndex, index) {
         $scope.Parameter = this.config[colIndex].Functions[index].Parameter;
         var Function = this.config[colIndex].Functions[index].Function;
         $scope.getVariableTypes();
         var FunctionCtrl = null;
         var FunctionTemp = null;
         FunctionCtrl = $scope.getFunctionCtrl(Function);
         FunctionTemp = $scope.getFunctionTempURL(Function);       
         if (Function != 'Input') {
             var modalInstance = $uibModal.open({
                 animation: true,
                 templateUrl: FunctionTemp,
                 scope: $scope,
                 controller: FunctionCtrl,
                 size: size,
                 resolve: {
                     Functions: function () { return $scope.Parameter }
                 }
             });
             modalInstance.result.then(function (selectedItem) {
                 $scope.config[colIndex].Functions[index].Parameter = selectedItem;
             }, function () {
                 $log.info('Modal dismissed at: ' + new Date());
             });
         };

     };

     $scope.LogicButtonClick = function (size, colIndex, index) {
         $scope.Logic = this.config[colIndex].Functions[index].Logic;
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/LogicModal.html',
             scope: $scope,
             controller: 'logicCtrl',
             size: size,
             resolve: {
                 Logic: function () { return $scope.Logic }
             }
         });

         modalInstance.result.then(function (selectedItem) {
             $scope.config[colIndex].Functions[index].Logic = selectedItem;
         }, function () {
             $log.info('Modal dismissed at: ' + new Date());
         });
     };

    
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
