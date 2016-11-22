gw.service('rechargeSvc', ['$http', '$q', '$rootScope', '$sce', '$window', rechargeSvc]);

function rechargeSvc($http, $q, $rootScope, $sce, $window) {

    var svc = {};

    svc.rechargeAccount = function(input) {

        var deferred = $q.defer();
        var url = 'api/stripe/recharge';

        $http.post(gw.baseURL + url, input).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err.data);
                    deferred.reject(err.data);
                });

        return deferred.promise;
    }

    svc.changePlan = function (input) {

        var deferred = $q.defer();
        var url = 'api/stripe/changeplan';

        $http.post(gw.baseURL + url, input).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err.data);
                    deferred.reject(err.data);
                });

        return deferred.promise;
    }

    svc.createAccount = function (input) {

        var deferred = $q.defer();
        var url = 'api/stripe/createcustomer';

        $http.post(gw.baseURL + url, input).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err.data);
                    deferred.reject(err.data);
                });

        return deferred.promise;
    }

    svc.AccountDetails = function () {
        var deferred = $q.defer();
        var url = 'api/stripe/getaccountdetails';

        $http.get(gw.baseURL + url, {}).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err.data);
                    deferred.reject(err.data);
                });

        return deferred.promise;
    }

    svc.CancelSubscription = function () {
        var deferred = $q.defer();
        var url = 'api/stripe/cancelsubscription';

        $http.get(gw.baseURL + url, {}).
                then(function (r) {
                    if (!r || !r.data) {
                        console.log('No Data Returned');
                        deferred.resolve([]);
                    }
                    deferred.resolve(r);

                }, function (err) {
                    console.log('Error: ' + err.data);
                    deferred.reject(err.data);
                });

        return deferred.promise;
    }

    return svc;
}