// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('groupCtrl', function ($scope, $uibModalInstance, $log, ID, Name, Description) {
    // Model
    //Map Back the input values
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.Group = [];
    //Click OK moves back to modal instantiation
    $scope.ok = function () {       
        $scope.Group.push({
            ID: $scope.ID,
            Name: $scope.Name,
            Description: $scope.Description
        })
        $uibModalInstance.close($scope.Group);
    };
    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
