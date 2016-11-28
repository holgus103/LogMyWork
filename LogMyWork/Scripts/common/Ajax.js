function sendAjax(controlls, url, callback) {
    var data = new Object();
    for (var prop in controlls) {
        data[prop] = controlls[prop].val();
    }
    sendAjaxData(data, url, callback);
}

function sendAjaxData(data, url, callback) {
    var headers = new Array();
    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();
    data.__RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();
    $.ajax(
        {
            headers: headers,
            url: url,
            method: 'POST',
            data: data,
            success: function (data) {
                callback && callback(data);
            }

        }
    )
}