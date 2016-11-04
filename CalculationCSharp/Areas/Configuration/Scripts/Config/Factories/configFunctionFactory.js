// Copyright (c) 2016 Project AIM
//Functions available to user at Config Level

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
        //function to store the array as a new array if linked back to a previous $scope
        convertToFromJson: function (arr) {
            var array = [];

            array = JSON.stringify(arr);

            return angular.fromJson(array);
        },

        setFormValidation: function (value, AttName, form, VariableNames, type) {
            if (VariableNames.length > 0) {
                if (type == "Decimal") {
                    var Input1Bool = isNaN(parseFloat(value));
                }
                else if (type == "Date") {
                    var Input1Bool = isNaN(new Date(value.split('/')[2], value.split('/')[1] - 1, value.split('/')[0]));
                }
                if (Input1Bool == true) {
                    if (VariableNames.indexOf(value) == -1) {
                        form[AttName].$setValidity("input", false);
                    }
                }
            }
        },

    }


});