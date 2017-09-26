// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configBuilderColumnCtrl', function ($scope, $uibModal, configFunctionFactory, configTypeaheadFactory) {
     $scope.columnMenuOptions = [
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
        item = {
            ID: colIndex + 1,
            Name: null,
            Description: null,
            Results: [],
            Logic: []
        };
        $scope.config.splice(colIndex + 1, 0, item);
        $scope.GroupButtonClick('lg', colIndex + 1);
        $scope.rebuildCategoryIDs();
    };

    $scope.AddFunctionRowsCat = function (colIndex, index) {
        var item;
        item = configFunctionFactory.buildResult(colIndex, $scope.config);
        $scope.config[colIndex].Results.splice(index + 1, 0, item);
        toastr.success("Rows Added", "Success");
    };

    $scope.CopyCategory = function (index, e) {
        var Category = $scope.config[index];
        var item = null;
        item = angular.copy(Category);
        item.Name = item.Name + " Copy " + (index +1);
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
        };
        $scope.form.$setDirty();
    };

    $scope.GroupButtonClick = function (size, colIndex) {
        $scope.ID = this.config[colIndex].ID;
        $scope.Name = this.config[colIndex].Name;
        $scope.Description = this.config[colIndex].Description;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/FunctionBarViews/Group.html',
            scope: $scope,
            controller: 'groupCtrl',
            backdrop: false,
            size: size,
            resolve: {
                ID: function () { return $scope.ID },
                Name: function () { return $scope.Name },
                Description: function () { return $scope.Description },
                ColIndex: function () { return colIndex }
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

    $scope.CategoryLogicButtonClick = function (size, index) {
        $scope.Logic = this.config[index].Logic;
        $scope.AllNames = [];
        $scope.configReplace = configFunctionFactory.convertToFromJson($scope.config);
        $scope.AllNames = configTypeaheadFactory.variableArrayBuilder($scope.configReplace, index, null, 0);
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Configuration/Scripts/ConfigFunctions/Logic/LogicModal.html',
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
});
