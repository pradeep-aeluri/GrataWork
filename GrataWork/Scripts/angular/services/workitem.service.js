gw.service('workItemSvc', ['$http', '$q', '$rootScope', '$sce', '$window', workItemSvc]);

function workItemSvc($http, $q, $rootScope, $sce, $window) {

    var svc = {};

    svc.loadWorkItems = function() {
        var deferred = $q.defer();
        var url = 'api/workitem/getitemsforuser';

        $http.get(gw.baseURL + url, {}).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err);
                    deferred.resolve([]);
                });

        return deferred.promise;
    }

    svc.createNewItem = function (wi) {
        var deferred = $q.defer();
        var url = 'api/workitem/create';

        $http.post(gw.baseURL + url, wi).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err);
                    deferred.resolve([]);
                });

        return deferred.promise;
    }

    return svc;
}