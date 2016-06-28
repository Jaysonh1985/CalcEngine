sulhome.kanbanBoardApp.controller('inputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {

    // Model

    $scope.form = [];
    $scope.data = [];

    $scope.form.templateOptions = [];
    if (Functions.length > 0) {

        $scope.form.key = Functions[0].key;
        $scope.form.templateOptions.label = Functions[0].templateOptions.label;
        $scope.form.templateOptions.type = Functions[0].templateOptions.type;
        $scope.form.templateOptions.required = Functions[0].templateOptions.required;
    }
    else {
        $scope.form = Functions
    }

    $scope.selected = [];
    $scope.addItem = function AddItem() {
        $scope.selected.push({
            key: $scope.form.key,
            type: 'input',
            templateOptions:{
                label: $scope.form.templateOptions.label,
                type: $scope.form.templateOptions.type,
                required: $scope.form.templateOptions.required}
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
