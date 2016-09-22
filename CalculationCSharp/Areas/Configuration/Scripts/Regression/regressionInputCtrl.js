sulhome.kanbanBoardApp.controller('regressionInputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions, configFunctionFactory, Input, $filter) {
    
    $scope.output = [];
    $scope.formset = [];
    $scope.fieldset = [];
    var vm = this;

    vm.model = [];

    function init() {
        $scope.isLoading = false;
        $scope.config = Functions;
        $scope.fieldset = [];
        $scope.getFormFields();
        if (Input != null)
        {
            $scope.mapFormFields(Input);
        }
    };
    
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

    $scope.mapFormFieldsfromBuilder = function()
    {
        angular.forEach($scope.config[0].Functions, function (value, key, obj) {
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
            }
        }
    }

    $scope.mapFormFields = function mapFormFields(Input) {
       var InputJson = angular.fromJson(Input);
       convertDateStringsToDates([InputJson]);
        $scope.isLoading = true;
        angular.forEach(angular.fromJson(InputJson.Functions), function (value, key, obj) {
            var index = configFunctionFactory.getIndexOf($scope.configreg.Functions, value.Name, 'Name');
            $scope.configreg.Functions[index].Output = value.Output;
        });
    }

    $scope.SaveButtonClick = function SaveButtonClick(form) {  //function that sets the parameters available under the different variable types     

        if (form.$valid == true) {
            $uibModalInstance.close($scope.configreg);
            toastr.success("Saved successfully", "Success");
        }
        else {
            toastr.error("Failed Validations", "Error");
        }
    }

    init();

})
