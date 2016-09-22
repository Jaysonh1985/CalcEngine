// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'gridster', 'dndLists', 'ngTouch', 'ngAnimate', 'ui.tree', 'dynamic-form', 'formly','formlyBootstrap', 'ds.objectDiff', 'ngCsv', 'ngSanitize', 'ngCsvImport'])
    .config(function ($routeProvider) {


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

