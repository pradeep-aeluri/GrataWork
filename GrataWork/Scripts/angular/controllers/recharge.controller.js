gw.controller('rechargeCtrl', ['$scope', 'rechargeSvc', rechargeCtrl]);

function rechargeCtrl($scope, rechargeSvc) {
    $scope.cardDetails = {
        CardHolder: "pradeep aeluri",
        CardNumber: "5555555555554444",
        CVV: "903",
        ExpiryMonth: "07",
        ExpiryYear: "2018",
        Address: "123 main st",
        City: "Orlando",
        State: "FL",
        Zip: "32817"
    }

    $scope.rechargeInput = {
        AuthorizationCode: '',
        HoursAdded: null,
        PlanId: ''
    }

    $scope.GetPlanRate = function (planId) {
        if (planId == 'premium') return 100;
        if (planId == 'standard') return 110;
        if (planId == 'small') return 120;
    }

    $scope.rechargeError = ''
    $scope.changePlanError = ''
    $scope.paymentError = ''
    $scope.validationMsg = ''
    $scope.newPlanId = ''
    
    function GetAccountDetails() {
        rechargeSvc.AccountDetails().then(function (d) {
            console.log(d);
            $scope.AccountDetails = d.data;
            $scope.rechargeInput.PlanId = $scope.AccountDetails.PlanId;
        }, function (err) {
            console.log('Error in GetAccountDetails - ' + err);
        });
    }

    $scope.ChangePlan = function () {
        $scope.rechargeInput.PlanId = $scope.newPlanId;

        rechargeSvc.changePlan($scope.rechargeInput).then(function (d) {
            console.log(d);
            $scope.validationMsg = 'You have successfully changed your plan to ' + $scope.newPlanId + '. This change is effective starting next month.';
            $("#divElements").hide();
        }, function (err) {
            $scope.changePlanError = 'Could not change your plan - ' + err.ExceptionMessage;
        });
    }

    $scope.Recharge = function () {
        if ($scope.rechargeInput.HoursAdded <= 0) {
            $scope.rechargeError = "Please specify accurate number of hours to recharge";
        }
        else {
            rechargeSvc.rechargeAccount($scope.rechargeInput).then(function (d) {
                console.log(d);
                $scope.validationMsg = 'Your account is successfully recharged.';
                $("#divElements").hide();
            }, function (err) {
                $scope.rechargeError = 'Could not recharge your account - ' + err.ExceptionMessage;
            });
        }        
    }

    $scope.CreateAccount = function () {
        if (!$scope.createAccountForm.$valid) return;

        $('#btnRecharge').prop('disabled', true);

        if (!Stripe.card.validateCardNumber($scope.cardDetails.CardNumber)) {
            $scope.paymentError = "Invalid card number"
        }
        else {
            $scope.paymentError = "";

            //call stripe.js to create a token for the card details.
            Stripe.card.createToken({
                number: $scope.cardDetails.CardNumber,
                cvc: $scope.cardDetails.CVV,
                exp_month: $scope.cardDetails.ExpiryMonth,
                exp_year: $scope.cardDetails.ExpiryYear,
                name: $scope.cardDetails.CardHolder,
                address_line1: $scope.cardDetails.Address,
                address_city: $scope.cardDetails.City,
                address_zip: $scope.cardDetails.Zip
            }, createAccountHandler);
        }
    }

    function createAccountHandler(status, response) {
        if (response.error) {
            $scope.paymentError = response.error.message;
        }
        else {
            $scope.rechargeInput.AuthorizationCode = response.id;
            rechargeSvc.createAccount($scope.rechargeInput).then(function (d) {
                console.log(d);
                $scope.paymentError = d.data;
            });
        }
    }

    $scope.isPlan = function (planId) {
        if (!$scope.AccountDetails) return false;

        if ($scope.AccountDetails.PlanId == planId)
            return true;
        else
            return false;
    }

    $scope.CancelSubscription = function(){
        rechargeSvc.CancelSubscription().then(function (d) {
            
        }, function (err) {
            console.log('Error in GetAccountDetails - ' + err);
        });
    }

    GetAccountDetails();
}