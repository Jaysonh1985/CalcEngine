// Copyright (c) 2016 Project AIM
//Typeahead functions on input forms shows variable available
sulhome.kanbanBoardApp.factory('configTypeaheadFactory', function ($filter, configFunctionFactory) {
    return {
        variableArrayBuilder: function (config, colIndex, type, rowIndex) {
            var counter = 0;
            var scopeid = 0;
            var functionID = 0;
            var spliceid = 0;
            var arrayID = 0;
            var Decimal = [];
            var DecimalValue = [];
            var DecimalParameter = [];
            var Names = [];
            var newArr = [];
            var TypeaheadValue = []
            angular.forEach(config, function (groups) {
                if (scopeid < colIndex) {
                    angular.forEach(groups.Functions, function (functionList) {
                        if (functionList.Function == 'Function') {
                            angular.forEach(functionList.FunctionOutput, function (functionOutput) {
                                if (functionOutput.Type == type) {
                                    TypeaheadValue.push(functionOutput.Name);
                                }
                                else if (type == "" || type == null) {
                                    TypeaheadValue.push(functionOutput.Name);
                                };                             
                            });
                        }
                        else if (type == functionList.Type) {
                            TypeaheadValue.push(functionList.Name);
                        }
                        else if (type == "" || type == null) {
                            TypeaheadValue.push(functionList.Name);
                        };
                    });
                }
                if (scopeid == colIndex) {
                    spliceid = 0
                    angular.forEach(groups.Functions, function (functionList) {
                        if (spliceid < rowIndex) {
                            if (functionList.Function == 'Function') {
                                angular.forEach(functionList.FunctionOutput, function (functionOutput) {
                                    if (functionOutput.Type == type) {
                                        TypeaheadValue.push(functionOutput.Name);
                                    }
                                    else if (type == "" || type == null) {
                                        TypeaheadValue.push(functionOutput.Name);
                                    };
                                });
                            }
                            else if (type == functionList.Type) {
                                TypeaheadValue.push(functionList.Name);
                            }
                            else if (type == "" || type == null) {
                                TypeaheadValue.push(functionList.Name);
                            };
                        };
                    });
                     spliceid++;
                };
                scopeid++;
            });
            functionID = 0;
            angular.forEach(TypeaheadValue, function (NamesDecimalValue) {
                if (Names.indexOf(NamesDecimalValue) == -1) {
                    Names[arrayID] = NamesDecimalValue;
                    arrayID = arrayID + 1;
                };
                functionID = functionID + 1;
            });
            
            scopeid = 0;
            return Names;
        },
        variableLastValue: function (config, colIndex, rowIndex, name) {
            var counter = 0;
            var scopeid = 0;
            var functionID = 0;
            var arrayID = 0;
            var Decimal = [];
            var DecimalValue = [];
            var DecimalParameter = [];
            var Names = [];
            angular.forEach(config, function (groups) {
                if (scopeid <= colIndex) {
                    DecimalValue = ($filter('filter')(config[scopeid].Functions, { Name: name },true));
                    if (scopeid == colIndex) {
                        var DecimalValue = ($filter('limitTo')(config[scopeid].Functions, rowIndex));
                        DecimalValue = ($filter('filter')(DecimalValue, { Name: name }, true));
                        var spliceid = rowIndex;
                        var DecimalValueID = 0;
                    };
                    functionID = 0;
                    if (DecimalValue.length > 0) {
                        angular.forEach(DecimalValue, function (NamesDecimalValue) {
                            DecimalParameter = ($filter('filter')(DecimalValue[functionID].Output));
                            Names = DecimalParameter;
                            functionID = functionID + 1;
                        });
                    };
                    scopeid = scopeid + 1
                };
            });
            scopeid = 0;
            return Names;
        },
    }
});