
var from;
var to;

function appendToTable(entry) {
    var html = "<tr>";
    html = appendColumn(html, entry.ProjectName);
    html = appendColumn(html, entry.TaskName);
    var startDate = new Date(parseInt(entry.Start.replace("/Date(", "")))
    html = appendColumn(html, startDate);
    var endDate = new Date(parseInt(entry.End.replace("/Date(", "")))
    html = appendColumn(html, endDate);
    html = appendColumn(html, new Date(endDate - startDate));
    var checkboxActive = '<input class="check-box" checked="true" disabled type="checkbox">';
    var checkboxInactive = '<input class="check-box" disabled type="checkbox">';
    if (entry.Active == 1) {
        html = appendColumn(html, checkboxActive);
    }
    else {
        html = appendColumn(html, checkboxInactive)
    }

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
                    from: (new Date(from.data().date)).getTime() / 1000,
                    to: (new Date(to.data().date)).getTime() / 1000

                },
            success: function (data) {
                $("#TimeEntriesTable tbody tr:has(td)").remove();
                var entries = $.parseJSON(data);
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

