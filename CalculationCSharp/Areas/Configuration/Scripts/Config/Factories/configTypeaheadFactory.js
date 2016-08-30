sulhome.kanbanBoardApp.factory('configTypeaheadFactory', function ($filter, configFunctionFactory) {
    return {
        //get ctrl file path for functions modal
        variableArrayBuilder: function (config, colIndex, type, rowIndex) {

            var counter = 0;
            var scopeid = 0;
            var functionID = 0;
            var arrayID = 0;
            var Decimal = [];
            var DecimalValue = [];
            var DecimalParameter = [];
            var Names = [];
            var newArr = [];

            angular.forEach(config, function (groups) {
                if (scopeid <= colIndex) {

                    if (type == null) {
                        DecimalValue = ($filter('filter')(config[scopeid].Functions));
                    }
                    else {
                        DecimalValue = ($filter('filter')(config[scopeid].Functions, { Type: type }));
                    }

                    if (scopeid == colIndex) {
                        var spliceid = rowIndex;
                        var DecimalValueID = 0;

                        angular.forEach(DecimalValue, function (Names) {
                            DecimalValue.splice(spliceid, 1);
                        });

                    };


                    functionID = 0;
                    angular.forEach(DecimalValue, function (Names) {
                        DecimalParameter = ($filter('filter')(DecimalValue[functionID].Name));
                        
                        var NamesIndex = configFunctionFactory.getIndexOf(Names, DecimalParameter, 'Name');

                        if (Names.indexOf(DecimalParameter) == -1) {

                            Names[arrayID] = DecimalParameter;

                            arrayID = arrayID + 1;

                        }
                        functionID = functionID + 1;

                    });
                    scopeid = scopeid + 1
                }
            });
            scopeid = 0;

            return Names;
        },
        

    }


});