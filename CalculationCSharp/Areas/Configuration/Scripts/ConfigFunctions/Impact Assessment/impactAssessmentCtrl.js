// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('impactAssessmentCtrl', function ($scope, $uibModalInstance, ID, configService, configFunctionFactory) {   
    
    function init() {
        configService.getConfig()
            .then(function (data) {               
                $scope.isLoading = false;
                $scope.configuration = [];
                var config  = data;
                findFunctionUsage(config);
            }, onError);
    };

    function findFunctionUsage(config){
        angular.forEach(config, function (valuec, keyc, propc) {           
           var columns = angular.fromJson(valuec.Configuration);
           var keepGoing = true;
           $scope.ID = valuec.ID;
           $scope.Scheme = valuec.Scheme;
           $scope.CalculationName = valuec.Name;
           $scope.Version = valuec.Version;
           angular.forEach(columns, function (valueCol, keyCol, propCol) {
                if(keepGoing) {
                    var findIndex = configFunctionFactory.getIndexOf(valueCol.Functions, 'Function', 'Function');
                    var findIndex = parseInt(findIndex)
                    if(findIndex >= 0)
                    {
                        $scope.configuration.push({
                            ID: $scope.ID,
                            Scheme: $scope.Scheme,
                            CalculationName: $scope.CalculationName,
                            Version: $scope.Version,
                        });
                    keepGoing = false;
                };
              };         
           });
        });
    };

    //Click OK
    $scope.ok = function (form) {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    init();   

    var onError = function (errorMessage) {
        toastr.error(errorMessage, "Error");
    }; 
});