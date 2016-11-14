$(document).ready(function () {
    $("#ShowTaskPanelBtn").click(showTaskAdditionPanel);
});

function showTaskAdditionPanel() {
    var panel = $("#AddTaskPnl");
    if (panel.hasClass("hidden"))
        panel.removeClass("hidden");
    else
        panel.addClass("hidden");
}