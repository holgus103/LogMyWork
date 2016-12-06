function TabbedView(menu, tabbedViewDiv) {
    function handleTabChange() {
        var tabID = $(this).attr("tabID");
        tabbedViewDiv.find("[tabID]").hide();
        tabbedViewDiv.find("[tabID=" + tabID + "]").show();
    }
    menu.children("li").click(handleTabChange);
}