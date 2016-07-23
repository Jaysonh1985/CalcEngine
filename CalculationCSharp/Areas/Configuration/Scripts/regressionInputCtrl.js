sulhome.kanbanBoardApp.controller('regressionInputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions, Input, $filter) {
    
    $scope.output = [];
    $scope.formset = [];
    $scope.fieldset = [];
    var vm = this;

    vm.model = [];

    function init() {

        $scope.isLoading = false;
        $scope.config = Functions;
        $scope.Input = angular.fromJson(Input);
        $scope.fieldset = [];
        $scope.getFormFields();
        $scope.mapFormFields($scope.Input);

    };


    $scope.getFormFields = function getFormFields() {  //function that sets the parameters available under the different variable types
        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        $scope.fields = [];
        $scope.fieldset = [];
        angular.forEach($scope.config, function (groups) {
            functionID = 0;
            $scope.fields = $filter('filter')($scope.config[scopeid].Functions, { Function: 'Input' });
            angular.forEach($scope.fields, function (functions) {
                $scope.fieldset.push($scope.fields[functionID].Parameter[0]);
                functionID = functionID + 1
            });
            scopeid = scopeid + 1
        });
    }

    function getIndexOf(arr, val, prop) {
        var l = arr.length,
          k = 0;
        for (k = 0; k < l; k = k + 1) {
            if (arr[k][prop] === val) {
                return k;
            }
        }
        return false;
    };
    
    var regexIso8601 = /^(\d{4}|\+\d{6})(?:-(\d{2})(?:-(\d{2})(?:T(\d{2}):(\d{2}):(\d{2})\.(\d{1,})(Z|([\-+])(\d{2}):(\d{2}))?)?)?)?$/;

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
        
       convertDateStringsToDates(Input);

        $scope.isLoading = true;

        $scope.array = [];

        $scope.array.push($scope.formset);
        $scope.prop = [];
        $scope.val = [];
        $scope.obj = [];

        angular.forEach(angular.fromJson(Input), function (value, key, obj) {

            $scope.prop.push(value);
            var index = getIndexOf($scope.fieldset, key, 'key');
            $scope.fieldset[index].defaultValue = value;
        });

        if (Input == null) {
            angular.forEach($scope.fieldset, function (value, obj, iterator) {

                $scope.fieldset[obj].defaultValue = null;
            })
        };

    }

    $scope.SaveButtonClick = function getFormFields() {  //function that sets the parameters available under the different variable types     

        $uibModalInstance.close($scope.formset.fields);
    }

    init();

})
