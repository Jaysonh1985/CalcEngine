// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('AddTeamCalcCtrl', function ($scope, $uibModalInstance, $log, Calcs, CalcTeams_CalcTeamID, Include) {
    $scope.Calcs = Calcs
    $scope.Group = [];
    $scope.Include = Include
    $scope.IncludeCalc = function (board) {
        board.CalcTeams = CalcTeams_CalcTeamID;
        $uibModalInstance.close(board);
    };
    $scope.ExcludeCalc = function (board) {
        board.CalcTeams = null;
        $uibModalInstance.close(board);
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
