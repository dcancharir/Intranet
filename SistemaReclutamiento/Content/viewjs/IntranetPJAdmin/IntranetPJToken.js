var PanelToken = function () {
    var listaSistemas;
    var usuariosSistemas;
    var listaUsuariosPostgres;
    var listaTokensEditar=[];
    var _ListarUsuarios = function () {
        $("#bloque_usuarios_postgres").show();
        $("#bloque_usuarios_sistemas").hide();
        $("#bloque_usuarios_sistemas").html("");
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetToken/IntranetTokenListarUsuariosJson",
            refresh: false,
            callBackSuccess: function (response) {
                if(response.respuesta){
                    listaSistemas=response.data_sistemas;
                    listaUsuariosPostgres=response.data;
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_usuariosListado",
                        table: "#usuariosListado",
                        tableColumnsData: response.data,
                        tableHeaderCheck: true,
                        tableHeaderCheckIndex: 0,
                        headerCheck: "chk_usuarios",
                        tableColumns: [
                            {
                                data: "usu_id",
                                title: "",
                                "bSortable": false,
                                className: 'align-center',
                                "render": function (value) {
                                    var check = '<input type="checkbox" class="form-check-input-styled-info usuariosListado" data-id="' + value + '" name="chk[]">';
                                    return check;
                                },
                                width: "50px",
                            },
                            {
                                data: "usu_id",
                                title: "Id Usuario",
                                width: "80px",
                            },
                            {
                                data: "usu_id",
                                title: "Nombre",
                                "render":function(value,type,row){
                                    var span=''+row.per_apellido_pat+' '+row.per_apellido_mat+', '+row.per_nombre;
                                    return span.toUpperCase();
                                },
                                width: "300px",
                            },
                            {
                                data: "usu_nombre",
                                title: "Usuario",
                                width: "150px",
                            },
                            {
                                data: "usu_token",
                                title: "Token",
                                width: "535px",
                            },
                        ]
                    })
                }
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','#btn_sincronizar',function(){
            let arrayDNIs = [];
            let arrayTokens=[];
            if(listaUsuariosPostgres.length>0){
                
                $.each(listaUsuariosPostgres,function(index,value){
                    arrayDNIs.push(value.per_numdoc);
                    arrayTokens.push(value.usu_token);
                })
            }
            var dataForm={
                listaSistemas:listaSistemas,
                listaDNIs:arrayDNIs,
                listaTokens:arrayTokens
            };
            responseSimple({
                url:"IntranetToken/IntranetListarUsuariosSistemasJson",
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    var span='';
                    usuariosSistemas=response.data;
                    if(response.respuesta){
                        span+=' <div class="widget-header widget-header-flat widget-header-small">'+
                        '<h4 class="widget-title">Listado Usuarios por Sistemas</h4>'+
                        '<span class="widget-toolbar">'+
                            '<a href="#" class="btn btn-sm btn-warning btn-minier" data-action="settings" id="btn_editar_token"><i class="ace-icon fa fa-file"></i> Modificar Tokens</a>'+
                            '<a href="#" class="btn btn-sm btn-danger btn-minier" data-action="settings" id="btn_cancelar"><i class="ace-icon fa fa-reply"></i> Cancelar</a>'+
                        '</span>'+
                    '</div>';
                        span+='<div class="panel panel-default">';
                        $.each(usuariosSistemas,function(index,value){
                            span+='<div class="panel-heading"><h4 class="panel-title">'+
                                '<a class="accordion-toggle collapsed dibujar_tabla" data-id="'+index+'" data-toggle="collapse" data-parent="#accordion" href="#collapse'+index+'" aria-expanded="false">'+
                                        '<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                        'Listado Sistema '+value.sist_nombre+'<div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;">Total Usuarios: <span id="span_total'+index+'"'+
                                        'class="label label-success label-white middle">'+(value.usuarios.length)+'</span></div>'+
                                    '</a>'+
                                '</h4>'+
                            '</div>'+
                            '<div class="panel-collapse collapse" id="collapse'+index+'" aria-expanded="false" style="height: 0px;">'+
                                '<div class="panel-body" id="panel'+index+'"><table id="table'+index+'" class="table table-condensed table-striped table-bordered table-hover datatableListado'+index+'"></table>'+
                                '</div>'+
                            '</div>';
                        })
                        span+='</div>';
                        
                        $("#bloque_usuarios_sistemas").html(span);
                        $.each(usuariosSistemas,function(index,value){
                            _crearDatatable(value.usuarios,index);
                        })
                        $("#bloque_usuarios_sistemas").show();
                        $("#bloque_usuarios_postgres").hide();
                        
                        //crear array para modificar token
                    }
                    else{
                        messageResponse({
                            text: "No hay Datos",
                            type: "error"
                        })
                    }
                }
            })
        });

        $(document).on('click','#btn_editar_token',function(){
            let arrayDNIs = [];
            let arrayTokens=[];
            if(listaUsuariosPostgres.length>0){
                
                $.each(listaUsuariosPostgres,function(index,value){
                    arrayDNIs.push(value.per_numdoc);
                    arrayTokens.push(value.usu_token);
                })
            }
            var dataForm={
                listaTokens:arrayTokens,
                listaDNIs:arrayDNIs,
                listasistemas:usuariosSistemas,
            };
            responseSimple({
                url:'IntranetToken/IntranetModificarTokensporSistemaJson',
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    usuariosSistemas=response.data;
                    if(response.respuesta){
                        span+=' <div class="widget-header widget-header-flat widget-header-small">'+
                        '<h4 class="widget-title">Listado Usuarios por Sistemas</h4>'+
                        '<span class="widget-toolbar">'+
                            '<a href="#" class="btn btn-sm btn-warning btn-minier" data-action="settings" id="btn_editar_token"><i class="ace-icon fa fa-file"></i> Modificar Tokens</a>'+
                            '<a href="#" class="btn btn-sm btn-danger btn-minier" data-action="settings" id="btn_cancelar"><i class="ace-icon fa fa-reply"></i> Cancelar</a>'+
                        '</span>'+
                    '</div>';
                        span+='<div class="panel panel-default">';
                        $.each(usuariosSistemas,function(index,value){
                            span+='<div class="panel-heading"><h4 class="panel-title">'+
                                '<a class="accordion-toggle collapsed dibujar_tabla" data-id="'+index+'" data-toggle="collapse" data-parent="#accordion" href="#collapse'+index+'" aria-expanded="false">'+
                                        '<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                        'Listado Sistema '+value.sist_nombre+'<div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;">Total Usuarios: <span id="span_total'+index+'"'+
                                        'class="label label-success label-white middle">'+(value.usuarios.length)+'</span></div>'+
                                    '</a>'+
                                '</h4>'+
                            '</div>'+
                            '<div class="panel-collapse collapse" id="collapse'+index+'" aria-expanded="false" style="height: 0px;">'+
                                '<div class="panel-body" id="panel'+index+'"><table id="table'+index+'" class="table table-condensed table-striped table-bordered table-hover datatableListado'+index+'"></table>'+
                                '</div>'+
                            '</div>';
                        })
                        span+='</div>';
                        
                        $("#bloque_usuarios_sistemas").html(span);
                        $.each(usuariosSistemas,function(index,value){
                            // _crearDatatable(value.usuarios,index);
                        })
                        $("#bloque_usuarios_sistemas").show();
                        $("#bloque_usuarios_postgres").hide();
                        
                        //crear array para modificar token
                    }
                    else{
                        messageResponse({
                            text: "No hay Datos",
                            type: "error"
                        })
                    }
                }
            })
        });
        $(document).on('click','.dibujar_tabla',function(){
            var indice=$(this).data("id");
            $("#table"+indice).DataTable().draw();
        });
        $(document).on('click','#btn_cancelar',function(){
            window.location.reload(true);
        })
    };
    var _crearDatatable=function(data,index){

        simpleDataTable({
            uniform: false,
            table: "#table"+index,
            tableNameVariable: "datatableListado"+index,
            tableColumnsData: data,
            tableHeaderCheck: false,
            tableColumns: [
                {
                    data: "NombreEmpleado",
                    title: "Empleado",
                    width:"250px"
                },
                {
                    data: "UsuarioNombre",
                    title: "Usuario",
                    width:"150px"
                },
                {
                    data: "Token",
                    title: "Token Sistema",
                    width: "300px",
                },
                {
                    data:"TokenPostgres",
                    title:"Token Actual",
                    width:"300px",
                },
                {
                    data:"DOI",
                    title:"Estado",
                    width:"50px",
                    "render":function(value,type,row){
                        var tokenSistema=row.Token;
                        var tokenPostgres=row.TokenPostgres;
                        var span='';
                        if(tokenPostgres==""){
                            span='<span class="label label-danger label-white middle">Sin Token</span>';
                        }
                        else if(tokenPostgres!=tokenSistema){
                            span='<span class="label label-warning label-white middle">Sin Coincidencia</span>';
                        }
                        else {
                            span='<span class="label label-success label-white middle">Verificado</span>';
                        }
                        return span;
                    }
                }
            ]
        })
    };
    var _metodos = function () {

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarUsuarios();
            _componentes();
            _metodos();

        },
        init_ListarUsuarios: function () {
            _ListarUsuarios();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelToken.init();
});