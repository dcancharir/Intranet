
var PanelMenus = function () {
    var _ListarMenus = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }

        responseSimple({
            url: "IntranetMenu/IntranetMenuListarTodoJson",
            refresh: false,
            callBackSuccess: function (response) {
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_menulistado",
                    table: "#menusListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 1,
                    headerCheck: "chk_menu",
                    rowReorder: {
                        selector: 'td.reorder',
                        dataSrc: 'menu_orden',
                    },
                    tableColumns: [
                        {
                            data: "menu_orden",
                            title: "Orden",
                            className: 'reorder',
                            name: "menu_orden",
                            visible: false
                        },
                        {
                            data: "menu_id",
                            title: "",
                            className: 'align-center',
                            "bSortable": false,
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info menusListado" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "menu_id",
                            title: "ID",
                            "bSortable": false,
                            className: 'reorder align-center',
                            visible: true,
                            width: "50px",
                        },
                        {
                            data: "menu_orden",
                            title: "Orden",
                            name: "menu_orden",
                            className: 'reorder',
                            width: "80px",
                        },
                        {
                            data: "menu_titulo",
                            title: "Titulo",
                            "bSortable": false,
                            className: 'reorder align-left',
                        },
                        {
                            data: "menu_url",
                            title: "URI",
                            "bSortable": false,
                            className: 'reorder',
                        },
                        {
                            data: "menu_estado",
                            title: "Estado",
                            "bSortable": false,
                            className: 'reorder',
                            "render": function (value) {
                                var estado = value;
                                var mensaje_estado = "";
                                if (estado === 'A') {
                                    estado = "success";
                                    mensaje_estado = "Activo";
                                } else {
                                    estado = "danger";
                                    mensaje_estado = "InActivo";
                                }
                                var span = '<span class="label label-sm label-' + estado + ' arrowed arrowed-righ">' + mensaje_estado + '</span>';
                                //var span = '<span class="badge badge-' + estado + '">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "menu_id",
                            title: "Acciones",
                            "bSortable": false,
                            "render": function (value, type, oData) {
                                var span = '';
                                var menu_id = value;
                                var span = '<div class="hidden-sm hidden-xs action-buttons"><a class="blue btn-detalle" href="#" data-id="' + menu_id + '"><i class="ace-icon fa fa-search-plus bigger-130"></i></a><a class="green btn-editar" href="#" data-id="' + menu_id + '"><i class="ace-icon fa fa-pencil bigger-130"></i></a><a class="red btn-eliminar" href="#" data-orden="' + oData.menu_orden + '" data-id="' + menu_id + '"><i class="ace-icon fa fa-trash-o bigger-130"></i></a></div><div class="hidden-md hidden-lg" ><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i>   </button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-info btn-detalle" data-id="' + menu_id + '" data-rel="tooltip" title="View"><span class="blue"><i class="ace-icon fa fa-search-plus bigger-120"></i></span></a></li><li><a href="#" class="tooltip-success btn-editar" data-id="' + menu_id + '" data-rel="tooltip" title="Edit"><span class="green"><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span></a></li><li><a href="#" class="tooltip-error btn-eliminar" data-orden="' + oData.menu_orden + '" data-id="' + menu_id + '" data-rel="tooltip" title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a>            </li></ul></div></div>';
                                return span;
                            }
                        }

                    ]
                });
                PanelMenus.init_ordenar();
            }
        });
    };
    var _componentes = function () {
        
        $(document).on("click", "#btn_nuevo", function (e) {
            $("#menu_id").val(0);
            $("#tituloModalMenu").text("Nuevo");
            $("#menu_titulo").prop('disabled', false);
            $("#menu_url").prop('disabled', false);
            $("#menu_orden").prop('disabled', false);
            $("#menu_estado").prop('disabled', false);
            $("#menu_blank").prop('disabled', false);
            _objetoForm_form_menus.resetForm();
            $("#menu_titulo").val("");
            $("#menu_url").val("");
            $("#menu_orden").val(1);
            $("#menu_estado").val("A");
            $("#menu_blank").val("false");

            $(".btn-guardar").show();

            $("#modalFormulario").modal("show");
        });

        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_menus").submit();
            if (_objetoForm_form_menus.valid()) {
                var dataForm = $('#form_menus').serializeFormJSON();
                var url = "";
                if ($("#menu_id").val() == 0) {
                    url = "IntranetMenu/IntranetMenuNuevoJson";
                }
                else {
                    url = "IntranetMenu/IntranetMenuEditarJson";
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            PanelMenus.init_ListarMenus();
                            $("#modalFormulario").modal("hide");
                            //refresh(true);
                        }
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })

        $(document).on("click", ".btn-detalle", function () {
            var menu_id = $(this).data("id");
            console.log(menu_id);
            _objetoForm_form_menus.resetForm();
            var dataForm = { menu_id: menu_id };
            responseSimple({
                url: "IntranetMenu/IntranetMenuIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    //llenando datos en inputs
                    if (response.respuesta) {
                        var menu = response.data;
                        $("#tituloModalMenu").text("Detalle");
                        $("#menu_titulo").val(menu.menu_titulo);
                        $("#menu_url").val(menu.menu_url);
                        $("#menu_orden").val(menu.menu_orden);
                        $("#menu_estado").val(menu.menu_estado);
                        menu.menu_blank == false ? $("#menu_blank").val("false") : $("#menu_blank").val("true");

                        $("#menu_id").val(menu.menu_id);

                        $("#menu_titulo").prop('disabled', true);
                        $("#menu_url").prop('disabled', true);
                        $("#menu_orden").prop('disabled', true);
                        $("#menu_estado").prop('disabled', true);
                        $("#menu_blank").prop('disabled', true);
                        $(".btn-guardar").hide();

                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-editar", function () {
            var menu_id = $(this).data("id");
            _objetoForm_form_menus.resetForm();
            var dataForm = { menu_id: menu_id };
            responseSimple({
                url: "IntranetMenu/IntranetMenuIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    //llenando datos en inputs
                    if (response.respuesta) {
                        var menu = response.data;
                        $("#tituloModalMenu").text("Editar");
                        $("#menu_titulo").val(menu.menu_titulo);
                        $("#menu_url").val(menu.menu_url);
                        $("#menu_orden").val(menu.menu_orden);
                        $("#menu_estado").val(menu.menu_estado);
                        menu.menu_blank == false ? $("#menu_blank").val("false") : $("#menu_blank").val("true");
                        $("#menu_id").val(menu.menu_id);

                        $("#menu_titulo").prop('disabled', false);
                        $("#menu_url").prop('disabled', false);
                        $("#menu_orden").prop('disabled', false);
                        $("#menu_estado").prop('disabled', false);
                        $("#menu_blank").prop('disabled', false);
                        $(".btn-guardar").show();

                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-eliminar", function (e) {
            var menu_id = $(this).data("id");
            if (menu_id != "" || menu_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este Menú?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetMenu/IntranetMenuEliminarJson",
                            data: JSON.stringify({
                                menu_id: menu_id
                            }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                //refresh(true);
                                PanelMenus.init_ListarMenus();
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error no se encontro ID",
                    type: "error"
                })
            }
        });

        //checkAll
        $(document).on("click", ".chk_menu", function (e) {
            console.log("click all");
            $('#menusListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#menusListado tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row

            $('.chk_menu').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
        //Boton Eliminar Varios Menus
        $(document).on("click", "#btn_eliminar_varios", function (e) {
            let arrayMenus = [];
            $('#menusListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayMenus.push($(this).data("id"));
            });
            if (arrayMenus.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR todos los Menús Seleccionados?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaMenuEliminar: arrayMenus };
                        responseSimple({
                            url: "IntranetMenu/IntranetMenuEliminarVariosJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                //refresh(true);
                                PanelMenus.init_ListarMenus();
                            }
                        })
                    }
                });
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar al menos un Menu",
                    type: "error"
                })
            }

        });  
       
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_menus',
            contenedor: '#form_menus',
            rules: {
                menu_titulo:
                {
                    required: true,

                },
                menu_url:
                {
                    required: true,

                }
            },
            messages: {
                menu_titulo:
                {
                    required: 'Campo Obligatorio',
                },
                menu_url:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });

    };

    var _ordenar = function () {
        _objetoDatatable_datatable_menulistado.off('row-reorder');
        _objetoDatatable_datatable_menulistado.on('row-reorder', function (e, diff, edit) {
            //console.log(edit.triggerRow.data().menu_titulo)

            let arrayOrdenesMenus = [];
            var result = '<div style="font-size:12px">Cambio empezo con: <strong>' + edit.triggerRow.data().menu_titulo + '</strong><br></div>';
            
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = _objetoDatatable_datatable_menulistado.row(diff[i].node).data();
               
                //console.log(rowData.menu_id + "-" + diff[i].newData);
                var posicionnew = (diff[i].newPosition + 1);
                var posicionold = (diff[i].oldPosition + 1)
                var obj = {
                    menu_orden: posicionnew,
                    menu_id: rowData.menu_id
                };
                arrayOrdenesMenus.push(obj);
                result += '<div style="font-size:12px"><strong>'+rowData.menu_titulo + '</strong> Pos. Actual: ' +
                    posicionnew + ' (Desde Pos. ' + posicionold + ')<br></div>';
            }
            //console.log(arrayOrdenesMenus);
            var dataform = {
                arrayMenus: arrayOrdenesMenus,
            }
            if (arrayOrdenesMenus.length > 0) {
                responseSimple({
                    url: "IntranetMenu/IntranetMenuEditarOrdenJson",
                    data: JSON.stringify(dataform),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            messageResponse({
                                text: result,
                                type: "warning"
                            });
                        }
                       
                    }
                })
            }
            
        });

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarMenus();
            _componentes();
            _metodos();

        },
        init_ListarMenus: function () {
            _ListarMenus();
        },
        init_ordenar: function () {
            _ordenar();
        }
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelMenus.init();
});