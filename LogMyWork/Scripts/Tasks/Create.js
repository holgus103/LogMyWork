﻿
var users;
$(document).ready(function () {
    $("#Deadline").datetimepicker();
    //loadUsers();
    var control = new RepeatableControl(
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
    );

    users = new RepeatableControl(
    function (i) {
        return "Users_" + i + "__Id";
    },
    function (i) {
        return "Users[" + i + "].Id";
    },
    "#Users select",
    "select",
    ".form-group",
    function () {
        users.updateDropDowns();
    }
    )
    //var control = new RepeatableControl()
    $("#ParentProjectID").change(
       
    )

    
})



function loadUsers() {
    sendAjax(
        {
            ProjectID: $("#ParentProjectID")
        },
        "/Projects/GetUsersForProject",
        function (data) {
            users.resetControl();
            $("#Users_0__Id").html(data);

        }
        );
}