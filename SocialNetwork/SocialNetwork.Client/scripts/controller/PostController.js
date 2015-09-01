app.controller('PostController',
    function ($scope, $rootScope, postService, notifyService, $routeParams, $localStorage, usSpinnerService, authenticationService) {
        if (authenticationService.isLoggedIn()) {
            $scope.busy = false;
        }

        $scope.addNewPost = function (postContent) {
            usSpinnerService.spin('spinner-1');
            postService.addNewPost(postContent, $routeParams.username).then(
                function (serverData) {
                    $scope.postContent = '';
                    $scope.wallPosts.unshift(serverData.data);
                    notifyService.showInfo('Successfully added new post');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to add new post' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.editPost = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.editPost(post.editedPostContent, post.id).then(
                function () {
                    post.postContent = post.editedPostContent;
                    post.editing = false;
                    notifyService.showInfo('Your post has been successfully edited');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to edit post' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.deletePost = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.deletePost(post.id).then(
                function () {
                    var deletedPostIndex =  $scope.wallPosts.indexOf(post);
                    $scope.wallPosts.splice(deletedPostIndex, 1);
                    notifyService.showInfo('The post has been successfully removed');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to remove this post' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.addNewComment = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.addNewComment(post.commentContent, post.id).then(
                function (serverData) {
                    post.commentContent  = "";
                    post.comments.unshift(serverData.data);
                    post.totalCommentsCount++;
                    notifyService.showInfo('Successfully added new comment');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to add new comment. ' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.editComment = function (comment, post) {
            usSpinnerService.spin('spinner-1');
            postService.editComment(comment.editedCommentContent, comment.id, post.id).then(
                function () {
                    comment.commentContent = comment.editedCommentContent;
                    comment.editing = false;
                    notifyService.showInfo('Your comment has been successfully edited');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to edit comment' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.deleteComment = function (comment, post) {
            usSpinnerService.spin('spinner-1');
            postService.deleteComment(comment.id, post.id).then(
                function () {
                    var deletedCommentIndex =  post.comments.indexOf(comment);
                    post.comments.splice(deletedCommentIndex, 1);
                    post.totalCommentsCount--;
                    notifyService.showInfo('Your comment has been successfully removed');
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to remove this comment' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.likePost = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.likePost(post.id).then(
                function () {
                    post.liked = true;
                    post.likesCount++;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to like this post' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.unlikePost = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.unlikePost(post.id).then(
                function () {
                    post.liked = false;
                    post.likesCount--;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to like this comment' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.likeComment = function (comment, postId) {
            usSpinnerService.spin('spinner-1');
            postService.likeComment(comment.id, postId).then(
                function () {
                    comment.liked = true;
                    comment.likesCount++;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to like this comment' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.unlikeComment = function (comment, postId) {
            usSpinnerService.spin('spinner-1');
            postService.unlikeComment(comment.id, postId).then(
                function () {
                    comment.liked = false;
                    comment.likesCount--;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to unlike this comment' + error.data.message);
                    usSpinnerService.stop('spinner-1');
                }
            )
        };

        $scope.getPostComments = function (post) {
            usSpinnerService.spin('spinner-1');
            postService.getPostComments(post.id).then(
                function (commentsData) {
                    post.comments = commentsData.data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError('Unable to show all comments of this post' + error.data.message);
                    usSpinnerService.stop('spinner-1');
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
    });