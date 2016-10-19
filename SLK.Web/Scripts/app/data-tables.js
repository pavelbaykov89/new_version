var TableDatatablesAjax = function () {
    var e = function (actionUrl, columnsInfo) {
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

        $('select.form-filter').on('change', function () {            
            i[this.name] = this.value;
            a.getDataTable().ajax.reload();
        });
    };

    return {
        init: function (actionUrl, columnsInfo) {
            e(actionUrl, columnsInfo)
        }
    }
}();


