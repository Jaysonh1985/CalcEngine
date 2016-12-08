// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('storyCtrl', function ($scope, $uibModalInstance, $interval, ID, Name, Description, AcceptanceCriteria, RAG, DueDate, ElapsedTime, Moscow, Timebox, User, Tasks, Comments) {
    
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.RAG = RAG;
    $scope.DueDate = new Date(DueDate);
    $scope.ElapsedTime = ElapsedTime
    $scope.AcceptanceCriteria = AcceptanceCriteria;
    $scope.Moscow = Moscow;
    $scope.Timebox = Timebox;
    $scope.User = User;
    $scope.Tasks = Tasks;
    $scope.Comments = Comments;
    

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

    $scope.ElapsedTime = ElapsedTime;
    var timerPromise;
    $scope.start = function () {
        timerPromise = $interval(function () {
            $scope.ElapsedTime = $scope.ElapsedTime + 1;
        }, 1000);
    };

    $scope.stop = function () {
        if (timerPromise) {
            $interval.cancel(timerPromise);
            timerPromise = undefined;
        }
    };

    $scope.btn_add = function () {

        if ($scope.Comments == null) {
            $scope.Comments = [];
        }
        
        if ($scope.txtcomment != '') {
            $scope.Comments.push({
                CommentName: $scope.txtcomment
            });
            $scope.txtcomment = "";
        }
    }

    $scope.remItem = function ($index) {
        $scope.Comments.splice($index, 1);
    }

    //Click OK
    $scope.ok = function () {

        $scope.selected = {
            ID: $scope.ID,
            Name: $scope.Name,
            Description: $scope.Description,
            RAG: $scope.RAG,
            DueDate: $scope.DueDate,
            ElapsedTime: $scope.ElapsedTime,
            AcceptanceCriteria: $scope.AcceptanceCriteria,
            Moscow: $scope.Moscow,
            Timebox: $scope.Timebox,
            User: $scope.User,
            Tasks: $scope.Tasks,
            Comments: $scope.Comments
        };

        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});