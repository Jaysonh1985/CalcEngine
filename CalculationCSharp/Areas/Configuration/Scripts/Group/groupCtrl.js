// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('groupCtrl', function ($scope, $uibModalInstance, $log, ID, Name, Description) {
    // Model
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.Group = [];


    //Click OK
    $scope.ok = function () {
        
        $scope.Group.push({
            ID: $scope.ID,
            Name: $scope.Name,
            Description: $scope.Description
        })

        $uibModalInstance.close($scope.Group);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

   
})
