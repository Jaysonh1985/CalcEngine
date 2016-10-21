// Copyright (c) 2016 Project AIM

// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'gridster', 'dndLists', 'ngTouch', 'ngAnimate', 'ui.tree', 'dynamic-form', 'ds.objectDiff', 'ngCsv', 'ngSanitize', 'ngCsvImport', 'ngIdle'])
    .config( function ($routeProvider) {
        $routeProvider.when('/', {
            controller: 'configCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Builder.html'

        })
        .otherwise({ redirectTo: '/' });    
        $routeProvider.
                when('/Configuration/Config/Config/:ID', {
                    templateUrl: '/Areas/Configuration/Views/Config/Index.cshtml',
                    controller: 'configCtrl'
                }).
                otherwise({
                    redirectTo: '/'
                });
    });
//Timeout functions
sulhome.kanbanBoardApp.controller('TimeoutCtrl', function ($scope, Idle, Keepalive, $uibModal, $window, configFunctionFactory) {
    $scope.started = false;

    function init() {
        Idle.watch();

        checkLocalStorage();

    };

    function closeModals() {
        if ($scope.warning) {
            $scope.warning.close();
            $scope.warning = null;
        }

        if ($scope.timedout) {
            $scope.timedout.close();
            $scope.timedout = null;
        }
    }
    
    function checkLocalStorage() {
        if ($window.localStorage["WebAddress"] != null)
        {
            var cf = confirm("Do you wish to use continuing using the previous unsaved version?");
            if (cf == true) {
                var webAddress = $window.localStorage.getItem("WebAddress");
                $window.localStorage.removeItem("WebAddress");
                $window.location.assign(webAddress);
            }
            else {
                $window.localStorage.removeItem("Config");
                $window.localStorage.removeItem("WebAddress");
            }
        }
    }

    $scope.$on('IdleStart', function() {
        closeModals();
        $scope.warning = $uibModal.open({
            templateUrl: '/AppScript/Timeout/WarningModal.html',
            windowClass: 'modal-danger'
        });
    });

    $scope.$on('IdleEnd', function() {
        closeModals();
    });

    $scope.$on('IdleTimeout', function() {
        closeModals();    
        $window.location.assign('/Account/Timeout');
    });
    
    init();

})
//Timeout config
sulhome.kanbanBoardApp.config(function(IdleProvider, KeepaliveProvider) {
     IdleProvider.idle(20*60-10);
     IdleProvider.timeout(10);
     KeepaliveProvider.interval(10);
 });
