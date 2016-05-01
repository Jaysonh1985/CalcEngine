// application global namespace
var sulhome = sulhome || {};
sulhome.kanbanBoardApp = angular.module('kanbanBoardApp', ['ui.bootstrap', 'ngRoute', 'ui.sortable', 'gridster'])
    .config(function($routeProvider){

        $routeProvider.when('/Maths', {
            controller: 'mathsCtrl',
            templateUrl: '/Areas/Configuration/Scripts/Maths.html'

        })
        .otherwise({ redirectTo: '/' });
    });


