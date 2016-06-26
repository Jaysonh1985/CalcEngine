// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'gridster', 'dndLists', 'pr.longpress', 'ngTouch', 'ngAnimate', 'ui.tree', 'dynamic-form', 'formly','formlyBootstrap'])
    .config(function ($routeProvider) {


        $routeProvider.when('/', {
            controller: 'configCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Builder.html'

        })
        .otherwise({ redirectTo: '/' });

        $routeProvider.when('/Maths', {
            controller: 'mathsCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Maths.html'

        })
        .otherwise({ redirectTo: '/' });

        $routeProvider.
                when('/Configuration/Config/Config#/:ID', {
                    templateUrl: '/Areas/Configuration/Views/Config/Index.cshtml',
                    controller: 'configCtrl'
                }).
                otherwise({
                    redirectTo: '/'
                });

    });

