// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('menuCtrl', function ($scope, $uibModal, $log, $location, $window, boardService) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.orderByField = '';
    $scope.orderByField = 'Name';
    function init() {
        $scope.isLoading = true;
        boardService.initialize().then(function (data) {
            $scope.refreshBoard();
           
        }, onError);
    };

     $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
        boardService.getBoards()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
     };

    $scope.openBoard = function () {
        $scope.ID = this.board.ID;
        var earl = '/Board/' + $scope.ID;
        $window.location.assign('/Project/Board/Board/' + $scope.ID);
    };

     $scope.deleteBoard = function (index) {
         var cf = confirm("Delete this Board?");
         $scope.ID = this.board.ID;
         
         if (cf == true) {
            boardService.updateBoard($scope.ID,"","","Delete")
            .then(function (data) {
                $scope.isLoading = false;
                $scope.Boards.splice(index, 1);
            }, onError);          
         }      
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
         boardService.addConfig($scope.selected).then(function (data) {
             $scope.Boards.push(data);
             $scope.isLoading = false;
         }, onError);
     };

     $scope.updateBoard = function (index) {
         $scope.editingData[this.Boards[index].ID] = false;
         boardService.putConfig(this.Boards[index].ID, this.Boards[index]).then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };

     $scope.deleteBoard = function (index) {
         var cf = confirm("Delete this Board?");
         if (cf == true) {
          boardService.deleteConfig(this.Boards[index].ID)
         .then(function (data) {
             $scope.isLoading = false;
             $scope.Boards.splice(index, 1);
         }, onError);
         }
     };

     $scope.editingData = {};
     
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }


     $scope.modify = function (Boards) {
         $scope.editingData[Boards.ID] = true;
     };

     $scope.update = function (Boards) {
         $scope.editingData[Boards.ID] = false;
         boardService.updateBoard(Boards.ID, Boards.Name, Boards.Configuration).then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
