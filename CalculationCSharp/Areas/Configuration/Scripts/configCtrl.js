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
        configService.initialize().then(function (data) {
            $scope.isLoading = true;
            var id = $scope.getConfigID();
             configService.getCalc(id)
               .then(function (data) {
                   $scope.isLoading = false;
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
             ID: this.config[colIndex].Functions.length,
             Logic: [],
             Parameter: []

         });
     }

     $scope.CopyFunction = function (colIndex, index) {
         var Functions = $scope.config[colIndex].Functions[index];
         $scope.config[colIndex].Functions.push(angular.copy(Functions));
     }

     $scope.AddCategory = function (colIndex) {
         $scope.config.push({
             ID: this.config.length,
             Name: null,
             Description: null,
             Functions: []
         });

         $scope.GroupButtonClick('lg', this.config.length - 1);
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

    $scope.editingData = [];

     $scope.setFunction = function (colindex, index, rows) {  //function that sets the value of selectedRow to current index

         $scope.function = $scope.config[colindex].Functions[index].Function;

         if($scope.function == 'Maths')
         {
             $scope.config[colindex].Functions[index].Type = 'Decimal';
             $scope.editingData[rows.ID] = false;
         }
         else if ($scope.function == 'Period') {
             $scope.config[colindex].Functions[index].Type = 'Decimal';
             $scope.editingData[rows.ID] = false;
         }
         else if ($scope.function == 'Factors') {
             $scope.config[colindex].Functions[index].Type = 'Decimal';
             $scope.editingData[rows.ID] = false;
         }
         else if ($scope.function == 'Input')
         {
             $scope.config[colindex].Functions[index].Type = null;
             $scope.editingData[rows.ID] = true;
         }
         else
         {
             $scope.config[colindex].Functions[index].Type = null;
             $scope.editingData[rows.ID] = false;
         }
        
     }

     $scope.getVariableTypes = function getVariableTypes() {  //function that sets the parameters available under the different variable types
         var counter = 0;
         var scopeid = 0;
         var functionID = 0;
         $scope.Decimal = [];
         $scope.DecimalNames = [];
         angular.forEach($scope.config, function (groups) {
             $scope.Decimal = $filter('filter')($scope.config[scopeid].Functions, { Type: 'Decimal' });
             angular.forEach($scope.Decimal, function (functions) {
                 if ($scope.Decimal[functionID].Name != null) {
                     $scope.DecimalNames.push($scope.Decimal[functionID].Name);
                 }
                 functionID = functionID + 1
             });
             scopeid = scopeid + 1
         });
     }

     $scope.getFunctionCtrl = function getFunctionCtrl(Function) {
         if (Function == 'Maths') {
             return 'mathsCtrl';
         }
         else if (Function == 'Period') {
             return 'periodCtrl'
         }
         else if (Function == 'Factors') {
             return 'factorsCtrl'
         }

     }
    
     $scope.getFunctionTempURL = function getFunctionTempURL(Function) {
         if (Function == 'Maths') {
             return '/Areas/Configuration/Scripts/MathsModal.html';
         }
         else if (Function == 'Period') {
             return '/Areas/Configuration/Scripts/PeriodModal.html'
         }
                  else if (Function == 'Factors') {
             return '/Areas/Configuration/Scripts/FactorsModal.html'
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

     $scope.GroupButtonClick = function (size, colIndex) {
         $scope.Name = this.config[colIndex].Name;
         $scope.Description = this.config[colIndex].Description;
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/GroupModal.html',
             scope: $scope,
             controller: 'groupCtrl',
             size: size,
             resolve: {
                 Name: function () { return $scope.Name },
                 Description: function () { return $scope.Description }
             }
         });

         modalInstance.result.then(function (selectedItem) {
             $scope.config[colIndex].Name = selectedItem[0].Name;
             $scope.config[colIndex].Description = selectedItem[0].Description;
            
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
