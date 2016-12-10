function TabbedView(menu, tabbedViewDiv) {
    function handleTabChange() {
        var tabID = $(this).attr("tabID");
        var element = tabbedViewDiv.find("[tabID=" + tabID + "]")
        var wasVisible = element.is(":visible");

        tabbedViewDiv.find("[tabID]").hide();

        if(!wasVisible)
            element.show();
    }
    tabbedViewDiv.find("[tabID]").hide();
    menu.children("li").click(handleTabChange);
}