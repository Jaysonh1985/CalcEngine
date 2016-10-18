// Copyright (c) 2016 Project AIM
//Typeahead functions on input forms shows variable available
sulhome.kanbanBoardApp.factory('configValidationFactory', function ($filter, configFunctionFactory, configTypeaheadFactory) {
    return {
        variablePreviouslySet: function (config, colID, type, rowID, value, form) {
            var VariableNames = configTypeaheadFactory.variableArrayBuilder(config, colID, type, rowID, value);
            var AttName = 'FunctionCog_' + colID + '_' + rowID;
            if (VariableNames.length > 0) {
                if(type == "Decimal"){
                   var Input1Bool = isNaN(parseFloat(value));    
                }
                else if(type == "Date")
                {
                   var Input1Bool = isNaN(Date.parse(value));
                }
                if (Input1Bool == true) {
                    if (VariableNames.indexOf(value) == -1) {
                        form[AttName].$setValidity("input", false);
                    }
                }
            };            
        }
    }
});