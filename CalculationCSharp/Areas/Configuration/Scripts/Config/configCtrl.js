sulhome.kanbanBoardApp.controller('configCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, configService, configFunctionFactory, configModalFactory, $filter, $timeout) {
    // Model
    $scope.config = [];
    $scope.DecimalNames = [];
    $scope.isLoading = true;
    $scope.oneAtATime = false;
    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
    $scope.openIndex = [true];
    $scope.openIndexRegression = [true];
    $scope.validationError = false;
    $scope.openIndexBackup = null;

    function init() {
        var id = $location.absUrl();
        configService.initialize().then(function (data) {
            $scope.isLoading = true;
            var id = configFunctionFactory.getConfigID();
             configService.getCalc(id)
               .then(function (data) {
                   $scope.isLoading = false;
                   $scope.config = data;                                                        
               }, onError);
        }, onError);
        var id = configFunctionFactory.getConfigID();
        configService.getCalcName(id)
        .then(function (namedata) {
                $scope.isLoading = false;
                $scope.configName = namedata;
        }, onError);
    };

    //Functions
    $scope.AddFunction = function (colIndex, index) {      
        if (colIndex == 0)
        {
            $scope.config[colIndex].Functions.push({
                ID: this.config[colIndex].Functions.length,
                Function: 'Input',
                Logic: [],
                Parameter: []
            });
        }
        else
        {
            $scope.config[colIndex].Functions.push({
                ID: this.config[colIndex].Functions.length,
                Logic: [],
                Parameter: []
            });
        }
     }

    $scope.AddFunctionRows = function (colIndex, index, rows, parentIndex) {
        var item = null;
        if (colIndex == 0)
        {
            item = {
                ID: this.config[colIndex].Functions.length,
                Function: 'Input',
                Logic: [],
                Parameter: []
            };
        }
        else
        {
            item = {
                ID: this.config[colIndex].Functions.length,
                Logic: [],
                Parameter: []
            };
        }
        $scope.config[colIndex].Functions.splice(index + 1, 0, item);
    }

    $scope.CopyFunction = function (colIndex, index) {
        var selectedRows = getSelectedRows(colIndex);
        angular.forEach(selectedRows, function (value, key, prop) {
            var Functions = selectedRows[key];
            var item = null;
            item = angular.copy(Functions);
            $scope.config[colIndex].Functions.splice(index + 1, 0, item);
            index =  index + 1;
        });
    }

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
        }
    }
    //Categories
    $scope.AddCategory = function (colIndex) {
        $scope.config.push({
            ID: this.config.length,
            Name: null,
            Description: null,
            Functions: []
        });
        $scope.GroupButtonClick('lg', this.config.length - 1);
    }

    $scope.AddCategoryRows = function (colIndex) {
        var item = null;
        item = {
            ID: colIndex + 1,
            Name: null,
            Description: null,
            Functions: []
        };
        $scope.config.splice(colIndex + 1, 0, item);
        $scope.GroupButtonClick('lg', colIndex + 1);
        $scope.rebuildCategoryIDs();
    }

    $scope.CopyCategory = function (index, e) {
        var Category = $scope.config[index];
        var item = null;
        item = angular.copy(Category);
        $scope.config.splice(index + 1, 0, item);
    }

    $scope.MoveDownCategory = function (Index, e) {

        if (e) {
            e.preventDefault();
            e.stopPropagation();
        }
        var Category = $scope.config[Index];
        var item = null;
        item = angular.copy(Category);
        $scope.config.splice(Index, 1);
        $scope.config.splice(Index + 1, 0, item);
        $scope.colindex = Index;
    }

    $scope.DeleteCategory = function (colIndex) {
        var cf = confirm("Delete this line?");
        if (cf == true) {
            $scope.config.splice(colIndex, 1);
            //$scope.rebuildCategoryIDs();
        }
    }

    $scope.rebuildCategoryIDs = function rebuildCategoryIDs() {
        colid = 0;
        angular.forEach($scope.config, function (groups) {
            $scope.config[colid].ID = colid;
            rowid = 0;
            angular.forEach($scope.config[colid].Functions, function(rows){
                $scope.config[colid].Functions[rowid].ID = rowid;
                rowid = rowid +1;
            });
            colid = colid + 1;
        });
    }
    ///Form Submission
    $scope.SaveButtonClick = function SaveBoard() {
        $scope.isLoading = true;
        var id = configFunctionFactory.getConfigID();
        $scope.rebuildCategoryIDs();
        configService.putCalc(id, $scope.config).then(function (data) {
            $scope.isLoading = false;
            toastr.success("Saved successfully", "Success");
            $scope.form.$setPristine();
        }, onError);
    };

    $scope.CalcButtonClick = function CalcBoard(form) {

        if ($scope.validationError == false)
        {
            $scope.openIndexBackup = angular.toJson($scope.openIndex, true);
        }    
        $scope.OpenAllButton();
        var id = configFunctionFactory.getConfigID();
        $scope.rebuildCategoryIDs();
        if (form.$valid == true) {
            $scope.validationError = false;
            configService.postCalc(id, $scope.config).then(function (data) {
                $scope.isLoading = false;
                $scope.config = data;
                $scope.openIndex = angular.fromJson($scope.openIndexBackup, true);
                toastr.success("Calculated successfully", "Success");
            }, onError);
        }
        else
        {
            $scope.validationError = true;
            toastr.error("Failed Validations", "Error");
        }
    };

    $scope.setFunction = function (rows) {  //function that sets the type of the row

        if(rows.Function == 'Maths'){
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'MathsFunctions') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'Period') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'Factors') {

        }
        else if (rows.Function == 'Dates') {
            rows.Type = 'Date';
        }
        else if (rows.Function == 'DatePart') {
            rows.Type = 'Decimal';
        }
        else if (rows.Function == 'Input') {
            rows.Type = null;
        }
        else {
            rows.Type = null;
        }      
    }
    //TypeAhead Functions
    $scope.variableArrayBuilder = function variableArrayBuilder(config, colIndex, type, rowIndex) {

        var counter = 0;
        var scopeid = 0;
        var functionID = 0;
        var arrayID = 0;
        $scope.Decimal = [];
        $scope.DecimalValue = [];
        $scope.DecimalParameter = [];
        $scope.Names = [];
        var newArr = [];

        angular.forEach(config, function (groups) {
            if (scopeid <= colIndex) {

                if(type == null)
                {
                    $scope.DecimalValue = ($filter('filter')(config[scopeid].Functions));
                }
                else
                {
                    $scope.DecimalValue = ($filter('filter')(config[scopeid].Functions, { Type: type }));
                }

                if (scopeid == colIndex) {
                    var spliceid = rowIndex;
                    var DecimalValueID = 0;

                    angular.forEach($scope.DecimalValue, function (Names) {
                        $scope.DecimalValue.splice(spliceid, 1);
                    });

                };
 
                functionID = 0;
                angular.forEach($scope.DecimalValue, function (Names) {
                    $scope.DecimalParameter = ($filter('filter')($scope.DecimalValue[functionID].Name));

                    if ($scope.Names.indexOf($scope.DecimalParameter) == -1) {

                        $scope.Names[arrayID] = $scope.DecimalParameter;
                         
                        arrayID = arrayID + 1;

                    }
                    functionID = functionID + 1;

                });
                scopeid = scopeid + 1
            }
        });
        scopeid = 0;
        return $scope.Names;
    }
    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.config, function(value,key,obj){
            $scope.openIndex[key] = true;
        })
    }

    $scope.CloseAllButton = function () {
        angular.forEach($scope.openIndex, function (value, key, obj) {
            $scope.openIndex[key] = false;
        })
    }

    $scope.getVariableTypes = function getVariableTypes(colIndex, rowIndex) {  //function that sets the parameters available under the different variable types
        $scope.DecimalNames = [];
        $scope.DateNames = [];
        $scope.StringNames = [];
        $scope.DecimalNames = $scope.variableArrayBuilder($scope.config, colIndex, 'Decimal', rowIndex);
        $scope.DateNames = $scope.variableArrayBuilder($scope.config, colIndex, 'Date', rowIndex);
        $scope.StringNames = $scope.variableArrayBuilder($scope.config, colIndex, 'String', rowIndex);
    }

    $scope.FunctionButtonClick = function (size, colIndex, index) {
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
                $timeout(function () {
                    var el = document.getElementById($scope.AttName);
                    angular.element(el).triggerHandler('click');
                    $scope.form.$setDirty();
                });

            }, function () {

            });
    };

    $scope.LogicButtonClick = function (size, colIndex, index) {
        $scope.Logic = this.config[colIndex].Functions[index].Logic;      
        $scope.AllNames = [];
        $scope.configReplace = configFunctionFactory.convertToFromJson($scope.config);     
        $scope.AllNames = $scope.variableArrayBuilder($scope.configReplace, colIndex, null, index);
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
            $scope.config[colIndex].Functions[index].Logic = selectedItem;
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
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
                Name: function () { return $scope.Name },
                Description: function () { return $scope.Description }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.config[colIndex].ID = selectedItem[0].ID;
            $scope.config[colIndex].Name = selectedItem[0].Name;
            $scope.config[colIndex].Description = selectedItem[0].Description;            
        }, function () {          
        });
    };

    $scope.HistoryButtonClick = function (size) {
        $scope.ID = configFunctionFactory.getConfigID();
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/History/HistoryModal.html',
            scope: $scope,
            controller: 'historyCtrl',
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
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
    //Higlight rows functions
    var selectedRowsIndexes = [];

    $scope.selectRow = function (event, rowIndex, colIndex) {
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
    }

    function getSelectedRows(colIndex) {
        var selectedRows = [];
        selectedRowsIndexesOrdered = $filter('orderBy')(selectedRowsIndexes[colIndex]);
        angular.forEach(selectedRowsIndexesOrdered, function (value, key, prop) {
            selectedRows.push($scope.config[colIndex].Functions[value]);
        });
        return selectedRows;
    }

    function selectRows(selectFromIndex, selectToIndex, colIndex) {
        for (var rowToSelect = selectFromIndex; rowToSelect <= selectToIndex; rowToSelect++) {
            select(rowToSelect, colIndex);
        }
    }

    function changeSelectionStatus(rowIndex, colIndex) {
        if ($scope.isRowSelected(rowIndex, colIndex)) {
            unselect(rowIndex, colIndex);
        } else {
            select(rowIndex, colIndex);
        }
    }

    function select(rowIndex, colIndex) {
        if (!$scope.isRowSelected(rowIndex, colIndex)) {
            selectedRowsIndexes[colIndex].push(rowIndex)
        }
    }

    function unselect(rowIndex, colIndex) {
        var rowIndexInSelectedRowsList = selectedRowsIndexes[colIndex].indexOf(rowIndex);
        var unselectOnlyOneRow = 1;
        selectedRowsIndexes[colIndex].splice(rowIndexInSelectedRowsList, unselectOnlyOneRow);
    }

    function resetSelection() {
        selectedRowsIndexes = [];
    }

    $scope.isRowSelected = function (rowIndex, colIndex) {
        if (selectedRowsIndexes[colIndex] != null)
        {
            return selectedRowsIndexes[colIndex].indexOf(rowIndex) > -1;
        }
        return false;       
    };

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
