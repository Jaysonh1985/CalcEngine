﻿// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('storyCtrl', function ($scope, $uibModalInstance, $interval, ID, Name, Description, AcceptanceCriteria, RAG, Requested, StartDate, DueDate, ElapsedTime, Moscow, Complexity, Effort, Timebox, User, Tasks, Comments) {
    
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.RAG = RAG;
    $scope.Requested = Requested;
    $scope.StartDate = new Date(StartDate);
    $scope.DueDate = new Date(DueDate);
    $scope.ElapsedTime = ElapsedTime
    $scope.AcceptanceCriteria = AcceptanceCriteria;
    $scope.Moscow = Moscow;
    $scope.Complexity = Complexity;
    $scope.Effort = Effort;
    $scope.Timebox = Timebox;
    $scope.User = User;
    $scope.Tasks = Tasks;
    $scope.Comments = Comments;
    $scope.timerStart = false;
    
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
        $scope.timerStart = true;
        $scope.StartDateString = $scope.StartDate.toDateString();
        if (isNaN(Date.parse($scope.StartDate)) == true || $scope.StartDate.toDateString() == 'Mon Jan 01    1')
        {
            $scope.StartDate = new Date();
        }      
        timerPromise = $interval(function () {
            if ($scope.ElapsedTime == null)
            {
                $scope.ElapsedTime = 1;
            }
            else
            {
                $scope.ElapsedTime = $scope.ElapsedTime + 1;
            }
        }, 1000);
    };

    $scope.stop = function () {
        $scope.timerStart = false;
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
                CommentName: $scope.txtcomment,
                CommentDateTime: new Date(),
                CommentType: $scope.txtCommentType,
            });
            $scope.txtcomment = "";
        }
    }

    $scope.remItem = function ($index) {
        $scope.Comments.splice($index, 1);
    }

    //Click OK
    $scope.ok = function () {
        $scope.stop();
        $scope.selected = {
            ID: $scope.ID,
            Name: $scope.Name,
            Description: $scope.Description,
            Requested: $scope.Requested,
            RAG: $scope.RAG,
            StartDate: $scope.StartDate,
            DueDate: $scope.DueDate,
            ElapsedTime: $scope.ElapsedTime,
            AcceptanceCriteria: $scope.AcceptanceCriteria,
            Moscow: $scope.Moscow,
            Complexity: $scope.Complexity,
            Effort: $scope.Effort,
            Timebox: $scope.Timebox,
            User: $scope.User,
            Tasks: $scope.Tasks,
            Comments: $scope.Comments
        };
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $scope.stop();
        $uibModalInstance.dismiss('cancel');
    };


});