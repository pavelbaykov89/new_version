(function () {
    'use strict';

    var TableDatatablesAjax = function () {
        var e = function () {
            var a = new Datatable;

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
                        url: "/Product/List"
                    },
                    columns: [
                          { data: 'Name', 
                              bSortable: true, 
                              render: function (data) {
                                  return $.localize(data, "~/Scrips/app/index.js");
                              }
                          },
                          { data: 'CategoryName' },
                          { data: 'ShortDescription' },
                          { data: 'FullDescription' },
                          { data: 'SKU' },
                          { data: 'ProductManufacturerName' },
                          { data: 'HasImage', className: 'dt-center' },
                          {
                            data: 'EditDelete',
                            bSortable: false,
                            render: function (data, type, row) {
                                return '<a class="btn btn-sm btn-outline grey-salsa" href=/Product/Edit/' + row['ID'] + '><i class="glyphicon glyphicon-edit"></i> Edit</a><a class="btn btn-sm btn-outline red-mint" href=/Product/Delete/' + row['ID'] + '><i class="fa fa-close"></i> Delete</a>';
                            }
                          }
                    ],
                    order: [[0, "asc"]]
                }
            });
            

            $('input.form-filter').on('keyup', function () {                                
                a.setAjaxParam(this.name, this.value);
                a.getDataTable().ajax.reload();
            })
        };


        return {
            init: function () {
                e()
            }
        }
    }();
    jQuery(document).ready(function () {
        TableDatatablesAjax.init();

        setTasksCount();
    });

    function setTasksCount() {
        $.get('/Product/TasksCount', {}, function callback(data) {
            $('.dropdown-tasks span.badge')[0].innerText = data.count;
            if (data.count == 1)
                $('.tasks h3')[0].innerText = '1 task is active';
            else
                $('.tasks h3')[0].innerText = data.count + ' tasks are active';
        });
    }

})();

function drawTasks(anyway) {

    var taskdata;

    if (anyway || $.inArray('open', $('.dropdown-tasks')[0].classList) !== -1) {

        $.ajax({
            async: false,
            type: 'GET',
            url: '/Product/ActiveTasks',
            success: function (data) {
                taskdata = data;
            }
        });

        var list = $('.dropdown-tasks .dropdown-menu-list')[0];
        list.innerHTML = "";

        for (var i = 0; i < taskdata.count; ++i) {
            list.innerHTML +=
                "<li>\
                    <a href='javascript:;'>\
                        <span class='task'>\
                            <span class='desc'>" + taskdata.tasks[i].Caption + "</span><br>\
                            <span class='desc font-blue-soft'>" + taskdata.tasks[i].FileName + "</span><br>\
                            <span class='desc'>" + taskdata.tasks[i].Elapsed + " / " + taskdata.tasks[i].Estimated + "</span>\
                            <span class='percent'>" + taskdata.tasks[i].Progress + "%</span>\
                        </span>\
                        <span class='progress'>\
                            <span style='width: " + taskdata.tasks[i].Progress + "%;' class='progress-bar progress-bar-success' aria-valuenow='98' aria-valuemin='0' aria-valuemax='100'>\
                                <span class='sr-only'>" + taskdata.tasks[i].Progress + "% Complete</span>\
                            </span>\
                        </span>\
                    </a>\
                </li>";
        }
    }

    if (!taskdata) {
        $.ajax({
            async: false,
            type: 'GET',
            url: '/Product/TasksCount',
            success: function (data) {
                $('.dropdown-tasks span.badge')[0].innerText = data.count;
                if (data.count == 1)
                    $('.tasks h3')[0].innerText = '1 task is active';
                else
                    $('.tasks h3')[0].innerText = data.count + ' tasks are active';

                if (data.count) setTimeout(drawTasks, 1000);
            }
        });
    }
    else {
        $('.dropdown-tasks span.badge')[0].innerText = taskdata.count;
        if (taskdata.count == 1)
            $('.tasks h3')[0].innerText = '1 task is active';
        else
            $('.tasks h3')[0].innerText = taskdata.count + ' tasks are active';

        if (taskdata.count) setTimeout(drawTasks, 1000);
    }    
}