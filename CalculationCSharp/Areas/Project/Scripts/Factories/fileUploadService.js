sulhome.kanbanBoardApp.factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2
 
    var fac = {};
    fac.UploadFile = function (file, description) {
        var formData = new FormData();
        formData.append("file", file);
        //We can send more data to server using append         
        formData.append("description", description);
 
        var defer = $q.defer();
        $http.post("/UploadFile/SaveFiles", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });
 
        return defer.promise;
 
    }
    fac.DeleteFile = function (FileName) {

        var defer = $q.defer();
        $http({
            method: 'DELETE',
            url: '/UploadFile/DeleteFiles/' + 1,
            data: {
                FileName: FileName
            },
            headers: {
                'Content-type': 'application/json;charset=utf-8'
            }
        })
        .then(function (d) {
            defer.resolve(d);
        }, function (rejection) {
            defer.reject("File Download Failed!");
        });
        return defer.promise;

    }
    return fac;

});
