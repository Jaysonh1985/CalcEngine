sulhome.kanbanBoardApp.controller('storyCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, boardService) {
    
     $scope.UpdateButtonClick = function (size) {

        $scope.Name = this.story.Name;
        $scope.Description = this.story.Description;
        $scope.AcceptanceCriteria = this.story.AcceptanceCriteria;
        $scope.Moscow = this.story.Moscow;
        $scope.Timebox = this.story.Timebox;
        $scope.User = this.story.User;
        $scope.ID = this.story.Id;
        $scope.Tasks = this.story.Tasks;
        $scope.Comments = this.story.Comments;
        $scope.colID = this.col.Id;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Project/Scripts/updateModal.html',
            scope:$scope,
            controller: function ($scope, $uibModalInstance, Name, Description, AcceptanceCriteria, Moscow, Timebox, User, ID, Tasks, Comments, colID) {
                $scope.Name = Name;
                $scope.Description = Description;
                $scope.AcceptanceCriteria = AcceptanceCriteria;
                $scope.Moscow = Moscow;
                $scope.Timebox = Timebox;
                $scope.User = User;
                $scope.Tasks = Tasks;
                $scope.Comments = Comments;
               
                $scope.ID = ID;
                
                $scope.colID = colID;

                $scope.DeleteButtonClick = function AddStory() {
                    $scope.isLoading = true;
                    boardService.moveStory($scope.ID, $scope.colID, "Delete", "1","false").then(function (data) {
                        $scope.isLoading = false;
                        boardService.sendRequest();
                    }, onError);
                    $uibModalInstance.dismiss('cancel')
                };

                $scope.selected = {
                    Tasks: [{
                        TaskName: "",
                        TaskUser: "",
                        RemainingTime: "",
                        Status: ""
                    }]
                };
            
                if (Tasks !== null)
                {
                    $scope.selected.Tasks = ($scope.Tasks);
                }
                    
                $scope.addItem = function () {
                    $scope.selected.Tasks.push({
                        TaskName: "",
                        TaskUser: "",
                        RemainingTime: "",
                        Status: ""
                    });
                },

                $scope.removeItem = function (index) {
                    $scope.selected.Tasks.splice(index, 1);
                },

                $scope.Comments = [];

                if (Comments !== null) {
                    $scope.Comments = (Comments);
                }

                $scope.btn_add = function () {
                    if ($scope.txtcomment != '') {
                        $scope.Comments.push({
                            CommentName:$scope.txtcomment});
                        $scope.txtcomment = "";
                    }
                }

                $scope.remItem = function ($index) {
                    $scope.Comments.splice($index, 1);
                }

                //Click OK
                $scope.ok = function () {


                    
                    $scope.selected = {
                        Name: $scope.Name,
                        Description: $scope.Description,
                        AcceptanceCriteria: $scope.AcceptanceCriteria,
                        Moscow: $scope.Moscow,
                        Timebox: $scope.Timebox,
                        User: $scope.User,
                        Tasks: $scope.selected.Tasks,
                        Comments: $scope.Comments
                    };
                    


                    boardService.moveStory($scope.ID, $scope.colID, "Edit", $scope.selected, "false", $scope.columns).then(function (data) {
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
                AcceptanceCriteria: function () { return $scope.AcceptanceCriteria; },
                Moscow: function () { return $scope.Moscow; },
                Timebox: function () { return $scope.Timebox;},
                User: function () { return $scope.User; },
                ID: function () { return $scope.ID },
                Tasks: function () { return $scope.Tasks },
                Comments: function () { return $scope.Comments},
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
