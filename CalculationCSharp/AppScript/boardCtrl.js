sulhome.kanbanBoardApp.controller('boardCtrl', function ($scope, $uibModal, $log, boardService) {
    // Model
    $scope.columns = [];
    $scope.isLoading = false;
  
    function init() {
        $scope.isLoading = true;
        boardService.initialize().then(function (data) {
            $scope.isLoading = false;
            $scope.refreshBoard();
           
        }, onError);
    };

     $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
        boardService.getColumns()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.columns = data;
           }, onError);
    };    

     $scope.AddButtonClick = function AddTask() {
         $scope.isLoading = true;
         boardService.moveTask(1,this.col.Id, "Add", "1").then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };



    $scope.UpdateButtonClick = function (size) {
        $scope.Name = this.task.Name;
        $scope.Description = this.task.Description;
        $scope.ID = this.task.Id;
        $scope.colID = this.col.Id;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/AppScript/updateModal.html',
            controller: function ($scope, $uibModalInstance, Name, Description,ID,colID) {
                $scope.Name = Name;
                $scope.Description = Description;
                $scope.ID = ID;
                $scope.colID = colID;

                $scope.DeleteButtonClick = function AddTask() {
                    $scope.isLoading = true;
                    boardService.moveTask($scope.ID, $scope.colID, "Delete", "1").then(function (data) {
                        $scope.isLoading = false;
                        boardService.sendRequest();
                    }, onError);
                    $uibModalInstance.dismiss('cancel')
                };

                $scope.ok = function () {

                    $scope.selected = {
                        Name: $scope.Name,
                        Description: $scope.Description
                    };

                    boardService.moveTask($scope.ID, $scope.colID, "Edit", $scope.selected).then(function (data) {
                        $scope.isLoading = false;
                        boardService.sendRequest();
                    }, onError);

                    $uibModalInstance.close($scope.selected.Name);
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            },
            size: size,
            resolve: {
                Name: function () { return $scope.Name },
                Description: function () { return $scope.Description; },
                ID: function () { return $scope.ID },
                colID: function () { return $scope.colID; }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };

     $scope.toggleAnimation = function () {
         $scope.animationsEnabled = !$scope.animationsEnabled;
     };

    $scope.onDrop = function (data, targetColId) {        
        boardService.canMoveTask(data.ColumnId, targetColId)
            .then(function (canMove) {                
                if (canMove) {                 
                    boardService.moveTask(data.Id, targetColId, "Move","1").then(function (taskMoved) {
                        $scope.isLoading = false;                        
                        boardService.sendRequest();
                    }, onError);
                    $scope.isLoading = true;
                }

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
