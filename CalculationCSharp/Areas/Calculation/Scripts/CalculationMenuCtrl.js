// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('calculationMenuCtrl', function ($scope,  $routeParams, $uibModal, $log, $location, $window, calculationService) {
   // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
    //Initialise
    function init() {
        $scope.isLoading = true;
        calculationService.initialize().then(function (data) {
            $scope.refreshBoard();        
        }, onError);
    };
    //Get List of Available Boards
    $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
         calculationService.getConfig()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
    };
    //Open Boards
    $scope.openBoard = function () {
        $scope.ID = this.board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.location.assign('/Calculation/Calculation/Form/' + $scope.ID);
    };
    $scope.editingData = {};   
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }
     $scope.modify = function (Boards) {
         $scope.editingData[Boards.ID] = true;
     };

     var onError = function (errorMessage) {
         $scope.isLoading = false;
         toastr.error(errorMessage, "Error");
     };

    init();
});
