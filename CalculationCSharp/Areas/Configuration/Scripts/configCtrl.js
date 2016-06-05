﻿sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService) {
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
               $scope.updateParameter();
            }, onError);
     };


     if ($scope.config !== null) {
         $scope.config = ($scope.config);
     }

     $scope.AddFunction = function (colIndex, index) {
         $scope.config[colIndex].Functions.push({

             ID: this.config[colIndex].Functions.length
                 
             });
     }

     $scope.AddCategory = function (colIndex, index) {
         $scope.config.push({

             ID: this.config.length,
             Name: "New Category"


         });
     }

     $scope.SaveButtonClick = function SaveBoard() {
         $scope.isLoading = true;
         configService.updateConfig($scope.config).then(function (data) {
             $scope.isLoading = false;
             $scope.refreshConfig();
         }, onError);
     };

     $scope.remItem = function ($index) {
         $scope.config.splice($index, 1);
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

     $scope.updateParameter = function () {
                  
         $scope.updateCol = $scope.getColid();
         $scope.updateRow = $scope.getRowid();
         $scope.parameter = $scope.getParameter();

         if ($scope.updateRow !== null & angular.isDefined($scope.updateRow)) {
             $scope.config[$scope.updateCol].Functions[$scope.updateRow].Parameter = $scope.parameter;
         }

         
     }    

     $scope.modify = function (colIndex, index) {

         $scope.editingData = {};

         for (var i = 0, length = $scope.config[colIndex].Functions.length; i < length; i++) {
             $scope.editingData[$scope.config[colIndex].Functions[i].ID] = false;
         }

         if ($scope.function == "Input") {
             $scope.editingData[index] = true;
         }

     };

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

     $scope.editFunction = function (colIndex,index) {  //function that sets the value of selectedRow to current index

         $scope.function = this.rows.Function;
         $scope.disableSelect = true;
         $scope.SaveButtonClick();


         $scope.parameter = $scope.config[colIndex].Functions[index].Parameter;

         if ($scope.parameter == "") {
             $scope.parameter = null;
         }
         configService.setParameters($scope.parameter, colIndex, index);


         if($scope.function != "Input")
         {
             $scope.getFunction($scope.function);
             $scope.modify(colIndex,index);
         }
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
