sulhome.kanbanBoardApp.controller('groupCtrl', function ($scope, $uibModalInstance, $log, $http, $location, $window, $routeParams, Name, Description) {
    // Model
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.Group = [];


    //Click OK
    $scope.ok = function () {
        
        $scope.Group.push({
            Name: $scope.Name,
            Description: $scope.Description
        })

        $uibModalInstance.close($scope.Group);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
