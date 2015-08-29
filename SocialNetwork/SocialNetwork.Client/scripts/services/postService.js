'use strict';

app.factory('postService', function ($http, baseServiceUrl, $localStorage, authenticationService) {
    var postService = {};

    postService.getWallPosts = function (username) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/users/' + username + '/wall?StartPostId&PageSize=5',
            headers: authenticationService.getHeaders()
        })
    };

    postService.addNewPost = function (postContent, username) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/posts/',
            headers: authenticationService.getHeaders(),
            data: {
                username: username,
                postContent: postContent
            }
        })
    };

    postService.editPost = function (postContent, postId) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/posts/' + postId,
            headers: authenticationService.getHeaders(),
            data: {
                postContent: postContent
            }
        })
    };

    postService.deletePost = function (postId) {
        return $http({
            method: 'DELETE',
            url: baseServiceUrl + '/posts/' + postId,
            headers: authenticationService.getHeaders()
        })
    };

    postService.addNewComment = function (commentContent, commentId) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/posts/' + commentId + '/comments',
            headers: authenticationService.getHeaders(),
            data: {
                commentContent: commentContent
            }
        })
    };

    postService.editComment = function (commentContent, commentId, postId) {
        return $http({
            method: 'PUT',
            url: baseServiceUrl + '/posts/' + postId + '/comments/' + commentId,
            headers: authenticationService.getHeaders(),
            data: {
                commentContent: commentContent
            }
        })
    };

    postService.deleteComment = function (commentId, postId) {
        return $http({
            method: 'DELETE',
            url: baseServiceUrl + '/posts/' + postId + '/comments/' + commentId,
            headers: authenticationService.getHeaders()
        })
    };

    postService.likePost = function (postId) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/posts/' + postId + '/likes/',
            headers: authenticationService.getHeaders()
        })
    };

    postService.unlikePost = function (postId) {
        return $http({
            method: 'DELETE',
            url: baseServiceUrl + '/posts/' + postId + '/likes/',
            headers: authenticationService.getHeaders()
        })
    };

    postService.likeComment = function (commentId, postId) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/posts/' + postId + '/comments/' + commentId + '/likes/',
            headers: authenticationService.getHeaders()
        })
    };

    postService.unlikeComment = function (commentId, postId) {
        return $http({
            method: 'DELETE',
            url: baseServiceUrl + '/posts/' + postId + '/comments/' + commentId + '/likes/',
            headers: authenticationService.getHeaders()
        })
    };

    postService.getPostComments = function (postId) {
        return $http({
            method: 'GET',
            url: baseServiceUrl + '/posts/' + postId + '/comments/',
            headers: authenticationService.getHeaders()
        })
    };

    return postService;
});