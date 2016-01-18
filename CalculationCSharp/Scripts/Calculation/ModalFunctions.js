//Create Modal
$(function () {
    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('body').on('click', '.modal-close-btn', function () {
        $('modal-container').modal('hide');
    });

    $('#modal-container').on('hidden.bd.modal', function () {
        $(this).removeData('bs.modal');
    });
});


//Hide buttons on Modal
$(document).ready(function () {
    // Show the Modal on load
    $("#myModal").modal("show");

    // Hide the Modal
    $("#myBtn").click(function () {
        $("#modal-container").modal("hide");
    });
});

//Clear Modal on hide
$('body').on('hidden.bs.modal', '.modal', function () {
    $(this).removeData('bs.modal');
});
