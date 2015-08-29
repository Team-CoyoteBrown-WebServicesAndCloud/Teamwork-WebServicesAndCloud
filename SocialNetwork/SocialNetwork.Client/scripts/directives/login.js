app.directive('login', function () {
    return {
        controller: 'MainController',
        restrict: 'E',
        templateUrl: 'templates/home',
        replace: true
    }
});