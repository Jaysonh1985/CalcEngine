// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configMenuAddCalcCtrl', function ($scope, $uibModalInstance, $log, Name, Scheme, SchemeList, Configuration, Copy) {
    // Model
    //Map Back the input values
    if (Copy == true) {
        $scope.Name = Name + " (Copy)";
    }
    else {
        $scope.Name = Name;
    };

    $scope.Scheme = Scheme;
    $scope.Group = [];
    $scope.SchemeList = SchemeList;
    $scope.Configuration = Configuration

    //Click OK moves back to modal instantiation
    $scope.ok = function () {       
        $scope.Group.push({
            Scheme: $scope.Scheme,
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
