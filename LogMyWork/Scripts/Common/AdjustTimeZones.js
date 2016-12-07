function adjustTimeZones() {
    $("[adjustment=timezone]").each(
        function (index, element) {
            var timestamp = $(element).attr("timestamp");
            var date = new Date(0);
            date.setUTCSeconds(timestamp);
            $(element).html(date.toLocaleString());
        }
    );
}