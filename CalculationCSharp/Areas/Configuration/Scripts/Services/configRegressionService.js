// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('configRegressionService', function ($http, $q, $rootScope) {
    var proxy = null;

    //Regression Services
    var getCalcRegression = function (id) {
        return $http.get("/api/CalcRegressionInputs", { params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var postCalcRegression = function (index, data) {
        return $http.post("/api/CalcRegressionInputs/" + index, data)
           .then(function (response) {
               return response.data;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var deleteCalcRegression = function (index) {
        return $http.delete("/api/CalcRegressionInputs/" + index)
           .then(function (response) {
               return response.status == 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var putCalcRegression = function (index, data) {
        return $http.put("/api/CalcRegressionInputs/" + index, data)
           .then(function (response) {
               return response.status == 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var getFunctionRegression = function (id) {
        return $http.get("/api/FunctionRegressionInputs", { params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var postFunctionRegression = function (index, data) {
        return $http.post("/api/FunctionRegressionInputs/" + index, data)
           .then(function (response) {
               return response.data;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var deleteFunctionRegression = function (index) {
        return $http.delete("/api/FunctionRegressionInputs/" + index)
           .then(function (response) {
               return response.status == 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var putFunctionRegression = function (index, data) {
        return $http.put("/api/FunctionRegressionInputs/" + index, data)
           .then(function (response) {
               return response.status == 200;
           }, function (error) {
               return $q.reject(error.data.Message);
           });
    };

    var exportRegression = function (index, data) {
        return $http.post("/api/CalcRegressionOutput/" + index, { data: data })
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
    // Call 'NotifyBoardUpdated' on SignalR server
    var sendRequest = function () {        
        this.proxy.invoke('NotifyBoardUpdated');
    };

    return {
        initialize: initialize,
        sendRequest: sendRequest,
        getCalcRegression: getCalcRegression,
        postCalcRegression: postCalcRegression,
        deleteCalcRegression: deleteCalcRegression,
        putCalcRegression: putCalcRegression,
        getFunctionRegression: getFunctionRegression,
        postFunctionRegression: postFunctionRegression,
        deleteFunctionRegression: deleteFunctionRegression,
        putFunctionRegression: putFunctionRegression,
        exportRegression: exportRegression,
       };
});