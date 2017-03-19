// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $location, $window, configService,
                                                            configFunctionFactory, configTypeaheadFactory,
                                                            configValidationFactory, $timeout, $filter, $q) {
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

    function init() {
        var id = $location.absUrl();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        $scope.Function = configFunctionFactory.isFunction($location.absUrl());
        var ViewOnly = $location.search().ViewOnly;
        if (ViewOnly == 'true') {
            $scope.viewOnly = true;
        };
        if (Function == true) {
            $scope.MenuHeader = 'Function';
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
        var isFunctionOutput = elementName.indexOf("FunctionOutput") !== -1;
        if (isFunctionOutput == true)
        {
            var removeLast = elementName.lastIndexOf("_");
            elementName = elementName.substring(0, removeLast);
        }
        var str =  elementName;
        var last = str.lastIndexOf("_");
        var first = str.indexOf("_") + 1;
        var length = str.length;
        var colIndex = str.substring(first, last);
        var rowIndex = str.substring(last + 1, length);
        $scope.selectRow(e, rowIndex, colIndex);
        //10 seconds delay
        if (isFunctionOutput == false) {
            $timeout(function () {
                document.getElementById(elementName).focus();
            }, 500);
        };
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
            $scope.validateForm();
        }, function () {
        });
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

    ///Form Submission
    $scope.SaveButtonClick = function SaveBoard(form) {
        if (form.$valid == true) {
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
        $scope.viewOnly = true;
        $timeout(function () {
            //$scope.validateForm();
            if ($scope.validationError == false) {
                $scope.openIndexBackup = angular.toJson($scope.openIndex, true);
            };
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
            rows.Type = 'String';
        }
        else if (rows.Function == 'Function') {
            $scope.SchemeList = [];
            configService.getSchemes().then(function (data) {
                $scope.SchemeList = data;
            }, onError);
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

    $scope.validateForm = function () {
        form = $scope.form;
        returnCount = 0;
        angular.forEach($scope.config, function (value, key, obj) {
            angular.forEach($scope.config[key].Functions, function (valueF, keyF, obj) {
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
            })
        })
    };

    $scope.addMathsItem = function (index) {
        var item = null;
        item = {
            ID: index + 1,
        };
        $scope.config[$scope.colIndex].Functions[$scope.rowIndex].Parameter.splice(index + 1, 0, item);
    },

    $scope.removeMathsItem = function () {
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

    $scope.selectRow = function (event, rowIndex, colIndex) {
        $scope.getVariableTypes(colIndex, rowIndex);
        $scope.Parameter = this.config[colIndex].Functions[rowIndex].Parameter;
        $scope.Function = this.config[colIndex].Functions[rowIndex].Function;
        $scope.rowIndex = rowIndex;
        $scope.colIndex = colIndex;
        if ($scope.Function == 'Function') {
            $scope.function = [];
            $scope.SchemeList = [];
            configService.getSchemes().then(function (data) {
                $scope.SchemeList = data;
            }, onError);
            $scope.FunctionList = [];
            //Add new Item to the selected array
            $scope.getFunctionList = function getFunctionList(rowIndex, colIndex) {
                configService.getFunctionDetails(this.config[colIndex].Functions[rowIndex].Parameter[0].Scheme, 0, "Scheme").then(function (data) {
                    $scope.FunctionList = data;
                }, onError);
            };

            //Add new Item to the selected array
            $scope.setFunctionName = function setFunctionName(rowIndex, colIndex) {
                var arrayID = configFunctionFactory.getIndexOf($scope.FunctionList, parseInt(this.config[colIndex].Functions[rowIndex].Parameter[0].ID), "ID");
                this.config[colIndex].Functions[rowIndex].Parameter[0].FunctionName = $scope.FunctionList[arrayID].Name;
                configService.getFunctionDetails(this.config[colIndex].Functions[rowIndex].Parameter[0].Scheme, $scope.FunctionList[arrayID].ID, "Config").then(function (data) {
                    $scope.getFormFields(angular.fromJson(data[0].Configuration), rowIndex, colIndex);
                }, onError);
            };

            //Single Calculation
            //Get the fields for the input form and null the values out
            $scope.getFormFields = function getFormFields(array, rowIndex, colIndex) {  //function that sets the parameters available under the different variable types
                var counter = 0;
                var scopeid = 0;
                var functionID = 0;
                $scope.fields = [];
                $scope.fieldset = [];
                this.config[colIndex].Functions[rowIndex].Parameter[0].Input = configFunctionFactory.convertToFromJson(array[0]);
                angular.forEach(this.config[colIndex].Functions[rowIndex].Parameter[0].Input.Functions, function (groups) {
                    functionID = 0;
                    $scope.config[colIndex].Functions[rowIndex].Parameter[0].Input.Functions[scopeid].Output = null;
                    scopeid = scopeid + 1
                });
                if (this.config[colIndex].Functions[rowIndex].Parameter[0].Input.length > 0) {
                    $scope.mapFormFields(this.config[colIndex].Functions[rowIndex].Parameter[0].Input);
                };
            };

            $scope.mapFormFields = function mapFormFields(Input) {
                var InputJson = angular.fromJson(Input);
                convertDateStringsToDates([InputJson]);
                $scope.isLoading = true;
                angular.forEach(angular.fromJson(InputJson), function (value, key, obj) {
                    var index = configFunctionFactory.getIndexOf(this.config[colIndex].Functions[rowIndex].Parameter[0].Input.Functions, value.Name, 'Name');
                    this.config[colIndex].Functions[rowIndex].Parameter[0].Input.Functions[index].Output = value.Output;
                });
                this.config[colIndex].Functions[rowIndex].Parameter[0].Input = $scope.configreg;
            };

        };

        if (event.ctrlKey) {
            if (selectedRowsIndexes[colIndex] != null) {
                changeSelectionStatus(rowIndex, colIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            };
        } else if (event.shiftKey) {
            if (selectedRowsIndexes[colIndex] != null) {
                selectWithShift(rowIndex, colIndex);
            }
            else {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            };
        } else {

            if (selectedRowsIndexes[colIndex] != null) {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            }
            else {
                resetSelection();
                selectedRowsIndexes[colIndex] = [rowIndex];
            };
        };
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
        };
    };

    function changeSelectionStatus(rowIndex, colIndex) {
        if ($scope.isRowSelected(rowIndex, colIndex)) {
            unselect(rowIndex, colIndex);
        } else {
            select(rowIndex, colIndex);
        };
    };

    function select(rowIndex, colIndex) {
        if (!$scope.isRowSelected(rowIndex, colIndex)) {
            selectedRowsIndexes[colIndex].push(rowIndex)
        };
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
        if (selectedRowsIndexes[colIndex] != null) {
            return selectedRowsIndexes[colIndex].indexOf(rowIndex) > -1;
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

});
