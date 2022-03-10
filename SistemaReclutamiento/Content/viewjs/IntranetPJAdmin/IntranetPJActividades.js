var PanelActividades = function () {
    var hoy = new Date();
    var fecha_hoy = moment(hoy).format('YYYY-MM-DD hh:mm A');

    var _ListarActividades = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetActividades/IntranetActividadesListarTodoJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_actividadesListado",
                    table: "#actividadesListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_actividades",
                    tableColumns: [
                        {
                            data: "act_id",
                            title: "",
                            "bSortable": false,
                            className: 'align-center',
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info actividadesListado" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "act_id",
                            title: "ID",
                            width: "50px",
                            className: 'align-center',
                        },
                        {
                            data: "act_descripcion",
                            title: "Descripcion",

                        },
                        {
                            data: "act_imagen",
                            title: "Imagen",
                            "render": function (value) {
                                var img = '';
                                if (value != "") {
                                    //$("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
                                    img += '<img src="'+basePath+"IntranetFiles/Actividades/" + value + '" style="width:50px;height:50px;" />';
                                    //img += '<img src="' + basePath + 'Content/intranet/images/png/' + value + '" / style="width:50px;height:50px;">';
                                }
                                else {
                                    img = '<img src="' + basePath + 'Content/intranet/images/png/actividad.png" style="width:40px;height:40px;"/>';
                                }
                                return img;
                            },
                            width: "80px",
                        },
                        {
                            data: "act_fecha",
                            title: "Fecha",
                            "render": function (value) {
                                var fecha = moment(value).format('YYYY-MM-DD');
                                return fecha;
                            },
                            width: "120px",
                        },

                        {
                            data: "act_fecha",
                            title: "Hora",
                            "render": function (value) {
                                var fecha = moment(value).format('hh:mm A');
                                return fecha;
                            },
                            width: "110px",
                        },
                        {
                            data: "act_estado",
                            title: "Estado",
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
                            data: "act_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var act_id = value;
                                var span = '<div class="hidden-sm hidden-xs action-buttons"><a class="blue btn-detalle" href="#" data-id="' + act_id + '"><i class="ace-icon fa fa-search-plus bigger-130"></i></a><a class="green btn-editar" href="#" data-id="' + act_id + '"><i class="ace-icon fa fa-pencil bigger-130"></i></a><a class="red btn-eliminar" href="#" data-id="' + act_id + '"><i class="ace-icon fa fa-trash-o bigger-130"></i></a></div><div class="hidden-md hidden-lg" ><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i>   </button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-info btn-detalle" data-id="' + act_id + '" data-rel="tooltip" title="View"><span class="blue"><i class="ace-icon fa fa-search-plus bigger-120"></i></span></a></li><li><a href="#" class="tooltip-success btn-editar" data-id="' + act_id + '" data-rel="tooltip" title="Edit"><span class="green"><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span></a></li><li><a href="#" class="tooltip-error btn-eliminar" data-id="' + act_id + '" data-rel="tooltip" title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a>            </li></ul></div></div>';
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    var _componentes = function () {

        $('#myDatepicker1').datetimepicker({
            format: 'YYYY-MM-DD hh:mm A',
            ignoreReadonly: true,
            allowInputToggle: true,
        });

        $(document).on("click", "#btn_nuevo", function (e) {
            $("#act_id").val(0);
            $("#tituloModalActividades").text("Nueva");
            $("#img_ubicacion").val("");
            $("#spancv").html("");
            $("#spancv").append('<i class="fa fa-upload"></i>  Subir Icono');
            _objetoForm_form_actividades.resetForm();
            $("#act_descripcion").prop('disabled', false);
            $("#act_fecha").prop('disabled', false);
            $("#act_imagen").prop('disabled', false);
            $("#act_estado").prop('disabled', false);

            $("#act_descripcion").val("");
            $("#act_fecha").val(fecha_hoy);
            $("#act_imagen").val("");
            $("#act_estado").val("A");
            $("#tituloIcono").text("Adjuntar Icono (Opcional)");
            $(".btn-guardar").show();

            $("#divCV").hide();
            $("#modalFormulario").modal("show");
        });

        $(document).off('click', ".btn-guardar")
        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_actividades").submit();
            if (_objetoForm_form_actividades.valid()) {
                var dataForm = new FormData(document.getElementById("form_actividades"));
                var url = "";
                if ($("#act_id").val() == 0) {
                    url = "IntranetActividades/IntranetActividadesNuevoJson";
                }
                else {
                    url = "IntranetActividades/IntranetActividadesEditarJson";
                }
                responseFileSimple({
                    url: url,
                    data: dataForm,
                    refresh: false,
                    callBackSuccess: function (response) {
                        //console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            PanelActividades.init_ListarActividades();
                            $("#modalFormulario").modal("hide");
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
            var act_id = $(this).data("id");
            var dataForm = { act_id: act_id };
            _objetoForm_form_actividades.resetForm();
            responseSimple({
                url: "IntranetActividades/IntranetActividadesIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    //llenando datos en inputs
                    console.log(response);
                    if (response.respuesta) {
                        var actividad = response.data;
                        if (actividad.act_imagen != "") {
                            $("#cvnombre").text("Nombre: " + actividad.img_ubicacion);
                            $("#cvfecha").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#divCV").show();

                            $("#icono_actual").attr("src", basePath+"IntranetFiles/Actividades/" + actividad.act_imagen);
                            $("#img_ubicacion").val(actividad.img_ubicacion);
                        }

                        $("#tituloModalActividades").text("Detalle");
                     
                        $("#act_descripcion").val(actividad.act_descripcion);
                        $("#act_fecha").val(moment(actividad.act_fecha).format('YYYY-MM-DD hh:mm A'));
                        $("#act_estado").val(actividad.act_estado);
                        $("#act_id").val(actividad.act_id);


                        $("#act_descripcion").prop('disabled', true);
                        $("#act_fecha").prop('disabled', true);
                        $("#act_imagen").prop('disabled', true);
                        $("#act_estado").prop('disabled', true);
                        $(".btn-guardar").hide();
                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-editar", function () {
            var act_id = $(this).data("id");
            _objetoForm_form_actividades.resetForm();
            var dataForm = { act_id: act_id };
            responseSimple({
                url: "IntranetActividades/IntranetActividadesIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    if (response.respuesta) {

                        $("#divCV").hide();
                        $("#spancv").html("");
                        $("#spancv").append('<i class="fa fa-upload"></i>  Subir Icono');
                        $("#img_ubicacion").val("");
                        var actividad = response.data;
                        if (actividad.act_imagen != "") {
                            $("#cvnombre").text("Nombre: " + actividad.img_ubicacion);
                            $("#cvfecha").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#divCV").show();

                            $("#icono_actual").attr("src", basePath+"IntranetFiles/Actividades/" + actividad.act_imagen);
                            $("#img_ubicacion").val(actividad.img_ubicacion);
                        }

                        $("#tituloModalActividades").text("Editar");
                        $("#tituloIcono").text("Cambiar de Icono");

                        $("#act_descripcion").val(actividad.act_descripcion);
                        $("#act_fecha").val(moment(actividad.act_fecha).format('YYYY-MM-DD hh:mm A'));
                        $("#act_estado").val(actividad.act_estado);
                        $("#act_id").val(actividad.act_id);


                        $("#act_descripcion").prop('disabled', false);
                        $("#act_fecha").prop('disabled', false);
                        $("#act_imagen").prop('disabled', false);
                        $("#act_estado").prop('disabled', false);
                        $(".btn-guardar").show();
                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-eliminar", function (e) {
            var act_id = $(this).data("id");
            console.log(act_id);
            if (act_id != "" || act_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR esta Actividad?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetActividades/IntranetActividadesEliminarJson",
                            data: JSON.stringify({ act_id: act_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelActividades.init_ListarActividades();
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

        $(document).on("change", "#act_imagen", function () {
            var _image = $('#act_imagen')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension != "jpg" && extension != "png") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg o png',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg") {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spancv").html("");
                    $("#spancv").append(icon + " " + nombre + "... ." + actualicon);
                    $("#img_ubicacion").val(nombre + "." + actualicon);
                    //$("#spancv").css({ 'font-size': '10px' });  
                }
            }
            else {
                $("#spancv").html("");
                $("#spancv").append('<i class="fa fa-upload"></i>  Subir Icono');
            }
        })

        //Boton Eliminar Varios Menus
        $(document).on("click", "#btn_eliminar_varios", function (e) {
            let arrayActividades = [];
            $('#actividadesListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayActividades.push($(this).data("id"));
            });

            if (arrayActividades.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR todas las Actividades Seleccionadas?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaActividadesEliminar: arrayActividades };
                        responseSimple({
                            url: "IntranetActividades/IntranetActividadesEliminarVariosJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelActividades.init_ListarActividades();
                                //refresh(true);
                            }
                        })
                    }
                });
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar al menos una Actividad",
                    type: "error"
                });
            }

        })

        $(document).on("click", ".chk_actividades", function (e) {
            $('#actividadesListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#actividadesListado  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_actividades').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_actividades',
            contenedor: '#form_actividades',
            rules: {
                act_descripcion:
                {
                    required: true,

                },
                act_estado:
                {
                    required: true,

                },
                act_fecha: {
                    required: true,
                }

            },
            messages: {
                act_descripcion:
                {
                    required: 'Campo Obligatorio',
                },
                act_esado:
                {
                    required: 'Campo Obligatorio',
                },
                act_fecha:
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
            _ListarActividades();
            _componentes();
            _metodos();

        },
        init_ListarActividades: function () {
            _ListarActividades();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelActividades.init();
});