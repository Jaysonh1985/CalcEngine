// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('functionCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions, configService, configFunctionFactory) {
    // Model
    $scope.function = [];

    function init() {
        $scope.SchemeList = [];
        configService.getSchemes().then(function (data) {
            $scope.SchemeList = data;
        }, onError);
    };

    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.function.ID = Functions[0].ID;
        $scope.function.Scheme = Functions[0].Scheme;
        $scope.function.FunctionName = Functions[0].FunctionName;
        configService.getFunctionDetails("Test", Functions[0].ID, "Config").then(function (data) {
            $scope.getFormFields(angular.fromJson(data[0].Configuration));
            if (Functions[0].Input.length > 0) {
                $scope.mapFormFields(Functions[0].Input);
            };            
        }, onError);
    }
    else {
        $scope.function = Functions
    };

    //Set up new object
    $scope.selected = [];

    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            ID: parseInt($scope.function.ID),
            Scheme: $scope.function.Scheme,
            FunctionName: $scope.function.FunctionName,
            Input: $scope.configreg.Functions,
        });
    };

    //Add new Item to the selected array
    $scope.getFunctionList = function getFunctionList() {
        $scope.FunctionList = [];
        configService.getFunctionDetails($scope.function.Scheme, 0, "Scheme").then(function (data) {
            $scope.FunctionList = data;
        }, onError);
    };

    //Add new Item to the selected array
    $scope.setFunctionName = function setFunctionName() {
        var arrayID = configFunctionFactory.getIndexOf($scope.FunctionList, parseInt($scope.function.ID), "ID");
        $scope.function.FunctionName = $scope.FunctionList[arrayID].Name;
        configService.getFunctionDetails($scope.function.Scheme, $scope.FunctionList[arrayID].ID, "Config").then(function (data) {
            $scope.getFormFields(angular.fromJson(data[0].Configuration));
        }, onError);       
    };

        //Single Calculation
    //Get the fields for the input form and null the values out
    $scope.getFormFields = function getFormFields(array) {  //function that sets the parameters available under the different variable types
        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        $scope.fields = [];
        $scope.fieldset = [];
        $scope.configreg = configFunctionFactory.convertToFromJson(array[0]);
        angular.forEach($scope.configreg.Functions, function (groups) {
            functionID = 0;
            $scope.configreg.Functions[scopeid].Output = null;
            scopeid = scopeid + 1
        });
    };

    $scope.mapFormFields = function mapFormFields(Input) {
        var InputJson = angular.fromJson(Input);
        convertDateStringsToDates([InputJson]);
        $scope.isLoading = true;
        angular.forEach(angular.fromJson(InputJson), function (value, key, obj) {
            var index = configFunctionFactory.getIndexOf($scope.configreg.Functions, value.Name, 'Name');
            $scope.configreg.Functions[index].Output = value.Output;
        });
    };

    var regexIso8601 = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*))(?:Z|(\+|-)([\d|:]*))?$/;;

    function convertDateStringsToDates(input) {
        // Ignore things that aren't objects.
        if (typeof input !== "object") return input;

        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;
            var value = input[key];
            var match;
            // Check for string properties which look like dates.
            if (typeof value === "string" && (match = value.match(regexIso8601))) {
                var milliseconds = Date.parse(match[0])
                if (!isNaN(milliseconds)) {
                    input[key] = new Date(milliseconds);
                }
            } else if (typeof value === "object") {
                // Recurse into object
                convertDateStringsToDates(value);
            };
        }
    };

    //Click OK moves back to modal instantiation
    $scope.ok = function (form) {
        if (form.$valid == true) {
            $scope.addItem();
            $uibModalInstance.close($scope.selected);
        }
        else {
            toastr.error("Failed Validations", "Error");
        };
    };

    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    
    init();

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };
})
