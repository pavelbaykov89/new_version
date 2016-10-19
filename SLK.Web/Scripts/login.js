
var slk = {};
slk.login = (function() {

    var _loader;
    function setLoader(loader) {
        _loader = loader;
    }

    function reloadPopup(event, element) {
        event.preventDefault();
        element = $(element);
        var url = element.attr("href");
        var target = $("" + element.attr("data-target"));
        var modalContent = target.find(".modal-content");
        //modalContent.html(_loader);

        // load the url and show modal on success
        modalContent.load(url, function () {
            target.modal("show");
            initMetronicLogin();
        });
    }

    function reloadIfSuccess(data, status, xhr, formId) {
        if (data && data.success) {
            var modalContent = $("#" + formId).closest(".modal-content");
            modalContent.html(_loader);
            document.location.reload();
        }
    }

    function initMetronicLogin() {
        Login.init();
    }

    return {
        setLoader: setLoader,
        reloadPopup: reloadPopup,
        reloadIfSuccess: reloadIfSuccess,
        initMetronicLogin: initMetronicLogin
    }
})();
