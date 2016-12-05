var projectSelect;
var userSelect;
var taskSelect;
$(document).ready(function () {
    projectSelect = $("#ProjectID");
    userSelect = $("#TaskID");
    taskSelect = $("#UserID");

    $("#From").datetimepicker();
    $("#To").datetimepicker();
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
            projectSelect.html(data);
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
        taskSelect.html(data);
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
        userSelect.html(data);
    })
}