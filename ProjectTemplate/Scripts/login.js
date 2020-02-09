var login = (function () {
    var userLogin,
        displayError;

    userLogin = function () {
        const url = 'ProjectServices.asmx/ValidateUser'; 
        const username = $('#username').val(); 
        const password = $('#password').val(); 

        if (username === '' ||
            password === '') {
            displayError('Please enter Username and Password.');
            return false; 
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                username: username,
                password: password
            }), 
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d === 'Success!') {
                    window.open('Discussion.html', '_self'); 
                } else {
                    displayError('Incorect Username or Password.');
                }
            },
            error: function (err) {
                displayError('Unexpected error occurred, please try again later.');
            }
        });
    }

    displayError = function (msg) {
        $(".error-container").show();
        $(".error-container").html('');
        $(".error-container").html('<p>&#8226; ' + msg + '</p>');
        $(".logincontainer").css({ "height": 325 });
    }

    return {
        userLogin: userLogin,
        displayError: displayError
    }

})();