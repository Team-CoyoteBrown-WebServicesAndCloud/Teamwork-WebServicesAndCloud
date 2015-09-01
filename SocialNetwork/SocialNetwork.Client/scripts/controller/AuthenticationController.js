app.controller('AuthenticationController',
    function ($scope, $location, $rootScope, authenticationService, notifyService, $localStorage, usSpinnerService) {
        if ($scope.isLogged) {
            $scope.userData = function () {
                usSpinnerService.spin('spinner-1');
                authenticationService.getCurrentUserData().then(
                    function (userData) {
                        usSpinnerService.stop('spinner-1');
                        $scope.userData = $scope.checkForEmptyImages(userData.data);
                    },
                    function (error) {
                        notifyService.showError('Unable to get current user data. ' + error.data.message);
                        usSpinnerService.stop('spinner-1');
                    }
                );
            }
        }

        $scope.register = function (userData) {
            usSpinnerService.spin('spinner-1');
            authenticationService.register(userData).then(
                function success(serverData) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Successfully registered');
                    authenticationService.setCredentials(serverData.data);
                    $location.path("/");
                },
                function error(error) {
                    notifyService.showError("Unable to register." + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            );
        };

        $scope.login = function (userData) {
            usSpinnerService.spin('spinner-1');
            authenticationService.login(userData).then(
                function success(serverData) {
                    usSpinnerService.stop('spinner-1');
                    authenticationService.setCredentials(serverData.data);
                    notifyService.showInfo("Hello, " + $localStorage.currentUser.userName);
                    $location.path("/");
                },
                function error(error) {
                    notifyService.showError('Unsuccessful login' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            );
        };

        $scope.logout = function () {
            usSpinnerService.spin('spinner-1');
            authenticationService.logout().then(
                function success(serverData) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Goodbye, ' + $localStorage.currentUser.userName);
                    authenticationService.clearCredentials(serverData.data);
                    $location.path('/');
                },
                function error(error) {
                    notifyService.showError("Unable to logout" + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            );
        };

        $scope.editProfile = function (userData) {
            usSpinnerService.spin('spinner-1');
            var data = {};
            data.name = userData.name;
            data.email = userData.email;
            data.profileImageData = userData.profileImageData.base64;
            data.coverImageData = userData.coverImageData.base64;
            data.gender = userData.gender;

            if (data.profileImageData == undefined) {
                data.profileImageData = userData.profileImageData;
            }

            if (data.coverImageData == undefined) {
                data.coverImageData = userData.coverImageData;
            }

            authenticationService.editProfile(data).then(
                function success() {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Your profile has been successfully edited');
                    $location.path('/');
                },
                function error(error) {
                    notifyService.showError("Unable to edit your profile. " + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            );
        };

        $scope.changePassword = function (userData) {
            usSpinnerService.spin('spinner-1');
            authenticationService.changePassword(userData).then(
                function success() {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Your password has been successfully changed');
                    $location.path('/');
                },
                function error(error) {
                    notifyService.showError('Unable to change password. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        }
    });