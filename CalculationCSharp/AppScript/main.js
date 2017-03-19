// Copyright (c) 2016 Project AIM

// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'ngAnimate', 'ui.tree',
                                        'ds.objectDiff', 'ngCsv', 'ngSanitize', 'ngCsvImport', 'ngIdle', 'angular.filter', 
                                        'ngMaterial', 'ngMessages', 'ui.bootstrap.contextMenu'])
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
sulhome.kanbanBoardApp.controller('TimeoutCtrl', function ($scope, Idle, Keepalive, $location, $uibModal, $window, configFunctionFactory, configService) {
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
        if ($window.localStorage["WebAddress"] != null) {
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

    $scope.$on('IdleStart', function () {
        closeModals();
        $scope.warning = $uibModal.open({
            templateUrl: '/AppScript/Timeout/WarningModal.html',
            windowClass: 'modal-danger'
        });
    });

    $scope.$on('IdleEnd', function () {
        closeModals();
    });

    $scope.$on('IdleTimeout', function () {
        closeModals();
        var id = configFunctionFactory.getConfigID();
        var Function = configFunctionFactory.isFunction($location.absUrl());
        var Section = "Calculation";
        if (Function = true) {
            Section = "Function";
        }
        if (id > 0) {
            configService.getUserSession(id, Section).then(function (data) {
                if (data == "") {
                    configService.deleteUserSession(id, Section).then(function (data) {
                    })
                }
            })

            $window.location.assign('/Account/Timeout');
        }
        else {
            $window.location.assign('/Account/Timeout');
        }

    });
    init();
});

sulhome.kanbanBoardApp.directive('ngRightClick', function($parse) {
    return function(scope, element, attrs) {
        var fn = $parse(attrs.ngRightClick);
        element.bind('contextmenu', function(event) {
            scope.$apply(function() {
                event.preventDefault();
                fn(scope, {$event:event});
            });
        });
    };
});

//Timeout config
sulhome.kanbanBoardApp.config(function(IdleProvider, KeepaliveProvider) {
     IdleProvider.idle(20*1-10);
     IdleProvider.timeout(10);
     KeepaliveProvider.interval(10);
 });

sulhome.kanbanBoardApp.filter('stopwatchTime', function () {
    return function (input) {
        if(input){
            input = input * 1000;
            var hours = parseInt(input / 3600000, 10);
            input  %= 3600000;
            var mins = parseInt(input / 60000, 10);
            input %= 60000;
            var secs = parseInt(input / 1000, 10);
            var ms = input % 1000;
            return hours + ' Hours ' + mins + ' Minutes ' + secs + ' Seconds ';
        }
    };
})