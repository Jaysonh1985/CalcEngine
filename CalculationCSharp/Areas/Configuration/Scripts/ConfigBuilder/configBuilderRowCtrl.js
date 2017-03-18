// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configBuilderRowCtrl', function ($scope, $location, $window, $uibModal, configFunctionFactory,   $filter) {

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
        item = configFunctionFactory.buildFunction(colIndex, $scope.config);
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
        };
        $scope.form.$setDirty();
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
});
