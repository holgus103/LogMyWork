var selectCounter = 1;
$(document).ready(function () {
    $("#Users_0__Id").change(handleSelectChange);
});

function createTask(input) {
    if (input > 0) {
        var taskName = $("#Name").val();
        $(taskNodeTemplate.replace(/TASK_NAME/g, taskName).replace(/TASK_ID/g, input)).insertAfter("#TaskTable tr:last");
    }
}

function handleSelectChange() {
    // if selected value is not null 
    if ($(this).val() != "") {
        // if the current drop down is the last one, spawn another one
        if ($(this)[0] == $("#AddTaskPnl select").last()[0]) {
            // get last form-group with a select inside AddTaskPnl
            var lastSelectGroup = $("#AddTaskPnl  .form-group:has(select)").last();
            // clone the node and insert as last
            var newSelect = lastSelectGroup.clone().insertAfter(lastSelectGroup);
            var newSelect = newSelect.find("select");
            // set the id and the name of the new select 
            newSelect.attr("Name", "Users[" + selectCounter + "].Id").attr("Id", "Users_" + selectCounter + "__Id");
            selectCounter++;
            newSelect.change(handleSelectChange);
        }
    }
    else {
        $(this).parents(".form-group").remove();
        prepareSelects();
    }
    updateDropDowns()
}

function updateDropDowns() {
    $("#AddTaskPnl option").attr("disabled", false);
    // foreach selected value
    var dropDowns = $("#AddTaskPnl select").toArray();
    dropDowns.forEach(function (dd) {
        if ($(dd).val() != "") {
            // disable the currently processed value foreach other select 
            dropDowns.forEach(function (element) {
                if (element != dd) {
                    $(element).children("option[value='" + $(dd).val() + "']").attr("disabled", true);
                }
            });
        }
    })
}
// updates Ids and Names for all selects withing the AddTaskPnl div
function prepareSelects() {
    var selects = $("#AddTaskPnl select");
    selectCounter = selects.length;
    selects.each(function (index, element) {
        $(element).attr("Name", "Users[" + index + "].Id").attr("Id", "Users_" + index + "__Id");
    })
}

