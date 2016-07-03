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
         $scope.config[colIndex].Functions.push({
             ID: this.config[colIndex].Functions.length,
             Logic: [],
             Parameter: []

         });
     }

     $scope.AddFunctionRows = function (colIndex, index) {
         var item = null;
         item = {
             ID: this.config[colIndex].Functions.length,
             Logic: [],
             Parameter: []
         };
         $scope.config[colIndex].Functions.splice(index + 1, 0, item);
     }

     $scope.CopyFunction = function (colIndex, index) {
         var Functions = $scope.config[colIndex].Functions[index];
         $scope.config[colIndex].Functions.push(angular.copy(Functions));
     }

     $scope.DeleteFunction = function (colIndex, $index) {
         $scope.config[colIndex].Functions.splice($index, 1);
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

     $scope.selectedRow = null;  // initialize our variable to null
     $scope.function = null;  // initialize our variable to null

     $scope.$on('parameterupdated', function (event, data) {
         console.log(data);
     });

    $scope.editingData = [];

     $scope.setFunction = function (rows) {  //function that sets the value of selectedRow to current index

         if(rows.Function == 'Maths')
         {
             rows.Type = 'Decimal';
             $scope.editingData[rows.ID] = true;
         }
         else if (rows.Function == 'Period') {
             rows.Type = 'Decimal';
             $scope.editingData[rows.ID] = true;
         }
         else if (rows.Function == 'Factors') {
             rows.Type = 'Decimal';
             $scope.editingData[rows.ID] = true;
         }
         else if (rows.Function == 'Input')
         {
             rows.Type = null;
             $scope.editingData[rows.ID] = false;
         }
         else
         {
             rows.Type = null;
             $scope.editingData[rows.ID] = false;
         }
        
     }

     $scope.getVariableTypes = function getVariableTypes(colIndex) {  //function that sets the parameters available under the different variable types
         var counter = 0;
         var scopeid = 0;
         var functionID = 0;
         var arrayID = 0;
         $scope.Decimal = [];
         $scope.DecimalValue = [];
         $scope.DecimalParameter = [];
         $scope.DecimalNames = [];
         angular.forEach($scope.config, function (groups) {
             if (scopeid <= colIndex)
             {

                 $scope.DecimalValue = ($filter('filter')($scope.config[scopeid].Functions, { Type: 'Decimal' }));

                 functionID = 0;
                 angular.forEach($scope.DecimalValue, function (Names) {
                     $scope.DecimalParameter = ($filter('filter')($scope.DecimalValue[functionID].Name));
                     $scope.DecimalNames[arrayID] = $scope.DecimalParameter;
                     functionID = functionID + 1;
                     arrayID = arrayID + 1;
                 });
                scopeid = scopeid + 1
             }
         });

         scopeid = 0;

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
         else if (Function == 'Input') {
             return 'inputCtrl'
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
         else if (Function == 'Input') {
             return '/Areas/Configuration/Scripts/InputModal.html'
         }
     }
    
     $scope.FunctionButtonClick = function (size, colIndex, index) {
         $scope.Parameter = this.config[colIndex].Functions[index].Parameter;
         var Function = this.config[colIndex].Functions[index].Function;
         $scope.getVariableTypes(colIndex);
         var FunctionCtrl = null;
         var FunctionTemp = null;
         FunctionCtrl = $scope.getFunctionCtrl(Function);
         FunctionTemp = $scope.getFunctionTempURL(Function);       

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

                 if ($scope.config[colIndex].Functions[index].Function == 'Input') {
                     $scope.config[colIndex].Functions[index].Name = selectedItem[0].key;
                     $scope.config[colIndex].Functions[index].Type = selectedItem[0].templateOptions.type;
                 }
             }, function () {
                 $log.info('Modal dismissed at: ' + new Date());
             });
        

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
