// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.directive('uibModalWindow', function () {
    return {
        restrict: 'EA',
        link: function(scope, element) {
            element.draggable();
        }
    }  
});
//Input previously set on the builder
sulhome.kanbanBoardApp.directive('inputpreviouslySet', function (configTypeaheadFactory, configValidationFactory) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', colIndex: '=', rowIndex: '=' },
        link: function (scope, element, attrs, form, scopectrl) {
            element.on('click', function () {
                angular.forEach(scope.config, function (value, key, obj) {
                    angular.forEach(scope.config[key].Functions, function (valueF, keyF, obj) {
                        var AttName = 'FunctionCog_' + key + '_' + keyF;
                        form[AttName].$setValidity("input", true);
                        angular.forEach(scope.config[key].Functions[keyF].Parameter, function (valueP, keyP, obj) {
                            if (key != 0)
                            {
                                //Maths
                                if (scope.config[key].Functions[keyF].Function == 'Maths') {                                  
                                    angular.forEach(obj, function (valueN, keyN, obj) {
                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Input1, form);
                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Input2, form);
                                        });
                                }
                                //Period
                                if (scope.config[key].Functions[keyF].Function == 'Period') {
                                    angular.forEach(obj, function (valueN, keyN, obj) {
                                        var Date1array = valueN.Date1.split('~');
                                        angular.forEach(Date1array, function (valueD1, keyD1, objD1) {
                                            configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueD1, form);
                                        });
                                        var Date2array = valueN.Date2.split('~');
                                        angular.forEach(Date2array, function (valueD2, keyD2, objD2) {
                                            configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueD2, form);
                                        });
                                    });
                                }
                                //Factors
                                if (scope.config[key].Functions[keyF].Function == 'Factors') {
                                    angular.forEach(obj, function (valueN, keyN, obj) {
                                        var Date1array = valueN.LookupValue.split('~');
                                        angular.forEach(Date1array, function (valueD1, keyD1, objD1) {
                                            configValidationFactory.variablePreviouslySet(scope.config, key, obj[0].LookupType, keyF, valueD1, form);
                                        });
                                    });                                      
                                }
                                //Date Adjustment
                                if (scope.config[key].Functions[keyF].Function == 'Dates') {
                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            if (obj[0].Type == 'Add' || obj[0].Type == 'Adjust' || obj[0].Type == 'Subtract') {
                                                var Date1array = valueN.Date1.split('~');
                                                angular.forEach(Date1array, function (valueNA, keyNA, obj) {
                                                    configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueNA, form);
                                                });
                                            }
                                            if (obj[0].Type == 'Earlier' || obj[0].Type == 'Later') {
                                                var Date1array = valueN.Date1.split('~');
                                                angular.forEach(Date1array, function (valueD1, keyD1, objD1) {
                                                    configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueD1, form);
                                                });
                                                var Date2array = valueN.Date2.split('~');
                                                angular.forEach(Date2array, function (valueD2, keyD2, objD2) {
                                                    configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueD2, form);
                                                });
                                            }
                                        });                            
                                }
                                //Date Part
                                if (scope.config[key].Functions[keyF].Function == 'DatePart') {               
                                        angular.forEach(obj, function (valueN, keyN, obj) {
                                            var array = valueN.Date1.split('~');
                                            angular.forEach(array, function (valueNA, keyNA, obj) {
                                                configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueNA, form);
                                            })
                                        });                                 
                                }
                                //Maths Functions
                                if (scope.config[key].Functions[keyF].Function == 'MathsFunctions') {
                                    angular.forEach(obj, function (valueN, keyN, obj) {
                                        var Number1array = valueN.Number1.split('~');
                                        angular.forEach(Number1array, function (valueD1, keyD1, objD1) {
                                            configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueD1, form);
                                        });
                                        if (valueN.Type == "Add" || valueN.Type == "Divide" || valueN.Type == "Max" || valueN.Type == "Min" || valueN.Type == "Multiply" || valueN.Type == "Power" || valueN.Type == "Subtract") {
                                            var Number2array = valueN.Number2.split('~');
                                            angular.forEach(Number2array, function (valueD2, keyD2, objD2) {
                                                configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueD2, form);
                                            });
                                        }
                                    });
                                }
                                //Array Functions
                                if (scope.config[key].Functions[keyF].Function == 'ArrayFunctions') {
                                    angular.forEach(obj, function (valueN, keyN, obj) {
                                        configValidationFactory.variablePreviouslySet(scope.config, key, obj[0].LookupType, keyF, valueN, form);
                                    });
                                }
                            }
                        })
                    })
                })
            })
        }
    }
});
//Validate the input form
sulhome.kanbanBoardApp.directive('inputformatValidation', function (configTypeaheadFactory, $filter) {
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
                    if (scope.config[0].Functions[attrs.rowindex].Output != null)
                    {
                        var array = scope.config[0].Functions[attrs.rowindex].Output.split('~');
                        if (array != "") {
                            angular.forEach(array, function (valueNA, keyNA, obj) {
                                var parts = valueNA.split("/");
                                var dt = new Date(parseInt(parts[2], 10),
                                                  parseInt(parts[1], 10) - 1,
                                                  parseInt(parts[0], 10));
                                var Input1Bool = isNaN(Date.parse(dt));
                                //Check year is 4 digits
                                if (Input1Bool == false && (parts[2].length != 4 || parts[1].length > 2 || parts[0].length > 2)) {
                                    Input1Bool = true;
                                }
                                if (Input1Bool == true) {
                                    form[AttName].$setValidity("inputformat", false);
                                }
                            })
                        }
                    }                  
                }
                if(dataType == 'Decimal')
                {
                    var AttName = 'Output_' + attrs.rowindex + '_' + attrs.rowindex;
                    form[AttName].$setValidity("inputformat", true);
                    if (scope.config[0].Functions[attrs.rowindex].Output != null)
                    {
                        if (scope.config[0].Functions[attrs.rowindex].Output.split('~') != null) {
                            var array = scope.config[0].Functions[attrs.rowindex].Output.split('~');
                            if (array != "") {
                                angular.forEach(array, function (valueNA, keyNA, obj) {
                                    var Input1Bool = isNaN(parseFloat(valueNA));
                                    if (Input1Bool == true) {
                                        form[AttName].$setValidity("inputformat", false);
                                    }
                                })
                            }
                        }
                    }
                }
            });
        }
    }
});