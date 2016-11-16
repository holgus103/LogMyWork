var dropDowns = Array();
var selectedValues = Array();
$(document).ready(function () {
    dropDowns.push($("#Users_0__Id"))
    dropDowns[0].change(handleSelectChange);
});

function createTask(input) {
    if (input > 0) {
        var taskName = $("#Name").val();
        $(taskNodeTemplate.replace(/TASK_NAME/g, taskName).replace(/TASK_ID/g, input)).insertBefore("#AddTaskPnl");
    }
}

function handleSelectChange() {
    if ($(this).val() == "") return;
    selectedValues.push($(this).val());
    var arr = $("#AddTaskPnl  .form-group")
    $($("#AddTaskPnl  .form-group:has(select)").last()).clone().insertAfter(arr[arr.length - 2])
    dropDowns.push($("select").last());
    dropDowns[dropDowns.length - 1].change(handleSelectChange);
    updateDropDowns()
}

function updateDropDowns() {
    dropDowns.forEach(function (dd) {
        if (dd.val() != "") {
            dropDowns.forEach(function (element) {
                if (element != dd) {
                    element.children("option[value='" + dd.val() + "']").attr("disabled", true);
                }
            });
        }
    })
}

