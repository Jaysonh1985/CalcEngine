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

sulhome.kanbanBoardApp.directive('form', function ($timeout) {
    return {
        restrict: 'E',
        link: function(scope, elem) {
            elem.on('submit', function () {
                scope.$broadcast('form:submit');
                scope.viewOnly = false;
                scope.isLoading = false;
                scope.CalcButtonClick(scope.form);                       
            });
        }
    }; 
})
sulhome.kanbanBoardApp.directive('myTable', function () {
    return {
        restrict: 'E',
        link: function (scope, element, attrs) {
            var html = '<table class="table table-bordered">';
            html += '<thead><tr>';
            angular.forEach(scope[attrs.rows], function (row, index) {
                html += '<td>' + row.ColumnName + '</td>';
            })
            html += '<tr></thead>';
            
            angular.forEach(scope[attrs.rowstd], function (row1, index1) {
                html += '<tr>';
                angular.forEach(row1, function (subrow, index) {
                    html += '<td>' + subrow.Name + '</td>';
                })
                html += '</tr>';
            })
            html += '</table>';
            element.replaceWith(html)
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