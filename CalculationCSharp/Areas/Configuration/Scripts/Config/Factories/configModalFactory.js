﻿// Copyright (c) 2016 Project AIM
//Modal functions from configuration
sulhome.kanbanBoardApp.factory('configModalFactory', function ($location) {
    return {
        //get ctrl file path for functions modal
        getFunctionCtrl: function (Function) {
            if (Function == 'Maths') {
                return 'mathsCtrl';
            }
            else if (Function == 'MathsFunctions') {
                return 'mathsFunctionsCtrl';
            }
            else if (Function == 'Period') {
                return 'periodCtrl'
            }
            else if (Function == 'Comments') {
                return 'commentsCtrl'
            }
            else if (Function == 'ErrorsWarnings') {
                return 'errorswarningsCtrl'
            }
            else if (Function == 'Factors') {
                return 'factorsCtrl'
            }
            else if (Function == 'Function') {
                return 'functionCtrl'
            }
            else if (Function == 'DateAdjustment') {
                return 'dateAdjustmentCtrl'
            }
            else if (Function == 'DatePart') {
                return 'datePartCtrl'
            }
            else if (Function == 'Input') {
                return 'inputCtrl'
            }
            else if (Function == 'ArrayFunctions') {
                return 'arrayFunctionsCtrl'
            }
            else if (Function == 'StringFunctions') {
                return 'stringFunctionsCtrl'
            }
            else if (Function == 'Return') {
                return 'ReturnCtrl'
            }
        },
        //get URL for modal file path
        getFunctionTempURL: function (Function) {
            if (Function == 'Maths') {
                return '/Areas/Configuration/Scripts/Maths/MathsModal.html';
            }
            else if (Function == 'MathsFunctions') {
                return '/Areas/Configuration/Scripts/Maths Functions/MathsFunctionsModal.html'
            }
            else if (Function == 'Comments') {
                return '/Areas/Configuration/Scripts/Comments/CommentsModal.html'
            }
            else if (Function == 'ErrorsWarnings') {
                return '/Areas/Configuration/Scripts/Errors Warnings/ErrorsWarningsModal.html'
            }
            else if (Function == 'Period') {
                return '/Areas/Configuration/Scripts/Period/PeriodModal.html'
            }
            else if (Function == 'Factors') {
                return '/Areas/Configuration/Scripts/Factors/FactorsModal.html'
            }
            else if (Function == 'Function') {
                return '/Areas/Configuration/Scripts/Function/FunctionModal.html'
            }
            else if (Function == 'DateAdjustment') {
                return '/Areas/Configuration/Scripts/Date Adjustment/DateAdjustmentModal.html'
            }
            else if (Function == 'DatePart') {
                return '/Areas/Configuration/Scripts/Date Part/DatePartModal.html'
            }
            else if (Function == 'Input') {
                return '/Areas/Configuration/Scripts/Input/InputModal.html'
            }
            else if (Function == 'ArrayFunctions') {
                return '/Areas/Configuration/Scripts/Array Functions/ArrayFunctionsModal.html'
            }
            else if (Function == 'StringFunctions') {
                return '/Areas/Configuration/Scripts/String Functions/StringFunctionsModal.html'
            }
            else if (Function == 'Return') {
                return '/Areas/Configuration/Scripts/Return/ReturnModal.html'
            }
        },     
    }
});