// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('projectMenuAddCalcCtrl', function ($scope, $uibModalInstance, $log, Name, Configuration, Group, Copy) {
    // Model
    //Map Back the input values
    if (Copy == true)
    {
        $scope.Name = Name + " (Copy)";
    }
    else
    {
        $scope.Name = Name;
    }  
    $scope.Group = [];
    $scope.Configuration = Configuration;
    $scope.Client = Group;

    //Click OK moves back to modal instantiation
    $scope.ok = function () {       
        $scope.Group.push({
            Group: $scope.Client,
            Name: $scope.Name, 
            Configuration: $scope.Configuration 
        })
        $uibModalInstance.close($scope.Group);
    };

    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

})
