var login = (function () {
    var userLogin,
        displayError;

    userLogin = function () {
        const url = 'ProjectServices.asmx/ValidateUser'; 
        const username = $('#username').val(); 
        const password = $('#password').val(); 

        if (username === '' ||
            password === '') {
            genError.displayError('Please enter Username and Password.');
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
                if (data.d.UserID !== null &&
                    data.d.UserID !== '' &&
                    data.d.UserID !== undefined &&
                    data.d.UserID !== 0) {
                    sessionStorage['UserID'] = data.d.UserID;
                    sessionStorage['UserName'] = data.d.FirstName + ' ' + data.d.LastName; 
                    window.open('Discussion.html', '_self'); 
                } else {
                    genError.displayError('Incorrect Username or Password.');
                }
            },
            error: function (err) {
                genError.displayError('Unexpected error occurred, please try again later.');
            }
        });
    }

    return {
        userLogin: userLogin,
    }

})();