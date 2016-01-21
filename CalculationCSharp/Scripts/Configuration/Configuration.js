var $TABLE = $('#table');
var $BTN = $('#export-btn');
var $BTNSave = $('#save-btn');
var $EXPORT = $('#export');
var $MODAL = $('#modal-container');

$('.table-add').click(function () {
    var $row = $(this).parents('tr');
    var $clone = $row.clone(true);
    $TABLE.find('table').append($clone);
});

$('.table-remove').click(function () {
    $(this).parents('tr').detach();
});

$('.table-up').click(function () {
    var $row = $(this).parents('tr');
    if ($row.index() === 1) return; // Don't go above the header
    $row.prev().before($row.get(0));
});

$('.table-down').click(function () {
    var $row = $(this).parents('tr');
    $row.next().after($row.get(0));
});

// A few jQuery helpers for exporting only
jQuery.fn.pop = [].pop;
jQuery.fn.shift = [].shift;

//Build and Submit JSON
$BTN.click(function () {
    var $rows = $TABLE.find('tr:not(:hidden)');
    var $input = $rows.find("input");
    var headers = [];
    var data = [];
    // Get the headers (add special header logic here)
    $($rows.shift()).find('th:not(:empty)').each(function () {
        headers.push($(this).text().toLowerCase());
    });

    // Turn all existing rows into a loopable array
    $rows.each(function () {
        var $td = $(this).find('tr :input');
        var h = {};

        var features = {};    // Create empty javascript object
        $input.each(function () {           // Iterate over inputs
            features[$(this).attr('name')] = $(this).val();           
        });
        data.push(features);// Add each to features object
    });

    var json = JSON.stringify(data); // Stringify to create json object (requires json2 library)

    $.ajax({

        type: 'POST',
        dataType: 'text',
        url: "Index",
        data: "jsonOfLog=" + json,
        success: function (data) {
           $("#table").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console && console.log("request failed");
        },

        processData: false,
        async: false
    });
});

//Autocomplete
$("input[name=Name]").focus(function () {
    var values = $("input[id='task']")
          .map(function () { return $(this).val(); }).get();
    
    $('input[name=Name]').autocomplete({
        source: values
    })

});
