$(document).ready(function () {
    $("#Users_0__Id").change(handleSelectChange);
});

function createTask(input) {
    if (input > 0) {
        var taskName = $("#Name").val();
        $(taskNodeTemplate.replace(/TASK_NAME/g, taskName).replace(/TASK_ID/g, input)).insertBefore("#AddTaskPnl");
        }
}

function handleSelectChange() {
    if ($(this).val() == "") return;
    var arr = $("#AddTaskPnl  .form-group")
    $($("#AddTaskPnl  .form-group")[1]).clone().insertAfter(arr[arr.length - 2])
    $("select").last().change(handleSelectChange);
}

