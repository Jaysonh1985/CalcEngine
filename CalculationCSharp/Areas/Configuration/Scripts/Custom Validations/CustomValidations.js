// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.directive('uibModalWindow', function () {
    return {
        restrict: 'EA',
        link: function(scope, element) {
            element.draggable();
        }
    }  
});
////Input previously set on the builder
//sulhome.kanbanBoardApp.directive('inputpreviouslySet', function (configTypeaheadFactory, configValidationFactory) {
//    return {
//        replace: true,
//        restrict: 'A',
//        require: '^form',
//        scope: { config: '=', colIndex: '=', rowIndex: '=' },
//        link: function (scope, element, attrs, form, scopectrl) {
//            element.on('blur', function () {
//                angular.forEach(scope.config, function (value, key, obj) {
//                    angular.forEach(scope.config[key].Functions, function (valueF, keyF, obj) {
//                        var AttName = 'FunctionCog_' + key + '_' + keyF;
//                        form[AttName].$setValidity("input", true);
//                        angular.forEach(scope.config[key].Functions[keyF].Parameter, function (valueP, keyP, obj) {
//                            if (key != 0)
//                            {
//                                //Maths
//                                if (scope.config[key].Functions[keyF].Function == 'Maths') {                                  
//                                    angular.forEach(obj, function (valueN, keyN, obj) {
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Input1, form);
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Input2, form);
//                                        });
//                                }
//                                //Period
//                                if (scope.config[key].Functions[keyF].Function == 'Period') {
//                                    angular.forEach(obj, function (valueN, keyN, obj) {
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueN.Date1, form, true);
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueN.Date2, form, true);
//                                    });
//                                }
//                                //Factors
//                                if (scope.config[key].Functions[keyF].Function == 'Factors') {
//                                    angular.forEach(obj, function (valueN, keyN, obj) {
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form, true);
//                                    });                                      
//                                }
//                                //Date Adjustment
//                                if (scope.config[key].Functions[keyF].Function == 'DateAdjustment') {
//                                        angular.forEach(obj, function (valueN, keyN, obj) {
//                                            configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueN.Date1, form, true);
//                                            if (obj[0].Type == 'Earlier' || obj[0].Type == 'Later') {
//                                                configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueN.Date2, form, true);
//                                            }
//                                        });                            
//                                }
//                                //Date Part
//                                if (scope.config[key].Functions[keyF].Function == 'DatePart') {               
//                                        angular.forEach(obj, function (valueN, keyN, obj) {
//                                            configValidationFactory.variablePreviouslySet(scope.config, key, "Date", keyF, valueN.Date1, form, true);
//                                        });                                 
//                                }
//                                //Maths Functions
//                                if (scope.config[key].Functions[keyF].Function == 'MathsFunctions') {
//                                    angular.forEach(obj, function (valueN, keyN, obj) {
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Number1, form, true);
//                                        if (valueN.Type == "Add" || valueN.Type == "Divide" || valueN.Type == "Max" || valueN.Type == "Min" || valueN.Type == "Multiply" || valueN.Type == "Power" || valueN.Type == "Subtract") {
//                                           configValidationFactory.variablePreviouslySet(scope.config, key, "Decimal", keyF, valueN.Number2, form, true);
//                                        }
//                                    });
//                                }
//                                //Array Functions
//                                if (scope.config[key].Functions[keyF].Function == 'ArrayFunctions') {
//                                    angular.forEach(obj, function (valueN, keyN, obj) {
//                                        configValidationFactory.variablePreviouslySet(scope.config, key, obj[0].LookupType, keyF, valueN.LookupValue, form);
//                                    });
//                                }
//                            }
//                        })
//                    })
//                })
//            })
//        }
//    }
//});
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
                                var year = parseInt(parts[2],10);
                                var month = parseInt(parts[1], 10) - 1;
                                var day = parseInt(parts[0], 10);
                                if (month != -1 && day !=0)
                                {
                                    var dt = new Date(year,
                                                      month,
                                                      day);
                                    var Input1Bool = isNaN(Date.parse(dt));
                                    //Check year is 4 digits
                                    if (Input1Bool == false && (parts[2].length != 4 || parts[1].length > 2 || parts[0].length > 2)) {
                                        Input1Bool = true;
                                    }
                                }
                                else
                                {
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