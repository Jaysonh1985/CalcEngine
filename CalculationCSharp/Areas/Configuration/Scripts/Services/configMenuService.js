// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('configMenuService', function ($http, $q, $rootScope) {
    var proxy = null;

    //Configuration Menu Services
    var getConfig = function () {
        return $http.get("/api/CalcConfigurations").then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var addConfig = function (data) {
        return $http.post("/api/CalcConfigurations/PostCalcConfiguration",  data )
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
    };

     var putConfig = function (index, data) {
         return $http.put("/api/CalcConfigurations/" + index, data)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var deleteConfig = function (index) {
         return $http.delete("/api/CalcConfigurations/" + index)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

    //Configuration Function Menu Services
     var getFunction = function () {
         return $http.get("/api/FunctionsConfiguration").then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var addFunction = function (data) {
         return $http.post("/api/FunctionsConfiguration/PostCalcFunctions", data)
             .then(function (response) {
                 return response.data;
             }, function (error) {
                 return $q.reject(error.data.Message);
             });
     };

     var putFunction = function (index, data) {
         return $http.put("/api/FunctionsConfiguration/" + index, data)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var deleteFunction = function (index) {
         return $http.delete("/api/FunctionsConfiguration/" + index)
            .then(function (response) {
                return response.status == 200;
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
        addConfig: addConfig,
        putConfig: putConfig,
        deleteConfig: deleteConfig,
        getConfig: getConfig,
        addFunction: addFunction,
        putFunction: putFunction,
        deleteFunction: deleteFunction,
        getFunction: getFunction,
    };
});