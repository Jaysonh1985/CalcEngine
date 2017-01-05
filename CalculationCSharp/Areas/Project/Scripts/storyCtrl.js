// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('storyCtrl', function ($scope, $uibModalInstance, $interval, story, UserList, CurrentUser) {

    $scope.ID = story.ID;
    $scope.Name = story.Name;
    $scope.Description = story.Description;
    $scope.RAG = story.RAG;
    $scope.SLADays = parseInt(story.SLADays);
    $scope.Requested = story.Requested;
    $scope.StartDate = new Date(story.StartDate);
    $scope.DueDate = new Date(story.DueDate);
    $scope.RequestedDate = story.RequestedDate;
    $scope.ElapsedTime = parseInt(story.ElapsedTime);
    $scope.AcceptanceCriteria = story.AcceptanceCriteria;
    $scope.Moscow = story.Moscow;
    $scope.Complexity = story.Complexity;
    $scope.Effort = story.Effort;
    $scope.Timebox = story.Timebox;
    $scope.User = story.User;
    $scope.Tasks = story.Tasks;
    $scope.Comments = story.Comments;
    $scope.timerStart = false;
    $scope.UserList = UserList;
    $scope.txtCommentUser = CurrentUser;
    $scope.Updates = story.Updates;

    $scope.openIndexUpdates = [true];
    
    $scope.addItem = function () {
        if ($scope.Tasks == null) {
            $scope.Tasks = [];
            $scope.Tasks[0] = {
                TaskName: null,
                TaskUser: null,
                RemainingTime: null,
                Status: null
            }
        }
        else
        {
            $scope.Tasks.push({
                TaskName: "",
                TaskUser: "",
                RemainingTime: "",
                Status: ""
            });
        }
    },

    $scope.removeItem = function (index) {
        $scope.Tasks.splice(index, 1);
    },

    $scope.ElapsedTime = story.ElapsedTime;
    var timerPromise;
    $scope.start = function () {
        $scope.timerStart = true;
        $scope.StartDateString = $scope.StartDate.toDateString();
        if (isNaN(Date.parse($scope.StartDate)) == true || $scope.StartDate.toDateString() == 'Mon Jan 01 1900')
        {
            var todayDate = new Date();
            $scope.StartDate = todayDate;
            var result = new Date();
            result.setDate(result.getDate() + parseInt($scope.SLADays, 10));
            $scope.DueDate = result;
        }      
        timerPromise = $interval(function () {
            if ($scope.ElapsedTime == null)
            {
                $scope.ElapsedTime = 1;
            }
            else
            {
                $scope.ElapsedTime = parseInt($scope.ElapsedTime) + 1 ;
            }
        }, 1000);
    };

    $scope.stop = function () {
        $scope.timerStart = false;
        if (timerPromise) {
            $interval.cancel(timerPromise);
            timerPromise = undefined;
        }
        $scope.logElapsedTime();
    };

    $scope.btn_add = function () {

        if ($scope.Comments == null) {
            $scope.Comments = [];
        }
        
        if ($scope.txtcomment != '') {
            $scope.Comments.push({
                CommentName: $scope.txtcomment,
                CommentDateTime: new Date(),
                CommentType: $scope.txtCommentType,
                CommentUser: $scope.txtCommentUser,
            });
            $scope.txtcomment = "";
        }
    }

    $scope.remItem = function ($index) {
        $scope.Comments.splice($index, 1);
    }

    $scope.logElapsedTime = function () {
        if ($scope.Updates == null) {
            $scope.Updates = [];
        }

        var UpdateValue = 0;
        if ($scope.ElapsedTime > 0 && (story.ElapsedTime != null || $scope.OldElapsedTime > 0)) {
            if ($scope.OldElapsedTime > 0) {
                UpdateValue = parseInt($scope.ElapsedTime) - parseInt($scope.OldElapsedTime);
            }
            else {
                UpdateValue = parseInt($scope.ElapsedTime) - parseInt(story.ElapsedTime);
            }
        }
        else {
            UpdateValue = $scope.ElapsedTime;
        }
        if (UpdateValue > 0)
        {
            $scope.Updates.push({
                UpdateField: 'Elapsed Time',
                UpdateValue: UpdateValue,
                UpdateDateTime: new Date(),
                UpdateUser: CurrentUser
            })
        }
        $scope.OldElapsedTime = $scope.ElapsedTime;
    }

    $scope.formChanges = function () {
        if ($scope.Updates == null) {
            $scope.Updates = [];
        }
       
        if ($scope.storyForm.$dirty) {
            angular.forEach($scope.storyForm, function (value, key) {
                if (key[0] == '$') return;
                if (!value.$pristine) {
                    $scope.Updates.push({
                        UpdateField: key,
                        UpdateValue: value.$modelValue,
                        UpdateDateTime: new Date(),
                        UpdateUser: CurrentUser
                    })
                }
            });
        }
    }

    //Click OK
    $scope.ok = function () {
        $scope.stop();
        $scope.formChanges();
        $scope.selected = {
            ID: $scope.ID,
            Name: $scope.Name,
            Description: $scope.Description,
            Requested: $scope.Requested,
            RAG: $scope.RAG,
            SLADays: $scope.SLADays,
            StartDate: $scope.StartDate,
            DueDate: $scope.DueDate,
            RequestedDate: $scope.RequestedDate,
            ElapsedTime: $scope.ElapsedTime,
            AcceptanceCriteria: $scope.AcceptanceCriteria,
            Moscow: $scope.Moscow,
            Complexity: $scope.Complexity,
            Effort: $scope.Effort,
            Timebox: $scope.Timebox,
            User: $scope.User,
            Tasks: $scope.Tasks,
            Comments: $scope.Comments,
            Updates: $scope.Updates
        };
        $scope.OldElapsedTime = 0;
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $scope.stop();
        $uibModalInstance.dismiss('cancel');
    };


});