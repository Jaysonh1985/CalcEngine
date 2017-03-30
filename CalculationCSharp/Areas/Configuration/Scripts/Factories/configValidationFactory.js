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
        mathsoperatorcheck: function (config, form, AttName) {
            form[AttName].$setValidity("clause", true);
            form[AttName].$setValidity("clauseblank", true);

            angular.forEach(config, function (value1, key1, obj1) {
                var currentRow = config[parseInt(key1)];
                var nextRow = config[parseInt(key1) + 1];
                if (currentRow.Logic2 != "" && currentRow.Logic2 != null) {
                    if (nextRow == null) {
                        form[AttName].$setValidity("clause", false);
                    };
                }
                else if (currentRow.Logic2 == "" || currentRow.Logic2 == null) {
                    if (nextRow != null) {
                        form[AttName].$setValidity("clauseblank", false);
                    };
                };
            });
        },
        bracketscheck: function (config, form, AttName) {
            var LBcounter = 0;
            var RBcounter = 0;
            var length = config.length - 1;
            angular.forEach(config, function (value, key, obj) {
                if (value.Bracket1 == "(") {
                    form[AttName].$setValidity("bracketnotclosed", true);
                    LBcounter = LBcounter + 1;
                }
                if (value.Bracket2 == ")") {
                    form[AttName].$setValidity("bracketnotopen", true);
                    RBcounter = RBcounter + 1;
                }
            });
            if (LBcounter > RBcounter) {
                form[AttName].$setValidity("bracketnotclosed", true);
                form[AttName].$setValidity("bracketnotclosed", false);
            }
            else if (LBcounter < RBcounter) {
                form[AttName].$setValidity("bracketnotopen", false);
            }
            else {
                form[AttName].$setValidity("bracketnotclosed", true);
                form[AttName].$setValidity("bracketnotopen", true);
            };
        },
    };
});