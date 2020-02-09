var discussion = (function () {
    var submit; 

    submit = function () {
        const url = 'ProjectServices.asmx/InsertPost'; 
        const postText = $('#txtPost').val(); 

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({
                post: postText
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                $('#postmodal').modal('hide');
                $(".card-container").html('<div class="card">' +
                    '<div class="card-block">' + 
                    '<h1 class="card-text square-card float-left">SD</h1>' +
                    '<h4 class="card-title">Employee Post</h4>' +
                    '<p class="card-text">' + postText + '</p>' + 
                    '</div>' + 
                    '<div class="card-footer">' + 
                    '<p class="card-text text-right">Reply</p>' + 
                    '</div>'
                    )
            },
            error: function (err) {

            }
        });
    }


    return {
        submit: submit
    }
})(); 

$("#postmodal").on('hide.bs.modal', function () {
    $('#txtPost').val(''); 
});