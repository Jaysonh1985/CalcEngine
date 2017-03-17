// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('configHistoryService', function ($http, $q, $rootScope) {
    var proxy = null;

    //History services
     var getCalcHistory = function (id) {
         return $http.get("/api/CalcHistories", { params: { id: id, SelectList: true } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var getCalcHistorySingle = function (id) {
         return $http.get("/api/CalcHistories", { params: { id: id, SelectList: false } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var postCalcHistory = function (id, data) {
         return $http.post("/api/CalcHistories/" + id, data).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var getFunctionHistory = function (id) {
         return $http.get("/api/FunctionHistories", { params: { id: id, SelectList: true } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var getFunctionHistorySingle = function (id) {
         return $http.get("/api/FunctionHistories", { params: { id: id, SelectList: false } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var postFunctionHistory = function (id, data) {
         return $http.post("/api/FunctionHistories/" + id, data).then(function (response) {
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
    // Call 'NotifyBoardUpdated' on SignalR server
    var sendRequest = function () {        
        this.proxy.invoke('NotifyBoardUpdated');
    };

    return {
        initialize: initialize,
        sendRequest: sendRequest,
        getCalcHistory: getCalcHistory,
        getCalcHistorySingle: getCalcHistorySingle,
        postCalcHistory: postCalcHistory,
        getFunctionHistory: getFunctionHistory,
        getFunctionHistorySingle: getFunctionHistorySingle,
        postFunctionHistory: postFunctionHistory
    };
});