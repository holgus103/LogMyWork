/**
 * 
 * @param {type} nextIdGen Function generating item ID according to the index supplied
 * @param {type} nextNameGen Function generating item Name according to the index supplied
 * @param {type} allSelector Selects all repeatable input elements (all selects, all inputs)
 * @param {type} innerSelector Selects the input element inside the div that is being repeated
 * @param {type} outerSelector Selects the repeated element from the input element
 * @param {type} onChangeCallback Callback
 * @returns {RepeatableControl}
 */
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

    this.updateDropDowns = function() {
        $("option").attr("disabled", false);
        // foreach selected value
        var dropDowns = $(allSelector).toArray();
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

    this.resetControl = function() {
        $(allSelector).each(function (index, element) {
            if (index > 0) {
                $(element).remove();
            }
        })
        $(allSelector).eq(0).val(null);
    }
    $(allSelector).change(handleSelectChange);
}