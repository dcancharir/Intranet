var PanelComentarios = function () {
    var _funcionCheckBox = function () {

    }
    var _ListarComentarios = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetSaludosCumpleanios/IntranetSaludoCumpleanioListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_comentarioListado",
                    table: "#comentarioListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_comentario",
                    tableColumns: [
                        {
                            data: "sld_id",
                            title: "",
                            className: 'align-center',
                            "bSortable": false,
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info comentarioListado" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "sld_id",
                            title: "ID",
                            className: 'align-center',
                            width: "50px",
                        },
                        {
                            data: "sld_cuerpo",
                            title: "Mensaje",

                        },
                        {
                            data: "sld_fecha_envio",
                            title: "Fecha",
                            "render": function (value) {
                                var fecha = moment(value).format('DD-MM-YYYY');
                                return fecha;
                            }
                        },
                        {
                            data: "sld_id",
                            title: "Persona que Saludó",
                            "render": function (value, type, row) {
                                var span = "";
                                span += row.apelpat_per_saluda + ' ' + row.apelmat_per_saluda + ', ' + row.per_saluda;
                                return span;
                            }
                        },
                        {
                            data: "sld_id",
                            title: "Persona que fue Saludada",
                            "render": function (value, type, row) {
                                var span = "";
                                span += row.apelpat_per_saludada + ' ' + row.apelmat_per_saludada + ', ' + row.per_saludada;
                                return span;
                            }
                        },
                        {
                            data: "sld_estado",
                            title: "Estado",
                            "render": function (value, type, row) {
                                var seleccionado = value=='A'?"selected":"";
                                var select = '<select class="browser-default custom-select" data-id=' + row.sld_id + '>';

                                if (value == 'A') {
                                    select += '<option value="A" selected>Activo</option><option value="I">Inactivo</option>'
                                }
                                else {
                                    select += '<option value="A">Activo</option><option value="I" selected>Inactivo</option>'
                                }
                                select += '</select>';
                                return select;
                            }
                        },
                        {
                            data: "sld_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var sld_id = value;
                                var span = '<div class="hidden-sm hidden-xs action-buttons"><a class="red btn-eliminar" href = "#" data-id="' + sld_id + '" > <i class="ace-icon fa fa-trash-o bigger-130"></i></a ></div ><div class="hidden-md hidden-lg"><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i> </button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-error btn-eliminar" data-id="' + sld_id + '" data-rel="tooltip"title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a> </li></ul></div></div>';
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    var _componentes = function () {

       
        $(document).on("change", "select", function () {
            var sld_id = $(this).data("id");
            var sld_estado = $(this).val();
            messageConfirmation({
                content: '¿Esta seguro de Cambiar de Estado a este Comentario de Cumpleaños?',
                callBackSAceptarComplete: function () {
                    var dataForm = {
                        sld_id: sld_id,
                        sld_estado: sld_estado
                    }
                    responseSimple({
                        url: "IntranetSaludosCumpleanios/IntranetSaludoCumpleanioEditarJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            PanelComentarios.init_ListarComentarios();
                            //refresh(true);
                        }
                    });
                }
            });

        });
         $(document).on("click", ".btn-eliminar", function (e) {
            var sld_id = $(this).data("id");
             if (sld_id != "" || sld_id > 0) {
                 messageConfirmation({
                     content: '¿Esta seguro de ELIMINAR este Comentario de Cumpleaños?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSaludosCumpleanios/IntranetSaludoCumpleanioEliminarJson",
                            data: JSON.stringify({ sld_id: sld_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelComentarios.init_ListarComentarios();
                                //refresh(true);
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
        $(document).on("click", ".chk_comentario", function (e) {
            $('#comentarioListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#comentarioListado tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row

            $('.chk_comentario').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
        //Boton Eliminar Varios Menus
        $(document).on("click", "#btn_eliminar_varios", function (e) {
            let arrayComentarios = [];
            $('#comentarioListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayComentarios.push($(this).data("id"));
            });
            if (arrayComentarios.length > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaComentariosEliminar: arrayComentarios };
                        responseSimple({
                            url: "IntranetSaludosCumpleanios/IntranetMenuSaludoCumpleanioVariosJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelComentarios.init_ListarComentarios();
                                //refresh(true);
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

        })

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

                },
                menu_orden: {
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

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarComentarios();
            _componentes();
            _metodos();

        },
        init_ListarComentarios: function () {
            _ListarComentarios();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelComentarios.init();
});