// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('historyCtrl', function ($scope, $uibModalInstance, $log, $http, $location, configService, ID) {
    init();
    //Map back the calculation histories where available for the specified calculation
    function init() {
      $scope.isLoading = true;
        configService.getHistory(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
    };
    //Click OK moves back to modal instantiation
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };
    //Reverts the configuration back to the previously stated configuration
    $scope.revert = function (Board) {      
        $uibModalInstance.close(Board.Configuration);
    };
    //Cancel Modal destroy modal values
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    //Error signalR
    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };
})
