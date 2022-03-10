var PanelDescargas = function () {
    var _ListarArchivos = function () {
      
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetPJ/IntranetObtenerListadoArchivos",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                var datos=response.data;
                if(response.respuesta){
                   
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_archivosListado",
                        table: "#archivosListado",
                        tableColumnsData: datos,
                        tableColumns: [
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
                                    var span = '<a href="javascript:void(0);"class="button_down btn_descargar" data-nombre="' + nombre_archivo + '" data-rel="tooltip"title="Descargar Archivo"><i class="fa fa-download"></i> Descargar</a>';
                                    return span;
                                }
                            },
                        ]
                    })
                }
                
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','.btn_descargar',function(){
            var archivo=$(this).data("nombre");
            if(archivo!=''){
                window.location.href = basePath + "IntranetPJ/IntranetDescargarArchivo?fileName=" + archivo;
            }
            
        })
    };



    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarArchivos();
            _componentes();

        },
        init_ListarActividades: function () {
            _ListarArchivos();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelDescargas.init();
});