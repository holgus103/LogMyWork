$("[status]").click(function () {
    var caller = $(this);
    var data = {
        id: caller.attr("projectID"),
        status: caller.attr("status")
    }
    sendAjaxData(
        data,
        "/Projects/UpdateStatus",
        function (response) {
            if (response == "1") {
                // change to awaiting feedback
                if (data.status == "AwaitingFeedback") {
                    caller.html("Mark as finished");
                    caller.attr("status", "Completed");
                }
                    // change to completed
                else if (data.status == "Completed") {
                    caller.html("Project closed");
                    caller.off("click");
                }
            }

        });
})