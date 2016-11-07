// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configMenuCtrl', function ($scope,  $routeParams, $uibModal, $log, $location, $window, $filter, configService, calculationService, configFunctionFactory) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
    $scope.openIndex = [true];
    $scope.orderByField = 'Scheme';
    $scope.reverseSort = false;

    function init() {
        $scope.isLoading = true;
        configService.initialize().then(function (data) {
            $scope.refreshBoard();
           
        }, onError);
    };

    $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
        configService.getConfig()
            .then(function (data) {               
                $scope.isLoading = false;
                $scope.Boards = data;
            }, onError);
     };

    $scope.openBoard = function (Board) {
        configService.getUserSession(Board.ID).then(function (data) {         
            if(data == "")
            {
                RecordEnabled(Board);
            }
            else
            {
                var cf = confirm("This record is locked by " + data.Username);
            }
        })
    };

    $scope.viewBoard = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.location.assign('/Configuration/Config/Config/' + $scope.ID + '#?ViewOnly=true');
    };


    $scope.addBoard = function AddBoard() {
        //Creates TypeAhead Values
        var SchemeList = [];
        configService.getSchemes().then(function (data) {
            SchemeList = data;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/Areas/Configuration/Scripts/CalculationMenu/ConfigMenuAddCalcModal.html',
                scope: $scope,
                controller: 'configMenuAddCalcCtrl',
                size: 'md',
                resolve: {
                    Name: function () { return $scope.Name },
                    Scheme: function () { return $scope.Scheme },
                    SchemeList: function () { return SchemeList },
                }
            });
            modalInstance.result.then(function (selectedItem) {
                $scope.selected = {
                    ID: null,
                    Name: selectedItem[0].Name,
                    Scheme: selectedItem[0].Scheme,
                    User: null,
                    Group: null,
                    Configuration: null
                };

                configService.addConfig($scope.selected).then(function (data) {
                    $scope.Boards.push(data);
                    $scope.isLoading = false;
                }, onError);

                toastr.success("Calculation created successfully", "Success");
            }, function () {
            });

        }, onError);

    };

    $scope.updateBoard = function (Board) {
        $scope.editingData[Board.ID] = false;
        configService.putConfig(Board.ID, Board).then(function (data) {
             $scope.isLoading = false;
             configService.sendRequest();
         }, onError);
     };

    $scope.releaseBoard = function (Board) {

         $scope.calcrelease = [];
         $scope.calcreleaseID = null;
         $scope.calcreleaseID = Board.ID;
         $scope.selected = [];
         $scope.selected = {
             ID: Board.ID,
             Scheme: Board.Scheme,
             Name: Board.Name,
             User: Board.User,
             Configuration: Board.Configuration,
             Version: Math.ceil(Board.Version)

         };

         Board.Version = Math.ceil(Board.Version);

         configService.putConfig(Board.ID, $scope.selected).then(function (data) {
             $scope.isLoading = false;
         }, onError);

         configService.getHistory(Board.ID).then(function (data) {
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

             var index = configFunctionFactory.getIndexOf(data, $scope.historySelected.Version, 'Version');

             if (index == false)
             {
                configService.postHistory($scope.selected.ID, $scope.historySelected).then(function (data) {
                    $scope.isLoading = false;
                }, onError);
             }

         }, onError);

        calculationService.getSingleConfig(Board.ID)
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

    $scope.deleteBoard = function (Board) {

        var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.ID,"ID");
        var cf = confirm("Delete this Calculation?");
        if (cf == true) {
            configService.deleteConfig(Board.ID)
        .then(function (data) {
            $scope.isLoading = false;
            $scope.Boards.splice(arrayID, 1);
        }, onError);
        }
     };

    $scope.editingData = {};
     
    for (var i = 0, length = $scope.Boards.length; i < length; i++) {
         $scope.editingData[$scope.Boards[i].ID] = false;
     }

    $scope.modify = function (Boards) {
        configService.getSchemes().then(function (data) {
            $scope.SchemeList = data;
            $scope.editingData[Boards.ID] = true;
        }, onError);
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
    
    var RecordEnabled = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.location.assign('/Configuration/Config/Config/' + $scope.ID);
    };

    init();
});
