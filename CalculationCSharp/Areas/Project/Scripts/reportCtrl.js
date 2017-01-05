// Copyright (c) 2016 Project AIM
sulhome.kanbanBoardApp.controller('reportCtrl', function ($scope, $uibModalInstance, $interval, $filter, UserList) {
    $scope.openIndexTimings = [true];
    $scope.openUserIndexTimings = [true];
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

            var Red0length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: null}).length;
            var Red1length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '1' }).length;
            var Red2length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '2' }).length;
            var Red3length = $filter('filter')(key.Stories, { RAG: 'Red', Moscow: '3' }).length;

            var Amber0length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '' }).length;
            var Amber1length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '1' }).length;
            var Amber2length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '2' }).length;
            var Amber3length = $filter('filter')(key.Stories, { RAG: 'Amber', Moscow: '3' }).length;

            var Green0length = $filter('filter')(key.Stories, { RAG: 'Green', Moscow: null }).length;
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
        Total0 = Red0 + Amber0 + Green0;
        Total1 = Red1 + Amber1 + Green1;
        Total2 = Red2 + Amber2 + Green2;
        Total3 = Red3 + Amber3 + Green3;
        Total = Total0 + Total1 + Total2 + Total3;

        $scope.RagReport = [];
        $scope.RagReport.push({
            Priority: '1',
            Red: Red1,
            Amber: Amber1,
            Green: Green1,
            Total: Total1
        })
        $scope.RagReport.push({
            Priority: '2',
            Red: Red2,
            Amber: Amber2,
            Green: Green2,
            Total: Total2
        })
        $scope.RagReport.push({
            Priority: '3',
            Red: Red3,
            Amber: Amber3,
            Green: Green3,
            Total: Total3
        })
        $scope.RagReport.push({
            Priority: 'Unassigned',
            Red: Red0,
            Amber: Amber0,
            Green: Green0,
            Total: Total0
        })
        $scope.RagReport.push({
            Priority: 'Total',
            Red: RedTotal,
            Amber: AmberTotal,
            Green: GreenTotal,
            Total: Total
        })

    }

    function UserReportRAG() {

        $scope.UserReportRAG = [];
        
        angular.forEach($scope.UserNames, function (keyUser, valueUser, objUser) {
            
            var Red = 0;
            var Amber = 0;
            var Green = 0;
            var Total = 0;

            angular.forEach($scope.columns, function (key, value, obj) {

                var Redlength = $filter('filter')(key.Stories, { RAG: 'Red', User: keyUser.Text }).length;
                var Amberlength = $filter('filter')(key.Stories, { RAG: 'Amber', User: keyUser.Text }).length;
                var Greenlength = $filter('filter')(key.Stories, { RAG: 'Green', User: keyUser.Text }).length;

                Red = Red + Redlength;
                Amber = Amber + Amberlength;
                Green = Green + Greenlength;

            });
            Total = Red + Amber + Green;
                   
            if (Red > 0 || Amber > 0 || Green > 0)
            {
                $scope.UserReportRAG.push({
                    User: keyUser.Text,
                    Red: Red,
                    Amber: Amber,
                    Green: Green,
                    Total: Total
                })
            }

        })
        //Unassigned count
        var Red = 0;
        var Amber = 0;
        var Green = 0;
        var Total = 0;

        angular.forEach($scope.columns, function (key, value, obj) {

            var Redlength = $filter('filter')(key.Stories, { RAG: 'Red', User: null }).length;
            var Amberlength = $filter('filter')(key.Stories, { RAG: 'Amber', User: null }).length;
            var Greenlength = $filter('filter')(key.Stories, { RAG: 'Green', User: null }).length;

            Red = Red + Redlength;
            Amber = Amber + Amberlength;
            Green = Green + Greenlength;

        });

        Total = Red + Amber + Green;
        if (Red > 0 || Amber > 0 || Green > 0) {
            $scope.UserReportRAG.push({
                User: 'Unassigned',
                Red: Red,
                Amber: Amber,
                Green: Green,
                Total: Total
            })
        }

    }

    function UserReportPrioirty() {

        $scope.UserReportPrioirty = [];

        angular.forEach($scope.UserNames, function (keyUser, valueUser, objUser) {

            var High = 0;
            var Medium = 0;
            var Low = 0;
            var Unassigned = 0;
            var Total = 0;

            angular.forEach($scope.columns, function (key, value, obj) {

                var Highlength = $filter('filter')(key.Stories, { Moscow: 1, User: keyUser.Text }).length;
                var Mediumlength = $filter('filter')(key.Stories, { Moscow: 2, User: keyUser.Text }).length;
                var Lowlength = $filter('filter')(key.Stories, { Moscow: 3, User: keyUser.Text }).length;
                var Unassignedlength = $filter('filter')(key.Stories, { Moscow: null, User: keyUser.Text }).length;

                High = High + Highlength;
                Medium = Medium + Mediumlength;
                Low = Low + Lowlength;
                Unassigned = Unassigned + Unassignedlength;

            });
            Total = High + Medium + Low + Unassigned;
            if (High > 0 || Medium > 0 || Low > 0 || Unassigned > 0) {
                $scope.UserReportPrioirty.push({
                    User: keyUser.Text,
                    High: High,
                    Medium: Medium,
                    Low: Low,
                    Unassigned: Unassigned,
                    Total: Total
                })
            }

        })
        //Unassigned count
        var High = 0;
        var Medium = 0;
        var Low = 0;
        var Unassigned = 0;
        var Total = 0;

        angular.forEach($scope.columns, function (key, value, obj) {

            var Highlength = $filter('filter')(key.Stories, { Moscow: 1, User: null }).length;
            var Mediumlength = $filter('filter')(key.Stories, { Moscow: 2, User: null }).length;
            var Lowlength = $filter('filter')(key.Stories, { Moscow: 3, User: null }).length;
            var Unassignedlength = $filter('filter')(key.Stories, { Moscow: null, User: null }).length;

            High = High + Highlength;
            Medium = Medium + Mediumlength;
            Low = Low + Lowlength;
            Unassigned = Unassigned + Unassignedlength;

        });

        Total = High + Medium + Low + Unassigned;
        if (High > 0 || Medium > 0 || Low > 0 || Unassigned > 0) {
            $scope.UserReportPrioirty.push({
                User: 'Unassigned',
                High: High,
                Medium: Medium,
                Low: Low,
                Unassigned: Unassigned,
                Total: Total
            })
        }
    }

    function ActivityTimeReport() {
        $scope.ActivityTimeReport = [];
        var TotalTime = 0;
        angular.forEach($scope.columns, function (keycol, valuecol, objcol) {
            angular.forEach(keycol.Stories, function (keystory, valuestory, objstory) {
                $scope.ActivityTimeReport.push({
                    Name: keystory.Name,
                    Time: keystory.ElapsedTime
                })
                var Input1Bool = isNaN(parseInt(keystory.ElapsedTime));
                if (Input1Bool == false)
                {
                    TotalTime = TotalTime + parseInt(keystory.ElapsedTime);
                }
                
            });
        });

        $scope.ActivityTimeReport.push({
            Name: 'Total',
            Time: TotalTime
        })

    }

    function UserTimeReport() {
        $scope.UserTimeReport = [];
        var TotalTime = 0;
        
        angular.forEach($scope.UserNames, function (key, value, obj)
        {
            var UserElapsedTime = 0;
            angular.forEach($scope.columns, function (keycol, valuecol, objcol) {
                angular.forEach(keycol.Stories, function (keystory, valuestory, objstory) {
                    angular.forEach(keystory.Updates, function (keyupdates, valueupdates, objupdates) {

                        if (keyupdates.UpdateUser == key.Value)
                        {
                            UserElapsedTime = UserElapsedTime + parseInt(keyupdates.UpdateValue);
                        }
                    });
                });
            });

            $scope.UserTimeReport.push({
                Name: key.Value,
                Time: UserElapsedTime
            })
            TotalTime = TotalTime + UserElapsedTime;
        })


        $scope.UserTimeReport.push({
            Name: 'Total',
            Time: TotalTime
        })

    }
    //Click OK
    $scope.ok = function () {
        $scope.selected = {

        };
        $uibModalInstance.close($scope.selected);
    };

    RAGReport();
    UserReportRAG();
    UserReportPrioirty();
    ActivityTimeReport();
    UserTimeReport();
});