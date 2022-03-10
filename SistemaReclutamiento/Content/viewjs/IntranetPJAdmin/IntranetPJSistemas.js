var PanelSistemas = function () {
    var _ListarSistemas = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetSistemas/IntranetSistemaListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_sistemasListado",
                    table: "#sistemasListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: false,
                    tableColumns: [
                  
                        {
                            data: "sist_id",
                            title: "ID",
                            width: "50px",
                            className: 'align-center',
                        },
                        {
                            data: "sist_nombre",
                            title: "Nombre",

                        },
                        {
                            data: "sist_ruta",
                            title: "Ruta",
                        },

                        {
                            data: "sist_descripcion",
                            title: "Descripcion",
                        },
                        {
                            data: "sist_estado",
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
                                return span;
                            }
                        },
                        {
                            data: "sist_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var span = '<a href="javascript:void(0);"class="btn btn-white btn-success btn-sm btn-round btn_editar" data-id="' + value + '" data-rel="tooltip"title="Editar Sistema">Editar</a> <a href="javascript:void(0);"class="btn btn-white btn-danger btn-sm btn-round btn_eliminar" data-id="' + value + '" data-rel="tooltip"title="Eliminar ">Eliminar</a>';
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    var _componentes = function () {

        $(document).on("click", "#btn_nuevo", function (e) {
            $("#sist_id").val(0);
            $("#tituloModalSistemas").text("Nuevo");
            $("#sist_nombre").prop('disabled', false);
            $("#sist_ruta").prop('disabled', false);
            $("#sist_descripcion").prop('disabled', false);
            $("#sist_estado").prop('disabled', false);

            $("#sist_nombre").val("");
            $("#sist_ruta").val("");
            $("#sist_descripcion").val("");
            $("#sist_estado").val("A");
            $("#modalFormulario").modal("show");
        });

        $(document).on('click', ".btn_guardar", function (e) {
            $("#form_sistemas").submit();
            if (_objetoForm_form_sistemas.valid()) {
                var urlValido=isValidURL($("#sist_ruta").val());
                if(urlValido==true){
                    var dataForm = $('#form_sistemas').serializeFormJSON();
                    var url = "";
                    if ($("#sist_id").val() == 0) {
                        url = "IntranetSistemas/IntranetSistemaInsertarJson";
                    }
                    else {
                        url = "IntranetSistemas/IntranetSistemaEditarJson";
                    }
                    responseSimple({
                        url: url,
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            //console.log(response);
                            var respuesta = response.respuesta;
                            if (respuesta) {
                                PanelSistemas.init_ListarSistemas();
                                $("#modalFormulario").modal("hide");
                            }
                        }
                    });
                }
                else{
                    messageResponse({
                        text: "Ruta no es Valida (revise http ó https)",
                        type: "error"
                    })
                }
                
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
          $(document).on("click", ".btn_editar", function () {
            var sist_id = $(this).data("id");
            _objetoForm_form_sistemas.resetForm();
            var dataForm = { sist_id: sist_id };
            responseSimple({
                url: "IntranetSistemas/IntranetSistemaIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {

                        var sistema = response.data;
                        $("#tituloModalSistemas").text("Editar");
                        $("#sist_nombre").val(sistema.sist_nombre);
                        $("#sist_ruta").val(sistema.sist_ruta);
                        $("#sist_estado").val(sistema.sist_estado);
                        $("#sist_descripcion").val(sistema.sist_descripcion);
                        $("#sist_id").val(sistema.sist_id);

                        $("#sist_descripcion").prop('disabled', false);
                        $("#sist_nombre").prop('disabled', false);
                        $("#sist_ruta").prop('disabled', false);
                        $("#sist_estado").prop('disabled', false);
                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn_eliminar", function (e) {
            var sist_id = $(this).data("id");
            if (sist_id != "" || sist_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este Sistema?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSistemas/IntranetSistemaEliminarJson",
                            data: JSON.stringify({ sist_id: sist_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if(response.respuesta){
                                    PanelSistemas.init_ListarSistemas();
                                }
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


    };
    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_sistemas',
            contenedor: '#form_sistemas',
            rules: {
                sist_nombre:
                {
                    required: true,

                },
                sist_ruta:
                {
                    required: true,

                },
                sist_descripcion: {
                    required: true,
                },
                sist_estado: {
                    required: true,
                }

            },
            messages: {
                sist_nombre:
                {
                    required: 'Campo Obligatorio',
                },
                sist_descripcion:
                {
                    required: 'Campo Obligatorio',
                },
                sist_estado:
                {
                    required: 'Campo Obligatorio',
                },
                sist_ruta:
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
            _ListarSistemas();
            _componentes();
            _metodos();

        },
        init_ListarSistemas: function () {
            _ListarSistemas();
        }
    }
}();
function isValidURL(string) {
    var pattern = new RegExp('^(http|https)\:\/\/[a-z0-9\.-]+\.[a-z]{2,4}','gi');
    return pattern.test(string);
  };

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelSistemas.init();
});