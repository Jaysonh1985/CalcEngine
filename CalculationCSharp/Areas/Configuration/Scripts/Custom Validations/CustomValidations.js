sulhome.kanbanBoardApp.directive('uibModalWindow', function () {
    return {
        restrict: 'EA',
        link: function(scope, element) {
            element.draggable();
        }
    }  
});

sulhome.kanbanBoardApp.directive('inputpreviouslySet', function (configTypeaheadFactory) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', colIndex: '=', rowIndex: '=' },
        link: function (scope, element, attrs, form, scopectrl) {
            element.on('click', function () {
                angular.forEach(scope.config, function (value, key, obj) {

                    angular.forEach(scope.config[key].Functions, function (valueF, keyF, obj) {

                        angular.forEach(scope.config[key].Functions[keyF].Parameter, function (valueP, keyP, obj) {

                            if (key != 0)
                            {
                                if (scope.config[key].Functions[keyF].Function == 'Maths') {

                                    var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Decimal", keyF);
                                    var AttName = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName].$setValidity("input", true);
                                    if (VariableNames.length > 0) {
                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            var Input1Bool = isNaN(parseFloat(valueN.Input1));
                                            var Input2Bool = isNaN(parseFloat(valueN.Input2));

                                            if (Input1Bool == true) {
                                                if (VariableNames.indexOf(valueN.Input1) == -1) {
                                                    form[AttName].$setValidity("input", false);
                                                }
                                            }

                                            if (Input2Bool == true) {
                                                if (VariableNames.indexOf(valueN.Input2) == -1) {
                                                    form[AttName].$setValidity("input", false);
                                                }
                                            }
                                        });
                                    }
                                }
                                if (scope.config[key].Functions[keyF].Function == 'Period') {

                                    var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Date", keyF);
                                    var AttName = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName].$setValidity("input", true);
                                    if (VariableNames.length > 0) {
                                        angular.forEach(obj, function (valueN, keyN, obj) {

                                            var Date1array = valueN.Date1.split('~');
                                            angular.forEach(Date1array, function (valueD1, keyD1, objD1) {
                                                var Input1Bool = isNaN(Date.parse(valueD1));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueD1) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                            var Date2array = valueN.Date2.split('~');
                                            angular.forEach(Date2array, function (valueD2, keyD2, objD2) {
                                                var Input2Bool = isNaN(Date.parse(valueD2));

                                                if (Input2Bool == true) {
                                                    if (VariableNames.indexOf(valueD2) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                        });
                                    }
                                }
                                if (scope.config[key].Functions[keyF].Function == 'Factors') {
                                    if (obj[0].LookupType == 'Date') {
                                        var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Date", keyF);
                                        var AttName = 'FunctionCog_' + key + '_' + keyF;
                                        form[AttName].$setValidity("input", true);
                                        if (VariableNames.length > 0) {
                                            angular.forEach(obj, function (valueN, keyN, obj) {
                                                var Input1Bool = isNaN(Date.parse(valueN.LookupValue));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueN.LookupValue) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                        }
                                    }
                                    else if (obj[0].LookupType == 'Decimal') {
                                        var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Decimal", keyF);
                                        var AttName = 'FunctionCog_' + key + '_' + keyF;
                                        form[AttName].$setValidity("input", true);
                                        if (VariableNames.length > 0) {
                                            angular.forEach(obj, function (valueN, keyN, obj) {
                                                var Input1Bool = isNaN(parseFloat(valueN.LookupValue));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueN.LookupValue) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                        }
                                    }
                                }
                                if (scope.config[key].Functions[keyF].Function == 'Dates') {
                                    var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Date", keyF);
                                    var AttName = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName].$setValidity("input", true);

                                    if (VariableNames.length > 0) {

                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            if (obj[0].Type == 'Add' || obj[0].Type == 'Adjust' || obj[0].Type == 'Subtract') {
                                                var Date1array = valueN.Date1.split('~');
                                                angular.forEach(Date1array, function (valueNA, keyNA, obj) {
                                                    var Input1Bool = isNaN(Date.parse(valueNA));
                                                    if (Input1Bool == true) {
                                                        if (VariableNames.indexOf(valueNA) == -1) {
                                                            form[AttName].$setValidity("input", false);
                                                        }
                                                    }
                                                });
                                            }

                                            if (obj[0].Type == 'Earlier' || obj[0].Type == 'Later') {
                                                var Date1array = valueN.Date1.split('~');
                                                angular.forEach(Date1array, function (valueD1, keyD1, objD1) {
                                                    var Input1Bool = isNaN(Date.parse(valueD1));
                                                    if (Input1Bool == true) {
                                                        if (VariableNames.indexOf(valueD1) == -1) {
                                                            form[AttName].$setValidity("input", false);
                                                        }
                                                    }
                                                });
                                                var Date2array = valueN.Date2.split('~');
                                                angular.forEach(Date2array, function (valueD2, keyD2, objD2) {
                                                    var Input2Bool = isNaN(Date.parse(valueD2));
                                                    if (Input2Bool == true) {
                                                        if (VariableNames.indexOf(valueD2) == -1) {
                                                            form[AttName].$setValidity("input", false);
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                    }
                                }
                                if (scope.config[key].Functions[keyF].Function == 'DatePart') {
                                    var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Date", keyF);
                                    var AttName = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName].$setValidity("input", true);
                                    if (VariableNames.length > 0) {
                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            var array = valueN.Date1.split('~');
                                            angular.forEach(array, function (valueNA, keyNA, obj) {
                                                var Input1Bool = isNaN(Date.parse(valueNA));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueNA) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            })
                                        });
                                    }
                                }
                                if (scope.config[key].Functions[keyF].Function == 'MathsFunctions') {
                                    var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Decimal", keyF);
                                    var AttName = 'FunctionCog_' + key + '_' + keyF;
                                    form[AttName].$setValidity("input", true);

                                    if (VariableNames.length > 0) {
                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            var Number1array = valueN.Number1.split('~');
                                            angular.forEach(Number1array, function (valueD1, keyD1, objD1) {
                                                var Input1Bool = isNaN(parseFloat(valueD1));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueD1) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });

                                            if (valueN.Type == "Add" || valueN.Type == "Divide" || valueN.Type == "Max" || valueN.Type == "Min" || valueN.Type == "Multiply" || valueN.Type == "Power" || valueN.Type == "Subtract") {
                                                var Number2array = valueN.Number2.split('~');
                                                angular.forEach(Number2array, function (valueD2, keyD2, objD2) {
                                                    var Input2Bool = isNaN(parseFloat(valueD2));
                                                    if (Input2Bool == true) {
                                                        if (VariableNames.indexOf(valueD2) == -1) {
                                                            form[AttName].$setValidity("input", false);
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                    }
                                }

                                if (scope.config[key].Functions[keyF].Function == 'ArrayFunctions') {

                                    if (obj[0].LookupType == 'Date') {
                                        var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Date", keyF);
                                        var AttName = 'FunctionCog_' + key + '_' + keyF;
                                        form[AttName].$setValidity("input", true);
                                        if (VariableNames.length > 0) {
                                            angular.forEach(obj, function (valueN, keyN, obj) {
                                                var Input1Bool = isNaN(Date.parse(valueN.LookupValue));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueN.LookupValue) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                        }
                                    }
                                    else if (obj[0].LookupType == 'Decimal') {
                                        var VariableNames = configTypeaheadFactory.variableArrayBuilder(scope.config, key, "Decimal", keyF);
                                        var AttName = 'FunctionCog_' + key + '_' + keyF;
                                        form[AttName].$setValidity("input", true);
                                        if (VariableNames.length > 0) {
                                            angular.forEach(obj, function (valueN, keyN, obj) {
                                                var Input1Bool = isNaN(parseFloat(valueN.LookupValue));
                                                if (Input1Bool == true) {
                                                    if (VariableNames.indexOf(valueN.LookupValue) == -1) {
                                                        form[AttName].$setValidity("input", false);
                                                    }
                                                }
                                            });
                                        }
                                    }

                                }
                            }

                        })
                    })
                })
            })
        }
    }
});

sulhome.kanbanBoardApp.directive('inputformatValidation', function (configTypeaheadFactory) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', rowIndex: '=' },
        link: function (scope, element, attrs, form, scopectrl) {

            element.on('blur', function () {

                var dataType = scope.config[0].Functions[attrs.rowindex].Type;

                if(dataType == 'Date')
                {
                    var AttName = 'Output_' + attrs.rowindex + '_' + attrs.rowindex;
                    form[AttName].$setValidity("inputformat", true);
                    var array = scope.config[0].Functions[attrs.rowindex].Output.split('~');
                    angular.forEach(array, function (valueNA, keyNA, obj) {
                        var Input1Bool = isNaN(Date.parse(valueNA));
                        if (Input1Bool == true) {
                            form[AttName].$setValidity("inputformat", false);
                        }
                    })
                }
                if(dataType == 'Decimal')
                {
                    var AttName = 'Output_' + attrs.rowindex + '_' + attrs.rowindex;
                    form[AttName].$setValidity("inputformat", true);
                    var array = scope.config[0].Functions[attrs.rowindex].Output.split('~');
                    angular.forEach(array, function (valueNA, keyNA, obj) {
                        var Input1Bool = isNaN(parseFloat(valueNA));
                        if (Input1Bool == true) {
                            form[AttName].$setValidity("inputformat", false);
                        }
                    })
                }
            });
        }
    }
});