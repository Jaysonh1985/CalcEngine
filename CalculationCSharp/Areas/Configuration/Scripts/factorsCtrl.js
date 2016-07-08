sulhome.kanbanBoardApp.controller('factorsCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.factors = [];

    if (Functions.length > 0) {
        
        $scope.factors.TableName = Functions[0].TableName;
        $scope.factors.LookupType = Functions[0].LookupType;
        $scope.factors.LookupValue = Functions[0].LookupValue;
        $scope.factors.DataType = Functions[0].DataType;
        $scope.factors.ColumnNo = Functions[0].ColumnNo;
    }
    else {
        $scope.factors = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            TableName: $scope.factors.TableName,
            LookupType: $scope.factors.LookupType,
            LookupValue: $scope.factors.LookupValue,
            DataType: $scope.factors.DataType,
            ColumnNo: $scope.factors.ColumnNo
        })
    },



    $scope.removeMathsItem = function (index) {
        $scope.factors.splice(index, 1);
    },

    //Click OK
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
