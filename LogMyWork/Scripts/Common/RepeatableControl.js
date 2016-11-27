function RepeatableControl(nextIdGen, nextNameGen, allSelector, innerSelector, outerSelector, onChangeCallback) {
    var selectCounter = 1;
    function handleSelectChange() {
        // if selected value is not null 
        if ($(this).val() != "") {
            // if the current drop down is the last one, spawn another one
            if ($(this)[0] == $(allSelector).last()[0]) {
                // get last form-group with a select inside AddTaskPnl
                var lastSelectGroup = $(allSelector).last().closest(outerSelector);
                // clone the node and insert as last
                var newSelect = lastSelectGroup.clone().insertAfter(lastSelectGroup);
                var newSelect = newSelect.find(innerSelector);
                // set the id and the name of the new select 
                newSelect.attr("Name", nextNameGen(selectCounter)).attr("Id", nextIdGen(selectCounter));
                newSelect.val(null);
                selectCounter++;
                newSelect.change(handleSelectChange);
            }
        }
        else {
            $(this).closest(outerSelector).remove();
            prepareSelects();
        }
        if (onChangeCallback != null)
            onChangeCallback();
    };


    function prepareSelects() {
        var selects = $(allSelector);
        selectCounter = selects.length;
        selects.each(function (index, element) {
            $(element).attr("Name", nextNameGen(index)).attr("Id", nextIdGen(index));
        })
    };
    $(allSelector).change(handleSelectChange);
}