﻿// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('boardService', function ($http, $q, $rootScope) {
    var proxy = null;
    
    var getColumns = function (id) {
        return $http.get("/api/ColumnWebApi/Get",{ params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var getUserList = function (id) {
        return $http.get("/api/UserListWebApi/Get", { params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var getCurrentUser = function (id) {
        return $http.get("/api/CurrentUserWebApi/Get", { params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var addConfig = function (data) {
        return $http.post("/api/ProjectBoards/PostProjectBoard", data)
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
    };

    var putConfig = function (index, data) {
        return $http.put("/api/ProjectBoards/" + index, data)
           .then(function (response) {
               return response.status === 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var deleteConfig = function (index) {
        return $http.delete("/api/ProjectBoards/" + index)
           .then(function (response) {
               return response.status === 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };
        
    var getCSV = function (id) {
        return $http.get("/api/CSVWebApi/CSV", { params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var getBoards = function () {
        return $http.get("/api/BoardWebApi/Get").then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var openBoard = function (ID) {
        return $http.get("/Project/Board/Board/" + ID).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var updateBoard = function (boardIdVal, data, updateType) {
        return $http.put("/api/BoardWebApi/UpdateBoard", { boardId: boardIdVal, data: data, updateType: updateType })
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
            $rootScope.$$childHead.$emit("refreshBoard");
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
        addConfig: addConfig,
        putConfig: putConfig,
        deleteConfig: deleteConfig,
        getBoards: getBoards,
        getCSV: getCSV,
        openBoard: openBoard,
        getColumns: getColumns,
        getUserList: getUserList,
        getCurrentUser: getCurrentUser,
        updateBoard: updateBoard
    };
});