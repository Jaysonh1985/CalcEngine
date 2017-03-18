// Copyright (c) 2016 Project AIM
//Modal functions from configuration
sulhome.kanbanBoardApp.factory('configModalFactory', function ($location) {
    return {
        //get ctrl file path for functions modal
        getFunctionCtrl: function (Function) {
            if (Function == 'Maths') {
                return 'mathsCtrl';
            }
            else if (Function == 'Function') {
                return 'functionCtrl'
            }
            else if (Function == 'Input') {
                return 'inputCtrl'
            }
            else if (Function == 'Return') {
                return 'ReturnCtrl'
            };
        },
        //get URL for modal file path
        getFunctionTempURL: function (Function) {
            if (Function == 'Maths') {
                return '/Areas/Configuration/Scripts/Maths/MathsModal.html';
            }
            else if (Function == 'Function') {
                return '/Areas/Configuration/Scripts/Function/FunctionModal.html'
            }
            else if (Function == 'Input') {
                return '/Areas/Configuration/Scripts/Input/InputModal.html'
            }
            else if (Function == 'Return') {
                return '/Areas/Configuration/Scripts/Return/ReturnModal.html'
            };
        },
    };
});