gw.controller('workItemCtrl', ['$scope', 'workItemSvc', workItemCtrl]);

function workItemCtrl ($scope, workItemSvc) {
    $scope.items = [];

    $scope.gridColDefs = [];
    $scope.gridOptions = {
        info: false,
        paging: false
    };

    $scope.workItem = {
        Title: "",
        Description: "",
        Attachment: null,
        FileName: ""
    };

    $scope.errorMsg = '';
    $scope.newItemKey = '';
    $scope.isDisabled = false;

    function load() {
        workItemSvc.loadWorkItems().then(function (d)
        {
            $scope.items = d.data;
        }, function (err) {
            $scope.errorMsg = 'An error occurred loading your items';
        });
    }

    $scope.CreateNewItem = function () {
        $scope.newItemKey = '';

        if (!$scope.newTaskForm.$valid) return;
        
        $scope.isDisabled = true;

        var f = document.getElementById('fileAttachment').files[0];
        console.log(f);
        if (f != null)
        {
            $scope.workItem.FileName = f.name;
            var r = new FileReader();

            r.onload = function (e) {
                $scope.workItem.Attachment = e.target.result;
                workItemSvc.createNewItem($scope.workItem).then(function (d) {
                    $scope.newItemKey = d.data;
                    $scope.workItem.Title = '';
                    $scope.workItem.Description = '';
                    $scope.workItem.Attachment = '';
                    $scope.isDisabled = false;
                }, function (err) {
                    $scope.errorMsg = 'An error occurred creating a task';
                    $scope.isDisabled = false;
                });
            }

            r.readAsDataURL(f);
        }
        else
        {
            workItemSvc.createNewItem($scope.workItem).then(function (d) {
                $scope.newItemKey = d.data;
                $scope.workItem.Title = '';
                $scope.workItem.Description = '';
                $scope.workItem.Attachment = '';
                $scope.isDisabled = false;
            }, function (err) {
                $scope.errorMsg = 'An error occurred creating a task';
                $scope.isDisabled = false;
                });
        }
    }

    load();
}