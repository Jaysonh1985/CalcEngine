sulhome.kanbanBoardApp.controller('regressionCtrl', function ($scope, $uibModal, $uibModalInstance, $log, $http, $location,configService, ID) {

    function init() {
      $scope.isLoading = true;
        configService.getRegression(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Regression = data;
           }, onError);
    };

    $scope.FunctionButtonClick = function (size, colIndex, index) {
        $scope.Input = this.Regression[colIndex].Input;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/RegressionInputModal.html',
            scope: $scope,
            controller: 'regressionInputCtrl',
            size: size,
            resolve: {
                Functions: function () { return $scope.config },
                Input: function () { return $scope.Input }
            }
        });
        modalInstance.result.then(function (selectedItem) {

            $scope.Regression[colIndex].Input = selectedItem;

            $scope.selected = {

                ID: $scope.Regression[colIndex].ID,
                CalcID: $scope.Regression[colIndex].CalcID ,
                Scheme: $scope.Regression[colIndex].Scheme,
                Type: $scope.Regression[colIndex].Type,
                Input: angular.toJson($scope.Regression[colIndex].Input),
                Comment: $scope.Regression[colIndex].Comment,
                UpdateDate: ""

            };

            configService.putRegression($scope.Regression[colIndex].ID, $scope.selected).then(function (data) {

            }, onError);


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
