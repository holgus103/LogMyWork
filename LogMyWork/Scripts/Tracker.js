function createTimeEntry(taskId) {
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
        .done(
            function (data) {
                console.log(data);
            }
        );
}
