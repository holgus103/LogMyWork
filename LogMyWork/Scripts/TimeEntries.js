
var from;
var to;

function adjustTimeZones() {
    $("#TimeEntriesTable td[adjustment=timezone]").each(
        function (index, element) {
            var timestamp = $(element).attr("timestamp");
            var date = new Date(0);
            date.setUTCSeconds(timestamp);
            $(element).html(date.toLocaleString());
        }
    );
}


function onDateChanged() {
    var headers = Array();
    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();

    $.ajax(
        {
            url: "/TimeEntries/GetFilteredValues",
            headers: headers,
            method: "POST",
            data:
                {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    from: (new Date(from.data().date)).getTime() / 1000,
                    to: (new Date(to.data().date)).getTime() / 1000

                },
            success: function (data) {
                $("#TimeEntriesTable tbody tr:has(td)").remove();
                $(data).insertAfter("#TimeEntriesTable tr:last");
                adjustTimeZones();

            }

        }
    );
}

$(document).ready(function () {
    from = $('#datetimepickerFrom');
    to = $('#datetimepickerTo');

    from.datetimepicker();
    to.datetimepicker();

    to.on("dp.change", onDateChanged);
    from.on("dp.change", onDateChanged);
    adjustTimeZones();
});


