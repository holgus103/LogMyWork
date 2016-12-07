
var from;
var to;
var projectSelect;
var userSelect;



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
                    to: (new Date(to.data().date)).getTime() / 1000,
                    projectId: projectSelect.val(),
                    taskId: taskSelect.val(),
                    user: userSelect.val()

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
    projectSelect = $("#Projects_0__ProjectID");
    taskSelect = $("#Tasks_0__TaskID");
    userSelect = $("#Users_0__Id");
    from.datetimepicker();
    to.datetimepicker();

    to.on("dp.change", onDateChanged);
    from.on("dp.change", onDateChanged);
    projectSelect.change(onDateChanged);
    taskSelect.change(onDateChanged);
    userSelect.change(onDateChanged);
    adjustTimeZones();
});


