sulhome.kanbanBoardApp.controller('datesCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {
    // Model
    $scope.dates = [];

    if (Functions.length > 0) {
        
        $scope.dates.Date1 = Functions[0].Date1;
        $scope.dates.DateAdjustmentType = Functions[0].DateAdjustmentType;
        $scope.dates.Date2 = Functions[0].Date2;
        $scope.dates.Inclusive = Functions[0].Inclusive;
        $scope.dates.DaysinYear = Functions[0].DaysinYear;
    }
    else {
        $scope.dates = Functions
    }
    
    $scope.selected = [];
    $scope.addItem = function AddItem () {
        $scope.selected.push({
            DateAdjustmentType: $scope.dates.DateAdjustmentType,
            Date1: $scope.dates.Date1,
            Date2: $scope.dates.Date2,
            Inclusive: $scope.dates.Inclusive,
            DaysinYear: $scope.dates.DaysinYear,
        })
    },



    $scope.removeMathsItem = function (index) {
        $scope.dates.splice(index, 1);
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
