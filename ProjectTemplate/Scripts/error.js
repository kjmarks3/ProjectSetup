var genError = (function () {
    var displayError; 

    displayError = function (msg) {
        $(".error-container").show();
        $(".error-container").html('');
        $(".error-container").html('<p>&#8226; ' + msg + '</p>');
        $(".logincontainer").css({ "height": 325 });
    }

    return {
        displayError: displayError
    }

})();