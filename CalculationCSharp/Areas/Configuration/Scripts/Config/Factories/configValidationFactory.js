sulhome.kanbanBoardApp.factory('configValidationFactory', function ($location) {
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

        },
        //get index of an array
        getIndexOf: function(arr, val, prop) {
        var l = arr.length,
          k = 0;
        for (k = 0; k < l; k = k + 1) {
            if (arr[k][prop] === val) {
                return k;
            }
        }
        return false;
        },

    }


});