// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configMenuCtrl', function ($scope,  $routeParams, $uibModal, $log, $location, $window, $filter, configService, calculationService) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
  
    function init() {
        $scope.isLoading = true;
        configService.initialize().then(function (data) {
            $scope.refreshBoard();
           
        }, onError);
    };

    function getIndexOf(arr, val, prop) {
        var l = arr.length,
          k = 0;
        for (k = 0; k < l; k = k + 1) {
            if (arr[k][prop] === val) {
                return k;
            }
        }
        return false;
    };

     $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
        configService.getConfig()
           .then(function (data) {               
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
     };

        $scope.openBoard = function () {
            $scope.ID = this.board.ID;
            var earl = '/Config/' + $scope.ID;
            $window.location.assign('/Configuration/Config/Config/' + $scope.ID);
        };

     $scope.addBoard = function AddBoard() {
         $scope.isLoading = true;

         $scope.selected = {
             ID: null,
             Name: null,
             User: null,
             Group: null,
             Configuration: null
            };

         configService.addConfig($scope.selected).then(function (data) {
             $scope.Boards.push(data);
             $scope.isLoading = false;
         }, onError);
     };

     $scope.updateBoard = function (index) {
         $scope.editingData[this.Boards[index].ID] = false;
         configService.putConfig(this.Boards[index].ID, this.Boards[index]).then(function (data) {
             $scope.isLoading = false;
             configService.sendRequest();
         }, onError);
     };

     $scope.releaseBoard = function (index) {

         $scope.calcrelease = [];
         $scope.calcreleaseID = null;
         $scope.calcreleaseID = this.Boards[index].ID;
         $scope.selected = [];
         $scope.selected = {
             ID: this.Boards[index].ID,
             Scheme: this.Boards[index].Scheme,
             Name: this.Boards[index].Name,
             User: this.Boards[index].User,
             Configuration: this.Boards[index].Configuration,
             Version: Math.ceil(this.Boards[index].Version)

         };

         this.Boards[index].Version = Math.ceil(this.Boards[index].Version);

         configService.putConfig(this.Boards[index].ID, $scope.selected).then(function (data) {
             $scope.isLoading = false;
         }, onError);

         configService.getHistory(this.Boards[index].ID).then(function (data) {
             $scope.isLoading = false;

             $scope.historySelected = {
                 CalcID: $scope.selected.ID,
                 Scheme: $scope.selected.Scheme,
                 Name: $scope.selected.Name,
                 User: $scope.selected.User,
                 Comment: 'Released',
                 Configuration: $scope.selected.Configuration,
                 Version: Math.ceil($scope.selected.Version)
             };

             var index = getIndexOf(data, $scope.historySelected.Version, 'Version');

             if (index == false)
             {
                configService.postHistory($scope.selected.ID, $scope.historySelected).then(function (data) {
                    $scope.isLoading = false;
                }, onError);
             }

         }, onError);

         calculationService.getSingleConfig(this.Boards[index].ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.calcrelease = data;

               if ($scope.calcrelease == null || $scope.calcrelease.length == 0) {
                   $scope.relaseBoardAdd($scope.selected);
               }
               else
               {
                   $scope.relaseBoardUpdate($scope.calcreleaseID, $scope.selected);
               }
               toastr.success("Released successfully", "Success");

           });


     };

     $scope.relaseBoardAdd = function (data) {
         calculationService.addConfig(data).then(function (data) {
             $scope.isLoading = false;
         }, onError);

     };
     $scope.relaseBoardUpdate = function (ID, data) {
         calculationService.putConfig(ID, data).then(function (data) {
             $scope.isLoading = false;
         }, onError);

     };

        $scope.deleteBoard = function (index) {
        var cf = confirm("Delete this Calculation?");
        if (cf == true) {
            configService.deleteConfig(this.Boards[index].ID)
        .then(function (data) {
            $scope.isLoading = false;
            $scope.Boards.splice(index, 1);
        }, onError);
        }};

     $scope.editingData = {};
     
     for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }


     $scope.modify = function (Boards) {
         $scope.editingData[Boards.ID] = true;
     };


    // Listen to the 'refreshBoard' event and refresh the board as a result
     $scope.$parent.$on("refreshBoard", function (e) {
         $scope.refreshBoard();
        toastr.success("Updated successfully", "Success");
    });

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    init();
});
