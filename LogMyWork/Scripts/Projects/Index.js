$("[status]").click(function () {
    sendAjaxData(
        {
            projectID: $(this).attr("projectID"),
            status: $(this).attr("status")
        },
        "/Projects/UpdateStatus",
        function (data) {

        }
        )
})