var RESULT_URL = '/php/get_poem.php';

var container = $('#resultContainer');

$.ajax({
    method: "GET",
    url: RESULT_URL,
    data: {id: "0"}
})
    .done(function (data) {
        $('.form_title').addClass('final');
        container.html("<p>" + data + "</p>");
    });

