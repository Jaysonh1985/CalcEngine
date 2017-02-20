// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('configService', function ($http, $q, $rootScope) {
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
    //Configuration Builder JSON Services
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
    //Function Builder JSON Services
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
    //Post values to Specification builder
     var specBuilder = function (index, data) {
         return $http.post("/api/CalcSpecification/" + index, { data: data })
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };
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
    //Regression Services
     var getCalcRegression = function (id) {
         return $http.get("/api/CalcRegressionInputs", { params: { id: id } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };
     var postCalcRegression = function (index, data) {
         return $http.post("/api/CalcRegressionInputs/" + index,  data )
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
         return $http.put("/api/CalcRegressionInputs/" + index,  data )
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
         return $http.post("/api/CalcRegressionOutput/" + index, {data: data})
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
    //Configuration Builder JSON Services
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
        getCalcFunction: getCalcFunction,
        putCalcFunction: putCalcFunction,
        postCalcFunction: postCalcFunction,
        deleteCalcFunction: deleteCalcFunction,
        getCalc: getCalc,
        putCalc: putCalc,
        postCalc: postCalc,
        deleteCalc: deleteCalc,
        getCalcHistory: getCalcHistory,
        getCalcHistorySingle: getCalcHistorySingle,
        postCalcHistory: postCalcHistory,
        getFunctionHistory: getFunctionHistory,
        getFunctionHistorySingle: getFunctionHistorySingle,
        postFunctionHistory: postFunctionHistory,
        getCalcRegression: getCalcRegression,
        postCalcRegression: postCalcRegression,
        deleteCalcRegression: deleteCalcRegression,
        putCalcRegression: putCalcRegression,
        getFunctionRegression: getFunctionRegression,
        postFunctionRegression: postFunctionRegression,
        deleteFunctionRegression: deleteFunctionRegression,
        putFunctionRegression: putFunctionRegression,
        exportRegression: exportRegression,
        getUserSession: getUserSession,
        deleteUserSession: deleteUserSession,
        specBuilder: specBuilder,
        getSchemes: getSchemes,
        getFunctionDetails: getFunctionDetails
    };
});