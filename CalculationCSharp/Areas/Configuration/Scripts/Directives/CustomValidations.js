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
sulhome.kanbanBoardApp.directive('savebutton', function (configTypeaheadFactory, configValidationFactory) {
    return {
        link: function (scope, element, attrs) {
            element.click(function () {
                scope.$parent.$broadcast('form:submit');
                scope.$parent.SaveButtonClick(scope.$parent.form);
            });
        }
    };
});
//Input previously set on the builder
sulhome.kanbanBoardApp.directive('differentvariabletype', function (configTypeaheadFactory, configValidationFactory, $filter) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', colIndex: '=', rowIndex: '=', inputtype: '=' },
        link: function (scope, element, attrs, form, scopectrl, ngModel) {
            scope.$on('form:submit', function () {
                form[attrs.name].$setValidity("variabletype", true);
                var scopeid = 0;
                var columnFilter = [];
                angular.forEach(scope.config, function (groups) {
                    if (scopeid < parseInt(attrs.colindex)) {
                        columnFilter = ($filter('filter')(groups.Functions));
                        columnFilter = ($filter('filter')(columnFilter, { Name: attrs.$$element[0].value }, true));
                        if (columnFilter.length > 0) {
                            columnFilter = ($filter('filter')(columnFilter, { Type: scope.config[parseInt(attrs.colindex)].Functions[parseInt(attrs.rowindex)].Type }, true));
                            if (columnFilter.length == 0) {
                                form[attrs.name].$setValidity("variabletype", false);
                            }
                        };
                    }    
                    else if (scopeid == parseInt( attrs.colindex)) {
                        columnFilter = ($filter('limitTo')(groups.Functions, parseInt(attrs.rowindex)));
                        columnFilter = ($filter('filter')(columnFilter, { Name: attrs.$$element[0].value }, true));
                        if (columnFilter.length > 0) {
                            columnFilter = ($filter('filter')(columnFilter, { Type: scope.config[parseInt(attrs.colindex)].Functions[parseInt(attrs.rowindex)].Type }, true));
                            if (columnFilter.length == 0) {
                                form[attrs.name].$setValidity("variabletype", false);
                            }
                        };
                    };
                    scopeid = scopeid + 1
                });
            });
        }
    };
});
//Input previously set on the builder
sulhome.kanbanBoardApp.directive('variablecheck', function (configTypeaheadFactory, configValidationFactory) {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', colIndex: '=', rowIndex: '=', inputtype: '=' },
        link: function (scope, element, attrs, form, scopectrl, ngModel) {
            scope.$on('form:submit', function() {
                form[attrs.name].$setValidity("input", true);
                configValidationFactory.variablePreviouslySet(scope.config[0], parseInt(attrs.colindex), attrs.inputtype, parseInt(attrs.rowindex), attrs.$$element[0].value, form, true, attrs.name);
            });
        }
    };
});
//Input previously set on the builder
sulhome.kanbanBoardApp.directive('mathsoperatorcheck', function () {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', rowIndex: '=' },
        link: function (scope, element, attrs, form, scopectrl, ngModel) {
            scope.$on('form:submit', function () {
                form[attrs.name].$setValidity("clause", true);
                form[attrs.name].$setValidity("clauseblank", true);
                var MathsScope = scope.config[0];
                var currentRow = MathsScope[parseInt(attrs.rowindex)];
                var nextRow = MathsScope[parseInt(attrs.rowindex) + 1];
                if (currentRow.Logic2 != "" && currentRow.Logic2 != null) {
                    if (nextRow == null) {
                        form[attrs.name].$setValidity("clause", false);
                    };
                }
                else if (currentRow.Logic2 == "" || currentRow.Logic2 == null) {
                    if (nextRow != null) {
                        form[attrs.name].$setValidity("clauseblank", false);
                    };
                };
            });
        }
    };
});

sulhome.kanbanBoardApp.directive('form', function ($timeout) {
    return {
        restrict: 'E',
        link: function(scope, elem) {
            elem.on('submit', function () {
                scope.$broadcast('form:submit');
                scope.validation = true;
                scope.isLoading = true;
                scope.$watch("repeatEnd", function (newVal, oldVal) {
                    if (newVal == true)
                    {
                        scope.$broadcast('form:submit');
                        scope.viewOnly = false;
                        scope.isLoading = false;
                        scope.CalcButtonClick(scope.form);
                    }               
                });
                        
            });
        }
    }; 
})

sulhome.kanbanBoardApp.directive("repeatEnd", function () {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            if (scope.$last) {
                scope.$eval(attrs.repeatEnd);
            }
        }
    };
});

sulhome.kanbanBoardApp.directive('bracketscheck', function () {
    return {
        replace: true,
        restrict: 'A',
        require: '^form',
        scope: { config: '=', rowIndex: '=', colIndex: '=' },
        link: function (scope, element, attrs, form, scopectrl, ngModel) {
            scope.$on('form:submit', function () {
                var LBcounter = 0;
                var RBcounter = 0;
                var length = scope.config.length - 1;
                angular.forEach(scope.config[0], function (value, key, obj) {

                    if (value.Bracket1 == "(") {
                        var AttName = 'BracketLeft_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                        form[AttName].$setValidity("bracketnotclosed", true);
                        LBcounter = LBcounter + 1;
                    }
                    if (value.Bracket2 == ")") {
                        var AttName = 'BracketRight_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                        form[AttName].$setValidity("bracketnotopen", true);
                        RBcounter = RBcounter + 1;
                    }
                });
                if (LBcounter > RBcounter) {
                    var AttName = 'BracketLeft_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                    form[AttName].$setValidity("bracketnotclosed", true);
                    form[AttName].$setValidity("bracketnotclosed", false);
                }
                else if (LBcounter < RBcounter) {
                    var AttName = 'BracketRight_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                    form[AttName].$setValidity("bracketnotopen", false);
                }
                else {
                    var AttName = 'BracketLeft_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                    form[AttName].$setValidity("bracketnotclosed", true);
                    var AttName = 'BracketRight_' + attrs.colindex + '_' + attrs.rowindex + '_' + length;
                    form[AttName].$setValidity("bracketnotopen", true);
                };
            });
        }
    };
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
                var dataType = scope.config.Type;
                if (dataType == 'Date') {
                    form[attrs.name].$setValidity("inputformat", true);
                    if (scope.config.Output != null) {
                        var array = scope.config.Output.split('~');
                        if (array != "") {
                            angular.forEach(array, function (valueNA, keyNA, obj) {
                                var parts = valueNA.split("/");
                                var year = parseInt(parts[2], 10);
                                var month = parseInt(parts[1], 10) - 1;
                                var day = parseInt(parts[0], 10);
                                if (month != -1 && day != 0) {
                                    var dt = new Date(year,
                                                      month,
                                                      day);
                                    var Input1Bool = isNaN(Date.parse(dt));
                                    //Check year is 4 digits
                                    if (Input1Bool == false && (parts[2].length != 4 || parts[1].length > 2 || parts[0].length > 2)) {
                                        Input1Bool = true;
                                    }
                                }
                                else {
                                    Input1Bool = true;
                                };
                                if (Input1Bool == true) {
                                    form[attrs.name].$setValidity("inputformat", false);
                                };
                            });
                        };
                    };
                };
                if (dataType == 'Decimal') {
                    form[attrs.name].$setValidity("inputformat", true);
                    if (scope.config.Output != null) {
                        if (scope.config.Output.split('~') != null) {
                            var array = scope.config.Output.split('~');
                            if (array != "") {
                                angular.forEach(array, function (valueNA, keyNA, obj) {
                                    var Input1Bool = isNaN(parseFloat(valueNA));
                                    if (Input1Bool == true) {
                                        form[attrs.name].$setValidity("inputformat", false);
                                    }
                                });
                            };
                        };
                    };
                };
            });
        }
    };
});