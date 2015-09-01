app.controller('MainController', ['$scope', 'authenticationService', function ($scope, authenticationService) {
    $scope.isLogged = authenticationService.isLoggedIn();

    $scope.checkForEmptyImages = function (usersData) {
        if (!usersData.profileImageData) {
            if (usersData.gender == 'Female' || usersData.gender == 2) {
                usersData.profileImageData = 'resourses/images/default-female.jpg';
            } else{
                usersData.profileImageData = 'resourses/images/default-male.png';
            }
        }

        if (!usersData.coverImageData) {
            usersData.coverImageData = 'resourses/images/default-cover.jpg'
        }

        return usersData;
    };
}]);