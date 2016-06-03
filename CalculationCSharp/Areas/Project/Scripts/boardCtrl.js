sulhome.kanbanBoardApp.controller('boardCtrl', function ($scope, $uibModal, $log, $http, $location, $window, $routeParams, boardService) {
    // Model
    $scope.columns = [];
    $scope.isLoading = true;

    $scope.columns = {
        selected: null,
    };

    // Model to JSON for demo purpose
    $scope.$watch('columns', function (model) {
        $scope.modelAsJson = angular.toJson(model, true);
    }, true);

    function init() {
        var id = $location.absUrl();
        $scope.isLoading = true;
        boardService.initialize().then(function (data) {
            $scope.isLoading = true;

            var url = location.pathname;
            var id = url.substring(url.lastIndexOf('/') + 1);
            id = parseInt(id, 10);
            if (angular.isNumber(id) == false) {
                id = null;
            }

            boardService.getColumns(id)
               .then(function (data) {
                   $scope.isLoading = true;
                   $scope.columns = data;
               }, onError);

        }, onError);
    };


     $scope.CSVButtonClick = function CSVButtonClick() {
         $scope.isLoading = true;
         var url = location.pathname;
         var id = url.substring(url.lastIndexOf('/') + 1);
         id = parseInt(id, 10);
         if (angular.isNumber(id) == false) {
             id = null;
         }
         boardService.getCSV(id)
             {

                 };
     };

     $scope.AddButtonClick = function AddStory(ID) {
         $scope.isLoading = true;
         storyID = $scope.columns[ID].Stories.length;
         $scope.columns[ID].Stories.push({
             Name: 'New',
             Description: '',
             AcceptanceCriteria: '',
             ID: storyID
         });

     };



     $scope.SaveButtonClick = function SaveBoard() {
         $scope.isLoading = true;
         var url = location.pathname;
         var id = url.substring(url.lastIndexOf('/') + 1);
         id = parseInt(id, 10);
         if (angular.isNumber(id) == false) {
             id = null;
         }
         boardService.updateBoard(id, "TestBoard", $scope.columns).then(function (data) {
             $scope.isLoading = false;
             boardService.sendRequest();
         }, onError);
     };

     $scope.UpdateButtonClick = function (size, index) {

        $scope.index = index;
        $scope.Name = this.story.Name;
        $scope.Description = this.story.Description;
        $scope.AcceptanceCriteria = this.story.AcceptanceCriteria;
        $scope.Moscow = this.story.Moscow;
        $scope.Timebox = this.story.Timebox;
        $scope.User = this.story.User;
        $scope.ID = this.story.ID;
        $scope.Tasks = this.story.Tasks;
        $scope.Comments = this.story.Comments;
        $scope.colID = this.$parent.index;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Project/Scripts/updateModal.html',
            scope:$scope,
            controller: 'storyCtrl',
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

            $scope.columns[$scope.colID].Stories[$scope.index].ID = selectedItem.ID;
            $scope.columns[$scope.colID].Stories[$scope.index].AcceptanceCriteria = selectedItem.AcceptanceCriteria;
            $scope.columns[$scope.colID].Stories[$scope.index].Comments = selectedItem.Comments;
            $scope.columns[$scope.colID].Stories[$scope.index].Description = selectedItem.Description;
            $scope.columns[$scope.colID].Stories[$scope.index].Moscow = selectedItem.Moscow;
            $scope.columns[$scope.colID].Stories[$scope.index].Name = selectedItem.Name;
            $scope.columns[$scope.colID].Stories[$scope.index].Timebox = selectedItem.Timebox;
            $scope.columns[$scope.colID].Stories[$scope.index].User = selectedItem.User;
            $scope.columns[$scope.colID].Stories[$scope.index].Tasks = selectedItem.Tasks;
            $scope.columns[$scope.colID].Stories[$scope.index].Comments = selectedItem.Comments;
            
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


