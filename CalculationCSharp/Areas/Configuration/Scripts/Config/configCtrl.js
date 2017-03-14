// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, $mdSidenav, configService, configFunctionFactory, configModalFactory, configTypeaheadFactory, configValidationFactory, $filter, $timeout) {
    // Model
    $scope.config = [];
    $scope.DecimalNames = [];
    $scope.FabOpen = false;
    $scope.isLoading = true;
    $scope.oneAtATime = false;
    $scope.Function = false;
    $scope.triggerValidation = false;
    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
    $scope.openIndex = [true];
    $scope.openIndexRegression = [true];
    $scope.validationError = false;
    $scope.openIndexBackup = null;
    $scope.noSpacesPattern = /^[a-zA-Z0-9-_]+$/;
    $scope.getHeader = ["Function ID", "Name", "Function", "Type", "Logic", "Parameter"];
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
    $scope.viewOnly = false;
    $scope.toggleLeft = buildToggler('left');
    $scope.toggleRight = buildToggler('right');

    function buildToggler(componentId) {
        return function () {
            $mdSidenav(componentId).toggle();
        };
    };

    $scope.testButtonClick = function (parent, child) {
        var test = 1;
    };

    function init() {
        var id = $location.absUrl();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        $scope.Function = configFunctionFactory.isFunction($location.absUrl());
        var ViewOnly = $location.search().ViewOnly;
        if (ViewOnly == 'true')
        {
            $scope.viewOnly = true;
        }
        if (Function == true)
        {
            $scope.MenuHeader = 'Function';
        }
        else
        {
            $scope.MenuHeader = 'Config';
        }

        //Check if using local storage for saved sessions after timeout
        if ($window.localStorage["Config"] != null) {
            $scope.isLoading = false;
            $scope.config = JSON.parse($window.localStorage.getItem("Config"));
            $window.localStorage.removeItem("Config");
            $window.localStorage.removeItem("WebAddress");
        }
        else {
            var History = $location.search().History;
            if(History == 'true')
            {
                var id = configFunctionFactory.getConfigID();
                configService.initialize().then(function (data) {
                    $scope.isLoading = true;
                    if (Function == true)
                    {
                        configService.getFunctionHistorySingle(id)
                       .then(function (data) {
                           $scope.isLoading = false;
                           $scope.config = JSON.parse(data.Configuration);
                       }, onError);
                    }
                    else
                    {
                        configService.getCalcHistorySingle(id)
                       .then(function (data) {
                           $scope.isLoading = false;
                           $scope.config = JSON.parse(data.Configuration);
                       }, onError);
                    }

                }, onError);
                var id = configFunctionFactory.getConfigID();
            }
            else
            {
                configService.initialize().then(function (data) {
                    $scope.isLoading = true;
                    var id = configFunctionFactory.getConfigID();
                    if (Function == true)
                    {
                        configService.getCalcFunction(id)
                        .then(function (data) {
                            $scope.isLoading = false;
                            $scope.config = data;
                        }, onError);
                    }
                    else
                    {
                      configService.getCalc(id)
                      .then(function (data) {
                          $scope.isLoading = false;
                          $scope.config = data;
                      }, onError);
                    }

                }, onError);
                var id = configFunctionFactory.getConfigID();
            }
        }
        $window.localStorage.removeItem("Copy");

    };
    $scope.$on('IdleTimeout', function() {
        if($scope.config != null && $scope.config != undefined)
            {
            var id = configFunctionFactory.getConfigID();
            $window.localStorage["Config"] = JSON.stringify($scope.config);
            var Function = configFunctionFactory.isFunction($location.absUrl())
            if (Function == true)
            {
                $window.localStorage["WebAddress"] = '/Configuration/Function/Function/' + id;
            }
            else
            {
                $window.localStorage["WebAddress"]= '/Configuration/Config/Config/' +id;
            }           
        }
    });

    //Functions
    function buildFunction(colIndex, config)
    {
        var item = null;
        if (colIndex == 0) {
            item = {
                ID: config[colIndex].Functions.length,
                Function: 'Input',
                Logic: [],
                Parameter: []
            };
        }
        else {
            item = {
                ID: config[colIndex].Functions.length,
                Logic: [],
                Parameter: []
            };
        }
        return item;
    };
    $scope.focusButtonClick = function (e, elementName) {
        //document.getElementById(elementName).focus();

        var str =  elementName;
        var last = str.lastIndexOf("_");
        var first = str.indexOf("_") + 1;
        var length = str.length;
        var colIndex = str.substring(first, last);
        var rowIndex = str.substring(last + 1, length);

        $scope.selectRow(e, rowIndex, colIndex);
    };

    $scope.rowMenuOptions = [
        ['Add Row', function ($itemScope) {
            $scope.AddFunctionRows($itemScope.$parentNodeScope.$index, $itemScope.$index);
        }],
        ['Delete Row', function ($itemScope) {
            $scope.DeleteFunction($itemScope.$parentNodeScope.$index, $itemScope.$index);
        }],
        ['Copy Rows', function ($itemScope) {
            $scope.CopyFunction($itemScope.$parentNodeScope.$index, $itemScope.$index);
        }],
        ['Paste Rows', function ($itemScope) {
            $scope.PasteFunction($itemScope.$parentNodeScope.$index, $itemScope.$index);
        }],
    ];

    $scope.AddFunctionRows = function (colIndex, index) {
        $scope.Function = configFunctionFactory.isFunction($location.absUrl());
        var item;
        item = buildFunction(colIndex, $scope.config);
        $scope.config[colIndex].Functions.splice(index + 1, 0, item);
        toastr.success("Rows Added", "Success");
    };

    $scope.CopyFunction = function (colIndex, index) {
        var selectedRows = getSelectedRows(colIndex);
        $window.localStorage["Copy"] = JSON.stringify(selectedRows);
        toastr.success("Rows Copied", "Success");
    };

    $scope.PasteFunction = function (colIndex, index) {
        var selectedRows = JSON.parse($window.localStorage.getItem("Copy"));
        angular.forEach(selectedRows, function (value, key, prop) {
            var Functions = selectedRows[key];
            var item = null;
            item = angular.copy(Functions);
            $scope.config[colIndex].Functions.splice(index + 1, 0, item);
            index = index + 1;
        });
        toastr.success("Rows Pasted", "Success");
        $scope.form.$setDirty();
    };

    $scope.DeleteFunction = function (colIndex, $index) {
        var cf = confirm("Delete these lines?");
        if (cf == true) {
            var selectedRows = [];
            selectedRows = selectedRowsIndexes[colIndex];
            selectedRows = $filter('orderBy')(selectedRows);
            selectRowsReverse = selectedRows.reverse();
            angular.forEach(selectRowsReverse, function (value, key, prop) {
                $scope.config[colIndex].Functions.splice(value, 1);
            });
            resetSelection();
            toastr.success("Rows Deleted", "Success");
        }
        $scope.form.$setDirty();
    };
    $scope.columnMenuOptions = [
        ['Add Category', function ($itemScope) {
            $scope.AddCategoryRows($itemScope.$index);
        }],
        ['Add Rows', function ($itemScope) {
            $scope.AddFunctionRows($itemScope.$index);
        }],
        ['Delete Category', function ($itemScope) {
            $scope.DeleteCategory($itemScope.$index);
        }],
        ['Copy Category', function ($itemScope) {
            $scope.CopyCategory($itemScope.$index);
        }],
        ['Update Details', function ($itemScope) {
            $scope.GroupButtonClick('lg', $itemScope.$index);
        }],
        ['Add Category Logic', function ($itemScope) {
            $scope.CategoryLogicButtonClick('lg', $itemScope.$index);
        }],
        ['Move Category Down', function ($itemScope) {
            $scope.MoveDownCategory($itemScope.$index);
        }],
    ];
    //Categories
    $scope.AddCategoryRows = function (colIndex) {
        var item = null;
        var FunctionsValue = [buildFunction(colIndex, $scope.config)];
        item = {
            ID: colIndex + 1,
            Name: null,
            Description: null,
            Functions: FunctionsValue,
            Logic: []
        };
        $scope.config.splice(colIndex + 1, 0, item);
        $scope.GroupButtonClick('lg', colIndex + 1);
        $scope.rebuildCategoryIDs();
    };

    $scope.CopyCategory = function (index, e) {
        var Category = $scope.config[index];
        var item = null;
        item = angular.copy(Category);
        $scope.config.splice(index + 1, 0, item);
        $scope.form.$setDirty();
    };

    $scope.MoveDownCategory = function (Index) {
        var Category = $scope.config[Index];
        var item = null;
        item = angular.copy(Category);
        $scope.config.splice(Index, 1);
        $scope.config.splice(Index + 1, 0, item);
        $scope.colindex = Index;
        $scope.form.$setDirty();
    };

    $scope.DeleteCategory = function (colIndex) {
        var cf = confirm("Delete this line?");
        if (cf == true) {
            $scope.config.splice(colIndex, 1);
            //$scope.rebuildCategoryIDs();
        }
        $scope.form.$setDirty();
    };

    $scope.rebuildCategoryIDs = function rebuildCategoryIDs() {
        colid = 0;
        angular.forEach($scope.config, function (groups) {
            $scope.config[colid].ID = colid;
            rowid = 0;
            angular.forEach($scope.config[colid].Functions, function (rows) {
                $scope.config[colid].Functions[rowid].ID = rowid;
                rowid = rowid + 1;
            });
            colid = colid + 1;
        });
    };
    ///Form Submission
    $scope.SaveButtonClick = function SaveBoard() {
        $scope.Validate();
        $scope.viewOnly = true;
        $timeout(function () {
            var id = configFunctionFactory.getConfigID();
            var Comment = prompt("Enter a Comment");
            $scope.rebuildCategoryIDs();
            var Function = configFunctionFactory.isFunction($location.absUrl())
            if (Function == true) {
                configService.putCalcFunction(id, $scope.config, Comment).then(function (data) {
                    $scope.viewOnly = false;
                    toastr.success("Saved successfully", "Success");
                    $scope.form.$setPristine();
                }, onError);
            }
            else {
                configService.putCalc(id, $scope.config, Comment).then(function (data) {
                    $scope.viewOnly = false;
                    toastr.success("Saved successfully", "Success");
                    $scope.form.$setPristine();
                }, onError);
            }
        });
    };
    // Model to JSON for demo purpose
    $scope.$watch('config', function (model) {
        $scope.Validate();
    }, true);

    $scope.Validate = function Validate() {
        if ($scope.triggerValidation == false) {
            $scope.triggerValidation = true;
        }
        else {
            $scope.triggerValidation = false;
        };
    };
    $scope.CalcButtonClick = function CalcBoard(form) {
        $scope.Validate();
        $scope.viewOnly = true;
        $timeout(function () {
            //$scope.validateForm();
            if ($scope.validationError == false) {
                $scope.openIndexBackup = angular.toJson($scope.openIndex, true);
            }
            var id = configFunctionFactory.getConfigID();
            $scope.rebuildCategoryIDs();
            if (form.$valid == true) {
                $scope.validationError = false;
                var Function = configFunctionFactory.isFunction($location.absUrl())
                if (Function == true) {
                    configService.postCalcFunction(id, $scope.config).then(function (data) {
                        $scope.viewOnly = false;
                        $scope.config = data;
                        $scope.openIndex = angular.fromJson($scope.openIndexBackup, true);
                        toastr.success("Calculated successfully", "Success");
                        $scope.form.$setDirty();
                    }, onError);
                }
                else {
                    configService.postCalc(id, $scope.config).then(function (data) {
                        $scope.viewOnly = false;
                        $scope.config = data;
                        $scope.openIndex = angular.fromJson($scope.openIndexBackup, true);
                        toastr.success("Calculated successfully", "Success");
                        $scope.form.$setDirty();
                    }, onError);
                }
            }
            else {
                $scope.validationError = true;
                $scope.viewOnly = false;
                toastr.error("Failed Validations", "Error");
            }
        });
    };

    $scope.setFunctionInput = function (rows) {  //function that sets the type of the row
        rows.Type = rows.Parameter[0].templateOptions.type;
   };

    $scope.setFunction = function (rows) {  //function that sets the type of the row
        if (rows.Function == 'Maths') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'MathsFunctions') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'Period') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'DateAdjustment') {
            rows.Type = 'Date';
        }
        else if (rows.Function == 'DatePart') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'Input') {
            rows.Type = null;
        }
        else if (rows.Function == 'StringFunctions') {

        }
        else {
            rows.Type = null;
        }
    };
   
    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.config, function (value, key, obj) {
            $scope.openIndex[key] = true;
        })
    };

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndex, function (value, key, obj) {
            $scope.openIndex[key] = false;
        })
    };
    $scope.RemoveExpectedResultsButton = function () {
        var cf = confirm("Are you sure you wish remove all Expected Results?");
        if (cf == true) {
            angular.forEach($scope.config, function (value, key, obj) {
                angular.forEach(value.Functions, function (valueF, keyF, objF) {
                    $scope.config[key].Functions[keyF].ExpectedResult = null;
                })
            })
        }
    };

    $scope.RemoveInputsButton = function () {
        var cf = confirm("Are you sure you wish remove all Input values?");
        if (cf == true) {
            angular.forEach($scope.config[0].Functions, function (value, key, obj) {
                $scope.config[0].Functions[key].Output = null;
            })
        }
    };

    $scope.ExitButton = function () {
        if (!$scope.form.$dirty) {
            var ID = configFunctionFactory.getConfigID();
            if ($scope.Function == true) {
                $window.location.assign('/Configuration/Function/Exit/' + ID);
            }
            else {
                $window.location.assign('/Configuration/Config/Exit/' + ID);
            }
        }
        else {
            var cf = confirm("Are you sure you wish to exit unsaved changes will be lost?");
            if (cf == true) {
                var ID = configFunctionFactory.getConfigID();
                if ($scope.Function == true) {
                    $window.location.assign('/Configuration/Function/Exit/' + ID);
                }
                else {
                    $window.location.assign('/Configuration/Config/Exit/' + ID);
                }
            }
        }
    };

    $scope.getVariableTypes = function getVariableTypes(colIndex, rowIndex) {  //function that sets the parameters available under the different variable types
        $scope.DecimalNames = [];
        $scope.DateNames = [];
        $scope.StringNames = [];
        $scope.AllNames = [];
        $scope.DecimalNames = configTypeaheadFactory.variableArrayBuilder($scope.config, colIndex, 'Decimal', rowIndex);
        $scope.DateNames = configTypeaheadFactory.variableArrayBuilder($scope.config, colIndex, 'Date', rowIndex);
        $scope.StringNames = configTypeaheadFactory.variableArrayBuilder($scope.config, colIndex, 'String', rowIndex);
        $scope.AllNames = configTypeaheadFactory.variableArrayBuilder($scope.config, colIndex, null, rowIndex);
    };

    $scope.FunctionButtonClick = function (size, colIndex, index, form) {
        $scope.Parameter = this.config[colIndex].Functions[index].Parameter;
        var Function = this.config[colIndex].Functions[index].Function;
        $scope.getVariableTypes(colIndex, index);
        var FunctionCtrl = null;
        var FunctionTemp = null;
        $scope.AttName = 'Row_' + colIndex + '_' + index;
        FunctionCtrl = configModalFactory.getFunctionCtrl(Function);
        FunctionTemp = configModalFactory.getFunctionTempURL(Function);
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: FunctionTemp,
                backdrop: false,
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
                if ($scope.config[colIndex].Functions[index].Function == 'ErrorsWarnings') {
                    $scope.config[colIndex].Functions[index].Name = selectedItem[0].Type + '_' + colIndex + '_' + index;
                }

                if ($scope.config[colIndex].Functions[index].Function == 'Comments') {
                    $scope.config[colIndex].Functions[index].Name = 'Comments_' + colIndex + '_' + index;
                    $scope.config[colIndex].Functions[index].Output = selectedItem[0].String1;
                }

                if ($scope.config[colIndex].Functions[index].Function == 'Return') {
                    $scope.config[colIndex].Functions[index].Name = 'Return' + colIndex + '_' + index;
                }
                $timeout(function () {
                    var el = document.getElementById($scope.AttName);
                    angular.element(el).triggerHandler('click');
                    $scope.form.$setDirty();
                    $scope.validateForm();
                });

            }, function () {

            });
    };

    $scope.LogicButtonClick = function (size, colIndex, index) {
        $scope.Logic = this.config[colIndex].Functions[index].Logic;      
        $scope.AllNames = [];
        $scope.configReplace = configFunctionFactory.convertToFromJson($scope.config);     
        $scope.AllNames = configTypeaheadFactory.variableArrayBuilder($scope.configReplace, colIndex, null, index);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Logic/LogicModal.html',
            scope: $scope,
            controller: 'logicCtrl',
            backdrop: false,
            size: size,
            resolve: {
                Logic: function () { return $scope.Logic },
                viewOnly: function () { return $scope.viewOnly },
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.config[colIndex].Functions[index].Logic = selectedItem;
            $scope.form.$setDirty();
            $scope.validateForm();
        }, function () {       
        });
    };

    $scope.CategoryLogicButtonClick = function (size, index) {
        $scope.Logic = this.config[index].Logic;
        $scope.AllNames = [];
        $scope.configReplace = configFunctionFactory.convertToFromJson($scope.config);
        $scope.AllNames = configTypeaheadFactory.variableArrayBuilder($scope.configReplace, index, null, 0);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Logic/LogicModal.html',
            scope: $scope,
            controller: 'logicCtrl',
            backdrop: false,
            size: size,
            resolve: {
                Logic: function () { return $scope.Logic }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.config[index].Logic = selectedItem;
            $scope.form.$setDirty();
            $scope.validateForm();
        }, function () {
        });
    };

    $scope.GroupButtonClick = function (size, colIndex) {
        $scope.ID = this.config[colIndex].ID;   
        $scope.Name = this.config[colIndex].Name;
        $scope.Description = this.config[colIndex].Description;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Group/GroupModal.html',
            scope: $scope,
            controller: 'groupCtrl',
            backdrop: false,
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
                Name: function () { return $scope.Name },
                Description: function () { return $scope.Description },
                ColIndex: function () { return colIndex}
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.config[colIndex].ID = selectedItem[0].ID;
            $scope.config[colIndex].Name = selectedItem[0].Name;
            $scope.config[colIndex].Description = selectedItem[0].Description;
            $scope.form.$setDirty();
        }, function () {          
        });
    };

    $scope.HistoryButtonClick = function (size) {
        $scope.ID = configFunctionFactory.getConfigID();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/History/HistoryModal.html',
            scope: $scope,
            controller: 'historyCtrl',
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
                isFunction: function () {return $scope.Function}
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.config = JSON.parse(selectedItem);
            toastr.success("Reverted successfully", "Success");
        }, function () {        
        });
    };

    $scope.RegressionButtonClick = function (size, form) {
        $scope.ID = configFunctionFactory.getConfigID();
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionModal.html',
            scope: $scope,
            controller: 'regressionCtrl',
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.CalcButtonClick(form);
        }, function () {           
        });
    };
    $scope.ImpactAssessmentButtonClick = function () {
        $scope.ID = configFunctionFactory.getConfigID();
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Impact Assessment/ImpactAssessmentModal.html',
            scope: $scope,
            controller: 'impactAssessmentCtrl',
            size: 'lg',
            resolve: {
                ID: function () { return $scope.ID },
            }
        });
        modalInstance.result.then(function (selectedItem) {
            
        }, function () {
        });
    };


    $scope.SpecButtonClick = function (form) {
        var id = configFunctionFactory.getConfigID();
        $scope.isLoading = true;
       var promise = configService.specBuilder(id, $scope.config).then(function (data) {
            $scope.isLoading = false;
            $scope.SpecOutput = data;
            toastr.success("Specification Produced", "Success");
            return data;
        }, onError);
       return promise;
    };

    $scope.SubOutputButtonClick = function (size, colIndex, type, SubOutput) {
        $scope.Output = angular.toJson(SubOutput);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/Regression/RegressionOutputModal.html',
            scope: $scope,
            controller: 'regressionOutputCtrl',
            size: size,
            resolve: {
                Output: function () { return $scope.Output },
                Header: function () { return "Output" }
            }
        });
        modalInstance.result.then(function (selectedItem) {
        }, function () {

        });
    };
    $scope.optionsBuild = function (rows) {
        $scope.options = [];
        var array = null;
        if (rows.Parameter[0].templateOptions.list == true) {
            if (angular.isArray(rows.Parameter[0].templateOptions.options) == false) {
                array = rows.Parameter[0].templateOptions.options.split(',');
                angular.forEach(array, function (object) {
                    $scope.options.push({
                        Name: object
                    });
                });
            }
            else {
                options = rows.Parameter[0].templateOptions.options;
            }
        };
    };


    //Higlight rows functions
    var selectedRowsIndexes = [];

    $scope.deselectRow = function () {
        resetSelection();
    };

    $scope.validateForm = function () {
        form = $scope.form;
        returnCount = 0;
        angular.forEach($scope.config, function (value, key, obj) {
            angular.forEach($scope.config[key].Functions, function (valueF, keyF, obj) {
                var AttName = 'FunctionCog_' + key + '_' + keyF;
                form[AttName].$setValidity("input", true);
                form[AttName].$setValidity("return", true);
                form[AttName].$setValidity("returnMissing", true);
                angular.forEach($scope.config[key].Functions[keyF].Parameter, function (valueP, keyP, obj) {
                    if (key != 0) {
                        //Maths
                        if ($scope.config[key].Functions[keyF].Function == 'Maths') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Input1, form);
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Input2, form);
                            });
                        };
                        //Period
                        if ($scope.config[key].Functions[keyF].Function == 'Period') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true);
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date2, form, true);
                            });
                        };
                        //Factors
                        if ($scope.config[key].Functions[keyF].Function == 'Factors') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form, true);
                            });
                        };
                        //Date Adjustment
                        if ($scope.config[key].Functions[keyF].Function == 'DateAdjustment') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true);
                                if (obj[0].Type == 'Earlier' || obj[0].Type == 'Later') {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date2, form, true);
                                }
                            });
                        };
                        //Date Part
                        if ($scope.config[key].Functions[keyF].Function == 'DatePart') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true);
                            });
                        };
                        //Maths Functions
                        if ($scope.config[key].Functions[keyF].Function == 'MathsFunctions') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Number1, form, true);
                                if (valueN.Type == "Add" || valueN.Type == "Divide" || valueN.Type == "Max" || valueN.Type == "Min" || valueN.Type == "Multiply" || valueN.Type == "Power" || valueN.Type == "Subtract") {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Number2, form, true);
                                }
                            });
                        };
                        //Array Functions
                        if ($scope.config[key].Functions[keyF].Function == 'ArrayFunctions') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form);
                            });
                        };
                        //Function Functions
                        if ($scope.config[key].Functions[keyF].Function == 'Function') {
                            angular.forEach(obj, function (valueN, keyN, obj) {
                                angular.forEach(obj[0].Input, function (valueNI, keyNI, objI) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, valueNI.Type, keyF, valueNI.Output, form, true);
                                });
                            });
                        };
                        if ($scope.Function == true) {
                            //Return
                            if ($scope.config[key].Functions[keyF].Function == 'Return') {
                                returnCount = returnCount + 1;
                                if (returnCount > 1) {
                                    var AttName3 = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName3].$setValidity("return", false);
                                }
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].Datatype, keyF, valueN.Variable, form, true);
                                });
                            };
                        };
                    }
                })
            })
        })
        if ($scope.Function == true) {
            //Check if no return values
            if (returnCount == 0) {

                if ($scope.config[0].Functions.length == parseInt(0)) {
                    $scope.form.$invalid = true;
                    toastr.error("Failed Validation - No Return variable set", "Error");
                }
                else {
                    var AttName4 = 'FunctionCog_' + 0 + '_' + 0;
                    form[AttName4].$setValidity("returnMissing", false);
                }
            };
            //Check if No Inputs
            if ($scope.config[0].Functions.length == parseInt(0)) {
                $scope.form.$invalid = true;
                toastr.error("Failed Validation - No Inputs Set", "Error");
            };
            var columnLength = $scope.config.length - 1;
            var functionLength = $scope.config[columnLength].Functions.length - 1;
            if ($scope.config[columnLength].Functions[functionLength].Function != "Return") {
                $scope.form.$invalid = true;
                toastr.error("Failed Validation - Return variable not on last row", "Error");
            };
        };
    };

    $scope.selectRow = function (event, rowIndex, colIndex) {
        $scope.getVariableTypes(colIndex, rowIndex);
        $scope.Parameter = this.config[colIndex].Functions[rowIndex].Parameter;
        $scope.Function = this.config[colIndex].Functions[rowIndex].Function;
        $scope.rowIndex = rowIndex;
        $scope.colIndex = colIndex;

        if (event.ctrlKey) {
            if (selectedRowsIndexes[colIndex] != null) {
                changeSelectionStatus(rowIndex, colIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            }          
        } else if (event.shiftKey) {
            if (selectedRowsIndexes[colIndex] != null) {
                selectWithShift(rowIndex, colIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            }         
        } else {

            if (selectedRowsIndexes[colIndex] != null) {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            }
            else
            {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            }
        }
    };

    function selectWithShift(rowIndex, colIndex) {
        var lastSelectedRowIndexInSelectedRowsList = selectedRowsIndexes.length - 1;
        var lastSelectedRowIndex = selectedRowsIndexes[lastSelectedRowIndexInSelectedRowsList];
        var selectFromIndex = Math.min(rowIndex, lastSelectedRowIndex);
        var selectToIndex = Math.max(rowIndex, lastSelectedRowIndex);
        selectRows(selectFromIndex, selectToIndex, colIndex);
    };

    function getSelectedRows(colIndex) {
        var selectedRows = [];
        selectedRowsIndexesOrdered = $filter('orderBy')(selectedRowsIndexes[colIndex]);
        angular.forEach(selectedRowsIndexesOrdered, function (value, key, prop) {
            selectedRows.push($scope.config[colIndex].Functions[value]);
        });
        return selectedRows;
    };

    function selectRows(selectFromIndex, selectToIndex, colIndex) {
        for (var rowToSelect = selectFromIndex; rowToSelect <= selectToIndex; rowToSelect++) {
            select(rowToSelect, colIndex);
        }
    };

    function changeSelectionStatus(rowIndex, colIndex) {
        if ($scope.isRowSelected(rowIndex, colIndex)) {
            unselect(rowIndex, colIndex);
        } else {
            select(rowIndex, colIndex);
        }
    };

    function select(rowIndex, colIndex) {
        if (!$scope.isRowSelected(rowIndex, colIndex)) {
            selectedRowsIndexes[colIndex].push(rowIndex)
        }
    };

    function unselect(rowIndex, colIndex) {
        var rowIndexInSelectedRowsList = selectedRowsIndexes[colIndex].indexOf(rowIndex);
        var unselectOnlyOneRow = 1;
        selectedRowsIndexes[colIndex].splice(rowIndexInSelectedRowsList, unselectOnlyOneRow);
    };

    function resetSelection() {
        selectedRowsIndexes = [];
    };

    $scope.isRowSelected = function (rowIndex, colIndex) {
        if (selectedRowsIndexes[colIndex] != null)
        {
            return selectedRowsIndexes[colIndex].indexOf(rowIndex) > -1;
        }
        return false;       
    };

    var onError = function (errorMessage) {
        $scope.viewOnly = false;
        toastr.error(errorMessage, "Error");
    };

    init();

    $window.onbeforeunload = function (event) {
        //Check if there was any change, if no changes, then simply let the user leave
        if (!$scope.form.$dirty) {
            return;
        }
        var message = 'If you leave this page you are going to lose all unsaved changes, are you sure you want to leave?';
        if (typeof event == 'undefined') {
            event = window.event;
        }
        if (event) {
            event.returnValue = message;
        }
        return message;
    };

    //This works only when user changes routes, not when user refreshes the browsers, goes to previous page or try to close the browser
    $scope.$on('$locationChangeStart', function (event) {
        if (!$scope.form.$dirty) return;
        var answer = confirm('If you leave this page you are going to lose all unsaved changes, are you sure you want to leave?')
        if (!answer) {
            event.preventDefault();
        }
    });

    $scope.$on('$destroy', function () {
        delete window.onbeforeunload;
    });

});
