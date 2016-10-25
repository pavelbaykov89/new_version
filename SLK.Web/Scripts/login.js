
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

$(function () {
    $('#login')
        .on('shown.bs.modal', function () {
            slk.login.initMetronicLogin();
        })
        .on('hidden.bs.modal', function () {
            //TODO code duplication with data-tables.js
            //This code is for reset form validation

            // get the form inside we are working - change selector to your form as needed
            var $target = $(this);
            $target.find(".alert-danger").addClass("display-hide");
            var $form = $target.find("form");
            // get validator object
            var $validator = $form.validate();
            // get errors that were created using jQuery.validate.unobtrusive
            var $errors = $form.find(".field-validation-error span");
            // trick unobtrusive to think the elements were succesfully validated
            // this removes the validation messages
            $errors.each(function () { $validator.settings.success($(this)); });
            // clear errors from validation
            $validator.resetForm();
            $form[0].reset();
            $form.find("input[type='text']").val("");
        });
});
