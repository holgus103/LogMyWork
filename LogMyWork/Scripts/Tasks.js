
function createTask(input) {
    if (input > 0) {
        var taskName = $("#Name").val();
        $(taskNodeTemplate.replace(/TASK_NAME/g, taskName).replace(/TASK_ID/g, input)).insertBefore("#AddTaskPnl");
        }
}

