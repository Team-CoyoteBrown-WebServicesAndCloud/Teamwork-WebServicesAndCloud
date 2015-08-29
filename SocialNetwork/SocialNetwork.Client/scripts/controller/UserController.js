app.controller('UserController',
    function ($scope, userService, notifyService, $routeParams, $localStorage) {
        $scope.getUserFullData = function getUserFullData() {
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
                },
                function (error) {
                    notifyService.showError('Unable to show user data' + error.data.message)
                }
            )
        };

        $scope.getUserPreviewData = function (username) {
            userService.getUserPreviewData(username).then(
                function (userData) {
                    $scope.userPreviewData = $scope.checkForEmptyImages(userData.data);
                },
                function (error) {
                    notifyService.showError('Unable to show user data' + error.data.message)
                }
            )
        };

        $scope.getOwnFriendsPreview = function () {
            userService.getOwnFriendsPreview().then(
                function (friendsData) {
                    friendsData.data.friends.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.ownFriendsPreview = friendsData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show your friends' + error.data.message)
                }
            )
        };

        $scope.getOwnFriendsDetailed = function () {
            userService.getOwnFriendsDetailed().then(
                function (friendsData) {
                    friendsData.data.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.ownFriendsDetailed = friendsData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show your friends detailed. ' + error.data.message)
                }
            )
        };

        $scope.getFriendFriendsPreview = function () {
            userService.getFriendFriendsPreview($routeParams.username).then(
                function (friendsData) {
                    friendsData.data.friends.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.friendFriendsPreview = friendsData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show friend friends. ' + error.data.message)
                }
            )
        };

        $scope.getFriendFriendsDetailed = function () {
            userService.getFriendFriendsDetailed($routeParams.username).then(
                function (friendsData) {
                    friendsData.data.forEach(function (friend) {
                        $scope.checkForEmptyImages(friend);
                    });

                    $scope.friendFriendsDetailed = friendsData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show friend friends detailed. ' + error.data.message)
                }
            )
        };

        $scope.getFriendRequests = function () {
            userService.getFriendRequests().then(
                function (friendRequestData) {
                    friendRequestData.data.forEach(function (requestData) {
                        $scope.checkForEmptyImages(requestData.user);
                    });
                    $scope.friendRequests = friendRequestData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show friend requests. ' + error.data.message)
                });
        };

        $scope.sendFriendRequest = function () {
            userService.sendFriendRequest($routeParams.username).then(
                function () {
                    notifyService.showInfo('Friend request has been successfully sent');
                    $scope.getUserFullData();
                },
                function (error) {
                    notifyService.showError('Unable to send friend request. ' + error.data.message)
                }
            )
        };

        $scope.approveFriendRequest = function (id) {
            userService.approveFriendRequest(id).then(
                function () {
                    $scope.getFriendRequests();
                    $scope.getOwnFriendsPreview();
                    notifyService.showInfo('Friend request from has been approved')
                },
                function (error) {
                    notifyService.showError('Unable to approve friend request. ' + error.data.message)
                }
            )
        };

        $scope.rejectFriendRequest = function (id) {
            userService.rejectFriendRequest(id).then(
                function () {
                    $scope.getFriendRequests();
                    notifyService.showInfo('Friend request from has been rejected')
                },
                function (error) {
                    notifyService.showError('Unable to reject friend request. ' + error.data.message)
                }
            )
        };

        $scope.searchUsersByName = function (term) {
            if (term.trim().length > 0) {
                userService.searchUsers(term).then(
                    function (serverData) {
                        serverData.data.forEach(function (user) {
                            $scope.checkForEmptyImages(user);
                        });

                        $scope.searchResult = serverData.data;
                    },
                    function (error) {
                        notifyService.showError('Unable to search with the given terms. ' + error.data.message);
                    });
            }
        };

        //if ($routeParams.username == undefined) {
        //    getOwnFriendsPreview();
        //} else{
        //    getFriendFriendsPreview();
        //}
    });