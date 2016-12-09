$(document).ready(adjustTimeZones);
    
function adjustTimeZones() {
    $("[adjustment=timezone]").each(
        function (index, element) {
            var timestamp = $(element).attr("timestamp");
            if (timestamp > 0) {
                var date = new Date(0);
                date.setUTCSeconds(timestamp);
                $(element).html(date.toLocaleString());
            }
            else {
                $(element).html("");
            }
        }
    );
}