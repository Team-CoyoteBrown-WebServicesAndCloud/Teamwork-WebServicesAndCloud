app.controller('GroupController',
    function ($scope, notifyService, $routeParams, $localStorage, $rootScope, $location, authenticationService, usSpinnerService, groupService) {
        if (authenticationService.isLoggedIn()) {
            $scope.groupData = [];
        }

        $scope.createGroup = function (groupData) {
            var data = {};

            data.CoverImageData = groupData.image.base64;
            data.description = groupData.description;
            data.name = groupData.name;

            usSpinnerService.spin('spinner-1');
            groupService.createGroup(data).then(
                function () {
                    notifyService.showInfo('The group "' + data.name + '" has been successfully created.');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Unable to create new group. " + error.data.message);
                })
        }
    });