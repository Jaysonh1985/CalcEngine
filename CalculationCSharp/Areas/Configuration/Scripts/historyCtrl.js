sulhome.kanbanBoardApp.controller('historyCtrl', function ($scope, $uibModalInstance, $log, $http, $location,configService, ID) {
    
    function init() {
      $scope.isLoading = true;
        configService.getHistory(ID)
           .then(function (data) {
               $scope.isLoading = false;
               $scope.Boards = data;
           }, onError);
    };
    //Click OK
    $scope.ok = function () {
        $scope.addItem();
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    init();

    var onError = function (errorMessage) {
        $scope.isLoading = false;
        toastr.error(errorMessage, "Error");
    };

})
