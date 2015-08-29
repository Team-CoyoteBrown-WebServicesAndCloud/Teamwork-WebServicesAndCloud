'use strict';

app.factory('authenticationService', function ($http, baseServiceUrl, $localStorage) {
    var authenticationService = {};

    authenticationService.setCredentials = function (serverData) {
        $localStorage.currentUser = serverData;
    };

    authenticationService.clearCredentials = function () {
        $localStorage.$reset();
    };

    authenticationService.isLoggedIn = function () {
        return $localStorage.currentUser != undefined;
    };

    authenticationService.getHeaders = function () {
        return {
            Authorization: "Bearer " + $localStorage.currentUser.access_token
        };
    };

    authenticationService.getCurrentUserData = function () {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me',
            headers: this.getHeaders()
        })
    };

    authenticationService.login = function (userData) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/users/login',
            data: userData
        })
    };

    authenticationService.register = function (userData) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/users/register',
            data: userData
        })
    };

    authenticationService.logout = function () {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/users/logout',
            headers: this.getHeaders()
        });
    };

    authenticationService.editProfile = function (userData) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/me',
            data: userData,
            headers: this.getHeaders()
        });
    };

    authenticationService.changePassword = function (userData) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/me/changepassword',
            data: userData,
            headers: this.getHeaders()
        });
    };

    return authenticationService;
});