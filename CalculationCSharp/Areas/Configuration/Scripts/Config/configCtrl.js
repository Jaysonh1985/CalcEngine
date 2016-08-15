sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService, $filter) {
    // Model
    $scope.config = [];
    $scope.DecimalNames = [];
    $scope.isLoading = true;
    $scope.oneAtATime = false;
    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
    $scope.openIndex = [true];
    $scope.openIndexRegression = [true];

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
         var item = null;

         item = angular.copy(Functions);
         $scope.config[colIndex].Functions.splice(index + 1, 0, item);

     }

     $scope.DeleteFunction = function (colIndex, $index) {
         var cf = confirm("Delete this line?");
         if (cf == true) {
             $scope.config[colIndex].Functions.splice($index, 1);
         }
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

     $scope.AddCategoryRows = function (colIndex) {

         var item = null;

         item = {
             ID: colIndex + 1,
             Name: null,
             Description: null,
             Functions: []
         };

         $scope.config.splice(colIndex + 1, 0, item);
         $scope.GroupButtonClick('lg', colIndex + 1);
         $scope.rebuildCategoryIDs();
     }

     $scope.CopyCategory = function (colIndex, index, e) {

         var Category = $scope.config[colIndex];
         var item = null;

         item = angular.copy(Category);
         $scope.config.splice(index + 1, 0, item);

     }

     $scope.MoveDownCategory = function (colIndex, Index, e) {

         if (e) {
             e.preventDefault();
             e.stopPropagation();
         }

         var Category = $scope.config[Index];
         var item = null;

         item = angular.copy(Category);
         $scope.config.splice(Index, 1);
         $scope.config.splice(Index + 1, 0, item);

     }

     $scope.DeleteCategory = function (colIndex) {
         var cf = confirm("Delete this line?");
         if (cf == true) {
             $scope.config.splice(colIndex, 1);
             $scope.rebuildCategoryIDs();
         }
     }

     $scope.rebuildCategoryIDs = function rebuildCategoryIDs() {
         colid = 0;
         angular.forEach($scope.config, function (groups) {
             $scope.config[colid].ID = colid;
             colid = colid + 1;
         });
     }

     $scope.SaveButtonClick = function SaveBoard() {
         $scope.isLoading = true;
         var id = $scope.getConfigID();
         configService.putCalc(id, $scope.config).then(function (data) {
             $scope.isLoading = false;
             toastr.success("Saved successfully", "Success");
         }, onError);
     };

     $scope.CalcButtonClick = function CalcBoard($event) {
         var id = $scope.getConfigID();
         configService.postCalc(id, $scope.config).then(function (data) {
             $scope.isLoading = false;
             $scope.config = data;
             toastr.success("Calculated successfully", "Success");
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
             $scope.editingData[rows.ID] = true;
         }
         else if (rows.Function == 'Dates') {
             rows.Type = 'Date';
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

     $scope.variableArrayBuilder = function variableArrayBuilder(config, colIndex, type) {

         var counter = 0;
         var scopeid = 0;
         var functionID = 0;
         var arrayID = 0;
         $scope.Decimal = [];
         $scope.DecimalValue = [];
         $scope.DecimalParameter = [];
         $scope.Names = [];
         var newArr = [];

         angular.forEach(config, function (groups) {
             if (scopeid <= colIndex) {

                 if(type == null)
                 {
                     $scope.DecimalValue = ($filter('filter')(config[scopeid].Functions));
                 }
                 else
                 {
                     $scope.DecimalValue = ($filter('filter')(config[scopeid].Functions, { Type: type }));
                 }
                 
                 functionID = 0;
                 angular.forEach($scope.DecimalValue, function (Names) {
                     $scope.DecimalParameter = ($filter('filter')($scope.DecimalValue[functionID].Name));
                     $scope.Names[arrayID] = $scope.DecimalParameter;
                     functionID = functionID + 1;
                     arrayID = arrayID + 1;
                 });
                 scopeid = scopeid + 1
             }
         });
         scopeid = 0;


         return $scope.Names;

     }


     $scope.getVariableTypes = function getVariableTypes(colIndex) {  //function that sets the parameters available under the different variable types

         $scope.DecimalNames = [];
         $scope.DateNames = [];
         $scope.StringNames = [];

         $scope.DecimalNames = $scope.variableArrayBuilder($scope.config, colIndex, 'Decimal');
         $scope.DateNames = $scope.variableArrayBuilder($scope.config, colIndex, 'Date');
         $scope.StringNames = $scope.variableArrayBuilder($scope.config, colIndex, 'String');

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
         else if (Function == 'Dates') {
             return 'dateAdjustmentCtrl'
         }
         else if (Function == 'Input') {
             return 'inputCtrl'
         }

     }
    
     $scope.getFunctionTempURL = function getFunctionTempURL(Function) {
         if (Function == 'Maths') {
             return '/Areas/Configuration/Scripts/Maths/MathsModal.html';
         }
         else if (Function == 'Period') {
             return '/Areas/Configuration/Scripts/Period/PeriodModal.html'
         }
         else if (Function == 'Factors') {
             return '/Areas/Configuration/Scripts/Factors/FactorsModal.html'
         }
         else if (Function == 'Dates') {
             return '/Areas/Configuration/Scripts/Date Adjustment/DateAdjustmentModal.html'
         }
         else if (Function == 'Input') {
             return '/Areas/Configuration/Scripts/Input/InputModal.html'
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
         
         $scope.AllNames = [];
         $scope.AllNames = $scope.variableArrayBuilder($scope.config, colIndex, null);

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/Logic/LogicModal.html',
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
         $scope.ID = this.config[colIndex].ID;   
         $scope.Name = this.config[colIndex].Name;
         $scope.Description = this.config[colIndex].Description;
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/Group/GroupModal.html',
             scope: $scope,
             controller: 'groupCtrl',
             size: size,
             resolve: {
                 ID: function () { return $scope.ID },
                 Name: function () { return $scope.Name },
                 Description: function () { return $scope.Description }
             }
         });

         modalInstance.result.then(function (selectedItem) {
             $scope.config[colIndex].ID = selectedItem[0].ID;
             $scope.config[colIndex].Name = selectedItem[0].Name;
             $scope.config[colIndex].Description = selectedItem[0].Description;
            
         }, function () {
             $log.info('Modal dismissed at: ' + new Date());
         });
     };

     $scope.HistoryButtonClick = function (size) {
         $scope.ID = $scope.getConfigID();
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/History/HistoryModal.html',
             scope: $scope,
             controller: 'historyCtrl',
             size: size,
             resolve: {
                 ID: function () { return $scope.ID },
             }
         });

         modalInstance.result.then(function (selectedItem) {

             $scope.config = JSON.parse(selectedItem);
             toastr.success("Reverted successfully", "Success");

         }, function () {
             $log.info('Modal dismissed at: ' + new Date());
         });
     };

     $scope.RegressionButtonClick = function (size) {
         $scope.ID = $scope.getConfigID();
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionModal.html',
             scope: $scope,
             controller: 'regressionCtrl',
             size: size,
             resolve: {
                 ID: function () { return $scope.ID },
             }
         });

         modalInstance.result.then(function (selectedItem) {

             $scope.config = JSON.parse(selectedItem);
             toastr.success("Reverted successfully", "Success");

         }, function () {
             $log.info('Modal dismissed at: ' + new Date());
         });
     };

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
