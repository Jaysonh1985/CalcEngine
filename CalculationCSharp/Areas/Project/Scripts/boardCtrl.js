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
        $scope.isLoading = true;
        $scope.todayDate = new Date();
        boardService.initialize().then(function (data) {
            $scope.isLoading = true;
            id = configFunctionFactory.getConfigID();
            $scope.refreshBoard(id);
            boardService.getUserList(id)
               .then(function (data) {
                   $scope.UserNames = data;
               }, onError);

        }, onError);
        
    };

    $scope.refreshBoard = function refreshBoard(id) {
        boardService.getColumns(id)
           .then(function (data) {
               $scope.isLoading = true;
               $scope.columns = data;
               $scope.setRAG();
           }, onError);
    };

    $scope.rebuildColumnIDs = function rebuildColumnIDs() {
        colid = 0;
        angular.forEach($scope.columns, function (groups) {
            $scope.columns[colid].Id = colid;
            rowid = 0;
            angular.forEach($scope.columns[colid].Stories, function (rows) {
                $scope.columns[colid].Stories[rowid].Id = rowid;
                rowid = rowid + 1;
            });
            colid = colid + 1;
        });
    };
    
    // Model to JSON for demo purpose
    $scope.$watch('columns', function (model) {
        if (initializing) {
            $timeout(function () { initializing = false; });
        } else {

            $scope.isLoading = true;
            id = configFunctionFactory.getConfigID();
            $scope.rebuildColumnIDs();
            boardService.updateBoard(id, $scope.columns).then(function (data) {
                $scope.isLoading = false;
                //toastr.success("Board Saved successfully", "Success");
                boardService.sendRequest();
            }, onError);
            $scope.setRAG();
           
        }
    }, true);

    $scope.setRAG = function setRAG() {
        angular.forEach($scope.columns, function (value, key, prop) {
            var columnIndex = key;
            angular.forEach(value.Stories, function (valueS, keyS, propS) {
                var DueDate = new Date(valueS.DueDate);
                var datediff = Date.daysBetween(new Date(), DueDate);

                if (isNaN(Date.parse(DueDate)) == true || DueDate.toDateString() == 'Mon Jan 01 1900') {
                    $scope.columns[columnIndex].Stories[keyS].RAG = "Green";
                }
                else
                {
                    if (datediff <= 0) {
                        $scope.columns[columnIndex].Stories[keyS].RAG = "Red";
                    }
                    else if (datediff <= (valueS.SLADays / 3)) {
                        $scope.columns[columnIndex].Stories[keyS].RAG = "Amber";
                    }
                    else {
                        $scope.columns[columnIndex].Stories[keyS].RAG = "Green";
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
                $scope.isLoading = true;
                $scope.CSV = data;
                return data;
            }, onError);
         return promise;
     };

     $scope.ImportButtonClick = function ImportButtonClick(CSV) {
         $scope.columnstest = [];
         $scope.Stories = [];
         var index = 0;
         angular.forEach(CSV, function (key, value, obj) {

             $scope.selected = {
                 ID: key.ActivityID,
                 Name: key.ActivityName,
                 Description: key.Description,
                 Requested: key.RequestedBy,
                 RAG: key.RAG,
                 SLADays: key.SLA,
                 StartDate: new Date(key.StartDate),
                 DueDate: new Date(key.DueDate),
                 RequestedDate: new Date (key.RequestedDate),
                 ElapsedTime: key.ElapsedTime,
                 AcceptanceCriteria: key.AcceptanceCriteria,
                 Moscow: key.Moscow,
                 Complexity: key.Complexity,
                 Effort: key.Effort,
                 Timebox: key.Timebox,
                 User: key.CurrentUser,
             };

             $scope.columns[key.ColumnID].Stories.push($scope.selected);

         });
     };

     $scope.toggle = function (scope) {
         scope.toggle();
     };

     $scope.AddButtonClick = function AddStory(ID) {
         $scope.isLoading = true;
         storyID = $scope.columns[ID].Stories.length;
         $scope.columns[ID].Stories.push({
             Name: 'New',
             Description: null,
             AcceptanceCriteria: null,
             ID: storyID,
             RAG: 'Green',
             ElapsedTime: null,
             StartDate: '01/01/1900',
             DueDate: '01/01/1900',
             RequestedDate: '01/01/1900',
         });
     };

     $scope.DeleteButtonClick = function AddStory(colIndex, index) {
         var cf = confirm("Delete this Story?");
         if (cf == true) {
             $scope.columns[colIndex].Stories.splice(index, 1);
             $uibModalInstance.dismiss('cancel')
         }
     };


     $scope.OrderColumnByClick = function OrderColumnByClick(id) {
         $scope.columns[id].Stories = $filter('orderBy')($scope.columns[id].Stories, 'Moscow');
     };
     $scope.ClearFilterClick = function ClearFilterClick() {
         $scope.search.Moscow = null;
         $scope.search.User = null;
     };


     $scope.UpdateButtonClick = function (size, colIndex, index) {
        $scope.index = index;
        $scope.Name = this.story.Name;
        $scope.Description = this.story.Description;
        $scope.RAG = this.story.RAG;
        $scope.SLADays = this.story.SLADays;
        $scope.Requested = this.story.Requested;
        $scope.StartDate = this.story.StartDate;
        $scope.DueDate = this.story.DueDate;
        $scope.RequestedDate = this.story.RequestedDate;
        $scope.ElapsedTime = this.story.ElapsedTime;
        $scope.AcceptanceCriteria = this.story.AcceptanceCriteria;
        $scope.Moscow = this.story.Moscow;
        $scope.Complexity = this.story.Complexity;
        $scope.Effort = this.story.Effort;
        $scope.Timebox = this.story.Timebox;
        $scope.User = this.story.User;
        $scope.ID = this.story.ID;
        $scope.Tasks = this.story.Tasks;
        $scope.Comments = this.story.Comments;
        $scope.colID = colIndex;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Areas/Project/Scripts/updateModal.html',
            scope:$scope,
            controller: 'storyCtrl',
            size: size,
            backdrop: false,
            resolve: {
                Name: function () { return $scope.Name },
                Description: function () { return $scope.Description; },
                Requested: function () { return $scope.Requested; },
                RAG: function () { return $scope.RAG; },
                SLADays: function () { return $scope.SLADays; },
                StartDate: function () { return $scope.StartDate; },
                DueDate: function () { return $scope.DueDate; },
                RequestedDate: function () { return $scope.RequestedDate; },
                ElapsedTime: function () { return $scope.ElapsedTime; },
                AcceptanceCriteria: function () { return $scope.AcceptanceCriteria; },
                Moscow: function () { return $scope.Moscow; },
                Timebox: function () { return $scope.Timebox; },
                Complexity: function () { return $scope.Complexity; },
                Effort: function () { return $scope.Effort; },
                User: function () { return $scope.User; },
                ID: function () { return $scope.ID },
                Tasks: function () { return $scope.Tasks },
                Comments: function () { return $scope.Comments},
                colID: function () { return $scope.colID; },

                UserList: function () {return $scope.UserList}
            }
        });

        modalInstance.result.then(function (selectedItem) {

            $scope.columns[$scope.colID].Stories[$scope.index].ID = selectedItem.ID;
            $scope.columns[$scope.colID].Stories[$scope.index].AcceptanceCriteria = selectedItem.AcceptanceCriteria;
            $scope.columns[$scope.colID].Stories[$scope.index].Comments = selectedItem.Comments;
            $scope.columns[$scope.colID].Stories[$scope.index].Description = selectedItem.Description;
            $scope.columns[$scope.colID].Stories[$scope.index].RAG = selectedItem.RAG;
            $scope.columns[$scope.colID].Stories[$scope.index].SLADays = selectedItem.SLADays;
            $scope.columns[$scope.colID].Stories[$scope.index].Requested = selectedItem.Requested;
            $scope.columns[$scope.colID].Stories[$scope.index].StartDate = selectedItem.StartDate;
            $scope.columns[$scope.colID].Stories[$scope.index].DueDate = selectedItem.DueDate;
            $scope.columns[$scope.colID].Stories[$scope.index].RequestedDate = selectedItem.RequestedDate;
            $scope.columns[$scope.colID].Stories[$scope.index].ElapsedTime = selectedItem.ElapsedTime;
            $scope.columns[$scope.colID].Stories[$scope.index].Moscow = selectedItem.Moscow;
            $scope.columns[$scope.colID].Stories[$scope.index].Complexity = selectedItem.Complexity;
            $scope.columns[$scope.colID].Stories[$scope.index].Effort = selectedItem.Effort;
            $scope.columns[$scope.colID].Stories[$scope.index].Name = selectedItem.Name;
            $scope.columns[$scope.colID].Stories[$scope.index].Timebox = selectedItem.Timebox;
            $scope.columns[$scope.colID].Stories[$scope.index].User = selectedItem.User;
            $scope.columns[$scope.colID].Stories[$scope.index].Tasks = selectedItem.Tasks;
            $scope.columns[$scope.colID].Stories[$scope.index].Comments = selectedItem.Comments;
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
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});


