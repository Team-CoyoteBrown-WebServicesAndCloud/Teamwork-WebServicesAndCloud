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
            url: baseServiceUrl + '/me/requests/' + id + '?status=approved',
            headers: authenticationService.getHeaders()
        })
    };

    userService.rejectFriendRequest = function (id) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/me/requests/' + id + '?status=rejected',
            headers: authenticationService.getHeaders()
        })
    };

    userService.searchUsers = function (term) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/search?searchTerm=' + term,
            headers: authenticationService.getHeaders()
        })
    };

    userService.getNewsFeed = function (pageSize, startPostId) {
        startPostId = startPostId ? "=" + startPostId : "";
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/me/feed?StartPostId' + startPostId + '&PageSize=' + pageSize,
            headers: authenticationService.getHeaders()
        })
    };

    userService.getUserPreviewData = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/preview',
            headers: authenticationService.getHeaders()
        })
    };

    return userService;
});


//service.getOwnFriendsPreview = function () {
//    return $http({
//        method: 'GET',
//        url: baseServiceUrl + '/me/friends/preview',
//        headers: authenticationService.getHeaders()
//    });
//}