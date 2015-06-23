$(document).ready(function () {
    $(".fileloader").each(function (index, element) {
        $(element).find("#idFileSelector").change(function (e) {
            var fileList = "";

            var files = e.target.files;
            for (var i = 0; i < files.length; i++) {
                fileList += files[i].name + ";";
            }

            $(element).find("#idFileList").attr("value", fileList);
        });
    });
});