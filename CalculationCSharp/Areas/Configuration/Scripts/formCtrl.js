sulhome.kanbanBoardApp.controller('formCtrl', function ($scope, $uibModalInstance, $log, $http, $location, Functions) {

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



    
$scope.fields = [
                 {
                     caption: 'Name',
                     model: 'name',
                     type: 'string',
                     maxLength: 25
                 },
                 {
                     caption: 'Date of Birth',
                     model: 'dateOfBirth',
                     type: 'date'
                 },
                 {
                     caption: 'Street Address',
                     model: 'address.street',
                     type: 'string',
                     maxLength: 25
                 },
                 {
                     caption: 'City',
                     model: 'address.city',
                     type: 'string',
                     maxLength: 25
                 },
                 {
                     caption: 'State',
                     model: 'address.state',
                     type: 'select',
                     options: [
                         {
                             caption: 'California',
                             value: 'CA'
                         },
                         {
                             caption: 'New York',
                             value: 'NY'
                         },
                         {
                             caption: 'Washington',
                             value: 'WA'
                         }
                     ]
                 },
                 {
                     caption: 'Zip Code',
                     model: 'address.zipCode',
                     type: 'integer',
                     maxLength: 5,
                     minValue: 10000,
                     maxValue: 99999
                 },
                 {
                     caption: 'Nicknames',
                     model: 'nicknames',
                     type: 'array<string>'
                 }
             ];

             $scope.data = {
                 name: 'John Smith',
                 address: {}
             };
        

});