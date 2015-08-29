app.filter('genderFilter', function () {
    return function(input) {
        switch (input) {
            case 0:
                return 'Other';
                break;
            case 1:
                return 'Male';
                break;
            case 2:
                return 'Female';
                break;
        }
    };
});