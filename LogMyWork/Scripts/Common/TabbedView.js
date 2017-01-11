function TabbedView(menu, tabbedViewDiv) {
    var activeTab = window.location.hash.substr(1);
    this.setActivateTab = function(tabID){
        tabbedViewDiv.find("[tabID]").hide();
        tabbedViewDiv.find("[tabID=" + tabID + "]").show();
    };
    function handleTabChange() {
        var tabID = $(this).attr("tabID");
        var element = tabbedViewDiv.find("[tabID=" + tabID + "]")
        var wasVisible = element.is(":visible");

        tabbedViewDiv.find("[tabID]").hide();

        if (!wasVisible) {
            element.show();
            window.location.hash = tabID;
        }

    }
    tabbedViewDiv.find("[tabID]").hide();
    menu.children("li").click(handleTabChange);
    if(activeTab != null && activeTab.length >0)
        this.setActivateTab(activeTab);
}