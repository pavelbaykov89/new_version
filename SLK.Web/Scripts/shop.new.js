$(function () {
    $.fn.select2.defaults.set("theme", "bootstrap");
    $("#select-user").select2({
        placeholder: "Select shop owner",
        width: null
    });
    $(".simple-dropdown").select2({
        width: null,
        minimumResultsForSearch: -1
    });

    $("#MainTab_FullDescription").summernote({
        minHeight: 300
    });
});