'use strict';

var app = angular.module('socialNetworkApp', ['ngRoute', 'ngResource', 'naif.base64', 'ngStorage', 'angularSpinner', 'rt.popup', 'infinite-scroll']);

app.constant({
    'baseServiceUrl': 'http://blackfacebook.azurewebsites.net/api',
    'pageSize': 5
});


app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'templates/home.html',
            controller: 'MainController'
        })
        .when('/login', {
            templateUrl: 'templates/login.html',
            controller: 'AuthenticationController'
        })
        .when('/register', {
            templateUrl: 'templates/register.html',
            controller: 'AuthenticationController'
        })
        .when('/logout', {
            templateUrl: 'templates/home.html',
            controller: 'AuthenticationController'
        })
        .when('/profile/edit-profile', {
            templateUrl: 'templates/user/edit-profile.html',
            controller: 'MainController',
        })
        .when('/profile/change-password', {
            templateUrl: 'templates/user/change-password.html',
            controller: 'MainController'
        })
        .when('/users/:username/', {
            templateUrl: 'templates/user/user-wall.html',
            controller: 'MainController'
        })
        .when('/friends/requests/', {
            templateUrl: 'templates/friend-requests.html',
            controller: 'MainController'
        })
        .when('/users/:username/friends/', {
            templateUrl: 'templates/user/user-all-friends.html',
            controller: 'MainController'
        })
        .when('/me/friends/', {
            templateUrl: 'templates/user/all-own-friends.html',
            controller: 'MainController'
        })
        .when('/users/:username/photos/', {
            templateUrl: 'templates/user/user-photos.html',
            controller: 'MainController'
        })
        .when('/users/:username/photos/add', {
            templateUrl: 'templates/partial/add-photo.html',
            controller: 'MainController'
        })
        .when('/users/:username/photos/:id', {
            templateUrl: 'templates/photo.html',
            controller: 'MainController'
        })
        .when('/groups/create', {
            templateUrl: 'templates/create-group.html',
            controller: 'MainController'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);

app.run(function ($rootScope, $location, authenticationService, notifyService) {
    $rootScope.$on('$locationChangeStart', function (event) {
        var isRegisterPage = $location.path().indexOf('/register') === -1,
            isLoginPage = $location.path().indexOf('/login') === -1,
            isHomePage = $location.path().indexOf('/') > -1 && $location.path().length === 1 || $location.path().length === 0,
            isLoggedIn = authenticationService.isLoggedIn();

        if (!isLoggedIn && (!isHomePage && isRegisterPage && isLoginPage)) {
            notifyService.showError('Login or register first');
            $location.path("/");
        } else if (isLoggedIn && (!isRegisterPage || !isLoginPage)) {
            notifyService.showError("Can't go there. Logout first.");
            $location.path("/");
        }
    });
});