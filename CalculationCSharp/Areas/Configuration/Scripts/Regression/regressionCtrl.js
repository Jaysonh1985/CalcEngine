// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('regressionCtrl', function ($scope, $uibModal, $uibModalInstance, $log, $http, $location, $filter, configService, ID, calculationService, configFunctionFactory, ObjectDiff) {

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

    function init() {
        $scope.isLoading = true;

        if ($scope.Function == true)
        {
            configService.getFunctionRegression(ID)
               .then(function (data) {
                   $scope.isLoading = false;
                   $scope.Regression = data;
                   $scope.DifferencesCheck();
               }, onError);

        }
        else
        {
           configService.getCalcRegression(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Regression = data;
               $scope.DifferencesCheck();
           }, onError);
        }
    };

    function setInputTypes() {
        angular.forEach($scope.config[0].Functions, function (value, key, obj) {
            if (value.Type == "Date") {
                if ($scope.config[0].Functions[key].Output != null) {
                    var _date = $filter('date')(new Date($scope.config[0].Functions[key].Output), 'MMM dd yyyy');
                    var newDate1 = new Date(_date);
                    $scope.config[0].Functions[key].Output = newDate1;
                }
            }
            if (value.Type == "Decimal") {
                if ($scope.config[0].Functions[key].Output != null) {
                    var decimal = parseFloat($scope.config[0].Functions[key].Output);
                    $scope.config[0].Functions[key].Output = decimal;
                }
            }
        });
    }

    $scope.DifferencesCheck = function() {
        angular.forEach($scope.Regression, function (value, key, obj) {
            if ($scope.Regression[key].Pass == "false") {
                $scope.showregressbuttons = true;
            }
        })
    };

    $scope.FunctionButtonClick = function (size, colIndex, index) {
        $scope.Input = this.Regression[colIndex].Input;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionInputModal.html',
            scope: $scope,
            controller: 'regressionInputCtrl',
            size: size,
            resolve: {
                Functions: function () { return $scope.config },
                Input: function () { return $scope.Input }
            }
        });
        modalInstance.result.then(function (selectedItem) {

            $scope.Regression[colIndex].Input = angular.toJson(selectedItem, true);
            var OutputOld = angular.fromJson($scope.Regression[colIndex].OutputOld, true);
            var OutputNew = angular.fromJson($scope.Regression[colIndex].OutputNew, true);
            $scope.selected = {

                ID: $scope.Regression[colIndex].ID,
                CalcID: $scope.Regression[colIndex].CalcID ,
                Scheme: $scope.Regression[colIndex].Scheme,
                Reference: $scope.Regression[colIndex].Reference,
                Type: $scope.Regression[colIndex].Type,
                Input: $scope.Regression[colIndex].Input,
                OutputOld: angular.toJson(OutputOld, true),
                OutputNew: angular.toJson(OutputNew, true),
                Pass: $scope.Regression[colIndex].Pass,
                Comment: $scope.Regression[colIndex].Comment,
                UpdateDate: ""

            };
            if ($scope.Function == true) {
                configService.putFunctionRegression($scope.Regression[colIndex].ID, $scope.selected).then(function (data) {
                }, onError);
            }
            else
            {
                configService.putCalcRegression($scope.Regression[colIndex].ID, $scope.selected).then(function (data) {
                }, onError);
            }

         }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };

    $scope.DifferencesButtonClick = function (size, colIndex, index) {
        $scope.Difference = this.Regression[colIndex].Difference;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionDifferenceModal.html',
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
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionOutputModal.html',
            scope: $scope,
            controller: 'regressionOutputCtrl',
            size: size,
            resolve: {
                Output: function () { return $scope.Output },
                Header: function () { return "Regression Output"}
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
            Comment: null,
        };

        if ($scope.Function == true) {
            configService.postFunctionRegression(ID, $scope.selected).then(function (data) {
                $scope.Regression.push(data);
                $scope.isLoading = false;
            }, onError);
        }
        else {
            configService.postCalcRegression(ID, $scope.selected).then(function (data) {
                $scope.Regression.push(data);
                $scope.isLoading = false;
            }, onError);
        }
    },

    $scope.removeRegressionItem = function (index) {
        $scope.removeRegressionItem = function (index) {
            var cf = confirm("Delete this Row?");
            if (cf == true) {
                if ($scope.Function == true) {
                    configService.deleteFunctionRegression(this.Regression[index].ID)
                    .then(function (data) {
                        $scope.isLoading = false;
                        $scope.Regression.splice(index, 1);
                    }, onError);
                }
                else {
                    configService.deleteCalcRegression(this.Regression[index].ID)
                    .then(function (data) {
                        $scope.isLoading = false;
                        $scope.Regression.splice(index, 1);
                    }, onError);
                }
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
         $scope.configReplace = JSON.stringify($scope.config);
         $scope.configReplace = angular.fromJson($scope.configReplace);
         angular.forEach($scope.Regression, function (value, key, obj) {
             $scope.configReplace = JSON.stringify($scope.config);
             $scope.configReplace = angular.fromJson($scope.configReplace);
             $scope.Input = angular.fromJson(angular.fromJson(angular.fromJson($scope.Regression[key].Input)));
             angular.forEach($scope.Input.Functions, function (value, key, obj) {
                 var index = configFunctionFactory.getIndexOf($scope.configReplace[0].Functions, value.Name, 'Name');
                 $scope.configReplace[0].Functions[index].Output = value.Output;
             });

             $scope.configReplace = JSON.stringify($scope.configReplace);
             $scope.configReplace = angular.fromJson($scope.configReplace);

             calculationService.postCalc(1, $scope.configReplace).then(function (data) {
                if ($scope.Regression[key].OutputOld == null || $scope.Regression[key].OutputOld == "null")
                {
                    $scope.Regression[key].OutputOld = angular.toJson(data, true);
                    $scope.Regression[key].Pass = true;
                }
                else
                {
                    $scope.Regression[key].OutputNew = angular.toJson(data, true);
                    // you can directly diff your objects js now or parse a Json to object and diff
                    var diff = ObjectDiff.diffOwnProperties(angular.fromJson($scope.Regression[key].OutputOld), angular.fromJson($scope.Regression[key].OutputNew));
                    // gives object view with onlys Diff highlighted
                    $scope.diffValueChanges = ObjectDiff.toJsonDiffView(diff);
                    if ($scope.diffValueChanges != "") {
                        $scope.Regression[key].Difference = '<table class="table table-bordered table table-responsive">' +
                        '<tr><th>Variable Name</th><th>Key</th><th>Old Value</th><th>New Value</th></tr>' +
                        $scope.diffValueChanges + '</table>';
                        $scope.Regression[key].Pass = false;
                        $scope.showregressbuttons = true;
                    }
                    else
                    {
                        $scope.Regression[key].OutputNew = null;
                        $scope.Regression[key].Difference = null;
                        $scope.Regression[key].Pass = true;
                    }
                }

                var Input = angular.fromJson($scope.Regression[key].Input, true)
                $scope.selected = {
                    ID: $scope.Regression[key].ID,
                    CalcID: $scope.Regression[key].CalcID,
                    Scheme: $scope.Regression[key].Scheme,
                    Reference: $scope.Regression[key].Reference,
                    Type: $scope.Regression[key].Type,
                    Input: angular.toJson(Input, true),
                    Comment: $scope.Regression[key].Comment,
                    OriginalRunDate: $scope.Regression[key].OriginalRunDate,
                    LatestRunDate: $scope.Regression[key].LatestRunDate,
                    OutputOld: $scope.Regression[key].OutputOld,
                    OutputNew: $scope.Regression[key].OutputNew,
                    Difference: $scope.Regression[key].Difference,
                    Pass: $scope.Regression[key].Pass,
                    UpdateDate: ""
                };

                if ($scope.Function == true) {
                    configService.putFunctionRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {
                        toastr.success("Regression Ran successfully", "Success");
                        $scope.isLoading = false;
                    }, onError);
                }
                else {
                    configService.putCalcRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {
                        toastr.success("Regression Ran successfully", "Success");
                        $scope.isLoading = false;
                    }, onError);
                };
               $scope.isLoading = true;
            });
         });
     };

    $scope.AcceptButtonClick = function CalcButtonClick() {
        $scope.showregressbuttons = false;
        angular.forEach($scope.Regression, function (value, key, obj) {
            if ($scope.Regression[key].Difference != null){
                $scope.Regression[key].OutputOld = $scope.Regression[key].OutputNew;
                $scope.Regression[key].OutputNew = null;
                $scope.Regression[key].Difference = null;
                $scope.Regression[key].Pass = true;
                var Input = angular.fromJson($scope.Regression[key].Input, true)
                $scope.selected = {

                    ID: $scope.Regression[key].ID,
                    CalcID: $scope.Regression[key].CalcID,
                    Scheme: $scope.Regression[key].Scheme,
                    Type: $scope.Regression[key].Type,
                    Input: angular.toJson(Input, true),
                    Reference: $scope.Regression[key].Reference,
                    Comment: $scope.Regression[key].Comment,
                    OriginalRunDate: $scope.Regression[key].OriginalRunDate,
                    LatestRunDate: $scope.Regression[key].LatestRunDate,
                    OutputOld: $scope.Regression[key].OutputOld,
                    OutputNew: $scope.Regression[key].OutputNew,
                    Difference: $scope.Regression[key].Difference,
                    Pass: $scope.Regression[key].Pass,
                    UpdateDate: ""

                };

                if ($scope.Function == true) {
                    configService.putCalcRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {
                    }, onError);
                }
                else {
                    
                    configService.putCalcRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {
                    }, onError);
                };
             }      
        })
    };

    $scope.RejectButtonClick = function CalcButtonClick() {
        $scope.showregressbuttons = false;
        angular.forEach($scope.Regression, function (value, key, obj) {

            if ($scope.Regression[key].Difference != null) {
                $scope.Regression[key].OutputNew = null;
                $scope.Regression[key].Difference = null;
                $scope.Regression[key].Pass = true;
                var Input = angular.fromJson($scope.Regression[key].Input, true)
                $scope.selected = {

                    ID: $scope.Regression[key].ID,
                    CalcID: $scope.Regression[key].CalcID,
                    Scheme: $scope.Regression[key].Scheme,
                    Type: $scope.Regression[key].Type,
                    Input: angular.toJson(Input, true),
                    Reference: $scope.Regression[key].Reference,
                    Comment: $scope.Regression[key].Comment,
                    OriginalRunDate: $scope.Regression[key].OriginalRunDate,
                    LatestRunDate: $scope.Regression[key].LatestRunDate,
                    OutputOld: $scope.Regression[key].OutputOld,
                    OutputNew: $scope.Regression[key].OutputNew,
                    Difference: $scope.Regression[key].Difference,
                    Pass: $scope.Regression[key].Pass,
                    UpdateDate: ""
                };

                if ($scope.Function == true) {
                    configService.putCalcRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {

                    }, onError);
                }
                else {
                    configService.putCalcRegression($scope.Regression[key].ID, $scope.selected).then(function (data) {

                    }, onError);
                };
            }
        })
    };

    $scope.setclickedrow = function (rowIndex) {
        $scope.selectedRow = rowIndex;  // initialize our variable to null
    }

    var regexIso8601 = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*))(?:Z|(\+|-)([\d|:]*))?$/;;

    $scope.pushInputsToBuilder = function (Input) {
        var InputJson = angular.fromJson(Input);
        angular.forEach(angular.fromJson(InputJson.Functions), function (value, key, obj) {
            var index = configFunctionFactory.getIndexOf($scope.config[0].Functions, value.Name, 'Name');
            $scope.config[0].Functions[index].Output = value.Output;
        });
        $uibModalInstance.close('run');
    }
    //Creates the array of the output
    $scope.BulkOutputBuilder = function BulkOutputBuilder() {
        $scope.isLoading = true;
        $scope.BulkOutputArrayBuilder = [];
        angular.forEach($scope.Regression, function (value, key, obj) {
            $scope.BulkOutputArrayBuilder.push(angular.fromJson(value.OutputOld));
        })
        var promise = configService.exportRegression(1, $scope.BulkOutputArrayBuilder).then(function (data) {
            $scope.isLoading = false;
            toastr.success("Exported successfully", "Success");
            return data;
        });
        return promise;

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
