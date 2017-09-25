// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('DashboardFunctionCtrl', function ($scope, $uibModal, $window, configMenuService, configHistoryService, configService, calculationService, configFunctionFactory) {

    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
    $scope.openIndex = [true];
    $scope.orderByField = 'Scheme';
    $scope.reverseSort = false;

    function init() {
        configMenuService.initialize()
			.then(function (data) {
			    $scope.refreshBoard();
			});
    };

    $scope.refreshBoard = function refreshBoard() {
        configMenuService.getFunction()
			.then(function (data) {
			    $scope.isLoading = false;
			    $scope.Boards = data;
			});
    };

    $scope.viewBoard = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.open('/Configuration/Function/Function/' + $scope.ID + '#!/?ViewOnly=true');
    };

    $scope.openBoard = function (Board) {
        var Section = "Function";
        configService.getUserSession(Board.ID, Section)
			.then(function (data) {
			    if (data == "") {
			        RecordEnabled(Board);
			    } else {
			        var cf = confirm("This record is locked by " + data.Username);
			    };
			})
    };

    $scope.addBoard = function AddBoard() {
        var SchemeList = [];
        configService.getSchemes()
			.then(function (data) {
			    SchemeList = data;
			    var modalInstance = $uibModal.open({
			        animation: true,
			        templateUrl: '/Areas/Configuration/Scripts/ConfigMenu/ConfigMenuAddCalcModal.html',
			        scope: $scope,
			        controller: 'configMenuAddCalcCtrl',
			        size: 'md',
			        resolve: {
			            Name: function () {
			                return $scope.Name
			            },
			            Scheme: function () {
			                return $scope.Scheme
			            },
			            SchemeList: function () {
			                return SchemeList
			            },
			            Configuration: function () {
			                return null
			            },
			            Copy: function () {
			                return false
			            }
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
			        configMenuService.addFunction($scope.selected)
						.then(function (data) {
						    $scope.Boards.push(data);
						    $scope.isLoading = false;
						}, onError);
			        toastr.success("Function created successfully", "Success");
			    }, function () { });
			}, onError);
    };

    $scope.copyBoard = function AddBoard(Board) {
        var SchemeList = [];
        configService.getSchemes()
			.then(function (data) {
			    SchemeList = data;
			    var modalInstance = $uibModal.open({
			        animation: true,
			        templateUrl: '/Areas/Configuration/Scripts/ConfigMenu/ConfigMenuAddCalcModal.html',
			        scope: $scope,
			        controller: 'configMenuAddCalcCtrl',
			        size: 'md',
			        resolve: {
			            Name: function () {
			                return Board.Name
			            },
			            Scheme: function () {
			                return Board.Scheme
			            },
			            SchemeList: function () {
			                return SchemeList
			            },
			            Configuration: function () {
			                return Board.Configuration
			            },
			            Copy: function () {
			                return true
			            }
			        }
			    });
			    modalInstance.result.then(function (selectedItem) {
			        $scope.selected = {
			            ID: null,
			            Name: selectedItem[0].Name,
			            Scheme: selectedItem[0].Scheme,
			            User: null,
			            Group: null,
			            Configuration: selectedItem[0].Configuration
			        };
			        configMenuService.addFunction($scope.selected)
						.then(function (data) {
						    $scope.Boards.push(data);
						    $scope.isLoading = false;
						}, onError);
			        toastr.success("Function created successfully", "Success");
			    }, function () { });
			}, onError);
    };

    $scope.updateBoard = function (Board) {
        var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.ID, "ID");
        var ID = this.Boards[arrayID].ID;
        var Name = this.Boards[arrayID].Name;
        var Scheme = this.Boards[arrayID].Scheme;
        var Configuration = this.Boards[arrayID].Configuration;
        var User = this.Boards[arrayID].User;
        var Version = this.Boards[arrayID].Version;
        //Creates TypeAhead Values
        var SchemeList = [];
        configService.getSchemes()
			.then(function (data) {
			    SchemeList = data;
			    var modalInstance = $uibModal.open({
			        animation: true,
			        templateUrl: '/Areas/Configuration/Scripts/ConfigMenu/ConfigMenuAddCalcModal.html',
			        scope: $scope,
			        controller: 'configMenuAddCalcCtrl',
			        size: 'md',
			        resolve: {
			            Name: function () {
			                return Name
			            },
			            Scheme: function () {
			                return Scheme
			            },
			            SchemeList: function () {
			                return SchemeList
			            },
			            Configuration: function () {
			                return Configuration
			            },
			            Copy: function () {
			                return false
			            }
			        }
			    });
			    modalInstance.result.then(function (selectedItem) {
			        $scope.selected = {
			            ID: ID,
			            Name: selectedItem[0].Name,
			            Scheme: selectedItem[0].Scheme,
			            User: User,
			            Group: null,
			            Configuration: Configuration,
			            UpdateDate: new Date(),
			            Version: Version
			        };
			        configMenuService.putFunction($scope.selected.ID, $scope.selected)
						.then(function (data) {
						    $scope.isLoading = false;
						    $scope.Boards[arrayID] = $scope.selected;
						    configMenuService.sendRequest();
						}, onError);
			        toastr.success("Function Updated successfully", "Success");
			    }, function () { });
			}, onError);
    };

    $scope.deleteBoard = function (Board) {
        var arrayID = configFunctionFactory.getIndexOf($scope.Boards, Board.ID, "ID");
        var cf = confirm("Delete this Calculation?");
        if (cf == true) {
            configMenuService.deleteFunction(Board.ID)
				.then(function (data) {
				    $scope.isLoading = false;
				    $scope.Boards.splice(arrayID, 1);
				}, onError);
        };
    };

    $scope.editingData = {};

    for (var i = 0, length = $scope.Boards.length; i < length; i++) {
        $scope.editingData[$scope.Boards[i].ID] = false;
    };

    $scope.modify = function (Boards) {
        configService.getSchemes()
			.then(function (data) {
			    $scope.SchemeList = data;
			    $scope.editingData[Boards.ID] = true;
			}, onError);
    };

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    var RecordEnabled = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Function/' + $scope.ID;
        $window.location.assign('/Configuration/Function/Function/' + $scope.ID);
    };

    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.Boards, function (value, key, obj) {
            $scope.openIndex[key] = true;
        })
    };

    $scope.CloseAllButton = function () {
        angular.forEach($scope.Boards, function (value, key, obj) {
            $scope.openIndex[key] = false;
        })
    };

    init();
});
