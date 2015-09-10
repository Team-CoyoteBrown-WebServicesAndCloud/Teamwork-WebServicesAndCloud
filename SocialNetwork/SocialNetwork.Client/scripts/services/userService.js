'use strict';

app.factory('userService', function ($http, baseServiceUrl, $localStorage, authenticationService) {
   var userService = {};

    userService.getOwnFriendsPreview = function () {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/friends/preview',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getOwnFriendsDetailed = function () {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/friends/',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getFriendFriendsPreview = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/friends/preview',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getFriendFriendsDetailed = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/friends/',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getUserFullData = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/',
            headers: authenticationService.getHeaders()
        })
    };

    userService.sendFriendRequest = function (username) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/me/requests/' + username + '/',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getFriendRequests = function () {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/requests/',
            headers: authenticationService.getHeaders()
        })
    };

    userService.approveFriendRequest = function (id) {
        return $http({
            method: 'PUT',
            //url: baseServiceUrl + '/me/requests/' + id + '?status=approved',
            url: baseServiceUrl + '/me/requests/' + id + "/approve",
            headers: authenticationService.getHeaders()
        })
    };

    userService.rejectFriendRequest = function (id) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/me/requests/' + id + '/reject',
            headers: authenticationService.getHeaders()
        })
    };

    userService.searchUsers = function (word) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/search?searchWord=' + word,
            headers: authenticationService.getHeaders()
        })
    };

    userService.getNewsFeed = function (postCount, startPostNumber) {
        startPostNumber = startPostNumber ? startPostNumber : 0;
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/feed?postsCount=' + postCount + '&startPostNumber=' + startPostNumber,
            headers: authenticationService.getHeaders()
        })
    };

    userService.getUserPreviewData = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/friends/' + username + '/preview',
            headers: authenticationService.getHeaders()
        })
    };

    userService.getUserPhotosPreview = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/photos/preview',
            headers: authenticationService.getHeaders()
        })
    };

    userService.addPhoto = function (photoData) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/users/photos',
            headers: authenticationService.getHeaders(),
            data: photoData
        })
    };

    return userService;
});