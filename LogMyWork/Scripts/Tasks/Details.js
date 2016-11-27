var now = new Date();
var startDate = new Date(0);
var endDate = new Date(0);
var progress;
var intervalHandle;
$(document).ready(function () {

    startDate.setUTCSeconds(start);
    endDate.setUTCSeconds(deadline);
    var refreshRate = (endDate - startDate) / 100;
    progress = $("#Progress");
    refreshBar();
    intervalHandle = setInterval(refreshBar, refreshRate);
});

function refreshBar() {
    now = new Date();
    var value = (now - startDate) / (endDate - startDate) * 100;
    if(value <= 100 &&value >= 0){
        progress.css("width", value + "%");
    }
    else{
        if(value>100){
            progress.css("background-color", "#8E001C");
            clearInterval(intervalHandle);
            progress.css("width", "100%");
        }
    }
}