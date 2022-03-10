var PanelArchivos = function () {
    var _ListarArchivos = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetArchivos/IntranetArchivosObtenerListadoJson",
            refresh: false,
            callBackSuccess: function (response) {
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_archivosListado",
                    table: "#archivosListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_archivos",
                    tableColumns: [
                        {
                            data: "nombre_completo",
                            title: "",
                            "bSortable": false,
                            className: 'align-center',
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info archivosListado" data-nombre="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "nombre",
                            title: "Nombre Archivo",
                        },
                        {
                            data: "extension",
                            title: "Extension",
                        },
                        {
                            data: "tamanio",
                            title: "Tamaño (MB)",
                        },
                        {
                            data: "nombre_completo",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var nombre_archivo = value;
                                var span ='<a href="javascript:void(0);"class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-archivo" data-nombre="' + nombre_archivo + '" data-rel="tooltip"title="Eliminar Seccion">Eliminar</a>';
                                return span;
                            }

                        },
                    ]
                })
            }
        });
    };
    var _componentes = function () {

        $(document).on("click", "#btn_nuevo", function (e) {
          
            _objetoForm_form_archivos.resetForm();
            $("#nombre_completo").val("");
            $("#modalFormulario").modal("show");
        });

        $(document).off('click', ".btn-guardar")
        $(document).on('click', ".btn-guardar", function (e) {
            debugger;
            var permitido=15728640;
            var fileSize = $('#nombre_completo')[0].files[0].size;
            if(fileSize<permitido){
                if (_objetoForm_form_archivos.valid()) {
                    var dataForm = new FormData(document.getElementById("form_archivos"));
                    responseFileSimple({
                        url: "IntranetArchivos/IntranetArchivosInsertar",
                        data: dataForm,
                        refresh: false,
                        callBackSuccess: function (response) {
                            //console.log(response);
                            var respuesta = response.respuesta;
                            if (respuesta) {
                                PanelArchivos.init_ListarArchivos();
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
            }
            else{
                messageResponse({
                    text: "Archivo Demasiado Grande, solo se permiten archivos de hasta 15MB",
                    type: "error"
                })
            }
        })
     
        $(document).on("click", ".btn-eliminar-archivo", function (e) {
            var nombre_completo = $(this).data("nombre");
            console.log(nombre_completo);
            if (nombre_completo != "") {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este Archivo?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetArchivos/IntranetArchivosEliminar",
                            data: JSON.stringify({ nombre_completo: nombre_completo }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if(response.respuesta){
                                    PanelArchivos.init_ListarArchivos();
                                }
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error no se encontro el Archivo",
                    type: "error"
                })
            }
        });



        //Boton Eliminar Varios Menus
        $(document).on("click", "#btn_eliminar_varios", function (e) {
            let arrayArchivos = [];
            $('#archivosListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayArchivos.push($(this).data("nombre"));
            });

            if (arrayArchivos.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR todas los Archivos Seleccionados?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { arrayArchivosEliminar: arrayArchivos };
                        responseSimple({
                            url: "IntranetArchivos/IntranetArchivosEliminarVarios",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if(response.respuesta){
                                    PanelArchivos.init_ListarArchivos();
                                }
                            }
                        })
                    }
                });
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar al menos un Archivo",
                    type: "error"
                })
            }

        })

        $(document).on("click", ".chk_archivos", function (e) {
            $('#archivosListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#archivosListado  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row

            $('.chk_archivos').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_archivos',
            contenedor: '#form_archivos',
            rules: {
                nombre_completo:
                {
                    required: true,

                },
            },
            messages: {
                nombre_completo:
                {
                    required: 'Debe Subir una Imagen',
                },
            }
        });

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarArchivos();
            _componentes();
            _metodos();

        },
        init_ListarArchivos: function () {
            _ListarArchivos();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelArchivos.init();
});