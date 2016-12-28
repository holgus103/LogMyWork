/*
callback(data, lastServedElement, lastUsedStatus, lastUsedID)
*/
function StatusUpdater(idFieldName, url, elements, callback) {
    this.lastServedElement;
    this.lastUsedID;
    this.lastUsedStatus;
    function sendStatusUpdate() {
        var headers = Array();
        headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();        
        lastServedElement = $(this)
        lastUsedID = $(this).attr(idFieldName);
        lastUsedStatus = $(this).attr("status");
        $.ajax(
            {
                url: url,
                headers: headers,
                method: "POST",
                data:
                    {
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                        id: lastUsedID,
                        status: lastUsedStatus
                    },
                success: function (data) {
                    callback && callback(data, lastServedElement, lastUsedStatus, lastUsedID);
                }

            }
        );
    }
    $(elements).click(sendStatusUpdate);
}
