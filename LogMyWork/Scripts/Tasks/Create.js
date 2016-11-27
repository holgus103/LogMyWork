$(document).ready(function () {
    $("#Deadline").datetimepicker();
    RepeatableControl(
        function (i) {
            return "Files_" + i;
        },
        function (i) {
            return "Files[" + i + "]";
        },
        "input[type=file]",
        "input[type=file]",
        ".form-group",
        null
    )
})



