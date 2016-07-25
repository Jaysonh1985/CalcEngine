sulhome.kanbanBoardApp.controller('regressionCtrl', function ($scope, $uibModal, $uibModalInstance, $log, $http, $location, configService, ID, calculationService, ObjectDiff) {

    function init() {
      $scope.isLoading = true;
        configService.getRegression(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Regression = data;
           }, onError);
    };

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
                Input: angular.toJson($scope.Regression[colIndex].Input, true),
                Comment: $scope.Regression[colIndex].Comment,
                UpdateDate: ""

            };

            configService.putRegression($scope.Regression[colIndex].ID, $scope.selected).then(function (data) {

            }, onError);


        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });


    };

    $scope.DifferencesButtonClick = function (size, colIndex, index) {
        $scope.Difference = this.Regression[colIndex].Difference;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/RegressionDifferenceModal.html',
            scope: $scope,
            controller: 'regressionDifferenceCtrl',
            size: size,
            resolve: {
                Difference: function () { return $scope.Difference },
            }
        });
        modalInstance.result.then(function (selectedItem) {


        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });


    };


     $scope.OutputButtonClick = function (size, colIndex, type) {

     if(type == "O")
     {
         $scope.Output = this.Regression[colIndex].OutputOld;
     }
     else
     {
         $scope.Output = this.Regression[colIndex].OutputNew;
     }
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/RegressionOutputModal.html',
            scope: $scope,
            controller: 'regressionOutputCtrl',
            size: size,
            resolve: {
                Output: function () { return $scope.Output },
                Input: function () { return $scope.Input }
            }
        });
        modalInstance.result.then(function (selectedItem) {


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


     $scope.RunAllButtonClick = function CalcButtonClick() {

         $scope.isLoading = true;

         $scope.array = [];

         $scope.array.push($scope.formset);
         $scope.prop = [];
         $scope.val = [];
         $scope.obj = [];

         angular.forEach($scope.Regression, function (value, key, obj) {

             angular.forEach(angular.fromJson(angular.fromJson(angular.fromJson($scope.Regression[key].Input))), function (value, key, obj) {
                 $scope.prop.push(value);
                 var index = getIndexOf($scope.config[0].Functions, key, 'Name');
                 $scope.config[0].Functions[index].Output = value;

            });

            calculationService.postCalc(1, $scope.config).then(function (data) {
                $scope.isLoading = false;

                if ($scope.Regression[key].OutputOld == null)
                {
                    $scope.Regression[key].OutputOld = angular.toJson(data,true);
                }
                else
                {
                    $scope.Regression[key].OutputNew = angular.toJson(data, true);
                }

                // you can directly diff your objects js now or parse a Json to object and diff
                var diff = ObjectDiff.diffOwnProperties(angular.fromJson($scope.Regression[key].OutputOld), angular.fromJson($scope.Regression[key].OutputNew));

                // gives a full object view with Diff highlighted
                $scope.diffValue = ObjectDiff.toJsonView(diff);

                // gives object view with onlys Diff highlighted
                $scope.diffValueChanges = ObjectDiff.toJsonDiffView(diff);

                $scope.Regression[key].Difference = '<table class="table table-bordered table table-responsive">' + 
                                                    '<tr><th>Variable Name</th><th>Key</th><th>Old Value</th><th>New Value</th></tr>'+             
                                                    $scope.diffValueChanges + '</table>';

                $scope.selected = {

                    ID: $scope.Regression[key].ID,
                    CalcID: $scope.Regression[key].CalcID,
                    Scheme: $scope.Regression[key].Scheme,
                    Input: $scope.Regression[key].Input,
                    Type: $scope.Regression[key].Type,
                    Comment: $scope.Regression[key].Comment,
                    OriginalRunDate: $scope.Regression[key].OriginalRunDate,
                    LatestRunDate: $scope.Regression[key].LatestRunDate,
                    OutputOld: $scope.Regression[key].OutputOld,
                    OutputNew: $scope.Regression[key].OutputNew,
                    Difference: $scope.Regression[key].Difference,
                    Pass: $scope.Regression[key].Pass,
                    UpdateDate: ""

                };

                configService.putRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {

                }, onError);
               
            });

         });
        
     };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    init();

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

})
