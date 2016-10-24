var startDate;
var taskClock = $("#TaskClock");
$(document).ready(function () {
    startDate = new Date($("#currentTask").val()*1000);
    setInterval(oneTick, 500);
});


function oneTick() {
    var now = new Date();
    var span = new TimeSpan(now - startDate);
    taskClock.html(span.getHours() + ":" + span.getMinutes() + ":" + span.getSeconds());
}

function TimeSpan(val) {
    this.val = Math.floor(val / 1000);
    this.getSeconds = function () {
        return this.val % 60;
    };
    this.getMinutes = function () {
        return Math.floor(this.val / 60) % 60;
    };
    this.getHours = function () {
        return Math.floor(this.val / 3600);
    };
}