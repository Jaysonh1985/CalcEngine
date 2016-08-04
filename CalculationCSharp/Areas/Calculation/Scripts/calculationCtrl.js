sulhome.kanbanBoardApp.controller('calculationCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, calculationService, $filter) {

    $scope.output = [];
    $scope.formset = [];
    $scope.fieldset = [];
    var vm = this;

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

    function init() {
        calculationService.initialize().then(function (data) {
                $scope.isLoading = true;
                var id = $scope.getConfigID();
                 calculationService.getCalc(id)
                   .then(function (data) {
                       $scope.isLoading = false;
                       $scope.config = data;
                       $scope.getFormFields();
                       $scope.getCSVFields();
                   });
            });
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
    $scope.getCSVFields = function getFormFields() {  //function that sets the parameters available under the different variable types
        var CSVcounter = 0;
        $scope.CSVfields = [];
        angular.forEach($scope.fieldset, function (groups) {
            $scope.CSVfields.push($scope.fieldset[CSVcounter].key);
            CSVcounter = CSVcounter + 1
        });
    }

    var _lastGoodResult = '';
    $scope.toPrettyJSON = function (json, tabWidth) {
        var objStr = JSON.stringify(json);
        var obj = null;
        try {
            obj = $parse(objStr)({});
        } catch (e) {
            // eat $parse error
            return _lastGoodResult;
        }

        var result = JSON.stringify(obj, null, Number(tabWidth));
        _lastGoodResult = result;

        return result;
    };

    $scope.getHeader = function () { return $scope.CSVfields };
    $scope.getBulkOutput = function () { return $scope.BulkOutput };

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


    $scope.BulkCalcButtonClick = function CalcButtonClick(input) {

        $scope.bulkarray = [];

        angular.forEach(input, function (value, key, obj) {
            
            angular.forEach(value, function (value, key, obj) {

                var index = getIndexOf($scope.config[0].Functions, key, 'Name');

                $scope.config[0].Functions[index].Output = value;

            })

            $scope.bulkarray.push($scope.config);

        })

        calculationService.postBulkCalc(1, $scope.bulkarray).then(function (data) {
            $scope.isLoading = false;
            $scope.BulkOutput = data;
            toastr.success("Parsed successfully", "Success");
        });

    };


    $scope.CalcButtonClick = function CalcButtonClick() {
     
        $scope.isLoading = true;

        console.log("2");
        $scope.array = [];

        $scope.array.push($scope.formset);
        $scope.prop = [];
        $scope.val = [];
        $scope.obj = [];

        angular.forEach($scope.formset.fields, function (value, key, obj) {
          
                $scope.prop.push(value);
                var index = getIndexOf($scope.config[0].Functions, key, 'Name');
                $scope.config[0].Functions[index].Output = value;
        });


        calculationService.postCalc(1, $scope.config).then(function (data) {
            $scope.isLoading = false;
            $scope.output = data;
            toastr.success("Calculated successfully", "Success");
        });

    };
        
    init();
});