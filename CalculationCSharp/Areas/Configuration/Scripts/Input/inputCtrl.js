// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('inputCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {

    // Model
    $scope.form = [];
    $scope.data = [];
    $scope.noSpacesPattern = /^[a-zA-Z0-9-_]+$/;

    $scope.form.templateOptions = [];
    if (Functions.length > 0) {

        $scope.form.key = Functions[0].key;
        $scope.form.templateOptions.label = Functions[0].templateOptions.label;
        $scope.form.templateOptions.type = Functions[0].templateOptions.type;
        $scope.form.templateOptions.list = Functions[0].templateOptions.list;
        $scope.form.templateOptions.required = Functions[0].templateOptions.required;
        var Output = '';
            angular.forEach(Functions[0].templateOptions.options, function (object) { 
                Output += object.Name + ',';
            });
            Output = Output.substring(0, Output.length - 1);
            $scope.form.templateOptions.options = Output;

    }
    else {
        $scope.form = Functions
    }

    $scope.selected = [];
    $scope.addItem = function AddItem() {

        var input = angular.isArray($scope.form.templateOptions.options);
        var options = [];
        var array = null;
        if ($scope.form.templateOptions.list == true)
        {
            if (angular.isArray($scope.form.templateOptions.options) == false) {
                array = $scope.form.templateOptions.options.split(',');
                angular.forEach(array, function (object) {

                    options.push({
                        Name: object
                    });

                });

            }
            else {
                options = $scope.form.templateOptions.options;
            }
        }


        $scope.selected.push({
            key: $scope.form.key,
            type: 'input',
            templateOptions:{
                label: $scope.form.templateOptions.label,
                type: $scope.form.templateOptions.type,
                required: $scope.form.templateOptions.required,
                list: $scope.form.templateOptions.list,
                options: options

                
            }
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
