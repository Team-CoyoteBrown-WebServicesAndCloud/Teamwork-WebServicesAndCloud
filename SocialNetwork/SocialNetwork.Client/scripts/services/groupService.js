'use strict';

app.factory('groupService', function ($http, baseServiceUrl, $localStorage, authenticationService) {
    var groupService = {};

    groupService.createGroup = function (groupData) {
        return $http({
            method: 'POST',
            url: baseServiceUrl + '/groups',
            headers: authenticationService.getHeaders(),
            data: groupData
        })
    };

    return groupService;
});