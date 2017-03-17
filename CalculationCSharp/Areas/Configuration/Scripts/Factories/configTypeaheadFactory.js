// Copyright (c) 2016 Project AIM
//Typeahead functions on input forms shows variable available
sulhome.kanbanBoardApp.factory('configTypeaheadFactory', function ($filter, configFunctionFactory) {
    return {
        variableArrayBuilder: function (config, colIndex, type, rowIndex) {
            var counter = 0;
            var scopeid = 0;
            var functionID = 0;
            var arrayID = 0;
            var Decimal = [];
            var DecimalValue = [];
            var DecimalParameter = [];
            var Names = [];
            var newArr = [];

            angular.forEach(config, function (groups) {
                if (scopeid <= colIndex) {

                    if (type == null) {
                        DecimalValue = ($filter('filter')(config[scopeid].Functions));
                    }
                    else {
                        DecimalValue = ($filter('filter')(config[scopeid].Functions, { Type: type }));
                    }
                    if (scopeid == colIndex) {

                        var DecimalValue = ($filter('limitTo')(config[scopeid].Functions, rowIndex));
                        if (type == null) {
                            DecimalValue = ($filter('filter')(DecimalValue));
                        }
                        else {
                            DecimalValue = ($filter('filter')(DecimalValue, { Type: type }));
                        }
                       
                        var spliceid = rowIndex;
                        var DecimalValueID = 0;
                    };
                    functionID = 0;
                    angular.forEach(DecimalValue, function (NamesDecimalValue) {
                        DecimalParameter = ($filter('filter')(DecimalValue[functionID].Name));
                        if (DecimalValue.indexOf(DecimalParameter) == -1) {
                            Names[arrayID] = DecimalParameter;
                            arrayID = arrayID + 1;
                        }
                        functionID = functionID + 1;
                     });
                    scopeid = scopeid + 1
                }
            });
            scopeid = 0;
            return Names;
        },      
    }
});