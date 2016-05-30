sulhome.kanbanBoardApp.service('boardService', function ($http, $q, $rootScope) {
    var proxy = null;

    
    var getColumns = function (id) {
        return $http.get("/api/ColumnWebApi/Get",{ params: { id: id } }).then(function (response) {
            return response.data;
        }, function (error) {
            return $q.reject(error.data.Message);
        });
    };

        
    var getCSV = function (id) {
        $http({
            url: "/Project/Board/CSV",
            method: "GET",
            params: { id: id },
            responseType : 'arraybuffer'
        })
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


    var canMoveStory = function (sourceColIdVal, targetColIdVal) {
        return $http.get("/api/ColumnWebApi/CanMove", { params: { sourceColId: sourceColIdVal, targetColId: targetColIdVal } })
            .then(function (response) {
                return response.data.canMove;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
    };

    var moveStory = function (storyIdVal, targetColIdVal, updateType, data, task, current_columns) {
        return $http.post("/api/ColumnWebApi/MoveStory", { storyId: storyIdVal, targetColId: targetColIdVal, updateType: updateType, data: data, task:task, columns: current_columns})
            .then(function (response) {
                return response.status == 200;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
    };

    var updateBoard = function (boardIdVal, boardName, data, updateType) {
        return $http.post("/api/BoardWebApi/UpdateBoard", { boardId: boardIdVal, boardName: boardName, data: data, updateType })
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
        getBoards: getBoards,
        getCSV: getCSV,
        openBoard: openBoard,
        getColumns: getColumns,
        canMoveStory: canMoveStory,
        moveStory: moveStory,
        updateBoard: updateBoard
    };
});