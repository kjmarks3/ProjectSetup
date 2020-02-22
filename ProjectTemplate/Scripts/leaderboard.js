var leaderboard = (function () {
    var getUserStats,
        displayLeaderboard; 

    getUserStats = function () {
        const url = 'ProjectServices.asmx/GetUserStats'; 

        $.ajax({
            url: url, 
            type: 'post', 
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                displayLeaderboard(data.d); 
            },
            error: function (err) {

            }
        });
    }

    displayLeaderboard = function (data) {
        $.each(data, function (index, item) {
            $('#leaderboard-table').append(
                '<tr>' +
                '<td>' + item.Rank + '</td>' + 
                '<td>' + item.FirstName + '</td>' + 
                '<td>' + item.LastName + '</td>' + 
                '<td>' + item.PointTotal + '</td>' + 
                '</tr>'
            );
        });
    }

    return {
        getUserStats: getUserStats,
        displayLeaderboard: displayLeaderboard
    }
})(); 

$(document).ready(function () {
    $('#navbar-container').load('navbar.html');
    $('#loggedInUser').html(sessionStorage['UserName']);
    leaderboard.getUserStats(); 
});