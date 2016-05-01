sulhome.kanbanBoardApp.controller('menuCtrl', function ($scope, $uibModal, $log, $location, $window, boardService) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
  
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


     $scope.deleteBoard = function () {
         var cf = confirm("Delete this Board?");
         $scope.ID = this.board.ID;
         
         if (cf == true) {
            boardService.updateBoard($scope.ID,"","","Delete")
            .then(function (data) {
                $scope.isLoading = false;
                
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
