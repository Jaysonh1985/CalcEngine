sulhome.kanbanBoardApp.service('calculationService', function ($http, $q, $rootScope) {
    var proxy = null;

    
    var getConfig = function () {
        return $http.get("/api/CalcReleases").then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

    var getSingleConfig = function (index) {
        return $http.get("/api/CalcReleases/"+ index).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };


    var addConfig = function (data) {
        return $http.post("/api/CalcReleases/PostCalcConfiguration", data)
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var putConfig = function (index, data) {
         return $http.put("/api/CalcReleases/" + index, data)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var deleteConfig = function (index) {
         return $http.delete("/api/CalcReleases/" + index)
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var getCalc = function (id) {
         return $http.get("/api/CalculationWebApi/Get", { params: { id: id } }).then(function (response) {
             return response.data;
         }, function (error) {
             return $q.reject(error.data.Message);
         });
     };

     var putCalc = function (index, data) {
         return $http.put("/api/CalculationWebApi/" + index, { data: data })
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

     var postCalc = function (index, data) {
         return $http.post("/api/CalculationWebApi/" + index, { data: data })
            .then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
     };

    var rowid = 0;
    var colid = 0;

    var getRowid = function () {
        return rowid;
    }
    var getColid = function () {
        return colid;
    }
     
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
        getSingleConfig: getSingleConfig,
        getRowid: getRowid,
        getColid: getColid,
        getCalc: getCalc,
        putCalc: putCalc,
        postCalc: postCalc
        
    };
});