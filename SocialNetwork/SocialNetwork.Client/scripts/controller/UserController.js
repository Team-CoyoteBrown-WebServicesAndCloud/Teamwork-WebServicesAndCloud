app.controller('UserController',
    function ($scope, userService, notifyService, $routeParams, $localStorage, $rootScope, $location, authenticationService, usSpinnerService, pageSize, postService) {
        if (authenticationService.isLoggedIn()) {
            $rootScope.isOwnWall = $localStorage.currentUser.userName  === $routeParams.username;
            $rootScope.isNewsFeed = $location.path() === '/';
            $scope.wallPosts = [];
            $scope.newsFeed = [];
            $scope.busy = false;
            var startNewsFeedPostId,
                startWallPagePostId;
        }

        $scope.getUserFullData = function getUserFullData() {
            usSpinnerService.spin('spinner-1');
            userService.getUserFullData($routeParams.username).then(
                function (userData) {
                    $scope.userFullData = $scope.checkForEmptyImages(userData.data);
                    if($localStorage.currentUser.userName !== $scope.userFullData.username){
                        if($scope.userFullData.isFriend){
                            $scope.userFullData.userStatus = 'friend';
                        } else if($scope.userFullData.hasPendingRequest){
                            $scope.userFullData.userStatus = 'pending';
                        } else {
                            $scope.userFullData.userStatus = 'invite';
                        }
                    }

                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show user data' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getUserPreviewData = function (username) {
            usSpinnerService.spin('spinner-1');
            userService.getUserPreviewData(username).then(
                function (userData) {
                    $scope.userPreviewData = $scope.checkForEmptyImages(userData.data);
                    if($localStorage.currentUser.userName !== $scope.userPreviewData.username){
                        if($scope.userPreviewData.isFriend){
                            $scope.userPreviewData.userStatus = 'friend';
                        } else if($scope.userPreviewData.hasPendingRequest){
                            $scope.userPreviewData.userStatus = 'pending';
                        } else {
                            $scope.userPreviewData.userStatus = 'invite';
                        }
                    }
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show user data. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getOwnFriendsPreview = function () {
            usSpinnerService.spin('spinner-1');
            userService.getOwnFriendsPreview().then(
                function (friendsData) {
                    friendsData.data.friends.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.ownFriendsPreview = friendsData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show your friends' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getOwnFriendsDetailed = function () {
            usSpinnerService.spin('spinner-1');
            userService.getOwnFriendsDetailed().then(
                function (friendsData) {
                    friendsData.data.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.ownFriendsDetailed = friendsData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show your friends detailed. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getFriendFriendsPreview = function () {
            usSpinnerService.spin('spinner-1');
            userService.getFriendFriendsPreview($routeParams.username).then(
                function (friendsData) {
                    friendsData.data.friends.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.friendFriendsPreview = friendsData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show friend friends. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getFriendFriendsDetailed = function () {
            usSpinnerService.spin('spinner-1');
            userService.getFriendFriendsDetailed($routeParams.username).then(
                function (friendsData) {
                    friendsData.data.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.friendFriendsDetailed = friendsData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show friend friends detailed. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getFriendRequests = function () {
            usSpinnerService.spin('spinner-1');
            userService.getFriendRequests().then(
                function (friendRequestData) {
                    friendRequestData.data.forEach(function (requestData) {
                        $scope.checkForEmptyImages(requestData.user);
                    });

                    $scope.friendRequests = friendRequestData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show friend requests. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                });
        };

        $scope.sendFriendRequest = function (username) {
            usSpinnerService.spin('spinner-1');
            var inviteUsername = username ? username : $routeParams.username;
            userService.sendFriendRequest(inviteUsername).then(
                function () {
                    notifyService.showInfo('Friend request has been successfully sent');
                    if (username) {
                        $scope.userPreviewData.userStatus = 'pending';
                    } else{
                        $scope.userFullData.userStatus = 'pending';
                    }

                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to send friend request. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.approveFriendRequest = function (request) {
            usSpinnerService.spin('spinner-1');
            userService.approveFriendRequest(request.id).then(
                function () {
                    $scope.getFriendRequests();
                    $scope.getOwnFriendsPreview();
                    notifyService.showInfo('Friend request from has been approved');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to approve friend request. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.rejectFriendRequest = function (id) {
            usSpinnerService.spin('spinner-1');
            userService.rejectFriendRequest(id).then(
                function () {
                    $scope.getFriendRequests();
                    notifyService.showInfo('Friend request has been rejected');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to reject friend request. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.searchUsersByName = function (term) {
            usSpinnerService.spin('spinner-1');
            if (term.trim().length > 0) {
                userService.searchUsers(term).then(
                    function (serverData) {
                        serverData.data.forEach(function (user) {
                            $scope.checkForEmptyImages(user);
                        });

                        $scope.searchResult = serverData.data;
                        usSpinnerService.stop('spinner-1');
                    },
                    function (error) {
                        notifyService.showError('Unable to search with the given terms. ' + error.data.message);
                        usSpinnerService.stop('spinner-1');
                    });
            } else{
                usSpinnerService.stop('spinner-1');
            }
        };

        $scope.getNewsFeed = function () {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;

            usSpinnerService.spin('spinner-1');
            userService.getNewsFeed(pageSize, startNewsFeedPostId).then(
                function (serverData) {
                    serverData.data.forEach(function (post) {
                        post.date = new Date(post.date);
                        post.author = $scope.checkForEmptyImages(post.author);
                        post.wallOwner = $scope.checkForEmptyImages(post.wallOwner);
                        post.comments.forEach(function (comment) {
                            comment.date = new Date(comment.date);
                            comment.author = $scope.checkForEmptyImages(comment.author);
                        })
                    });

                    $scope.busy = false;
                    $scope.newsFeed = $scope.newsFeed.concat(serverData.data);
                    if($scope.newsFeed.length > 0){
                        startNewsFeedPostId = $scope.newsFeed[$scope.newsFeed.length - 1].id;
                    }

                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show news feed. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getUserWallPage =  function () {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;

            usSpinnerService.spin('spinner-1');
            postService.getWallPosts($routeParams.username, pageSize, startWallPagePostId).then(
                function (postsData) {
                    postsData.data.forEach(function (post) {
                        post.date = new Date(post.date);
                        post.author = $scope.checkForEmptyImages(post.author);
                        post.comments.forEach(function (comment) {
                            comment.date = new Date(comment.date);
                            comment.author = $scope.checkForEmptyImages(comment.author);
                        })
                    });

                    $scope.busy = false;
                    $scope.wallPosts = $scope.wallPosts.concat(postsData.data);
                    if($scope.wallPosts.length > 0){
                        startWallPagePostId = $scope.wallPosts[$scope.wallPosts.length - 1].id;
                    }

                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Error while loading posts' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };
    });