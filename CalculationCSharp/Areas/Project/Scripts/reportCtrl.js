// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('reportCtrl', function ($scope, $uibModalInstance, $interval, $filter) {
    
    function RAGReport() {

        var Red0 = 0;
        var Red1 = 0;
        var Red2 = 0;
        var Red3 = 0;

        var Amber0 = 0;
        var Amber1 = 0;
        var Amber2 = 0;
        var Amber3 = 0;

        var Green0 = 0;
        var Green1 = 0;
        var Green2 = 0;
        var Green3 = 0;

        angular.forEach($scope.columns, function (key, value, obj) {

            var Red0length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '' }).length;
            var Red1length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '1' }).length;
            var Red2length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '2' }).length;
            var Red3length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '3' }).length;

            var Amber0length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '' }).length;
            var Amber1length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '1' }).length;
            var Amber2length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '2' }).length;
            var Amber3length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '3' }).length;

            var Green0length = $filter('filter')(key.Stories, { RAG: 'Green', Moscow: '' }).length;
            var Green1length = $filter('filter')(key.Stories, { RAG: 'Green', Moscow: '1' }).length;
            var Green2length = $filter('filter')(key.Stories, { RAG: 'Green', Moscow: '2' }).length;
            var Green3length = $filter('filter')(key.Stories, { RAG: 'Green', Moscow: '3' }).length;

            Red0 = Red0 + Red0length;
            Red1 = Red1 + Red1length;
            Red2 = Red2 + Red2length;
            Red3 = Red3 + Red3length;

            Amber0 = Amber0 + Amber0length;
            Amber1 = Amber1 + Amber1length;
            Amber2 = Amber2 + Amber2length;
            Amber3 = Amber3 + Amber3length;

            Green0 = Green0 + Green0length;
            Green1 = Green1 + Green1length;
            Green2 = Green2 + Green2length;
            Green3 = Green3 + Green3length;

        });

        RedTotal = Red0 + Red1 + Red2 + Red3;
        AmberTotal = Amber0 + Amber1 + Amber2 + Amber3;
        GreenTotal = Green0 + Green1 + Green2 + Green3;

        $scope.RagReport = [];
        $scope.RagReport.push({
            Priority: '0',
            Red: Red0,
            Amber: Amber0,
            Green: Green0
        })
        $scope.RagReport.push({
            Priority: '1',
            Red: Red1,
            Amber: Amber1,
            Green: Green1
        })
        $scope.RagReport.push({
            Priority: '2',
            Red: Red2,
            Amber: Amber2,
            Green: Green2
        })
        $scope.RagReport.push({
            Priority: '3',
            Red: Red3,
            Amber: Amber3,
            Green: Green3
        })
        $scope.RagReport.push({
            Priority: 'Total',
            Red: RedTotal,
            Amber: AmberTotal,
            Green: GreenTotal
        })
    }
    function ColumnReport() {

        var BacklogCount = $scope.columns[0].Stories.length;
        var InProgressCount = $scope.columns[1].Stories.length;
        var PendingCount = $scope.columns[2].Stories.length;
        var ReleaseCount = $scope.columns[3].Stories.length;
        $scope.ColumnReport = [];
        $scope.ColumnReport.push({
            BacklogColumn: BacklogCount,
            InProgressColumn: InProgressCount,
            PendingColumn: PendingCount,
            ReleaseColumn: ReleaseCount
        })

    }
    //Click OK
    $scope.ok = function () {
        $scope.selected = {

        };
        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    RAGReport();
    ColumnReport();
});