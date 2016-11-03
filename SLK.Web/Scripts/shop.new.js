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

    $("[data-toggle='tab']").on("click", function () {
        var tabId = $(this).attr("href").replace("#", "");
        var newUri = updateQueryStringParameter(document.location.href, "tab", tabId);
        window.history.replaceState(tabId, tabId, newUri);
    });

    var updateQueryStringParameter = function(uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }
});