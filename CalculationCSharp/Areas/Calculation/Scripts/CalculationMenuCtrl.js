// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('calculationMenuCtrl', function ($scope,  $routeParams, $uibModal, $log, $location, $window, calculationService) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
  
    function init() {
        $scope.isLoading = true;
        calculationService.initialize().then(function (data) {
            $scope.refreshBoard();
           
        }, onError);
    };

     $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
         calculationService.getConfig()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
     };

        $scope.openBoard = function () {
            $scope.ID = this.board.ID;
            var earl = '/Config/' + $scope.ID;
            $window.location.assign('/Calculation/Calculation/Form/' + $scope.ID);
        };

     $scope.addBoard = function AddBoard() {
         $scope.isLoading = true;

         $scope.selected = {
             ID: '',
             Name: '',
             User: '',
             Group: '',
             Configuration: ''

            };

         configService.addConfig($scope.selected).then(function (data) {
             $scope.Boards.push(data);
             $scope.isLoading = false;
         }, onError);
     };

     $scope.updateBoard = function (index) {
         $scope.editingData[this.Boards[index].ID] = false;
         calculationService.putConfig(this.Boards[index].ID, this.Boards[index]).then(function (data) {
             $scope.isLoading = false;
             configService.sendRequest();
         }, onError);
     };

        $scope.deleteBoard = function (index) {
        var cf = confirm("Delete this Board?");
        if (cf == true) {
            calculationService.deleteConfig(this.Boards[index].ID)
        .then(function (data) {
            $scope.isLoading = false;
            $scope.Boards.splice(index, 1);
        }, onError);
        }};

     $scope.editingData = {};
     
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }


     $scope.modify = function (Boards) {
         $scope.editingData[Boards.ID] = true;
     };


     

    // Listen to the 'refreshBoard' event and refresh the board as a result
    $scope.$parent.$on("refreshBoard", function (e) {
        $scope.refreshBoard();
        toastr.success("Board updated successfully", "Success");
    });

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
