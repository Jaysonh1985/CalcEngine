sulhome.kanbanBoardApp.controller('regressionCtrl', function ($scope, $uibModalInstance, $log, $http, $location,configService, ID) {
    
    function init() {
      $scope.isLoading = true;
        configService.getRegression(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Regression = data;
           }, onError);
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

    $scope.FunctionButtonClick = function (size, colIndex, index) {

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

    $scope.Add = function () {
        
        $scope.isLoading = true;
        $scope.selected = {
            CalcID: ID,
            Comment: "",
        };

        configService.postRegression(ID, $scope.selected).then(function (data) {
            $scope.Regression.push(data);
            $scope.isLoading = false;
        }, onError);

    },


    $scope.removeRegressionItem = function (index) {

        $scope.removeRegressionItem = function (index) {
            var cf = confirm("Delete this Row?");
            if (cf == true) {
                configService.deleteRegression(this.Regression[index].ID)
            .then(function (data) {
                $scope.isLoading = false;
                $scope.Regression.splice(index, 1);
            }, onError);
            }
        };
    },

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    init();

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

})
