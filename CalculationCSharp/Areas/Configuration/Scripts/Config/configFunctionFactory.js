sulhome.kanbanBoardApp.factory('configFunctionFactory', function ($location) {
    return {
        //get config ID from url
        getConfigID: function () {
            var url = location.pathname;
            var id = url.substring(url.lastIndexOf('/') + 1);
            id = parseInt(id, 10);
            if (angular.isNumber(id) == false) {
                id = null;
            }
            return id;

        }
    }

});