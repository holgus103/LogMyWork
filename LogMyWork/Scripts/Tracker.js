var isTracking;
function createTimeEntry(node) {
    var taskId = node.attr("taskId");
    var headers = new Array();

    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();

    $.ajax(
        {
            url: "/TimeEntries/Create",
            headers: headers,
            method: "POST",
            data: 
                {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    ParentTaskId: taskId
            }


        }
    )
        .success(
            function (data) {
                isTracking = !isTracking;
                if (data == "1") {
                    console.log("hooray");
                    if (isTracking)
                        node.html("Stop");
                    else
                        node.html("Track");
                }
                else {
                    console.log("fail");
                }
            }
        );
}
