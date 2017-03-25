// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configMenuAddCalcCtrl', function ($scope, $uibModalInstance, $log, Name, Scheme, SchemeList, Configuration, Copy) {
    if (Copy == true) {
        $scope.Name = Name + " (Copy)";
    }
    else {
        $scope.Name = Name;
    };

    $scope.Scheme = Scheme;
    $scope.Group = [];
    $scope.SchemeList = SchemeList;
    $scope.Configuration = Configuration;
    $scope.ok = function () {       
        $scope.Group.push({
            Scheme: $scope.Scheme,
            Name: $scope.Name, 
            Configuration: $scope.Configuration 
        })
        $uibModalInstance.close($scope.Group);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
