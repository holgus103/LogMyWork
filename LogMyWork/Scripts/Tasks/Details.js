var now = new Date();
var startDate = new Date(0);
var endDate = new Date(0);
var progress;
var intervalHandle;
$(document).ready(function () {
    adjustTimeZones();
    startDate.setUTCSeconds(start);
    endDate.setUTCSeconds(deadline);
    var refreshRate = (endDate - startDate) / 100;
    progress = $("#Progress");
    refreshBar();
    intervalHandle = setInterval(refreshBar, refreshRate);
    prepareAjaxDelete();

});

function refreshBar() {
    now = new Date();
    var value = (now - startDate) / (endDate - startDate) * 100;
    if (value <= 100 && value >= 0) {
        progress.css("width", value + "%");
    }
    else {
            progress.css("background-color", "#8E001C");
            clearInterval(intervalHandle);
            progress.css("width", "100%");

    }
}

function prepareAjaxDelete() {
    $("#Files label[attachmentID]").click(function () {
        var node = $(this).closest("div");
        sendAjaxData({ attachmentID: $(this).attr("attachmentID") },
        "/Attachments/DeleteAttachment",
        function (data) {
            if (data == "1") {
                node.remove();
            }
        });
    }

   );
}