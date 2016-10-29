
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
