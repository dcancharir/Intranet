var PanelAgenda = function () {
    var table='';
    var _inicio = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        // var url = basePath + "IntranetPJ/IntranetListarAgenda";
        responseSimple({
            url:"IntranetPJ/IntranetListarAgenda",
            refresh:false,
            callBackSuccess:function(response){
                if(response.respuesta){
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_agenda",
                        table: "#agenda_table",
                        tableColumnsData: response.data,
                        tableColumns: [
                            {
                                "className":      'details-control',
                                "orderable":      false,
                                "data":           null,
                                "defaultContent": '',
                                "width":"50px",
                                "render":function(){
                                   return '<i class="fa fa-angle-double-up"></i><span class="sr-only">Detalle</span>';
                                }
                            },
                            { 
                                "title":"Nombre",
                                "data" : "NO_TRAB", 
                                "render":function(value, type, oData, meta){
                                    var span='';
                                    span+= oData.NO_APEL_PATE+' ' +oData.NO_APEL_MATE+ ', '+oData.NO_TRAB;
                                    return span;
                                },
                            },
                            { 
                                "title":"Celular",
                                "data": "NU_TLF2", 
                                "autoWidth": true 
                            },
                            { 
                                "title":"Email",
                                "data": "NO_DIRE_MAI1",
                                 "autoWidth": true 
                            },
                        ]
                    })
                }
            }
        })
    };

    var detalle=function(d){
        return '<div style="border-left:6px solid rgb(255, 0, 0);"><table cellpadding="5" cellspacing="0" border="0" class="table table-sm table-bordered">'+
        '<tr>'+
            '<td colspan="4"><h6 style="font-weigth:bold;"> DATOS DE : '+d.NO_APEL_PATE+' ' +d.NO_APEL_MATE+ ', '+d.NO_TRAB+'<h3></td>'+
        '</tr>'+
        '<tr>'+
            '<td style="width:30px;">EMPRESA </td>'+
            '<td>'+d.DE_NOMB+'</td>'+
            '<td style="width:30px;">ÁREA</td>'+
            '<td>'+d.DE_AREA+'</td>'+
        '</tr>'+
        '<tr>'+
            '<td style="width:30px;">PUESTO</td>'+
            '<td>'+d.DE_PUES_TRAB+'</td>'+
            '<td style="width:30px;">CELULAR</td>'+
            '<td>'+d.NU_TLF2+'</td>'+
        '</tr>'+
        '<tr>'+
            '<td style="width:30px;">TELÉFONO</td>'+
            '<td>'+d.NU_TLF1+'</td>'+
            '<td style="width:30px;">CORREO</td>'+
            '<td>'+d.NO_DIRE_MAI1+'</td>'+
        '</tr>'+
    '</table></div>';
    }
    var _componentes = function () {
        $('#agenda_table tbody').on('click', 'td.details-control', function () {
            table=_objetoDatatable_datatable_agenda;
        var tr = $(this).closest('tr');
        var row = table.row( tr );
 
        if ( row.child.isShown() ) {
            // la fila ya esta abierra, cerrar la fila y cambiar el icono
            row.child.hide();
            tr.removeClass('shown');
            tr.find('i').removeClass('fa fa-angle-double-down'); 
            tr.find('i').addClass('fa fa-angle-double-up'); 
            tr.find('i').css("color","rgb(255, 0, 0)");
            
        }
        else {
            // Abrir la fila de detalle
            row.child( detalle(row.data()) ).show();
            var td=row.child().find("td:eq(0)");
            td.css("padding-left",'2%')
            tr.addClass('shown');
            tr.find('i').removeClass('fa fa-angle-double-up'); 
            tr.find('i').addClass('fa fa-angle-double-down');
            tr.find('i').css("color","rgb(119, 119, 119)");
        }
        } );
    };



    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();

        },
        init_ListarAgenda: function () {
            _inicio();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelAgenda.init();
});