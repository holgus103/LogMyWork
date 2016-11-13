
function showTaskAdditionPanel() {
    var panel = $("#pnlAddTask");
    if (panel.hasClass("hidden"))
        panel.removeClass("hidden");
    else
        panel.addClass("hidden");
}

var controlls =
    {
        ParentProjectID: $("#ParentProjectID"),
        Name: $("#Name")
    };

var url = "/Tasks/Create";

function createTask() {
    sendAjax(controlls, url, function (input) {
        if (input > 0) {
            $(asd.replace(/TASK_NAME/g, data.Name).replace(/TASK_ID/g, input)).insertBefore("#pnlAddTask");
        }
    });
}

