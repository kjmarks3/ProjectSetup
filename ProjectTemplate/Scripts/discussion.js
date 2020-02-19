var discussion = (function () {
    var submit,
        getAllPosts,
        logout,
        getAllPostTopics,
        getRandomPostQuestion,
        getAllPostQuestionResponses,
        openQuestionResponse,
        submitQuestionResponse,
        resetResponseRadioButton,
        getCurrentUser,
        displayCurrentUserStats; 

    submit = function () {
        const url = 'ProjectServices.asmx/InsertPost'; 
        const postText = $('#txtPost').val(); 
        const userId = sessionStorage['UserID']; 
        const topicText = $('#topic-select option:selected').text();
        const isAnonymous = $('#chkIsAnonyous').is(':checked');

        if (postText === '' ||
            postText === null ||
            postText === undefined) {
            genError.displayError("Please fill out all required fields."); 
            return false; 
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                userId: userId,
                post: postText,
                pointValue: 5,
                topic: topicText,
                anonymous: isAnonymous
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                $('#postmodal').modal('hide');
                openQuestionResponse(); 
                getAllPosts();
                getCurrentUser(); 
            },
            error: function (err) {

            }
        });
    }

    getAllPosts = function () {
        const url = 'ProjectServices.asmx/ViewPosts'; 

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $(".card-container").html(''); 
                    $.each(data.d, function (index, item) {
                        var initials = '?';
                        if ((item.Anonymous === '' ||
                            item.Anonymous === 'False') &&
                            item.FirstName &&
                            item.LastName) {
                            var firstNameChar = item.FirstName.substring(0, 1);
                            var lastNameChar = item.LastName.substring(0, 1);
                            initials = firstNameChar + lastNameChar; 
                        } 
                        var footerHtml = '';
                        if (item.Anonymous === '' || item.Anonymous === 'False') {
                            footerHtml = '<p class="card-text">' + '<span class="card-text text-left">Posted by: ' + item.FirstName + ' ' + item.LastName + '(' + item.UserPointTotal + ')' + ' ' + item.PostTime + '</span>' + '<span class="card-text" id="card-reply">Reply</span>' + '</p>';
                        } else {
                            footerHtml = '<p class="card-text">' + '<span class="card-text text-left">Posted by: Anonymous ' + item.PostTime + '</span>' + '<span class="card-text" id="card-reply">Reply</span>' + '</p>';
                        }
                        $(".card-container").append('<div id='+ item.PostId +' class="card">' +
                            '<div class="card-block">' +
                            '<h1 class="card-text square-card float-left">' + initials + '</h1>' +
                            //'<h4 class="card-title">Employee Post</h4>' +
                            '<p class="card-text">' + item.Post + '</p>' +
                            '</div>' +
                            '<div class="card-footer">' +
                            footerHtml +
                            '</div>'
                        )
                    });
                }
            }, 
            error: function (err) {

            }
        });
    }

    logout = function () {
        sessionStorage.clear(); 
        window.open('/index.html', '_self');
    }

    getAllPostTopics = function () {
        const url = 'ProjectServices.asmx/GetPostTopics'; 

        $.ajax({
            url: url, 
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $.each(data.d, function (index, item) {
                        $('#topic-select').append($('<option>').val(item.TopicId).text(item.Topic));
                    });
                }
                
            },
            error: function (err) {

            }
        }); 
    }

    getRandomPostQuestion = function () {
        const url = 'ProjectServices.asmx/GetNextQuestion';
        const userId = sessionStorage['UserID']; 

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                userId: userId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#lblQuestion').text(data.d.QuestionText);
                    sessionStorage['QuestionId'] = data.d.QuestionId; 
                    getAllPostQuestionResponses(data.d.Responses); 
                }
            },
            error: function (err) {

            }
        });
    }

    getAllPostQuestionResponses = function (responses) {
        $('.radio-group').html(''); 
        $.each(responses, function (index, item) {
            $('.radio-group').append(
                '<input type="radio" id="' + item.ResponseId + '" name="answers" value="' + item.ResponseId + '">' +
                '<label for="' + item.ResponseId + '">' + item.ResponseValue + '</label>');
        });
    }

    openQuestionResponse = function () {
        discussion.getRandomPostQuestion(); 
        resetResponseRadioButton(); 
        $('#questionModal').modal({ backdrop: 'static', keyboard: false })
    }

    submitQuestionResponse = function () {
        const url = 'ProjectServices.asmx/InsertQuestionResponse';
        const userId = sessionStorage['UserID'];
        const questionId = sessionStorage['QuestionId']; 
        const responseId = $('input[name=answers]:checked').val(); 

        if (responseId === undefined) {
            genError.displayError('Please select a response.');
            return false; 
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                userId: userId,
                questionId: questionId,
                responseId: responseId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                $('#questionModal').modal('hide');
                $(".error-container").html('');
                sessionStorage.removeItem('QuestionId'); 
            },
            error: function (err) {

            }
        });
    }

    resetResponseRadioButton = function () {
        $('.radio-group').each(function () {
            $(this).removeAttr('checked');
            $('input[type="radio"]').prop('checked', false);
        });
    }

    getCurrentUser = function () {
        const url = 'ProjectServices.asmx/GetUser';
        const userId = sessionStorage['UserID'];
        var userObject = {}; 

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                userId: userId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    userObject = data.d;
                    console.log(userObject);
                    displayCurrentUserStats(userObject);
                }
            },
            error: function (err) {

            }
        });
    }

    displayCurrentUserStats = function (userStats) {
        if (userStats &&
            userStats !== undefined) {
            const firstName = userStats.FirstName;
            const lastName = userStats.LastName;
            var totalUserPoints = 0; 
            if (userStats.Stats &&
                userStats.Stats !== undefined) {
                totalUserPoints = userStats.Stats.PointTotal; 
            }
            $('#loggedInUser').html(firstName + ' ' + lastName + '(' + totalUserPoints + ')');
        }
    }

    return {
        submit: submit,
        getAllPosts: getAllPosts,
        logout: logout,
        getAllPostTopics: getAllPostTopics,
        getRandomPostQuestion: getRandomPostQuestion,
        getAllPostQuestionResponses: getAllPostQuestionResponses,
        openQuestionResponse: openQuestionResponse,
        submitQuestionResponse: submitQuestionResponse,
        resetResponseRadioButton: resetResponseRadioButton,
        getCurrentUser: getCurrentUser,
        displayCurrentUserStats: displayCurrentUserStats
    }
})(); 

$("#postmodal").on('hide.bs.modal', function () {
    $('#txtPost').val(''); 
    $('.error-container').html('');
    $('#chkIsAnonyous').prop('checked', false);
    $("#topic-select").prop("selectedIndex", 0);
});

$("#questionModal").on('hide.bs.modal', function () {
    $(".error-container").html('');
    discussion.getRandomPostQuestion(); 
});

$(document).ready(function () {
    $('#loggedInUser').html(sessionStorage['UserName']);
    discussion.getCurrentUser();
    discussion.getAllPosts();
    discussion.getAllPostTopics();
    discussion.getRandomPostQuestion(); 
});