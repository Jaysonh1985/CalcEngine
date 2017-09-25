// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('factorsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.factors = [];
    //Map Back the input values if they exist
    if (Functions.length > 0) {
        $scope.factors.TableName = Functions[0].TableName;
        $scope.factors.LookupType = Functions[0].LookupType;
        $scope.factors.LookupValue = Functions[0].LookupValue;
        $scope.factors.OutputType = Functions[0].OutputType;
        $scope.factors.RowMatch = Functions[0].RowMatch;
        $scope.factors.RowMatchLookupType = Functions[0].RowMatchLookupType;
        $scope.factors.RowMatchRowNo = Functions[0].RowMatchRowNo;
        $scope.factors.RowMatchValue = Functions[0].RowMatchValue;
        $scope.factors.ColumnNo = Functions[0].ColumnNo;
        $scope.factors.Interpolate = Functions[0].Interpolate;
    }
    else {
        $scope.factors = Functions
    };
    //Set up new object
    $scope.selected = [];
    //Add new Item to the selected array
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            TableName: $scope.factors.TableName,
            LookupType: $scope.factors.LookupType,
            LookupValue: $scope.factors.LookupValue,
            OutputType: $scope.factors.OutputType,
            RowMatch: $scope.factors.RowMatch,
            RowMatchLookupType: $scope.factors.RowMatchLookupType,
            RowMatchRowNo: $scope.factors.RowMatchRowNo,
            RowMatchValue: $scope.factors.RowMatchValue,
            ColumnNo: $scope.factors.ColumnNo,
            Interpolate: $scope.factors.Interpolate
        })
    };
    //Click OK moves back to modal instantiation
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };
    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
