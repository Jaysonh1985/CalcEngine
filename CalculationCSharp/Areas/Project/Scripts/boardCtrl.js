// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('boardCtrl', function ($scope, $uibModal, $log, $http, $location, $timeout, $window, $routeParams, $filter, boardService, configFunctionFactory) {
    // Model
    $scope.columns = [];
    $scope.isLoading = true;
    $scope.csv = {
        content: null,
        header: true,
        headerVisible: true,
        separator: ',',
        separatorVisible: true,
        result: null,
        encoding: 'ISO-8859-1',
        encodingVisible: true,
        uploadButtonLabel: "Upload File"
    };

    $scope.columns = {
        selected: null,
    };

    var initializing = true;
    $scope.state = false;

    $scope.toggleState = function () {
        $scope.state = !$scope.state;
    };

    function init() {
        var id = $location.absUrl();
        $scope.todayDate = new Date();
        boardService.initialize().then(function (data) {
            $scope.isLoading = true;
            id = configFunctionFactory.getConfigID();
            $scope.refreshBoard(id);
            boardService.getUserList(id)
               .then(function (data) {
                   $scope.UserNames = data;
               }, onError);
            boardService.getCurrentUser(id)
                .then(function (dataUser) {
                    $scope.CurrentUser = dataUser;
                }, onError);
            $scope.isLoading = false;
        }, onError);
        
    };

    function ColumnReport() {
        $scope.BacklogCount = $scope.columns[0].ProjectStories.length;
        $scope.InProgressCount = $scope.columns[1].ProjectStories.length;
        $scope.PendingCount = $scope.columns[2].ProjectStories.length;
        $scope.ReleaseCount = $scope.columns[3].ProjectStories.length;
        $scope.TotalCount = $scope.BacklogCount + $scope.InProgressCount + $scope.PendingCount + $scope.ReleaseCount;
    }

    $scope.refreshBoard = function refreshBoard(id) {
        boardService.getColumns(id)
           .then(function (data) {
               $scope.isLoading = true;
               $scope.columns = data;
               $scope.setRAG();
               ColumnReport();
               $scope.isLoading = false;
           }, onError);
    };

    var waitForRenderAndDoSomething = function () {
        if ($http.pendingRequests.length > 0) {
            $timeout(waitForRenderAndDoSomething); // Wait for all templates to be loaded
        } else {
            $scope.isLoading = false;
        }
    }

    //$scope.UpdateBoard = function () {
    //    id = configFunctionFactory.getConfigID();
    //    var promise = boardService.updateBoard(id, $scope.columns).then(function (data) {         
    //        $scope.columns = data;
    //        $scope.isLoading = false;
    //        boardService.sendRequest();
    //        return data;
    //    }, onError);
    //    return promise;
    //};

    // Model to JSON for demo purpose
    $scope.$watch('columns', function (model) {
        if (initializing) {
            $timeout(function () { initializing = false; });
        } else {
            $scope.isLoading = true;
            id = configFunctionFactory.getConfigID();
            boardService.updateBoard(id, $scope.columns).then(function (data) {
                boardService.sendRequest();
                $timeout(waitForRenderAndDoSomething);
                $scope.isLoading = false;
            }, onError);
            $scope.setRAG();
        }
    }, true);
    

    $scope.setRAG = function setRAG() {
        angular.forEach($scope.columns, function (value, key, prop) {
            var columnIndex = key;
            angular.forEach(value.ProjectStories, function (valueS, keyS, propS) {
                var DueDate = new Date(valueS.DueDate);
                var datediff = Date.daysBetween(new Date(), DueDate);
                if (isNaN(Date.parse(DueDate)) == true || DueDate.toDateString() == 'Mon Jan 01 1900') {
                    $scope.columns[columnIndex].ProjectStories[keyS].RAG = "Green";
                }
                else
                {
                    if (datediff <= 0) {
                        $scope.columns[columnIndex].ProjectStories[keyS].RAG = "Red";
                    }
                    else if (datediff <= (valueS.SLADays / 3)) {
                        $scope.columns[columnIndex].ProjectStories[keyS].RAG = "Amber";
                    }
                    else {
                        $scope.columns[columnIndex].ProjectStories[keyS].RAG = "Green";
                    }
                }
            });
        });
    };

    Date.daysBetween = function (date1, date2) {
        //Get 1 day in milliseconds
        var one_day = 1000 * 60 * 60 * 24;
        // Convert both dates to milliseconds
        var date1_ms = date1.getTime();
        var date2_ms = date2.getTime();
        // Calculate the difference in milliseconds
        var difference_ms = date2_ms - date1_ms;
        // Convert back to days and return
        return Math.round(difference_ms / one_day);
    }

     $scope.CSVButtonClick = function CSVButtonClick() {
         $scope.isLoading = true;
         var url = location.pathname;
         var id = url.substring(url.lastIndexOf('/') + 1);
         id = parseInt(id, 10);
         if (angular.isNumber(id) == false) {
             id = null;
         }
         var promise = boardService.getCSV(id)
            .then(function (data) {              
                $scope.CSV = data;
                $scope.isLoading = false;
                $scope.setRAG();
                toastr.success("Exported Successfully", "Success");
                return data;
            }, onError);
         return promise;
     };

     ExcelCSVJSONReplace = function (str) {
         var newString = str.replace(/""/g, '"');
         newString = newString.substring(0, newString.length - 1);
         return newString.substring(1);
     }

     $scope.ImportButtonClick = function ImportButtonClick(CSV) {

         if (CSV == null)
         {
             $window.alert("You have not imported a file");
         }
         else
         {
             var cf = confirm("This will remove all current cards permanently, do you wish to continue?");
             if (cf == true) {
                 angular.forEach($scope.columns, function (key, value, obj) {
                     $scope.columns[value].ProjectStories = [];
                 });

                 angular.forEach(CSV, function (key, value, obj) {

                     if (key.Moscow == '') {
                         key.Moscow = null;
                     };
                     if (key.CurrentUser == '') {
                         key.CurrentUser = null;
                     };
                     if (key.ActivityName == '') {
                         key.ActivityName = null;
                     };
                     if (key.Description == '') {
                         key.Description = null;
                     };
                     if (key.RequestedBy == '') {
                         key.RequestedBy = null;
                     };
                     if (key.RAG == '') {
                         key.RAG = null;
                     };
                     if (key.Timebox == '') {
                         key.Timebox = null;
                     };
                     if (key.AcceptanceCriteria == '') {
                         key.AcceptanceCriteria = null;
                     };
                     if (key.Complexity == '') {
                         key.Complexity = null;
                     };
                     if (key.Effort == '') {
                         key.Effort = null;
                     };
                     var TaskString = null;
                     var CommentString = null;
                     var UpdateString = null;

                     if (key.ProjectTasks != null && key.Task != "")
                     {
                         TaskString = angular.fromJson(ExcelCSVJSONReplace(key.ProjectTasks));
                     }
                     if (key.ProjectComments != null && key.ProjectComments != "")
                     {
                         CommentString = angular.fromJson(ExcelCSVJSONReplace(key.ProjectComments));
                     }
                     if (key.ProjectUpdates != null && key.ProjectUpdates != "")
                     {
                         UpdateString = angular.fromJson(ExcelCSVJSONReplace(key.ProjectUpdates));
                     }
                     
                     $scope.selected = {
                         StoryId: key.ActivityID,
                         Name: key.ActivityName,
                         Description: key.Description,
                         Requested: key.RequestedBy,
                         RAG: key.RAG,
                         SLADays: key.SLA,
                         ElapsedTime: null,
                         StartDate: '01/01/1900',
                         DueDate: '01/01/1900',
                         RequestedDate: '01/01/1900',
                         AcceptanceCriteria: key.AcceptanceCriteria,
                         Moscow: key.Moscow,
                         Complexity: key.Complexity,
                         Effort: key.Effort,
                         Timebox: key.Timebox,
                         User: key.CurrentUser,
                         ProjectTasks: TaskString,
                         ProjectComments: CommentString,
                         ProjectUpdates: UpdateString
                     };
                     var columnKey = 0;
                     if (key.ColumnName == "Backlog") {
                         columnKey = 0;
                     }
                     else if (key.ColumnName == "In Progress") {
                         columnKey = 1;
                     }
                     else if (key.ColumnName == "Pending") {
                         columnKey = 2;
                     }
                     else if (key.ColumnName == "Release") {
                         columnKey = 3;
                     };
                     $scope.columns[columnKey].ProjectStories.push($scope.selected);
                 });

                 toastr.success("Imported Successfully", "Success");
             }            
         }
     };

     $scope.toggle = function (scope) {
         scope.toggle();
     };

     $scope.AddButtonClick = function AddStory(ID) {
         storyID = $scope.columns[ID].ProjectStories.length;
         $scope.columns[ID].ProjectStories.push({
             Name: 'New',
             Description: null,
             AcceptanceCriteria: null,
             StoryId: storyID,
             RAG: 'Green',
             ElapsedTime: null,
             StartDate: new Date('01/01/1900'),
             DueDate: new Date('01/01/1900'),
             RequestedDate: new Date('01/01/1900'),
             Moscow:null
         });
     };

     $scope.DeleteButtonClick = function AddStory(colIndex, index) {
         var cf = confirm("Delete this Story?");
         if (cf == true) {
             $scope.columns[colIndex].ProjectStories.splice(index, 1);
             $uibModalInstance.dismiss('cancel')
         }
     };

     $scope.OrderColumnByClick = function OrderColumnByClick(id) {
         $scope.columns[id].ProjectStories = $filter('orderBy')($scope.columns[id].ProjectStories, 'Moscow');
     };
     $scope.ClearFilterClick = function ClearFilterClick() {
         $scope.search = null;
     };

     $scope.UpdateButtonClick = function (size, colIndex, index) {
        
         $scope.index = index;
         $scope.colID = colIndex;
         $scope.story = [];

         $scope.story = {
             Name: this.story.Name,
             Description: this.story.Description,
             Requested: this.story.Requested,
             RAG: this.story.RAG,
             SLADays: this.story.SLADays,
             StartDate: this.story.StartDate,
             DueDate: this.story.DueDate,
             RequestedDate: this.story.RequestedDate,
             ElapsedTime: this.story.ElapsedTime,
             AcceptanceCriteria: this.story.AcceptanceCriteria,
             Moscow: this.story.Moscow,
             Timebox: this.story.Timebox,
             Complexity: this.story.Complexity,
             Effort: this.story.Effort,
             User: this.story.User,
             StoryId: this.story.StoryId,
             ProjectTasks: this.story.ProjectTasks,
             ProjectComments: this.story.ProjectComments,
             ProjectUpdates: this.story.ProjectUpdates,
             colID: colIndex,
         };

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Project/Scripts/updateModal.html',
            scope:$scope,
            controller: 'storyCtrl',
            size: size,
            backdrop: false,
            resolve: {
                story: function () { return $scope.story },
                UserList: function () { return $scope.UserList },
                CurrentUser: function () {return $scope.CurrentUser}
            }
        });

        modalInstance.result.then(function (selectedItem) {

            $scope.columns[$scope.colID].ProjectStories[$scope.index].StoryId = selectedItem.StoryId;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].AcceptanceCriteria = selectedItem.AcceptanceCriteria;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Comments = selectedItem.Comments;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Description = selectedItem.Description;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].RAG = selectedItem.RAG;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].SLADays = selectedItem.SLADays;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Requested = selectedItem.Requested;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].StartDate = selectedItem.StartDate;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].DueDate = selectedItem.DueDate;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].RequestedDate = selectedItem.RequestedDate;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].ElapsedTime = selectedItem.ElapsedTime;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Moscow = selectedItem.Moscow;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Complexity = selectedItem.Complexity;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Effort = selectedItem.Effort;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Name = selectedItem.Name;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].Timebox = selectedItem.Timebox;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].User = selectedItem.User;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].ProjectTasks = selectedItem.ProjectTasks;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].ProjectComments = selectedItem.ProjectComments;
            $scope.columns[$scope.colID].ProjectStories[$scope.index].ProjectUpdates = selectedItem.ProjectUpdates;
        }, function () {

        });      

    };

     $scope.ReportButtonClick = function () {

         var modalInstance = $uibModal.open({
             animation: true,
             templateUrl: '/Areas/Project/Scripts/reportModal.html',
             scope: $scope,
             controller: 'reportCtrl',
             size: 'lg',
             backdrop: false,
             resolve: {
                 Name: function () { return $scope.Name },
                 UserList: function () { return $scope.UserList }
             }
         });

         modalInstance.result.then(function () {          
         }, function () {

         });
     };

     $scope.toggleAnimation = function () {
         $scope.animationsEnabled = !$scope.animationsEnabled;
     };

    // Listen to the 'refreshBoard' event and refresh the board as a result
     $scope.$parent.$on("refreshBoard", function (e) {
         id = configFunctionFactory.getConfigID();
         $scope.refreshBoard(id);
    });

     var onError = function (errorMessage) {
        toastr.error(errorMessage, "Error");
    };

    init();
});


