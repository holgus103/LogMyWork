
var from;
var to;

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
                    from: from.data().date,
                    to: to.data().date

                },
            success: function (data) {


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

});

