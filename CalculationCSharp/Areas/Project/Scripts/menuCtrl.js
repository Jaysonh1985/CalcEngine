// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('menuCtrl', function ($scope, $uibModal, $log, $location, $window, boardService, configFunctionFactory) {
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
        $scope.ID = this.board.BoardId;
        var earl = '/Board/' + $scope.ID;
        $window.location.assign('/Project/Board/Board/' + $scope.ID);
    };

     $scope.addBoard = function AddBoard() {

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/ProjectMenuAddCalcModal.html',
             scope: $scope,
             controller: 'projectMenuAddCalcCtrl',
             size: 'md',
             resolve: {
                 Name: function () { return $scope.Name },
                 Configuration: function () { return null },
                 Copy: function () { return false }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 BoardId: null,
                 Name: selectedItem[0].Name,
                 User: null,
                 Group: null,
                 Configuration: null
             };

             boardService.addConfig($scope.selected).then(function (data) {
                 $scope.Boards.push(data);
                 $scope.isLoading = false;
             }, onError);

             toastr.success("Project Board created successfully", "Success");
         }, function () {
         });

     };

     $scope.copyBoard = function CopyBoard(Board) {

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/ProjectMenuAddCalcModal.html',
             scope: $scope,
             controller: 'projectMenuAddCalcCtrl',
             size: 'md',
             resolve: {
                 Name: function () { return Board.Name },
                 Configuration: function () { return Board.Configuration },
                 Copy: function () { return true }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 BoardId: null,
                 Name: selectedItem[0].Name,
                 User: null,
                 Group: null,
                 Configuration: selectedItem[0].Configuration
             };

             boardService.addConfig($scope.selected).then(function (data) {
                 $scope.Boards.push(data);
                 $scope.isLoading = false;
             }, onError);

             toastr.success("Project Board created successfully", "Success");
         }, function () {
         });

     };

     $scope.updateBoard = function (index) {

         var ID = this.Boards[index].BoardId;
         var Name = this.Boards[index].Name;
         var Configuration = this.Boards[index].Configuration;
         var User = this.Boards[index].User;

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/ProjectMenuAddCalcModal.html',
             scope: $scope,
             controller: 'projectMenuAddCalcCtrl',
             size: 'md',
             resolve: {
                 Name: function () { return Name },
                 Configuration: function () { return Configuration },
                 Copy: function () { return false }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 BoardId: ID,
                 Name: selectedItem[0].Name,
                 User: User,
                 Group: null,
                 Configuration: Configuration
             };
             
             boardService.putConfig($scope.selected.ID, $scope.selected).then(function (data) {
                 $scope.isLoading = false;
                 boardService.sendRequest();
                 $scope.Boards[index] = $scope.selected;
             }, onError);

             toastr.success("Project Board updated successfully", "Success");
         }, function () {
         });

     };

     $scope.deleteBoard = function (Board) {
         var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.BoardId, "BoardId");
         var cf = confirm("Delete this Board?");
         if (cf == true) {
             boardService.deleteConfig(Board.BoardId)
         .then(function (data) {
             $scope.isLoading = false;
             $scope.Boards.splice(arrayID, 1);
         }, onError);
         }
     };

     $scope.editingData = {};
     
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].BoardId] = false;
     }


     $scope.modify = function (index) {
         $scope.editingData[index] = true;
     };

     $scope.update = function (Boards) {
         $scope.editingData[Boards.BoardId] = false;
         boardService.updateBoard(Boards.BoardId, Boards.Name, Boards.Configuration).then(function (data) {
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
