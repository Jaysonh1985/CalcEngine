﻿// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configBuilderRowCtrl', function ($scope, $location, $window, $uibModal, configFunctionFactory, ngClipboard, $filter, $timeout) {

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
        ['Copy Name', function ($itemScope) {
            ngClipboard.toClipboard($itemScope.rows.Name);
        }],
    ];



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
