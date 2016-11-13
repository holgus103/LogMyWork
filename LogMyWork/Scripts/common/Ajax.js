function sendAjax(controlls, url, callback) {
    var headers = new Array();
    var data = new Object();
    for (var prop in controlls) {
        data[prop] = controlls[prop].val();
    }
    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();
    data.__RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();
    $.ajax(
        {
            headers: headers,
            url: url,
            method: 'POST',
            data: data
        }
    ).success(
        function (data) {
            callback(data);
        }
    );
}