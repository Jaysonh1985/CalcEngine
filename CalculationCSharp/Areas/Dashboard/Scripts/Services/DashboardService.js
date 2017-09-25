// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.service('DashboardService', function ($http, $q, $rootScope) {

    var proxy = null;

    var getTeams = function (id) {
        return $http.get("/api/CalcTeams/Get", {
            params: {
                id: id
            }
        })
			.then(function (response) {
			    return response.data;
			}, function (error) {
			    return $q.reject(error.data.Message);
			});
    };

    var putTeams = function (index, data, comment) {
        return $http.put("/api/CalcTeams/" + index, {
            data: data,
            comment: comment
        })
			.then(function (response) {
			    return response.status == 200;
			}, function (error) {
			    return $q.reject(error.data.Message);
			});
    };

    var postTeams = function (data) {
        return $http.post("/api/CalcTeams/", data)
			.then(function (response) {
			    return response.data;
			}, function (error) {
			    return $q.reject(error.data.Message);
			});
    };

    var deleteTeams = function (id) {
        return $http.delete("/api/CalcTeams/" + id)
			.then(function (response) {
			    return response.status == 200;
			}, function (error) {
			    return $q.reject(error.data.Message);
			});
    };

    var getTeamMembers = function (id) {
        return $http.get("/api/CalcTeamMembers/Get", {
            params: {
                id: id
            }
        })
			.then(function (response) {
			    return response.data;
			}, function (error) {
			    return $q.reject(error.data.Message);
			});
    };

    var postTeamMembers = function (data) {
        return $http.post("/api/CalcTeamMembers/", data)
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
        getTeams: getTeams,
        putTeams: putTeams,
        postTeams: postTeams,
        deleteTeams: deleteTeams,
        getTeamMembers: getTeamMembers,
        postTeamMembers: postTeamMembers
    };
});
