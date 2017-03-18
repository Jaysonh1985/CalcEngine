// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('historyCtrl', function ($scope, $uibModalInstance, $window, $log, $http, $location, configHistoryService, ID, isFunction) {
    init();

    //Map back the calculation histories where available for the specified calculation
    function init() {
        $scope.isLoading = true;
        if (isFunction == true) {
            configHistoryService.getFunctionHistory(ID)
            .then(function (data) {
                $scope.isLoading = false;
                $scope.Boards = data;
            }, onError);
        }
        else {
            configHistoryService.getCalcHistory(ID)
            .then(function (data) {
                $scope.isLoading = false;
                $scope.Boards = data;
            }, onError);
        };
    };

    //Click OK moves back to modal instantiation
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };

    //View Only
    $scope.View = function (boardID) {
        if ($scope.Function == true) {
            $window.open('/Configuration/Function/Function/' + boardID + '#!/?ViewOnly=true&History=true');
        }
        else {
            $window.open('/Configuration/Config/Config/' + boardID + '#!/?ViewOnly=true&History=true');
        };
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
