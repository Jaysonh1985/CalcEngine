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
sulhome.kanbanBoardApp.directive('variablecheck', function (configTypeaheadFactory, configValidationFactory) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', colIndex: '=', rowIndex: '=', inputtype: '='},
        link: function (scope, element, attrs, form, scopectrl, ngModel) {
            scope.$watch('$parent.$parent.$parent.$parent.triggerValidation', function (newValue, oldValue) {
                form[attrs.name].$setValidity("input", true);
                configValidationFactory.variablePreviouslySet(scope.config[0], parseInt(attrs.colindex), attrs.inputtype, parseInt(attrs.rowindex), attrs.$$element[0].value, form, true, attrs.name);

                });
            element.on('blur', function () {
                form[attrs.name].$setValidity("input", true);
                configValidationFactory.variablePreviouslySet(scope.config[0], parseInt(attrs.colindex), attrs.inputtype, parseInt(attrs.rowindex), attrs.$$element[0].value, form, true, attrs.name);
            });
            }
        }
    }
);

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