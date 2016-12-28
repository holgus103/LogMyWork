var tabs;
$(document).ready(function () {
    tabs = new TabbedView($("#TabMenu"), $("#TabbedView"));
    new StatusUpdater("IssueID", "/Issues/UpdateStatus", $("#Issues [status]"), function (data, element, status, id) {
        if (data == '1') {
            location.hash = "#Issues";
            location.reload();

        }
    })
})