var asd = "<dt>Task Name</dt><dd>TASK_NAME <label for='Track' onclick='createTimeEntry($(this))' taskid='TASK_ID'>Track</label> | <a href='/Tasks/Edit/TASK_ID'>Edit</a> | <a href='/Tasks/Details/TASK_ID'>Details</a> | <a href='/Tasks/Delete/TASK_ID'>Delete</a></dd>"
function showTaskAdditionPanel() {
    var panel = $("#pnlAddTask");
    if (panel.hasClass("hidden"))
        panel.removeClass("hidden");
    else
        panel.addClass("hidden");
}

function sendAjax() {
    var headers = new Array();

    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();
    var data = {
        ParentProjectID: $("#ParentProjectID").val(),
        Name: $("#Name").val(),
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
    }
    $.ajax(
        {
            headers: headers,
            url: "/Tasks/Create",
            method: 'POST',
            data: data
        }
    ).success(
        function (input) {
            if (input > 0) {
                $(asd.replace(/TASK_NAME/g, data.Name).replace(/TASK_ID/g, input)).insertBefore("#pnlAddTask");
            }
        }
    );
}
