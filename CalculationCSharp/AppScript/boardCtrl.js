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

     $scope.AddButtonClick = function AddStory() {
         $scope.isLoading = true;
         boardService.moveStory(1,1, "Add", "1","false").then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };

    $scope.UpdateButtonClick = function (size) {
        $scope.Name = this.story.Name;
        $scope.Description = this.story.Description;
        $scope.Moscow = this.story.Moscow;
        $scope.User = this.story.User;
        $scope.ID = this.story.Id;
        $scope.Tasks = this.story.Tasks;
        $scope.colID = this.col.Id;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/AppScript/updateModal.html',
            controller: function ($scope, $uibModalInstance, Name, Description, Moscow, User, ID, Tasks, colID) {
                $scope.Name = Name;
                $scope.Description = Description;
                $scope.Moscow = Moscow;
                $scope.User = User;
                $scope.ID = ID;
                $scope.Tasks = Tasks;
                $scope.colID = colID;

                $scope.DeleteButtonClick = function AddStory() {
                    $scope.isLoading = true;
                    boardService.moveStory($scope.ID, $scope.colID, "Delete", "1","false").then(function (data) {
                        $scope.isLoading = false;
                        boardService.sendRequest();
                    }, onError);
                    $uibModalInstance.dismiss('cancel')
                };

                $scope.AddButtonClickTask = function () {

                    $scope.selected = {
                        Name: $scope.Name,
                        Description: $scope.Description,
                        Moscow: $scope.Moscow,
                        User: $scope.User,
                        Tasks: $scope.Tasks
                    };

                    boardService.moveStory($scope.ID, $scope.colID, "Edit", $scope.selected, "true").then(function (data) {
                        $scope.isLoading = false;
                        boardService.sendRequest();
                    }, onError);

                    $scope.Tasks.push({

                    });

                };

                //Click OK
                $scope.ok = function () {

                    $scope.selected = {
                        Name: $scope.Name,
                        Description: $scope.Description,
                        Moscow: $scope.Moscow,
                        User: $scope.User
                    };

                    boardService.moveStory($scope.ID, $scope.colID, "Edit", $scope.selected, "false").then(function (data) {
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
                Moscow: function () { return $scope.Moscow; },
                User: function () { return $scope.User; },
                ID: function () { return $scope.ID },
                Tasks: function () { return $scope.Tasks },
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
        boardService.canMoveStory(data.ColumnId, targetColId)
            .then(function (canMove) {                
                if (canMove) {                 
                    boardService.moveStory(data.Id, targetColId, "Move","1").then(function (StoryMoved) {
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
