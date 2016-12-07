// Copyright (c) 2016 Project AIM
//Validates the Previous Set Variables to make sure they exist
sulhome.kanbanBoardApp.factory('configValidationFactory', function ($filter, configFunctionFactory, configTypeaheadFactory) {
    return {
            variablePreviouslySet: function (config, colID, type, rowID, value, form, array) {
                var VariableNames = configTypeaheadFactory.variableArrayBuilder(config, colID, type, rowID, value);
                var AttName = 'FunctionCog_' + colID + '_' + rowID;
                //Allows Arrays
                if(array == true)
                {
                    if (value != null && value != 'undefined')
                    {
                        var arraySplit = value.split('~');
                        angular.forEach(arraySplit, function (value1, key1, obj1) {
                            configFunctionFactory.setFormValidation(value1, AttName, form, VariableNames, type);
                        });
                    }
                }
                else
                {
                    configFunctionFactory.setFormValidation(value, AttName, form, VariableNames, type);
                }
                
            },
        
    }
});