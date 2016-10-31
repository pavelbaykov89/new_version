$(function () {
    $.fn.select2.defaults.set("theme", "bootstrap");
    $("#user_new_form .select2").select2({
        placeholder: "Select shop owner",
        width: null
    });

    $("#FullDescription").summernote({
        height: 300
    });
});