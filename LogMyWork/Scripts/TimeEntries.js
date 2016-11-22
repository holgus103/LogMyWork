
var from;
var to;

function appendToTable(entry) {
    var html = "<tr>";
    html = appendColumn(html, entry.ProjectName);
    html = appendColumn(html, entry.TaskName);
    html = appendColumn(html, entry.StartString);
    html = appendColumn(html, entry.EndString);
    html = appendColumn(html, "1");
    html = appendColumn(html, entry.Active);

    html += "</tr>"
    $(html).insertAfter("#TimeEntriesTable tr:last")
}

function appendColumn(string, val) {
    return string + "<td>" + val + "</td>";
}

function onDateChanged() {
    var headers = Array();
    headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();

    $.ajax(
        {
            url: "/TimeEntries/GetFilteredValues",
            headers: headers,
            method: "POST",
            data:
                {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    from: (new Date(from.data().date)).getTime()/1000,
                    to: (new Date(to.data().date)).getTime()/1000

                },
            success: function (data) {
                $("#TimeEntriesTable tbody tr:has(td)").remove();
                var entries = JSON.parse(data);
                entries.forEach(appendToTable)

            }

        }
    );
}

$(document).ready(function () {
    from = $('#datetimepickerFrom');
    to = $('#datetimepickerTo');

    from.datetimepicker();
    to.datetimepicker();

    to.on("dp.change", onDateChanged);
    from.on("dp.change", onDateChanged);

});

