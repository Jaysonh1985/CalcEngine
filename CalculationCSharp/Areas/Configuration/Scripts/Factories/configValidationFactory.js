// Copyright (c) 2016 Project AIM
//Validates the Previous Set Variables to make sure they exist
sulhome.kanbanBoardApp.factory('configValidationFactory', function ($filter, configFunctionFactory, configTypeaheadFactory) {
    return {
        variablePreviouslySet: function (config, colID, type, rowID, value, form, array, AttName) {
            var VariableNames = configTypeaheadFactory.variableArrayBuilder(config, colID, type, rowID, value);
            //Allows Arrays
            if (array == true) {
                if (value != null && value != 'undefined') {
                    var arraySplit = value.split('~');
                    angular.forEach(arraySplit, function (value1, key1, obj1) {
                        configFunctionFactory.setFormValidation(value1, AttName, form, VariableNames, type);
                    });
                }
            }
            else {
                configFunctionFactory.setFormValidation(value, AttName, form, VariableNames, type);
            };
        },
        variablePreviouslySetDifferentDataType: function (config, colID, type, rowID, value, form, array, AttName) {
            var VariableNames = configTypeaheadFactory.variableArrayBuilder(config, colID, type, rowID, value);
            //Allows Arrays
            if (array == true) {
                if (value != null && value != 'undefined') {
                    var arraySplit = value.split('~');
                    angular.forEach(arraySplit, function (value1, key1, obj1) {
                        configFunctionFactory.setFormValidation(value1, AttName, form, VariableNames, type);
                    });
                }
            }
            else {
                configFunctionFactory.setFormValidation(value, AttName, form, VariableNames, type);
            };
        },
    };
});