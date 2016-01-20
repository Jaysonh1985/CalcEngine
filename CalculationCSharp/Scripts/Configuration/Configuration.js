var $TABLE = $('#table');
var $BTN = $('#export-btn');
var $BTNSave = $('#save-btn');
var $EXPORT = $('#export');

$('.table-add').click(function () {
    var $clone = $TABLE.find('tr.hide').clone(true).removeClass('hide table-line');
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

//$BTN.click(function () {
//    var $rows = $TABLE.find('tr:not(:hidden)');
//    var $input = $rows.find("input");
//    var headers = [];
//    var data = [];
//    // Get the headers (add special header logic here)
//    $($rows.shift()).find('th:not(:empty)').each(function () {
//        headers.push($(this).text().toLowerCase());
//    });

//    // Turn all existing rows into a loopable array
//    $rows.each(function () {
//        var $td = $(this).find('tr :input');
//        var h = {};

//        var features = {};    // Create empty javascript object
//        $input.each(function () {           // Iterate over inputs
//            features[$(this).attr('name')] = $(this).val();           
//        });
//        data.push(features);// Add each to features object
//    });

//    var json = JSON.stringify(data); // Stringify to create json object (requires json2 library)

//    $.ajax({

//        type: 'POST',
//        dataType: 'text',
//        url: "Index",
//        data: "jsonOfLog=" + json,
//        success: function (data) {
//           $("#table").html(data);
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            console && console.log("request failed");
//        },

//        processData: false,
//        async: false
//    });
//});

$BTNSave.click(function(){

})

//$(document).ready(function () {
//    //Helper function to keep table row from collapsing when being sorted
//    var fixHelperModified = function (e, tr) {
//        var $originals = tr.children();
//        var $helper = tr.clone();
//        $helper.children().each(function (index) {
//            $(this).width($originals.eq(index).width())
//        });
//        return $helper;
//    };

//    //Make diagnosis table sortable
//    $("#table tbody").sortable({
//        helper: fixHelperModified,
//        stop: function (event, ui) { renumber_table('#table') }
//    }).disableSelection();

//    //Delete button in table rows
//    $('table-editable').on('click', '.btn-delete', function () {
//        tableID = '#' + $(this).closest('table-editable').attr('id');
//        r = confirm('Delete this item?');
//        if (r) {
//            $(this).closest('tr').remove();
//            renumber_table(tableID);
//        }
//    });
//});

////Renumber table rows
//function renumber_table(tableID) {
//    $(tableID + " tr").each(function () {
//        count = $(this).parent().children().index($(this)) + 1;
//        $(this).find('.priority').html(count);
//    });
//}

$(function()
{
    var arrLinks = $('#table td:input').map(function () {
        return $(this).val();
    }).get();
    $('input[name=Name]').autocomplete({
        source: arrLinks,
        create: function () {
            $(this).data("item.autocomplete", item)._renderItem = arrLinks;
        }
    })
})