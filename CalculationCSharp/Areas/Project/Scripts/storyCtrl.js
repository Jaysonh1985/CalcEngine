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



    $scope.btn_add = function () {

        if ($scope.Comments == null) {
            $scope.Comments = [];
        }
        

        if (Comments !== null) {
            $scope.Comments = (Comments);
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