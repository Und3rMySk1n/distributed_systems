var form = $('#valuesEnterForm');
form.submit(function(event){
    event.preventDefault();
    $(this).find('.submit_button_container').addClass('loading');

    $.ajax({
        method: "POST",
        url: $(this).attr('action'),
        data: $(this).serialize()
    })
        .done(function() {
            window.location = "/result/";
        });
});