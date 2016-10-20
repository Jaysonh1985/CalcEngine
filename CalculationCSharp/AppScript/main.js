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
//Timeout
sulhome.kanbanBoardApp.controller('TimeoutCtrl', function ($scope, Idle, Keepalive, $uibModal, $window) {
    $scope.started = false;

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
    
    function init() {
        Idle.watch();
    };
    $scope.$on('IdleStart', function() {
        closeModals();

        $scope.warning = $uibModal.open({
            templateUrl: '/AppScript/WarningModal.html',
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
sulhome.kanbanBoardApp.config(function(IdleProvider, KeepaliveProvider) {
     IdleProvider.idle(20*60-10);
     IdleProvider.timeout(10);
     KeepaliveProvider.interval(10);
 });
