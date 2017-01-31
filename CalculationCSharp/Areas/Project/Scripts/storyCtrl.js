// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('storyCtrl', function ($http, $scope, $uibModalInstance, $interval, story, UserList, CurrentUser, FileUploadService) {

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
    $scope.FileRepository = story.FileRepository;

    $scope.openIndexUpdates = [true];
    $scope.openIndexUploads = [true];
    
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
            Updates: $scope.Updates,
            FileRepository: $scope.FileRepository
        };
        $scope.OldElapsedTime = 0;
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $scope.stop();
        $uibModalInstance.dismiss('cancel');
    };


    // Upload File Functions

    // Variables
    $scope.Message = "";
    $scope.FileInvalidMessage = "";
    $scope.SelectedFileForUpload = null;
    $scope.FileDescription = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFileValid = false;
    $scope.IsFormValid = false;

    // THIS IS REQUIRED AS File Control is not supported 2 way binding features of Angular
    // ------------------------------------------------------------------------------------
    //File Validation
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ($scope.SelectedFileForUpload != null) {
            if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                $scope.FileInvalidMessage = "";
                isValid = true;
            }
            else {
                $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
            }
        }
        else {
            $scope.FileInvalidMessage = "Image required!";
        }
        $scope.IsFileValid = isValid;
    };

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //----------------------------------------------------------------------------------------

    //Save File
    $scope.SaveFile = function () {
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.FileDescription).then(function (d) {
            if ($scope.FileRepository == null) {
                $scope.FileRepository = [];
            }
            $scope.FileRepository.push({
                FieldID: parseInt( d.FileRepo.FileID, 10),
                FileName: d.FileRepo.FileName,
                FilePath: d.FileRepo.FilePath,
                Description: d.FileRepo.Description,
                FileSize: d.FileRepo.FileSize,
            })
            ClearForm();
        }, function (e) {
            alert(e);
        });

    };
    //Clear form 
    function ClearForm() {
        $scope.FileDescription = "";
        //as 2 way binding not support for File input Type so we have to clear in this way
        //you can select based on your requirement
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });

        $scope.IsFormSubmitted = false;
    }

    //Delete File
    $scope.DeleteFile = function (FileName, ID) {
        var cf = confirm("Delete this File?");
        if (cf == true) {
            $scope.IsFormSubmitted = true;
            $scope.Message = "";
            FileUploadService.DeleteFile(FileName).then(function (d) {
                $scope.FileRepository.splice(ID, 1);
            }, function (e) {
                alert(e);
            });
        }
    };

        //Save File
    $scope.DownloadFile = function (FileName) {

        $http({
            method: 'GET',
            url: '/DownloadFile/DownloadFile',
            params: { FileName: FileName },
            responseType: 'arraybuffer'
        }).success(function (data, status, headers) {
            headers = headers();

            var filename = headers['x-filename'];
            var contentType = headers['content-type'];

            var linkElement = document.createElement('a');
            try {
                var blob = new Blob([data], { type: contentType });
                var url = window.URL.createObjectURL(blob);

                linkElement.setAttribute('href', url);
                linkElement.setAttribute("download", filename);

                var clickEvent = new MouseEvent("click", {
                    "view": window,
                    "bubbles": true,
                    "cancelable": false
                });
                linkElement.dispatchEvent(clickEvent);
            } catch (ex) {
                console.log(ex);
            }
        }).error(function (data) {
            console.log(data);
        });

    };

});