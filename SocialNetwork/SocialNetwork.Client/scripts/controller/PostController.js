app.controller('PostController',
    function ($scope, $rootScope, postService, notifyService, $routeParams, $localStorage) {
        $scope.getUserWallPage =  function () {
            postService.getWallPosts($routeParams.username).then(
                function (postsData) {
                    postsData.data.forEach(function (post) {
                        post.author = $scope.checkForEmptyImages(post.author);
                        post.comments.forEach(function (comment) {
                            comment.author = $scope.checkForEmptyImages(comment.author);
                        })
                    });

                    $scope.wallPosts = postsData.data;
                },
                function (error) {
                    notifyService.showError('Error while loading posts' + error.data.message)
                }
            )
        };

        $scope.addNewPost = function (postContent) {
            postService.addNewPost(postContent, $routeParams.username).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('Successfully added new post')
                },
                function (error) {
                    notifyService.showError('Unable to add new post' + error.data.message)
                }
            )
        };

        $scope.editPost = function (postContent, postId) {
            postService.editPost(postContent, postId).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('Your post has been successfully edited')
                },
                function (error) {
                    notifyService.showError('Unable to edit post' + error.data.message)
                }
            )
        };

        $scope.deletePost = function (postId) {
            postService.deletePost(postId).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('The post has been successfully removed')
                },
                function (error) {
                    notifyService.showError('Unable to remove this post' + error.data.message)
                }
            )
        };

        $scope.addNewComment = function (commentContent, commentId) {
            postService.addNewComment(commentContent, commentId).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('Successfully added new comment')
                },
                function (error) {
                    notifyService.showError('Unable to add new comment' + error.data.message)
                }
            )
        };

        $scope.editComment = function (commentContent, commentId, postId) {
            postService.editComment(commentContent, commentId, postId).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('Your comment has been successfully edited')
                },
                function (error) {
                    notifyService.showError('Unable to edit comment' + error.data.message)
                }
            )
        };

        $scope.deleteComment = function (commentId, postId) {
            postService.deleteComment(commentId, postId).then(
                function () {
                    $scope.getUserWallPage();
                    notifyService.showInfo('Your comment has been successfully removed')
                },
                function (error) {
                    notifyService.showError('Unable to remove this comment' + error.data.message)
                }
            )
        };

        $scope.likePost = function (postId) {
            postService.likePost(postId).then(
                function () {
                    $scope.getUserWallPage();
                },
                function (error) {
                    notifyService.showError('Unable to like this post' + error.data.message)
                }
            )
        };

        $scope.unlikePost = function (postId) {
            postService.unlikePost(postId).then(
                function () {
                    $scope.getUserWallPage();
                },
                function (error) {
                    notifyService.showError('Unable to like this comment' + error.data.message)
                }
            )
        };

        $scope.likeComment = function (commentId, postId) {
            postService.likeComment(commentId, postId).then(
                function () {
                    $scope.getUserWallPage();
                },
                function (error) {
                    notifyService.showError('Unable to like this comment' + error.data.message)
                }
            )
        };

        $scope.unlikeComment = function (commentId, postId) {
            postService.unlikeComment(commentId, postId).then(
                function () {
                    $scope.getUserWallPage();
                },
                function (error) {
                    notifyService.showError('Unable to unlike this comment' + error.data.message)
                }
            )
        };

        $scope.getPostComments = function (post) {
            postService.getPostComments(post.id).then(
                function (commentsData) {
                    post.comments = commentsData.data;
                },
                function (error) {
                    notifyService.showError('Unable to show all comments of this post' + error.data.message)
                }
            )
        };

        $scope.hidePostComments = function (post) {
            post.comments = post.comments.slice(0,3);
        };

        $scope.isEditable = function(author) {
            return author.username === $localStorage.currentUser.userName;
        };

        $scope.isDeletable = function(post, commentAuthor) {
            if (commentAuthor) {
                return post.author.username === $localStorage.currentUser.userName ||
                    commentAuthor.username === $localStorage.currentUser.userName;
            }

            return post.author.username === $localStorage.currentUser.userName ||
                post.wallOwner.username === $localStorage.currentUser.userName ;
        };

        $scope.isAuthorized = function (post) {
            if (post) {
                return post.author.isFriend || post.wallOwner.isFriend || post.author.username === $localStorage.currentUser.userName;
            }

            return $routeParams.username === $localStorage.currentUser.userName;
        };

        //if ($routeParams.username == undefined) {
        //} else{
        //    getUserWallPage();
        //}
    });