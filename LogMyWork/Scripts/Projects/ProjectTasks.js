﻿
$(document).ready(function () {
    $("[status][taskid]").click(sendStatusUpdate);
});

function sendStatusUpdate() {
    var headers = Array();
    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();

    $.ajax(
        {
            url: "/Tasks/UpdateStatus",
            headers: headers,
            method: "POST",
            data:
                {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    id: $(this).attr("taskID"),
                    status: $(this).attr("status")

                },
            success: function (data) {
                location.reload();
            }

        }
    );
}

