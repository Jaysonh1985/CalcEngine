// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('calculationCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configFunctionFactory, calculationService, $filter) {

    $scope.output = [];
    $scope.formset = [];
    $scope.fieldset = [];
    $scope.bulkarray = [];
    $scope.bulkArrayOutput = [];
    var vm = this;
    $scope.openIndex = [true];
    $scope.openIndexOuter = [true];
    $scope.openIndexRegression = [true];
    $scope.csv = {
        content: null,
        header: true,
        headerVisible: true,
        separator: ',',
        separatorVisible: true,
        result: null,
        encoding: 'ISO-8859-1',
        encodingVisible: true,
    };

    vm.model = [];
    //Initialise
    function init() {
        calculationService.initialize().then(function (data) {
                $scope.isLoading = true;
                var id = configFunctionFactory.getConfigID();
                 calculationService.getCalc(id)
                   .then(function (data) {
                       $scope.isLoading = false;
                       $scope.config = data;
                       $scope.getFormFields();
                       $scope.getCSVFields();
                   });
            });
    };
    //Single Calculation
    //Get the fields for the input form and null the values out
    $scope.getFormFields = function getFormFields() {  //function that sets the parameters available under the different variable types
        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        $scope.fields = [];
        $scope.fieldset = [];
        $scope.configreg = configFunctionFactory.convertToFromJson($scope.config[0]);
        angular.forEach($scope.configreg.Functions, function (groups) {
            functionID = 0;
            $scope.configreg.Functions[scopeid].Output = null;
            scopeid = scopeid + 1
        });
    }
    //Calculation Button click - this checks the form is valid then maps the inputs into the config and passes the calculation over to the server for calculation
    $scope.CalcButtonClick = function CalcButtonClick(form) {
        if (form.$valid == true) {
            $scope.isLoading = true;
            angular.forEach($scope.configreg.Functions, function (value, key, obj) {
                var index = configFunctionFactory.getIndexOf($scope.config[0].Functions, value.Name, 'Name');
                $scope.config[0].Functions[index].Output = value.Output;
            });
            calculationService.postCalc(1, $scope.config).then(function (data) {
                $scope.isLoading = false;
                $scope.output = data;
                toastr.success("Calculated successfully", "Success");
            });
        }
        else {
            $scope.validationError = true;
            toastr.error("Failed Validations", "Error");
        }
    };
    //Bulk Calculations
    //Get the row and group names for the CSV file in the bulk calculations
    $scope.getCSVFields = function getFormFields() {  //function that sets the parameters available under the different variable types
        var CSVcounter = 0;
        $scope.CSVfields = [];
        angular.forEach($scope.configreg.Functions, function (groups) {
            $scope.CSVfields.push($scope.configreg.Functions[CSVcounter].Name);
            CSVcounter = CSVcounter + 1
        });
    }
    //Get Header row for the CSV file
    $scope.getHeader = function () { return $scope.CSVfields };
    $scope.BulkCalcButtonClick = function CalcButtonClick(input) {
        $scope.bulkArrayOutput = [];
        angular.forEach(input, function (value, key, obj) {       
            angular.forEach(value, function (value, key, obj) {
                var index = configFunctionFactory.getIndexOf($scope.config[0].Functions, key, 'Name');
                $scope.config[0].Functions[index].Output = value;
            })
            $scope.bulkarray = $scope.config;
            $scope.BulkOutputBuilder($scope.bulkarray);
            $scope.bulkarray = [];
        })
        $scope.BulkOutputArray = [];
        angular.forEach($scope.bulkArrayOutput, function (value, key, obj) {         
            $scope.BulkOutputArray.push(angular.fromJson(value));
        })
       var promise = calculationService.postBulkCalc(1, $scope.BulkOutputArray).then(function (data) {
            $scope.isLoading = false;
            $scope.BulkOutput = data;
            toastr.success("Parsed successfully", "Success");
            return data;
       });
       return promise;
    };

    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.output, function (value, key, obj) {
            $scope.openIndex[key] = true;
        })
    }

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndex, function (value, key, obj) {
            $scope.openIndex[key] = false;
        })
    }
    //Creates the array of the output
    $scope.BulkOutputBuilder = function BulkOutputBuilder(Output) {
        var Test = [];
        Test = angular.toJson(Output);
        $scope.bulkArrayOutput.push(Test);
    };

    init();
});