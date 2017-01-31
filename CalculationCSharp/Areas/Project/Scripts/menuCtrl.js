// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('menuCtrl', function ($scope, $uibModal, $log, $location, $window, boardService, configFunctionFactory) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.orderByField = '';
    $scope.orderByField = 'Name';
    $scope.openIndex = [true];
    $scope.reverseSort = false;

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

     $scope.addBoard = function AddBoard() {
         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/ProjectMenuAddCalcModal.html',
             scope: $scope,
             controller: 'projectMenuAddCalcCtrl',
             size: 'md',
             resolve: {
                 Group: function () { return $scope.Group },
                 Name: function () { return $scope.Name },
                 Configuration: function () { return null },
                 Copy: function () { return false }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 ID: null,
                 Name: selectedItem[0].Name,
                 User: null,
                 Group: selectedItem[0].Group,
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
                 Group: function () { return Board.Group },
                 Name: function () { return Board.Name },
                 Configuration: function () { return Board.Configuration },
                 Copy: function () { return true }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 ID: null,
                 Name: selectedItem[0].Name,
                 User: null,
                 Group: selectedItem[0].Group,
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

     $scope.updateBoard = function (Board) {

         var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.ID, "ID");
         var ID = this.Boards[arrayID].ID;
         var Group = this.Boards[arrayID].Group;
         var Name = this.Boards[arrayID].Name;
         var Configuration = this.Boards[arrayID].Configuration;
         var User = this.Boards[arrayID].User;

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/ProjectMenuAddCalcModal.html',
             scope: $scope,
             controller: 'projectMenuAddCalcCtrl',
             size: 'md',
             resolve: {
                 Group: function () { return Group },
                 Name: function () { return Name },
                 Configuration: function () { return Configuration },
                 Copy: function () { return false }
             }
         });
         modalInstance.result.then(function (selectedItem) {
             $scope.selected = {
                 ID: ID,
                 Name: selectedItem[0].Name,
                 User: User,
                 Group: selectedItem[0].Group,
                 Configuration: Configuration,
                 UpdateDate: new Date()
             };
             
             boardService.putConfig($scope.selected.ID, $scope.selected).then(function (data) {
                 $scope.isLoading = false;
                 boardService.sendRequest();
                 $scope.Boards[arrayID] = $scope.selected;
             }, onError);

             toastr.success("Project Board updated successfully", "Success");
         }, function () {
         });

     };

     $scope.deleteBoard = function (Board) {
         var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.ID, "ID");
         var cf = confirm("Delete this Board?");
         if (cf == true) {
             boardService.deleteConfig(Board.ID)
         .then(function (data) {
             $scope.isLoading = false;
             $scope.Boards.splice(arrayID, 1);
         }, onError);
         }
     };

     $scope.editingData = {};
     
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }


     $scope.modify = function (index) {
         $scope.editingData[index] = true;
     };

     $scope.update = function (Boards) {
         $scope.editingData[Boards.ID] = false;
         boardService.updateBoard(Boards.ID, Boards.Name, Boards.Configuration).then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };

    //UI
     $scope.OpenAllButton = function () {
         angular.forEach($scope.Boards, function (value, key, obj) {
             $scope.openIndex[key] = true;
         })
     }

     $scope.CloseAllButton = function () {
         angular.forEach($scope.Boards, function (value, key, obj) {
             $scope.openIndex[key] = false;
         })
     }

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
