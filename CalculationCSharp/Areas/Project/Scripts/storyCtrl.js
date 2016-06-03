sulhome.kanbanBoardApp.controller('storyCtrl', function ($scope, $uibModalInstance, ID, Name, Description, AcceptanceCriteria, Moscow, Timebox, User, Tasks, Comments) {
    
    $scope.ID = ID;
    $scope.Name = Name;
    $scope.Description = Description;
    $scope.AcceptanceCriteria = AcceptanceCriteria;
    $scope.Moscow = Moscow;
    $scope.Timebox = Timebox;
    $scope.User = User;
    $scope.Tasks = Tasks;
    $scope.Comments = Comments;




    $scope.selected = {
        Tasks: [{
            TaskName: "",
            TaskUser: "",
            RemainingTime: "",
            Status: ""
        }]
    };

    if (Tasks !== null) {
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
            AcceptanceCriteria: $scope.AcceptanceCriteria,
            Moscow: $scope.Moscow,
            Timebox: $scope.Timebox,
            User: $scope.User,
            Tasks: $scope.selected.Tasks,
            Comments: $scope.Comments
        };

        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});