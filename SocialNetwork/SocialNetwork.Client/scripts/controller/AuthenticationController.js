app.controller('AuthenticationController',
    function ($scope, $location, $rootScope, authenticationService, notifyService, $localStorage) {
        if ($scope.isLogged) {
            $scope.userData = function () {
                authenticationService.getCurrentUserData().then(
                    function (userData) {
                        $scope.userData = $scope.checkForEmptyImages(userData.data);
                    },
                    function (error) {
                        notifyService.showError('Unable to get current user data', error.data)
                    }
                );
            }
        }

        $scope.register = function (userData) {
            authenticationService.register(userData).then(
                function success(serverData) {
                    notifyService.showInfo('Successfully registered');
                    authenticationService.setCredentials(serverData.data);
                    $location.path("/");
                },
                function error(error) {
                    notifyService.showError("Unable to register." + error.data.message);
                }
            );
        };

        $scope.login = function (userData) {
            authenticationService.login(userData).then(
                function success(serverData) {
                    authenticationService.setCredentials(serverData.data);
                    notifyService.showInfo("Hello, " + $localStorage.currentUser.userName);
                    $location.path("/");
                },
                function error(error) {
                    notifyService.showError('Unsuccessful login', error.data);
                }
            );
        };

        $scope.logout = function () {
            authenticationService.logout().then(
                function success(serverData) {
                    notifyService.showInfo('Goodbye, ' + $localStorage.currentUser.userName);
                    $location.path('/');
                    authenticationService.clearCredentials(serverData.data);
                },
                function error(error) {
                    notifyService.showError("Unable to logout", error.data.message);
                }
            );
        };

        $scope.editProfile = function (userData) {
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
                    notifyService.showInfo('Your profile has been successfully edited');
                    $location.path('/');
                },
                function error(error) {
                    notifyService.showError("Unable to edit your profile", error.data);
                }
            );
        };

        $scope.changePassword = function (userData) {
            authenticationService.changePassword(userData).then(
                function success() {
                    notifyService.showInfo('Your password has been successfully changed');
                    $location.path('/');
                },
                function error(error) {
                    notifyService.showError('Unable to change password', error.data);
                }
            )
        }
    });