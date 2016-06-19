sulhome.kanbanBoardApp.controller('inputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {

    // Model
    $scope.form = [];

    if (Functions.length > 0) {

        $scope.form.caption = Functions[0].caption;
        $scope.form.model = Functions[0].model;
        $scope.form.type = Functions[0].type;
        $scope.form.maxLength = Functions[0].maxLength;
    }
    else {
        $scope.form = Functions
    }

    $scope.selected = [];
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            caption: $scope.form.caption,
            model: $scope.form.model,
            type: $scope.form.type,
            maxLength: $scope.form.maxLength
        })
    },



    $scope.removeMathsItem = function (index) {
        $scope.form.splice(index, 1);
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
