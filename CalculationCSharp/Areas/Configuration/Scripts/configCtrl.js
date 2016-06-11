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

            var url = location.pathname;
            var id = url.substring(url.lastIndexOf('/') + 1);
            id = parseInt(id, 10);
            if (angular.isNumber(id) == false) {
                id = null;
            }

            configService.getCalc(id)
               .then(function (data) {
                   $scope.isLoading = true;
                   $scope.config = data;
               }, onError);


        }, onError);
    };

    $scope.refreshConfig = function refreshBoard() {
        $scope.routeID = $routeParams.ID;
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
         $scope.Functions = [];
         $scope.Functions = $scope.config[colIndex].Functions;
         $scope.config[colIndex].Functions.push({

             ID: this.config[colIndex].length,
             logic: []

         });
     }

     $scope.AddCategory = function (colIndex, index) {
         $scope.config.push({
             ID: this.config.length,
             Name: "New Category",
             Functions: []
         });
     }

     $scope.SaveButtonClick = function SaveBoard() {
         $scope.isLoading = true;

         var url = location.pathname;
         var id = url.substring(url.lastIndexOf('/') + 1);
         id = parseInt(id, 10);
         if (angular.isNumber(id) == false) {
             id = null;
         }

         configService.putCalc(id, $scope.config).then(function (data) {
             $scope.isLoading = false;
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

         
         $scope.SaveButtonClick();


         $scope.parameter = $scope.config[colIndex].Functions[index].Parameter;

         if ($scope.parameter == "") {
             $scope.parameter = null;
         }
         configService.setParameters($scope.parameter, colIndex, index, $scope.DecimalNames);


         if($scope.function != "Input")
         {
             $scope.getFunction($scope.function);
             $scope.modify(colIndex,index);
         }
     }

     $scope.LogicButtonClick = function (size, colIndex, index) {


         $scope.logic = this.config[0].Functions[0].logic;
  

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Configuration/Scripts/LogicModal.html',
             scope: $scope,
             controller: 'logicCtrl',
             size: size,
             resolve: {
                 logic: function () { return $scope.logic }
             }
         });

         modalInstance.result.then(function (selectedItem) {

             $scope.config[colIndex].Functions[index].logic = selectedItem;

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
