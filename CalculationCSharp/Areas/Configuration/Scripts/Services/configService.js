// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('configService', function ($http, $q, $rootScope) {
    var proxy = null;

     var getCalc = function (id) {
         return $http.get("/api/ConfigWebApi/Get", { params: { id: id } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var putCalc = function (index, data, comment) {
         return $http.put("/api/ConfigWebApi/" + index, { data: data, comment: comment })
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var postCalc = function (index, data) {
         return $http.post("/api/ConfigWebApi/" + index, { data: data })
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var deleteCalc = function (id) {
         return $http.delete("/api/ConfigWebApi/" + id)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var getCalcFunction = function (id) {
         return $http.get("/api/FunctionWebApi/Get", { params: { id: id } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var putCalcFunction = function (index, data, comment) {
         return $http.put("/api/FunctionWebApi/" + index, { data: data, comment: comment })
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var postCalcFunction = function (index, data) {
         return $http.post("/api/FunctionWebApi/" + index, { data: data })
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var deleteCalcFunction = function (id) {
         return $http.delete("/api/FunctionWebApi/" + id)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var specBuilder = function (index, data) {
         return $http.post("/api/CalcSpecification/" + index, { data: data })
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };
    
     var getUserSession = function (id, Section) {
         return $http.get("/api/UserSessions", { params: { id: id, Section: Section } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var deleteUserSession = function (id) {
         return $http.delete("/api/UserSessions", { params: { id: id, Section: Section } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var getSchemes = function () {
         return $http.get("/api/SchemeWebApi/Get").then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var getFunctionDetails = function (Scheme, ID, Type) {
         return $http.get("/api/FunctionNameWebApi/GetName", { params: { Scheme: Scheme, ID: ID, Type: Type } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var getFactorTables = function (Scheme, ID, Type) {
         return $http.get("/api/FactorTableWebApi/Get").then(function (response) {
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
        getCalc: getCalc,
        putCalc: putCalc,
        postCalc: postCalc,
        deleteCalc: deleteCalc,
        getCalcFunction: getCalcFunction,
        putCalcFunction: putCalcFunction,
        postCalcFunction: postCalcFunction,
        deleteCalcFunction: deleteCalcFunction,
        getUserSession: getUserSession,
        deleteUserSession: deleteUserSession,
        specBuilder: specBuilder,
        getSchemes: getSchemes,
        getFunctionDetails: getFunctionDetails,
        getFactorTables: getFactorTables
    };
});