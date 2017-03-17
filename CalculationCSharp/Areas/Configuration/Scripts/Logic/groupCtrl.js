// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('groupCtrl', function ($scope, $uibModalInstance, $log, ID, Name, Description, ColIndex) {
    // Model
    //Map Back the input values
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.Group = [];
    //Click OK moves back to modal instantiation
    $scope.ok = function () {

        $scope.groupForm.Name.$setValidity("columnNameUsed", true);

        angular.forEach($scope.config, function(value, key, obj)
        {
            if(value.Name == $scope.Name && ColIndex != key)
            {
                $scope.groupForm.Name.$setValidity("columnNameUsed", false);
            }
        })

        if ($scope.groupForm.$valid == true) {

            $scope.Group.push({
                ID: $scope.ID,
                Name: $scope.Name,
                Description: $scope.Description
            })
            $uibModalInstance.close($scope.Group);
        }
    };
    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})