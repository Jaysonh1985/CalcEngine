// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $location, $window, configService, configFunctionFactory,
                                                            configTypeaheadFactory, ngClipboard, configValidationFactory, $timeout, $filter, $q) {
    // Model
    $scope.config = [];
    $scope.DecimalNames = [];
    $scope.FabOpen = false;
    $scope.isLoading = true;
    $scope.oneAtATime = false;
    $scope.triggerValidation = false;
    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
    $scope.openIndex = [true];
    $scope.openIndexRegression = [true];
    $scope.validationError = false;
    $scope.openIndexBackup = null;
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
    $scope.repeatEnd = false;

    function init() {
        var id = $location.absUrl();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        var ViewOnly = $location.search().ViewOnly;
        if (ViewOnly == 'true') {
            $scope.viewOnly = true;
        };
        if (Function == true) {
            $scope.MenuHeader = 'Function';
            $scope.Function = true;
        }
        else {
            $scope.MenuHeader = 'Configuration';
        };
        //Check if using local storage for saved sessions after timeout
        if ($window.localStorage["Config"] != null) {
            $scope.isLoading = false;
            $scope.config = JSON.parse($window.localStorage.getItem("Config"));
            $window.localStorage.removeItem("Config");
            $window.localStorage.removeItem("WebAddress");
        }
        else {
            var History = $location.search().History;
            if (History == 'true') {
                var id = configFunctionFactory.getConfigID();
                configService.initialize().then(function (data) {
                    $scope.isLoading = true;
                    if (Function == true) {
                        configService.getFunctionHistorySingle(id)
                       .then(function (data) {
                           $scope.isLoading = false;
                           $scope.config = JSON.parse(data.Configuration);
                       }, onError);
                    }
                    else {
                        configService.getCalcHistorySingle(id)
                       .then(function (data) {
                           $scope.isLoading = false;
                           $scope.config = JSON.parse(data.Configuration);
                       }, onError);
                    };

                }, onError);
                var id = configFunctionFactory.getConfigID();
            }
            else {
                configService.initialize().then(function (data) {
                    $scope.isLoading = true;
                    var id = configFunctionFactory.getConfigID();
                    if (Function == true) {
                        configService.getCalcFunction(id)
                        .then(function (data) {
                            $scope.isLoading = false;
                            $scope.config = data;
                        }, onError);
                    }
                    else {
                        configService.getCalc(id)
                        .then(function (data) {
                            $scope.isLoading = false;
                            $scope.config = data;
                        }, onError);
                    };
                }, onError);
                var id = configFunctionFactory.getConfigID();
            };
        }
        $window.localStorage.removeItem("Copy");
    };

    $scope.treeOptions = {
        beforeDrop: function (e) {
            var sourceValue = e.source.nodeScope.$parent.$parent.$parent.$parent.$index;
            var destValue = e.dest.nodesScope.$parent.$parent.$parent.$index;
            // display modal if the node is being dropped into a smaller container
            if (destValue == 0) {
                return $q.reject();
            };
        }
    };

    $scope.focusButtonClick = function (e, elementName) {
        var originalElementName = elementName;
        var isFunctionOutput = elementName.indexOf("FunctionOutput") !== -1;
        var isBracketLeft = elementName.indexOf("BracketLeft") !== - 1;
        var isMathsInput1 = elementName.indexOf("MathsInput1") !== - 1;
        var isMathsInput2 = elementName.indexOf("MathsInput2") !== - 1;
        var isBracketRight = elementName.indexOf("BracketRight") !== - 1;
        var isOperator2 = elementName.indexOf("Operator2") !== - 1;
        if (isFunctionOutput == true || isBracketLeft == true || isMathsInput1 == true || isMathsInput2 == true || isBracketRight == true || isOperator2 == true) {
            var removeLast = elementName.lastIndexOf("_");
            elementName = elementName.substring(0, removeLast);
        };
        var str =  elementName;
        var last = str.lastIndexOf("_");
        var first = str.indexOf("_") + 1;
        var length = str.length;
        var groupIndex = str.substring(first, last-2);
        var stringIndex = str.substring(first + 2, length);
        length = stringIndex.length;
        last = stringIndex.lastIndexOf("_");
        resultIndex = stringIndex.substring(0, last);
        functionIndex = stringIndex.substring(last + 1, length);
        $scope.selectRow(e, parseInt(groupIndex), parseInt(resultIndex), parseInt(functionIndex));
        angular.forEach($scope.openIndex, function (value, key, obj) {
            $scope.openIndex[key] = false;
        });
        $scope.openIndex[groupIndex] = true;
        var domElement = "#RowForm_" + groupIndex + "_" + resultIndex + "_" + functionIndex
        $timeout(function () {
            angular.element(domElement).triggerHandler('click');
            document.getElementById(originalElementName).focus();
        }, 500);
    };

    $scope.LogicButtonClick = function (size, colIndex, index) {
        $scope.Logic = this.config[colIndex].Functions[index].Logic;
        $scope.AllNames = [];
        $scope.configReplace = configFunctionFactory.convertToFromJson($scope.config);
        $scope.AllNames = configTypeaheadFactory.variableArrayBuilder($scope.configReplace, colIndex, null, index);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/ConfigFunctions/Logic/LogicModal.html',
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
        }, function () {
        });
    };

    $scope.rowMenuOptions = [
        ['Copy Name', function ($itemScope) {
            ngClipboard.toClipboard($itemScope.rows.Name);
        }],
    ];

    $scope.ctrlDown = false;
    $scope.ctrlKey = 17, $scope.vKey = 86, $scope.cKey = 67;

    $scope.keyDownFunc = function ($event, colIndex, rowIndex, Name) {
        if ($scope.ctrlDown && ($event.keyCode == $scope.cKey)) {
            $scope.CopyFunction(colIndex, rowIndex);
        } else if ($scope.ctrlDown && ($event.keyCode == $scope.vKey)) {
            $scope.PasteFunction(colIndex, rowIndex);
        } else if ($scope.ctrlDown && ($event.keyCode == 88)) {
            ngClipboard.toClipboard(Name);
        }
    };

    angular.element($window).bind("keyup", function ($event) {
        if ($event.keyCode == $scope.ctrlKey)
            $scope.ctrlDown = false;
        $scope.$apply();
    });

    angular.element($window).bind("keydown", function ($event) {
        if ($event.keyCode == $scope.ctrlKey)
            $scope.ctrlDown = true;
        $scope.$apply();
    });

    $scope.SubOutputButtonClick = function (size, colIndex, type, SubOutput) {
        $scope.Output = angular.toJson(SubOutput);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/ConfigFunctions/Regression/RegressionOutputModal.html',
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
    $scope.TableButtonClick = function (size, colIndex, rowIndex, Parameters, TableName) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/ConfigFunctions/Table/TableModal.html',
            scope: $scope,
            controller: 'tableCtrl',
            size: size,
            windowClass: 'app-modal-window',
            resolve: {
                parameters: function () { return Parameters },
                colIndex: function () { return colIndex },
                rowIndex: function () { return rowIndex },
                TableName: function () { return TableName }
            }
        });
        modalInstance.result.then(function (selectedItem) {
        }, function () {
        });
    };

    $scope.AddResultRows = function (groupIndex, resultIndex, e) {
        var item;
        item = configFunctionFactory.buildResult(groupIndex, $scope.config);
        resultIndex = resultIndex + 1;
        $scope.config[groupIndex].Results.splice(resultIndex, 0, item);
        toastr.success("Result Added", "Success");
    };
    $scope.DeleteResults = function (groupIndex, resultIndex) {
        var cf = confirm("Delete these lines?");
        if (cf == true) {
            $scope.config[groupIndex].Results.splice(resultIndex, 1);      
            resetSelection();
            toastr.success("Rows Deleted", "Success");
            $scope.form.$setDirty();
        };      
    };

    $scope.AddFunctionRows = function (groupIndex, resultIndex, functionIndex, e) {
        var item;
        item = configFunctionFactory.buildFunction(groupIndex, $scope.config);
        functionIndex = functionIndex + 1;
        $scope.config[groupIndex].Results[resultIndex].Functions.splice(functionIndex, 0, item);
        toastr.success("Rows Added", "Success");
        $scope.selectRow(e, parseInt(groupIndex), parseInt(resultIndex), parseInt(functionIndex));
    };

    $scope.CopyFunction = function (groupIndex, resultIndex, functionIndex, e) {
        var selectedRows = getSelectedRows(groupIndex, resultIndex, functionIndex);
        $window.localStorage["Copy"] = JSON.stringify(selectedRows);
        toastr.success("Rows Copied", "Success");
    };

    $scope.PasteFunction = function (groupIndex, resultIndex, functionIndex, e) {
        var selectedRows = JSON.parse($window.localStorage.getItem("Copy"));
        angular.forEach(selectedRows, function (value, key, prop) {
            var Functions = selectedRows[key];
            var item = null;
            item = angular.copy(Functions);
            $scope.config[groupIndex].Results[resultIndex].Functions.splice(functionIndex + 1, 0, item);
            functionIndex = functionIndex + 1;
        });
        toastr.success("Rows Pasted", "Success");
        $scope.form.$setDirty();
    };

    $scope.DeleteFunction = function (groupIndex, resultIndex, functionIndex, e) {
        var cf = confirm("Delete these lines?");
        if (cf == true) {
            var selectedRows = [];
            if (selectedRowsIndexes[resultIndex] != null) {
                var arrayID = selectedRowsIndexes[resultIndex].indexOf(functionIndex);
            }
            else {
                var arrayID = -1;
            };
            
            if (arrayID == -1) {
                $scope.config[resultIndex].Functions.splice(functionIndex, 1);
            }
            else {
                selectedRows = selectedRowsIndexes[resultIndex];
                selectedRows = $filter('orderBy')(selectedRows);
                selectRowsReverse = selectedRows.reverse();
                angular.forEach(selectRowsReverse, function (value, key, prop) {
                    $scope.config[groupIndex].Results[resultIndex].Functions.splice(value, 1);
                });
            };
            resetSelection();
            toastr.success("Rows Deleted", "Success");
        };
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

    $scope.onEnd = function () {
        $timeout(function () {
            $scope.repeatEnd = true;
        }, 1);
    };

    ///Form Submission
    $scope.SaveButtonClick = function SaveBoard(form) {
        $scope.functionValidateForm();
        $scope.validateForm();
        if (form.$valid == true && form.$invalid == false) {
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
                };
            });
        }
        else {
            $scope.validationError = true;
            $scope.viewOnly = false;
            toastr.error("Failed Validations", "Error");
        };
    };

    $scope.CalcButtonClick = function CalcBoard(form) {
        $scope.functionValidateForm();
        $scope.viewOnly = true;
        $timeout(function () {
            $scope.validateForm();
            if ($scope.validationError == false) {
                $scope.openIndexBackup = angular.toJson($scope.openIndex, true);
            };
            var id = configFunctionFactory.getConfigID();
            $scope.rebuildCategoryIDs();
            if (form.$valid == true && form.$invalid == false) {
                $scope.validationError = false;
                $scope.repeatEnd = true;
                toastr.success("Calculating Please Wait...", "Success");
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
                };
            }
            else {
                $scope.validationError = true;
                $scope.viewOnly = false;
                toastr.error("Failed Validations", "Error");
            };
        });
    };

    $scope.setFunctionInput = function (rows) {  //function that sets the type of the row
        rows.Type = rows.Parameter[0].templateOptions.type;
    };

    $scope.variableReplace = function (value, colID, rowID) {
        $scope.variableReplaced == null;
        $scope.variableReplaced = configTypeaheadFactory.variableLastValue($scope.config, colID, rowID, value);
        if (angular.isUndefined($scope.variableReplaced) == true) {
            $scope.variableReplaced = "No value found";
        } else if ($scope.variableReplaced.length == 0) {
            $scope.variableReplaced = value;
        };
    };

    $scope.setFunction = function (rows, colIndex, rowIndex) {  //function that sets the type of the row
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
            rows.Type = 'String';
        }
        else if (rows.Function == 'Function') {
            $scope.SchemeList = [];
            configService.getSchemes().then(function (data) {
                $scope.SchemeList = data;
            }, onError);
        }
        else if (rows.Function == 'Comments') {
            rows.Name = 'Comments_' + colIndex + '_' + rowIndex;
        }
        else if (rows.Function == 'ErrorsWarnings') {
            rows.Name = 'ErrorsWarnings_' + colIndex + '_' + rowIndex;
        }
        else if (rows.Function == 'Return') {
            rows.Name = 'Return_' + colIndex + '_' + rowIndex;
        }
        else if (rows.Function == 'Function') {
            rows.Name = 'Function' + colIndex + '_' + rowIndex;
        }
        else {
            rows.Type = null;
        };
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


    $scope.getFactorTablesList = function getFactorTablesList() {  //function that sets the parameters available under the different variable types
        configService.getFactorTables().then(function (data) {
            $scope.FactorTableList = data;
        }, onError);
    };

    $scope.validateForm = function () {
        form = $scope.form;
        returnCount = 0;
        angular.forEach($scope.config, function (value, key, obj) {
            angular.forEach($scope.config[key].Results, function (valueR, keyR, objR)
            {
                angular.forEach($scope.config[key].Results[keyR].Functions, function (valueF, keyF, obj) {
                    var AttName = 'Errors_' + key + '_' + keyF;
                    form[AttName].$setValidity("input", true);
                    form[AttName].$setValidity("return", true);
                    form[AttName].$setValidity("returnMissing", true);
                    if ($scope.config[key].Functions[keyF].Function != 'Comments') {
                        if ($scope.config[key].Functions[keyF].Parameter.length == 0) {
                            configValidationFactory.requiredfieldcheck(AttName, null);
                        };
                    };
                    angular.forEach($scope.config[key].Functions[keyF].Parameter, function (valueP, keyP, obj) {
                        if (key != 0) {
                            //Maths
                            if ($scope.config[key].Functions[keyF].Function == 'Maths') {
                                configValidationFactory.mathsoperatorcheck($scope.config[key].Functions[keyF].Parameter, form, AttName);
                                configValidationFactory.bracketscheck($scope.config[key].Functions[keyF].Parameter, form, AttName);
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Input1, form, false, AttName);
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Input2, form, false, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Input1);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Logic);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Input2);
                                });
                            };
                            //Period
                            if ($scope.config[key].Functions[keyF].Function == 'Period') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true, AttName);
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date2, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.DateAdjustmentType);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Date1);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Date2);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Inclusive);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.DaysinYear);
                                });

                            };
                            //Factors
                            if ($scope.config[key].Functions[keyF].Function == 'Factors') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.TableName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.LookupType);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.LookupValue);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.OutputType);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.RowMatch);
                                    if (valueN.RowMatch == 'True') {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.RowMatchRowNo);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.RowMatchLookupType);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.RowMatchValue);
                                    }
                                    else {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.ColumnNo);
                                    };
                                    if (valueN.LookupType == 'Decimal') {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Interpolate);
                                    };
                                });
                            };
                            //Date Adjustment
                            if ($scope.config[key].Functions[keyF].Function == 'DateAdjustment') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Type);
                                    if (obj[0].Type != 'Today') {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Date1);
                                    };
                                    if (obj[0].Type == 'Earlier' || obj[0].Type == 'Later' || obj[0].Type == 'DatesBetween') {
                                        configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date2, form, true, AttName);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Date2);
                                    };
                                    if (obj[0].Type == 'Add' || obj[0].Type == 'Subtract') {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.PeriodType);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Period);
                                    };
                                    if (obj[0].Type == 'Adjust') {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Adjustment);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Day);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Month);
                                    };
                                });
                            };
                            //Date Part
                            if ($scope.config[key].Functions[keyF].Function == 'DatePart') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Date", keyF, valueN.Date1, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Date1);
                                });
                            };
                            //Maths Functions
                            if ($scope.config[key].Functions[keyF].Function == 'MathsFunctions') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Number1, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Number1);
                                    if (valueN.Type == "Add" || valueN.Type == "Divide" || valueN.Type == "Max" || valueN.Type == "Min" || valueN.Type == "Multiply" || valueN.Type == "Power" || valueN.Type == "Subtract") {
                                        configValidationFactory.variablePreviouslySet($scope.config, key, "Decimal", keyF, valueN.Number2, form, true, AttName);
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.Number2);
                                    };
                                    if (valueN.Type == "AddPeriod" || valueN.Type == "SubtractPeriod") {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.PeriodType);
                                    };
                                });
                            };
                            //Logic Functions
                            if ($scope.config[key].Functions[keyF].Function == 'LogicFunctions') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, null, keyF, valueN.Input1, form, true, AttName);
                                    configValidationFactory.variablePreviouslySet($scope.config, key, null, keyF, valueN.Input2, form, true, AttName);
                                    configValidationFactory.variablePreviouslySet($scope.config, key, null, keyF, valueN.TrueValue, form, true, AttName);
                                    configValidationFactory.variablePreviouslySet($scope.config, key, null, keyF, valueN.FalseValue, form, true, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Input1);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Input2);
                                    //configValidationFactory.requiredfieldcheck(AttName, valueN.TrueValue);
                                    //configValidationFactory.requiredfieldcheck(AttName, valueN.FalseValue);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.LogicInd);
                                    configValidationFactory.requiredfieldcheck(AttName, $scope.config[key].Functions[keyF].Type);
                                });
                            };
                            //Array Functions
                            if ($scope.config[key].Functions[keyF].Function == 'ArrayFunctions') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form, false, AttName);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.LookupType);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.LookupValue);
                                    configValidationFactory.requiredfieldcheck(AttName, valueN.Function);
                                    if (valueN.Function == "TotalPeriod" || valueN.Function == "Decimal") {
                                        configValidationFactory.requiredfieldcheck(AttName, valueN.PeriodType);
                                    };
                                });
                            };
                            //Function Functions
                            if ($scope.config[key].Functions[keyF].Function == 'Function') {
                                angular.forEach(obj, function (valueN, keyN, obj) {
                                    angular.forEach(obj[0].Input[0].Functions, function (valueNI, keyNI, objI) {
                                        configValidationFactory.variablePreviouslySet($scope.config, key, valueNI.Type, keyF, valueNI.Output, form, true, AttName);
                                        if (valueNI.Parameter[0].templateOptions.required == true) {
                                            configValidationFactory.requiredfieldcheck(AttName, valueNI.Output);
                                        };
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
                                        configValidationFactory.variablePreviouslySet($scope.config, key, obj[0].Datatype, keyF, valueN.Variable, form, true, AttName3);
                                    });
                                };
                            };
                        }
                    })
                })
            });

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

   $scope.functionValidateForm = function () {
       if ($scope.MenuHeader == 'Function') {
           var returnCount = 0;
           angular.forEach($scope.config, function (value, key, obj) {
               angular.forEach($scope.config[key].Functions, function (valueF, keyF, obj) {
                   //Return
                   if ($scope.config[key].Functions[keyF].Function == 'Return') {
                       returnCount = returnCount + 1;
                   };
               });
           });
            //Check if no return values
            if (returnCount == 0) {
                $scope.form.$invalid = true;
                toastr.error("Failed Validation - No Return variable set", "Error");
            };
       }
   };

   $scope.OptionsObject = function OptionsObject(row, array) {
       var options = [];
       if (row.Parameter[0].templateOptions.list == true) {
         array = row.Parameter[0].templateOptions.options.split(',');
         angular.forEach(array, function (object) {
             options.push({
                 Name: object
             });
         });
         row.Parameter[0].templateOptions.optionsList = options;
       }       
   };

   $scope.addMathsItem = function () {
       var item = null;
       item = {
           ID: $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.length,
       };
       $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.splice($scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.length, 0, item);
   };

   $scope.addReturnItem = function ($index) {
       var item = null;
       item = {
           Datatype: "Decimal",
           Name: "",
           Variable: "",
           Result: "Result"
       };
       $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.splice($scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.length, 0, item);
   };

   $scope.setArrayType = function (rows) {
       rows.Type = rows.Parameter[0].LookupType;
   };

    $scope.removeMathsItem = function () {
        $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.splice($scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.length - 1, 1);
    };
    $scope.removeReturnItem = function () {
        $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.splice($scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.length - 1, 1);
    };

    var onError = function (errorMessage) {
        $scope.viewOnly = false;
        toastr.error(errorMessage, "Error");
    };

    //Higlight rows functions
    var selectedRowsIndexes = [];

    $scope.deselectRow = function () {
        resetSelection();
    };

    $scope.SchemeList = [];

    $scope.getSchemeList = function getSchemeList(rowIndex, colIndex) {
        $scope.SchemeList = [];
        configService.getSchemes().then(function (data) {
            $scope.SchemeList = data;
        }, onError);
    };

    $scope.FunctionList =[];

    $scope.getFunctionList = function getFunctionList(rowIndex, colIndex) {
        configService.getFunctionDetails(this.config[colIndex].Functions[rowIndex].Parameter[0].Scheme, 0, "Scheme").then(function (data) {
            $scope.FunctionList = data;
        }, onError);
    };

    $scope.getFunctionListSingleCall = function getFunctionListSingleCall(rowIndex, colIndex) {
        configService.getFunctionDetails(this.config[colIndex].Functions[rowIndex].Parameter[0].Scheme, 0, "Scheme").then(function(data) {
            $scope.FunctionList = data;
            var arrayID = configFunctionFactory.getIndexOf($scope.FunctionList, parseInt($scope.config[colIndex].Functions[rowIndex].Parameter[0].ID), "ID");
            $scope.config[colIndex].Functions[rowIndex].Parameter[0].FunctionName = $scope.FunctionList[arrayID].Name;
            var oldInput = $scope.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].Functions;
            $scope.getFormFields(angular.fromJson($scope.FunctionList[arrayID].Configuration), rowIndex, colIndex, false);
            $scope.mapFormFields(oldInput, colIndex, rowIndex);              
        }, onError);
    };

        //Add new Item to the selected array
    $scope.setFunctionName = function setFunctionName(rowIndex, colIndex) {
        var arrayID = configFunctionFactory.getIndexOf($scope.FunctionList, parseInt(this.config[colIndex].Functions[rowIndex].Parameter[0].ID), "ID");
        this.config[colIndex].Functions[rowIndex].Parameter[0].FunctionName = $scope.FunctionList[arrayID].Name;
        configService.getFunctionDetails(this.config[colIndex].Functions[rowIndex].Parameter[0].Scheme, $scope.FunctionList[arrayID].ID, "Config").then(function(data) {
            $scope.getFormFields(angular.fromJson(data[0].Configuration), rowIndex, colIndex, true);
        }, onError);
    };

    //Get the fields for the input form and null the values out
    $scope.getFormFields = function getFormFields(array, rowIndex, colIndex, mapForm) {  //function that sets the parameters available under the different variable types
        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        $scope.fields =[];
        $scope.fieldset =[];
        this.config[colIndex].Functions[rowIndex].Parameter[0].Input = [configFunctionFactory.convertToFromJson(array[0])];
        angular.forEach(this.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].Functions, function (groups) {
            functionID = 0;
            $scope.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].Functions[scopeid].Output = null;
            scopeid = scopeid + 1
        });
        if (this.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].length > 0 && mapForm == true) {
            $scope.mapFormFields(this.config[colIndex].Functions[rowIndex].Parameter[0].Input[0], colIndex, rowIndex);
        };
    };

    $scope.mapFormFields = function mapFormFields(Input, colIndex, rowIndex) {
        var InputJson = angular.fromJson(Input);
        convertDateStringsToDates([InputJson]);
        angular.forEach(angular.fromJson(InputJson), function (value, key, obj) {
            var index = configFunctionFactory.getIndexOf($scope.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].Functions, value.Name, 'Name');
            if (angular.isNumber(index) == false) {
                index = -1;
            };
            if (index >= 0) {
                $scope.config[colIndex].Functions[rowIndex].Parameter[0].Input[0].Functions[index].Output = value.Output;
            };   
        });
    };

    function convertDateStringsToDates(input) {
        // Ignore things that aren't objects.
        if (typeof input !== "object") return input;
        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;
            var value = input[key];
            var match;
            // Check for string properties which look like dates.
            if (typeof value === "string" && (match = value.match(configFunctionFactory.regexIso8601))) {
                var milliseconds = Date.parse(match[0])
                if (!isNaN(milliseconds)) {
                    input[key] = new Date(milliseconds);
                }
            } else if (typeof value === "object") {
                // Recurse into object
                convertDateStringsToDates(value);
            }
        }
    };

    $scope.InputUnderscore = function InputUnderscore(row, value) {
        NameUndefined = angular.isUndefined(value);
        if (NameUndefined == false) {
            var newString = value.replace(/\s+/g, '_');
            row.Name = newString;
        };
    };

    $scope.selectRow = function (event, groupIndex, resultIndex, functionIndex) {
        $scope.getVariableTypes(groupIndex, resultIndex);
        //$scope.Parameter = this.config[groupIndex].Results[resultIndex].Functions[functionIndex].Parameter;
        //$scope.Function = this.config[groupIndex].Results[resultIndex].Functions[functionIndex].Function;
        $scope.resultIndex = resultIndex;
        $scope.groupIndex = groupIndex;
        if ($scope.Function == 'Function') {
            $scope.function = [];
            if (this.config[groupIndex].Results[resultIndex].Functions[functionIndex].Parameter.length > 0) {
                $scope.SchemeList =[];
                $scope.getSchemeList();
                $scope.FunctionList =[];
                if (this.config[groupIndex].Results[resultIndex].Functions[functionIndex].Parameter[0].Scheme != null) {
                    $scope.getFunctionList(resultIndex, groupIndex);
                };
                if (this.config[groupIndex].Results[resultIndex].Parameter[0].FunctionName != null) {
                   $scope.getFunctionListSingleCall(resultIndex, groupIndex);
                };
            };
        };
        if ($scope.Function == 'Factors') {
            $scope.getFactorTablesList();
        };

        if (event.ctrlKey) {
            if (selectedRowsIndexes[resultIndex] != null) {
                changeSelectionStatus(functionIndex, resultIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[resultIndex] = [functionIndex];
            };
        } else if (event.shiftKey) {
            if (selectedRowsIndexes[resultIndex] != null) {
                selectWithShift(functionIndex, resultIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[resultIndex] = [functionIndex];
            };
        } else {
            if (selectedRowsIndexes[resultIndex] != null) {
                resetSelection();
                selectedRowsIndexes[resultIndex] = [functionIndex];
            }
            else {
                resetSelection();
                selectedRowsIndexes[resultIndex] = [functionIndex];
            };
        };
    };

    function selectWithShift(functionIndex, resultIndex) {
        var lastSelectedRowIndexInSelectedRowsList = selectedRowsIndexes.length - 1;
        var lastSelectedRowIndex = selectedRowsIndexes[lastSelectedRowIndexInSelectedRowsList];
        var selectFromIndex = Math.min(functionIndex, lastSelectedRowIndex);
        var selectToIndex = Math.max(functionIndex, lastSelectedRowIndex);
        selectRows(selectFromIndex, selectToIndex, resultIndex);
    };

    function getSelectedRows(groupIndex, resultIndex, functionIndex) {
        var selectedRows = [];
        selectedRowsIndexesOrdered = $filter('orderBy')(selectedRowsIndexes[resultIndex]);
        angular.forEach(selectedRowsIndexesOrdered, function (value, key, prop) {
            selectedRows.push($scope.config[groupIndex].Results[resultIndex].Functions[value]);
        });
        return selectedRows;
    };

    function selectRows(selectFromIndex, selectToIndex, resultIndex) {
        for (var rowToSelect = selectFromIndex; rowToSelect <= selectToIndex; rowToSelect++) {
            select(rowToSelect, resultIndex);
        };
    };

    function changeSelectionStatus(functionIndex, resultIndex) {
        if ($scope.isRowSelected(functionIndex, resultIndex)) {
            unselect(functionIndex, resultIndex);
        } else {
            select(functionIndex, resultIndex);
        };
    };

    function select(functionIndex, resultIndex) {
        if (!$scope.isRowSelected(functionIndex, resultIndex)) {
            selectedRowsIndexes[resultIndex].push(functionIndex)
        };
    };

    function unselect(functionIndex, resultIndex) {
        var rowIndexInSelectedRowsList = selectedRowsIndexes[resultIndex].indexOf(functionIndex);
        var unselectOnlyOneRow = 1;
        selectedRowsIndexes[resultIndex].splice(rowIndexInSelectedRowsList, unselectOnlyOneRow);
    };

    function resetSelection() {
        selectedRowsIndexes = [];
    };

    $scope.isRowSelected = function (functionIndex, resultIndex) {
        if (selectedRowsIndexes[resultIndex] != null) {
            return selectedRowsIndexes[resultIndex].indexOf(functionIndex) > -1;
        }
        return false;
    };

    init();

    $window.onbeforeunload = function (event) {
        //Check if there was any change, if no changes, then simply let the user leave
        if (!$scope.form.$dirty) {
            return;
        };
        var message = 'If you leave this page you are going to lose all unsaved changes, are you sure you want to leave?';
        if (typeof event == 'undefined') {
            event = window.event;
        };
        if (event) {
            event.returnValue = message;
        };
        return message;
    };

    //This works only when user changes routes, not when user refreshes the browsers, goes to previous page or try to close the browser
    $scope.$on('$locationChangeStart', function (event) {
        if (!$scope.form.$dirty) return;
        var answer = confirm('If you leave this page you are going to lose all unsaved changes, are you sure you want to leave?')
        if (!answer) {
            event.preventDefault();
        };
    });

    $scope.$on('$destroy', function () {
        delete window.onbeforeunload;
    });
    
    $scope.$on('IdleTimeout', function() {
        if($scope.config != null && $scope.config != undefined) {
            var id = configFunctionFactory.getConfigID();
            $window.localStorage["Config"] = JSON.stringify($scope.config);
            var Function = configFunctionFactory.isFunction($location.absUrl())
            if (Function == true) {
                $window.localStorage["WebAddress"] = '/Configuration/Function/Function/' + id;
            }
            else {
                $window.localStorage["WebAddress"] = '/Configuration/Config/Config/' + id;
            };
        };
    });
});
