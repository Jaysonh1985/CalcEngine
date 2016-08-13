sulhome.kanbanBoardApp.directive('integer', function() {
    return {
        require: 'ngModel',
        scope: true,
        link: function (scope, elm, attrs, ctrl) {

            var Test = []

            Test = scope;
            
            ctrl.$validators.integer = function (modelValue, viewValue, scope) {

                var Test = []

                Test = scope;

                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty models to be valid
                    return true;
                }
                // it is invalid
                return false;
            };
        }
    };
});

sulhome.kanbanBoardApp.directive('uibModalWindow', function () {
    return {
        restrict: 'EA',
        link: function(scope, element) {
            element.draggable();
        }
    }  
});