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

                    var parts = value.split("/");
                    var year = parseInt(parts[2], 10);
                    var month = parseInt(parts[1], 10) - 1;
                    var day = parseInt(parts[0], 10);
                    if (month != -1 && day != 0) {
                        var dt = new Date(year,
                                          month,
                                          day);
                        var Input1Bool = isNaN(Date.parse(dt));
                        //Check year is 4 digits
                        if (Input1Bool == false && (parts[2].length != 4 || parts[1].length > 2 || parts[0].length > 2)) {
                            Input1Bool = true;
                        }
                    }
                    else {
                        Input1Bool = true;
                    }

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