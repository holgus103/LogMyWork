
var from;
var to;
var projectSelect;
var userSelect;
var taskSelect;


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

function applyStaticFilter() {
    var projectID = $(this).attr("ProjectID");
    var taskID = $(this).attr("TaskID");
    var toVal = $(this).attr("To");
    var fromVal = $(this).attr("From");
    var userID = $(this).attr("UserID");
    if (projectID > 0) {
        projectSelect.val(projectID);
    }
    else {
        projectSelect.val(null);
    }

    if (taskID > 0) {
        taskSelect.val(taskID)
    }
    else {
        taskSelect.val(null);
    }

    if (userID > 0) {
        userSelect.val(userID);
    }
    else {
        userSelect.val(null);
    }

    if (toVal > 0) {
        to.find("input").val(moment.unix(toVal).format("MM/DD/YYYY"))
    }
    else {
        to.find("input").val(null);
    }
    if (fromVal > 0) {
        from.find("input").val(moment.unix(fromVal).format("MM/DD/YYYY"))
    }
    else {
        from.find("input").val(null);
    }
    projectSelect.change();

}
$(document).ready(function () {
    new TabbedView($("#TabbedMenu"), $("#TabbedView"));
    from = $('#datetimepickerFrom');
    to = $('#datetimepickerTo');
    projectSelect = $("#Projects_0__ProjectID");
    taskSelect = $("#Tasks_0__TaskID");
    userSelect = $("#Users_0__Id");
    from.datetimepicker();
    to.datetimepicker();
    $("[tabID=StaticFilters] label").click(applyStaticFilter);
    to.on("dp.change", onDateChanged);
    from.on("dp.change", onDateChanged);
    projectSelect.change(onDateChanged);
    taskSelect.change(onDateChanged);
    userSelect.change(onDateChanged);
    //adjustTimeZones();
});


