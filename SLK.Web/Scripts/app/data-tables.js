var TableDatatablesAjax = function () {
    var table;

    var a = function() {
        $(".date-picker").datepicker({
            rtl: App.isRTL(),
            autoclose: !0
        })
    },
    e = function (actionUrl, columnsInfo) {
        var a = new Datatable;
        var i = {};

        a.init({
            src: $("#datatable_ajax"),
            onSuccess: function (a, e) { },
            onError: function (a) { },
            onDataLoad: function (a) { },
            loadingMessage: "Loading...",
            dataTable: {
                deferRender: true,
                bFilter: true,
                bStateSave: !0,
                //fnStateSaveParams: function (a, e) {
                //    return $("#datatable_ajax tr.filter .form-control").each(function () {
                //        e[$(this).attr("name")] = $(this).val()
                //    }),
                //    e
                //},
                //fnStateLoadParams: function (a, e) {
                //    return $("#datatable_ajax tr.filter .form-control").each(function () {
                //        var a = $(this);
                //        e[a.attr("name")] && a.val(e[a.attr("name")])
                //    }),
                //    !0
                //},
                lengthMenu: [[10, 20, 50, 100, 150, -1], [10, 20, 50, 100, 150, "All"]],
                //"paging": false,
                initComplete: function (settings) {
                    var api = new $.fn.dataTable.Api(settings);
                    var pagination = $(this)
                        .closest('.dataTables_wrapper')
                        .find('.dataTables_paginate, .dataTables_length,.dataTables_info .seperator');
                    pagination.toggle(api.page.info().pages > 1);
                },
                pageLength: 10,
                ajax: {
                    url: actionUrl,
                    data: function(d) {
                        $.each(i, function (n, p) {
                            d[n] = p;
                        });
                        delete d['columns'];
                        delete d['search'];                        
                    }
                },
                columns: columnsInfo,                
                order: [[0, "asc"]]
            }
        });

        $('input.form-filter').on('keyup', function () {            
            i[this.name] = this.value;
            a.getDataTable().ajax.reload();
        });

        $('.date-picker input.form-filter').on('change', function () {
            i[this.name] = this.value;
            a.getDataTable().ajax.reload();
        });

        $('select.form-filter').on('change', function () {            
            i[this.name] = this.value;
            a.getDataTable().ajax.reload();
        });


        $.fn.modal_extended.defaults.spinner = $.fn.modalmanager.defaults.spinner = '<div class="loading-spinner" style="width: 200px; margin-left: -100px;"><div class="progress progress-striped active"><div class="progress-bar" style="width: 100%;"></div></div></div>';
        var editPopup = $("#edit_entity_popup");
        $(document).on("click", ".edit-popup", function () {
            $("body").modalmanager("loading");
            var d = $(this);
            setTimeout(function () {
                editPopup.load(d.attr("data-url"), "", function () {
                    editPopup.modal_extended();
                    initSelect2(editPopup);
                })
            }, 1e3)
        });

        $(document).on('click.modal.data-api', '.add-popup[data-toggle="modal-extended"]', function (e) {
            var $this = $(this),
				href = $this.attr('href'),
				$target = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, ''))); //strip for ie7

            $target
                .one('hidden', function () {
                    // get the form inside we are working - change selector to your form as needed
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
                })
                .one("show", function () {
                    initSelect2($target);
                })
        });

        $(document).on("click", ".delete-entity", function (e) {
            e.preventDefault();
            var that = $(this);
            if (confirm("Are you sure?")) {
                $.get(that.attr("href"), function (data) {
                    if (data.success)
                        a.getDataTable().ajax.reload();
                    else
                        alert("Error");
                });
            }
        });

        $.fn.select2.defaults.set("theme", "bootstrap");

        table = a;
    },
    initSelect2 = function (modal) {
        modal.find(".simple-dropdown").select2({
            width: null,
            minimumResultsForSearch: -1
        });
    };

    return {
        init: function (actionUrl, columnsInfo) {
            a(),
            e(actionUrl, columnsInfo)
        },
        onAddEdit: function(data, status, xhr, formId) {
            if (data && data.success) {
                var modal = $("#" + formId).closest(".modal");
                modal.modal_extended("hide");
                table.getDataTable().ajax.reload();
            }
            else {
                $("#" + formId).replaceWith(data);
                initSelect2($("#" + formId).closest(".modal"));
            }
        }
    }
}();


