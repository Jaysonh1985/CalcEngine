// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('DashboardTeamCtrl', function ($scope, $uibModal, $rootScope, $window, $mdDialog, DashboardService, configMenuService, configHistoryService, configService, calculationService, configFunctionFactory) {
    $scope.TeamMembers = [];
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
        DashboardService.getTeams()
			.then(function (data) {
			    $scope.isLoading = false;
			    $scope.Teams = data;
			    $scope.displayTeamMember(data[0]);
			});
    };

    $scope.displayTeamMember = function displayTeamMember(item) {
        $scope.TeamMembers = item.TeamMembers;
        displayTeamCalcs(item);
    };

    function displayTeamCalcs(item) {
        $scope.Boards = item.TeamCalcs;
    };

    init();

    $scope.openBoard = function (Board) {
        var Section = "Calculation";
        configService.getUserSession(Board.ID, Section)
			.then(function (data) {
			    if (data == "") {
			        RecordEnabled(Board);
			    } else {
			        var cf = confirm("This record is locked by " + data.Username);
			    };
			})
    };
    $scope.viewBoard = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.open('/Configuration/Config/Config/' + $scope.ID + '#!/?ViewOnly=true');
    };

    $scope.addTeam = function addTeam(ev) {
        var confirm = $mdDialog.prompt()
			.title('Please input new Team Name')
			.placeholder('Team name')
			.ariaLabel('Team name')
			.targetEvent(ev)
			.ok('OK')
			.cancel('Cancel');
        $mdDialog.show(confirm)
			.then(function (result) {

			    $scope.selected = {
			        CalcTeamID: null,
			        TeamName: result,
			        TeamOwner: null
			    };
			    DashboardService.postTeams($scope.selected)
					.then(function (data) {
					    $scope.refreshBoard();
					    toastr.success("Team created successfully", "success");
					}, onerror);
			})
    };
    $scope.addTeamMember = function addTeamMember(ev) {
        var confirm = $mdDialog.prompt()
			.title('Please input Email of New Team Member')
			.placeholder('Email Address')
			.ariaLabel('Email Address')
			.targetEvent(ev)
			.ok('OK')
			.cancel('Cancel');
        $mdDialog.show(confirm)
			.then(function (result) {
			    $scope.selected = {
			        UserId: result,
			        CalcTeams: $scope.Teams[0].CalcTeamID
			    };
			    DashboardService.postTeamMembers($scope.selected)
					.then(function (data) {
					    $scope.refreshBoard();
					    toastr.success("Team Member Added successfully", "success");
					}, toastr.error("Team Member not Added", "error"));

			})
    };

    $scope.addExcludeTeamCalc = function addExcludeTeamCalc(Include) {
        configMenuService.getConfig()
			.then(function (data) {
			    $scope.isLoading = false;
			    if (Include == true) {
			        $scope.Calcs = data;
			    } else {
			        $scope.Calcs = $scope.Boards;
			    }
			    var modalInstance = $uibModal.open({
			        animation: true,
			        templateUrl: '/Areas/Dashboard/Scripts/Views/AddTeamCalcModal.html',
			        scope: $scope,
			        controller: 'AddTeamCalcCtrl',
			        size: 'md',
			        resolve: {
			            Calcs: function () {
			                return $scope.Calcs
			            },
			            CalcTeams_CalcTeamID: function () {
			                return $scope.Teams[0].CalcTeamID
			            },
			            Include: function () {
			                return Include
			            }
			        }
			    });
			    modalInstance.result.then(function (selectedItem) {
			        configMenuService.putConfig(selectedItem.ID, selectedItem)
						.then(function (data) {
						    $scope.refreshBoard();
						    $rootScope.$broadcast('updated-team-calc');
						    if (Include == true) {
						        toastr.success("Calculation added to Team", "success");
						    } else {
						        toastr.success("Calculation added to Personal", "success");
						    }
						}, onError);
			        //
			    }, function () { });
			});
    };
    $scope.releaseBoard = function (Board) {
        var cf = confirm("Release this Calculation?");
        if (cf == true) {
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
            configMenuService.putConfig(Board.ID, $scope.selected)
				.then(function (data) {
				    $scope.isLoading = false;
				}, onError);
            configHistoryService.getCalcHistory(Board.ID)
				.then(function (data) {
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
				    if (index == false) {
				        configHistoryService.postCalcHistory($scope.selected.ID, $scope.historySelected)
							.then(function (data) {
							    $scope.isLoading = false;
							}, onError);
				    };
				}, onError);
            calculationService.getSingleConfig(Board.ID)
				.then(function (data) {
				    $scope.isLoading = false;
				    $scope.calcrelease = data;
				    if ($scope.calcrelease == null || $scope.calcrelease.length == 0) {
				        $scope.relaseBoardAdd($scope.selected);
				    } else {
				        $scope.relaseBoardUpdate($scope.calcreleaseID, $scope.selected);
				    };
				    toastr.success("Released successfully", "Success");
				});
        };
    };

    $scope.relaseBoardAdd = function (data) {
        calculationService.addConfig(data)
			.then(function (data) {
			    $scope.isLoading = false;
			}, onError);
    };

    $scope.relaseBoardUpdate = function (ID, data) {
        calculationService.putConfig(ID, data)
			.then(function (data) {
			    $scope.isLoading = false;
			}, onError);
    };

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

    var RecordEnabled = function (Board) {
        $scope.ID = Board.ID;
        var earl = '/Config/' + $scope.ID;
        $window.location.assign('/Configuration/Config/Config/' + $scope.ID);
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
});
