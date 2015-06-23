$(document).ready(function () {
    var ind = getCurrentMenuItemIndex();
    $("#idMainMenuNav li:eq(" + ind + ")").addClass("active");
});

function getCurrentMenuItemIndex() {
    var index = 0;

    if (window.location.href.indexOf("BaseProperties") != -1) {
        index = 2;
    }
    else if (window.location.href.indexOf("StringDefinitions") != -1) {
        index = 3;
    }
    else if (window.location.href.indexOf("SourceDirectories") != -1) {
        index = 4;
    }
    else if (window.location.href.indexOf("DestinationDirectories") != -1) {
        index = 5;
    }
    else if (window.location.href.indexOf("DestinationRegistryValues") != -1) {
        index = 6;
    }
    else if (window.location.href.indexOf("DestinationShortcuts") != -1) {
        index = 7;
    }
    else if (window.location.href.indexOf("Output") != -1) {
        index = 8;
    }

    return index;
}