// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('configMenuCtrl', function ($scope,  $routeParams, $uibModal, $log, $location, $window, $filter, configMenuService, calculationService, configFunctionFactory) {
    // Model
    $scope.Boards = [];
    $scope.isLoading = false;
    $scope.selected = [];
    $scope.openIndex = [true];
    $scope.orderByField = 'Scheme';
    $scope.reverseSort = false;
    $scope.Function = configFunctionFactory.isFunction($location.absUrl());

    function init() {
        $scope.isLoading = true;
        configMenuService.initialize().then(function (data) {
            $scope.refreshBoard();
           
        }, onError);
    };

    $scope.refreshBoard = function refreshBoard() {        
        $scope.isLoading = true;
	    if ($scope.Function == true) {
	        configMenuService.getFunction()
                .then(function (data) {               
                    $scope.isLoading = false;
                    $scope.Boards = data;
                }, onError);
	    }
	    else {
	    configService.getConfig()
                .then(function (data) {               
                    $scope.isLoading = false;
                    $scope.Boards = data;
                }, onError);
	    }
     };

    $scope.openBoard = function (Board) {
        var Section = "Calculation";
        if ($scope.Function == true) {
            Section = "Function";
        }
        configService.getUserSession(Board.ID, Section).then(function (data) {         
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
	    if ($scope.Function == true) {
	        $window.open('/Configuration/Function/Function/' + $scope.ID + '#!/?ViewOnly=true');
	    }
	    else
	    {
	        $window.open('/Configuration/Config/Config/' + $scope.ID + '#!/?ViewOnly=true');
	    }   
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
                    Configuration: function () { return null },
                    Copy: function () { return false }
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

	        if($scope.Function == true){
		        configService.addFunction($scope.selected).then(function (data) {
                    $scope.Boards.push(data);
                    $scope.isLoading = false;
                }, onError);
                toastr.success("Function created successfully", "Success");
	        }
	        else{
		        configService.addConfig($scope.selected).then(function (data) {
	                $scope.Boards.push(data);
	                $scope.isLoading = false;
	            }, onError);
	            toastr.success("Calculation created successfully", "Success");
	        }	
            }, function () {
        });

        }, onError);
    };

    $scope.copyBoard = function AddBoard(Board) {
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
                    Name: function () { return Board.Name },
                    Scheme: function () { return Board.Scheme },
                    SchemeList: function () { return SchemeList },
                    Configuration: function () { return Board.Configuration },
                    Copy: function () { return true }
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
		        if($scope.Function == true){
		            configService.addFunction($scope.selected).then(function (data) {
                        $scope.Boards.push(data);
                        $scope.isLoading = false;
                    }, onError);
                    toastr.success("Function created successfully", "Success");
		        }
		        else{
		            configService.addConfig($scope.selected).then(function (data) {
                        $scope.Boards.push(data);
                        $scope.isLoading = false;
                    }, onError);

                    toastr.success("Calculation created successfully", "Success");
		        }
            }, function () {
                        });

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
        configService.getSchemes().then(function (data) {
            SchemeList = data;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/Areas/Configuration/Scripts/CalculationMenu/ConfigMenuAddCalcModal.html',
                scope: $scope,
                controller: 'configMenuAddCalcCtrl',
                size: 'md',
                resolve: {
                    Name: function () { return Name },
                    Scheme: function () { return Scheme },
                    SchemeList: function () { return SchemeList },
                    Configuration: function () { return Configuration },
                    Copy: function () { return false }
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
		        if($scope.Function == true)
		        {
		            configService.putFunction($scope.selected.ID, $scope.selected).then(function (data) {
                        $scope.isLoading = false;
                        $scope.Boards[arrayID] = $scope.selected;
                        configService.sendRequest();                   
                    }, onError);
                    toastr.success("Function Updated successfully", "Success");
		        }
		        else
		        {
		            configService.putConfig($scope.selected.ID, $scope.selected).then(function (data) {
                        $scope.isLoading = false;
                        $scope.Boards[arrayID] = $scope.selected;
                        configService.sendRequest();                   
                    }, onError);
                    toastr.success("Calculation Updated successfully", "Success");
		        }
            }, function () {
                    });
        }, onError);
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

            configService.putConfig(Board.ID, $scope.selected).then(function (data) {
                $scope.isLoading = false;
            }, onError);

            configService.getCalcHistory(Board.ID).then(function (data) {
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
                    configService.postCalcHistory($scope.selected.ID, $scope.historySelected).then(function (data) {
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
                   else {
                       $scope.relaseBoardUpdate($scope.calcreleaseID, $scope.selected);
                   }
                   toastr.success("Released successfully", "Success");

               });
        }
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
            if ($scope.Function == true) {
                configService.deleteFunction(Board.ID)
                .then(function (data) {
                    $scope.isLoading = false;
                    $scope.Boards.splice(arrayID, 1);
                }, onError);
            }
            else {
                configService.deleteConfig(Board.ID)
                .then(function (data) {
                    $scope.isLoading = false;
                    $scope.Boards.splice(arrayID, 1);
                }, onError);
            }
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

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };
    
    var RecordEnabled = function (Board) {
        $scope.ID = Board.ID;
        if ($scope.Function == true){
            var earl = '/Function/' +$scope.ID;
            $window.location.assign('/Configuration/Function/Function/' +$scope.ID);
        }
        else {
            var earl = '/Config/' + $scope.ID;
            $window.location.assign('/Configuration/Config/Config/' + $scope.ID);
        }
    };

    //UI
    $scope.OpenAllButton = function () {
        angular.forEach($scope.Boards, function (value, key, obj) {
            $scope.openIndex[key] = true;
        })
    }

    $scope.CloseAllButton = function () {
        angular.forEach($scope.Boards, function (value, key, obj) {
            $scope.openIndex[key] = false;
        })
    }

    init();
});
