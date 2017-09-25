angular.module('inputErrorsApp', ['ngMaterial', 'ngMessages'])
// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('AppCtrl', function ($scope,AppService) {
    function init() {
        $scope.sent = false;
    };
    $scope.SubmitButtonClick = function SubmitButtonClick(form) {
        if (form.$valid == true && form.$invalid == false) {
            AppService.postContactForm($scope.user).then(function (data) {
                $scope.sent = true;
            }, onError);
            var onError = function (errorMessage) {
                $scope.sent = false;
            };
        };
    };
    init();
});
sulhome.kanbanBoardApp.directive('expand', function () {
    function link(scope, element, attrs) {
        scope.$on('onExpandAll', function (event, args) {
            scope.expanded = args.expanded;
        });
    }
    return {
        link: link
    };
});
sulhome.kanbanBoardApp.service('AppService', function ($http, $q, $rootScope) { 
    var postContactForm = function (data) {
        return $http.post("/api/Contact/PostContactForm", data)
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
    };
    var initialize = function () {
        connection = jQuery.hubConnection();
        this.proxy = connection.createHubProxy('KanbanBoard');
        // Listen to the 'BoardUpdated' event that will be pushed from SignalR server
        this.proxy.on('BoardUpdated', function () {
            $rootScope.$emit("refreshBoard");
        });
        // Connecting to SignalR server        
        return connection.start()
        .then(function (connectionObj) {
            return connectionObj;
        }, function (error) {
            return error.message;
        });
    };
    return {
        postContactForm: postContactForm,
    };
});