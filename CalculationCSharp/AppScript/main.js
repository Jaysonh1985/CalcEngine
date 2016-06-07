// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'gridster','dndLists','pr.longpress','ngTouch', 'ngAnimate', 'ui.tree'])
    .config(function ($routeProvider) {

        $routeProvider.when('/', {
            controller: 'configCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Input.html'

        })
        .otherwise({ redirectTo: '/' });

        $routeProvider.when('/Maths', {
            controller: 'mathsCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Maths.html'

        })
        .otherwise({ redirectTo: '/' });

    });

