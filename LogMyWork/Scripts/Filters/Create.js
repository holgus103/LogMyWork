var projectSelect;
var userSelect;
var taskSelect;
$(document).ready(function () {
    projectSelect = $("#ProjectID");
    userSelect = $("#UserID");
    taskSelect = $("#TaskID");

    $("#From").datetimepicker();
    $("#To").datetimepicker();
    UpdateDates();
    $("#CreateForm").submit(function () {
        var to = $("#To").val();
        var from = $("#From").val();
        $("#ToHidden").val(moment(to).format("X"));
        $("#FromHidden").val(moment(from).format("X"));
    })
    projectSelect.change(function () {
        refreshTasks();
        refreshUsers();
    });
    userSelect.change(function () {
        refreshProjects();
        refreshTasks();
    });
    taskSelect.change(function () {
        refreshUsers();
        refreshProjects();
    });
});

function refreshProjects() {
    sendAjax(
        {
            userID: userSelect,
            taskID: taskSelect
        },
        "/Projects/GetProjects",
        function (data) {
            var val = projectSelect.val();
            projectSelect.html(data);
            projectSelect.val(val);
        })
}

function refreshTasks() {
    sendAjax(
    {
        projectID: projectSelect,
        userID: userSelect
    },
    "/Tasks/GetTasks",
    function (data) {
        var val = taskSelect.val();
        taskSelect.html(data);
        taskSelect.val(val);
    })
}

function refreshUsers() {
    sendAjax(
    {
        projectID: projectSelect,
        taskID: taskSelect
    },
    "/Account/GetUsers",
    function (data) {
        var val = userSelect.val();
        userSelect.html(data);
        userSelect.val(val);
    })
}

function UpdateDates() {
    var to = moment(1000 * parseInt($("#ToHidden").val())).format("L LT");
    var from = moment(1000 * parseInt($("#FromHidden").val())).format("L LT");
    if (to > 0) {
        $("#To").val(to);
    }
    if (from > 0) {
        $("#From").val(from);
    }
}