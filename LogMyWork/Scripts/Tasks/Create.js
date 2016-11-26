$(document).ready(function () {
    $("#Deadline").datetimepicker();
})

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


function prepareSelects() {
    var selects = $("#AddTaskPnl select");
    selectCounter = selects.length;
    selects.each(function (index, element) {
        $(element).attr("Name", "Users[" + index + "].Id").attr("Id", "Users_" + index + "__Id");
    })
}