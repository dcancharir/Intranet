var PanelSecciones = function () {
    var _inicio = function () {
      
     
        $("#btn_nuevo").prop('disabled', true);
        $("#btn_eliminar_varios").prop('disabled', true);
        // if (menu.length > 0) {
        //     var appendMenus = '';
        //     var selectMenus = $("#cboMenus");
        //     selectMenus.html("");
        //     $.each(menu, function (index, value) {
        //         appendMenus += '<option value="' + value.menu_id + '">"' + value.menu_titulo + '"</option>';
        //     });
        //     selectMenus.html(appendMenus);
        //     selectMenus.select2();
        // }
        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "cboTipoElementoModal",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            allOption: false,
            closeMessages:true,
        })
        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "cboTipoElemento",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            allOption: false,
            closeMessages:true,
        });
        selectResponse({
            url:"IntranetMenu/IntranetMenuListarTodoJson",
            select:"cboMenus",
            campoID:"menu_id",
            CampoValor:"menu_titulo",
            select2:true,
            allOption:false,
            closeMessages:true,
        });
    };
    var _ListarSecciones = function (menu_id) {
        var dataForm = {
            menu_id: menu_id,
        }
        responseSimple({
            url: "IntranetSeccion/IntranetSeccionListarTodoxMenuIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                $("#btn_nuevo").prop('disabled', false);
                $("#btn_eliminar_varios").prop('disabled', false);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "seccionesListado",
                    table: "#seccionesListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: false,
                    tableColumns: [
                        {
                            "className": 'details-control',
                            "orderable": false,
                            "data": "sec_id",
                            "title": "Secciones",
                            "defaultContent": '',
                            "render": function (value) {
                               

                                // if (validId.charAt(0) === "@") {

                                //     $("#datatable tbody").closest('tr').find('td:eq(6)').removeClass('valid-id');
                                // }
                                var sec_id = value;
                                var span = '';
                                span += '<a href="javascript:void(0);" class="tooltip-info" data-id="' + sec_id + '" data-rel="tooltip"                  title="Ver Detalle"><span class="blue" >             <i class="ace-icon fa fa-search-plus bigger-120"></       i></span ></a>';
                                return span;
                            },
                            width: "50px"
                        },
                        {
                            data: "sec_id",
                            title: "Id"
                        },
                        {
                            data: "menu_titulo",
                            title: "Menu"
                        },
                        {
                            data: "sec_orden",
                            title: "Orden"
                        },
                        {
                            data: "sec_estado",
                            title: "Estado",
                            "render": function (value, type, row) {
                                var select = '<select class="browser-default custom-select select-estado-seccion" data-id=' + row.sec_id + '>';

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
                            data: "sec_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var sec_id = value;
                                var span ='<a href="javascript:void(0);"class="btn btn-white btn-success btn-sm btn-round btn-nuevo-elemento" data-id="' + sec_id + '" data-rel="tooltip"title="Nuevo Elemento">Nuevo Elemento</a> <a href="javascript:void(0);"class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-seccion" data-id="' + sec_id + '" data-rel="tooltip"title="Eliminar Seccion">Eliminar</a>';
                                return span;
                            }
                        }

                    ],
                    rowCallback:function(row,data,index){
                        var data=data;
                        $("td:eq(0)",row).addClass("details-control"+data.sec_id);
                       
                    }
                })
            }
        })
        
    };
    var _componentes = function () {
        $(document).on("change", "#cboMenus", function () {
            var menu_id = $(this).val();
            if (menu_id != "") {
               PanelSecciones.init_ListarSecciones(menu_id);
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar un Menu",
                    type: "error"
                })
            }
        });
        $('#seccionesListado tbody').on('click', 'td.details-control', function () {
            var table = $("#seccionesListado").DataTable();
            var tr = $(this).closest('tr');
            var row = table.row(tr);
            var sec_id = row.data().sec_id;
            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                if (sec_id > 0) {
                    var dataForm = {
                        sec_id: sec_id
                    };
                    responseSimple({
                        url: "IntranetElemento/IntranetElementoListarxSeccionIDJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            var rows = response.data;
                            row.child(MostrarDetalle(rows)).show();
                            tr.addClass('shown');
                        }
                    })
                }
            }
        });
        //Secciones
        $(document).on("click", "#btn_nuevo", function (e) {
            if ($("#cboMenus").val() != "") {
                $("#sec_id").val(0);
                var menu_seleccionado = $('#cboMenus').val();
                $("#fk_menu").val(menu_seleccionado);
                $("#tituloModalSeccion").text("Nueva ");
                $("#sec_estado").prop('disabled', false);
                $("#sec_estado").val("A");
                $(".btn-guardar").show();
                $("#modalFormularioSeccion").modal("show");
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar un Menu para poder crear Secciones",
                    type: "error"
                })
            }

        });
        
        $(document).on('click', '.btn-guardar-seccion', function () {
            // $("#form_seccion").submit();
            var dataForm = $('#form_seccion').serializeFormJSON();
            var url = '';
            if ($("#sec_id").val() == 0) {
                url = 'IntranetSeccion/IntranetSeccionInsertarJson';
            }
            responseSimple({
                url: url,
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var menu_id=$("#cboMenus").val();
                        PanelSecciones.init_ListarSecciones(menu_id);
                    }
                    $("#modalFormularioSeccion").modal("hide");
                },
            })
        });

        $(document).on("change", ".select-estado-seccion", function () {
            var sec_id = $(this).data("id");
            var sec_estado = $(this).val();
            messageConfirmation({
                content: '¿Esta seguro de Cambiar de Estado a esta Sección?',
                callBackSAceptarComplete: function () {
                    var dataForm = {
                        sec_id: sec_id,
                        sec_estado: sec_estado
                    }
                    responseSimple({
                        url: "IntranetSeccion/IntranetSeccionEditarJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            var menu_id=$("#cboMenus").val();
                            if(response.respuesta)
                            {
                                PanelSecciones.init_ListarSecciones(menu_id);
                            }
                            
                            //$("#cboMenus").val($("#id_menu").val());
                            //$("#cboMenus").change();
                            //refresh(true);
                        }
                    });
                }
            });


        });

        $(document).on("click", ".btn-eliminar-seccion", function (e) {
            var sec_id = $(this).data("id");
            if (sec_id != "" || sec_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR esta Seccion?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSeccion/IntranetSeccionEliminarJson",
                            data: JSON.stringify({ sec_id: sec_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    var menu_id=$("#cboMenus").val();
                                    PanelSecciones.init_ListarSecciones(menu_id);
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
        //Mostrar Detalle de Elemento Si lo Tuviera por Elemento
        $(document).on("click", ".detalle-elemento", function (e) {
            var elem_id = $(this).data("id");
            var row = $('tr.elemento' + elem_id);
            var rowlength = $('tr.elemento' + elem_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                elem_id: elem_id
            };
            if (row.hasClass('detalle-oculto')) {
                row.removeClass('detalle-oculto');
                responseSimple({
                    url: "IntranetDetalleElemento/IntranetDetalleElementoListarxElementoIDJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var data = response.data;
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Detalle de Elemento</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>Detalle</th>';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Descripcion</th>';
                            htmlTags += '<th>Nombre</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags += '<th>Posicion</th>'
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {
                                var spanAgregarElementoModal = '';
                                var spanVerDetalleElemento = '';
                                var spanEditarDetalleElemento='';
                                var posicion='';
                                if(value.detel_posicion=='L'){
                                    posicion='Izquierda';
                                }
                                else if(value.detel_posicion=='C'){
                                    posicion='Centro';
                                }
                                else if(value.detel_posicion=='R'){
                                    posicion='Derecha';
                                }
                                else{
                                    posicion='';
                                }
                                if (value.fk_seccion_elemento > 0) {
                                    spanAgregarElementoModal += '<a href="javascript:void(0);" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-elemento-modal" data-seccionelemento="' + value.fk_seccion_elemento + '" data-rel="tooltip" title="Nuevo">Nuevo Elem. Modal</a>';
                                    spanVerDetalleElemento += '<tr class="det-elemento' + value.detel_id + ' detalle-oculto"><td data-elemento="' + value.detel_id + '" data-id="' + value.fk_seccion_elemento + '" class="elemento-modal elemento-modal'+value.fk_seccion_elemento+'"><a href="javascript:void(0);" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                    spanEditarDetalleElemento+='<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-rel="tooltip" title="Editar Detalle Elemento">Editar</a>';
                              
                                }
                                else {
                                    spanEditarDetalleElemento+='<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-rel="tooltip" title="Editar Detalle Elemento">Editar</a>';

                                    spanAgregarElementoModal += '';
                                    spanVerDetalleElemento += '<tr><td></td>';
                                }
                                htmlTags += spanVerDetalleElemento;
                                htmlTags += '<td>' + value.detel_id + '</td>';
                                htmlTags += '<td>' + value.detel_descripcion + '</td>';
                                htmlTags += '<td>' + value.detel_nombre + '</td>';
                                htmlTags += '<td>' + value.detel_orden + '</td>';
                                htmlTags += '<td>' + posicion + '</td>';
                                htmlTags += '<td>' + value.detel_estado + '</td>';
                                htmlTags += '<td>' +spanEditarDetalleElemento + spanAgregarElementoModal + '<a href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-detalle-elemento" data-fkelemento= "'+value.fk_elemento+'" data-id="' + value.detel_id + '">Eliminar</a></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    },
                })
            } else {
                row.addClass('detalle-oculto');
                if (!rownext.hasClass('detalle-oculto')) {
                    rownext.remove();
                }
            }

        });
        //Mostrar Elemento Modal Si lo Tuviera por Elemento
        $(document).on("click", ".elemento-modal", function (e) {
            var detel_id = $(this).data("elemento");
            var fk_seccion_elemento = $(this).data("id");
            var row = $('tr.det-elemento' + detel_id);
            var rowlength = $('tr.det-elemento' + detel_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                fk_seccion_elemento: fk_seccion_elemento
            };
            if (row.hasClass('detalle-oculto')) {
                row.removeClass('detalle-oculto');
                responseSimple({
                    url: 'IntranetElementoModal/IntranetElementoModalListarxSeccionElementoJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        var data = response.data;
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Elemento Modal</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>Detalle</th>';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Titulo</th>';
                            htmlTags += '<th>Tipo Elemento</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {
                                /*Ver si tiene Detalle para Agregar*/
                                var fk_tipo_elemento = value.fk_tipo_elemento;
                                var spanAgregarElemento = '';
                                var spanDetalleElemento = '';
                                var spanEditarElementoModal='';
                                if (fk_tipo_elemento == 1 || fk_tipo_elemento == 2 || fk_tipo_elemento == 3 || fk_tipo_elemento == 4) {
                                    spanAgregarElemento += '';
                                    spanDetalleElemento += '<tr><td></td>';
                                    spanEditarElementoModal+='<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento_modal" data-id="' + value.emod_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                                }
                                else {
                                    spanAgregarElemento += '<a href="javascript:void(0);" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento-modal" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.emod_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo Detalle</a>';
                                    spanDetalleElemento += '<tr class="elem-modal' + value.emod_id + ' detalle-oculto"><td data-id="' + value.emod_id + '" class="detalle-elemento-modal detalle-elemento-modal'+value.emod_id+'"><a href="javascript:void(0);" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                    spanEditarElementoModal+='<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento_modal" data-id="' + value.emod_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                                }

                                //htmlTags += '<tr class="elem-modal' + value.emod_id + '"><td data-id="' + value.emod_id + '" class="detalle-elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                htmlTags += spanDetalleElemento;
                                htmlTags += '<td>' + value.emod_id + '</td>';
                                htmlTags += '<td>' + value.emod_titulo + '</td>';
                                htmlTags += '<td>' + value.tipo_nombre + '</td>';
                                htmlTags += '<td>' + value.emod_orden + '</td>';
                                htmlTags += '<td>' + value.emod_estado + '</td>';
                                htmlTags += '<td>' +spanEditarElementoModal+ spanAgregarElemento + '<a href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-elemento-modal" data-id="' + value.emod_id + '" data-fkseccionelemento="'+value.fk_seccion_elemento+'">Eliminar</a></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    }
                });
            }
            else {
                row.addClass('detalle-oculto');
                if (!rownext.hasClass('detalle-oculto')) {
                    rownext.remove();
                }
            }
        });
        //Mostrar Detalle Elemento Modal por Elemento Modal Si lo Tuviera
        $(document).on('click', '.detalle-elemento-modal', function () {
            var emod_id = $(this).data("id");
            var row = $('tr.elem-modal' + emod_id);
            var rowlength = $('tr.elem-modal' + emod_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                fk_elemento_modal: emod_id
            };
            if (row.hasClass('detalle-oculto')) {
                row.removeClass('detalle-oculto');
                responseSimple({
                    url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalListarxElementoModalJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        var data = response.data;
                      
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Detalle Elemento Modal</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Titulo</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags+='<th>Posicion</th>';
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {
                                var posicion='';
                                if(value.detelm_posicion=='L'){
                                    posicion='Izquierda';
                                }
                                else if(value.detelm_posicion=='C'){
                                    posicion='Centro';
                                }
                                else if(value.detelm_posicion=='R'){
                                    posicion='Derecha';
                                }
                                else {
                                    posicion='';
                                }
                                htmlTags += '<td>' + value.detelm_id + '</td>';
                                htmlTags += '<td>' + value.detelm_descripcion + '</td>';
                                htmlTags += '<td>' + value.detelm_orden + '</td>';
                                htmlTags+='<td>'+posicion+'</td>';
                                htmlTags += '<td>' + value.detelm_estado + '</td>';
                                htmlTags += '<td><a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn-editar-detalle-elemento-modal" data-id="' + value.detelm_id + '">Editar</a> <a href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-detalle-elemento-modal" data-emodid="'+emod_id+'" data-id="' + value.detelm_id + '">Eliminar</a></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    }
                });
            }
            else {
                row.addClass('detalle-oculto');
                if (!rownext.hasClass('detalle-oculto')) {
                    rownext.remove();
                }
            }
        });
        //Botones para abrir modal de nuevo elemento y evento para guardar nuevo elemento
        $(document).on('click', '.btn-nuevo-elemento', function () {
            var sec_id = $(this).data('id');
            $("#div_tipo_elemento").show();
            $("#fk_seccion").val(sec_id);
            $("#elem_id").val(0);
            $(".elem-orden").hide();
            $("#elem_orden").val("");
            $("#elem_titulo").val("");  
            //bloquear algunas selecciones
            $("#cboTipoElemento").select2('destroy');
            $("#cboTipoElemento").select2();
            $('#cboTipoElemento option[value="5"]').prop('disabled', false);
            $('#cboTipoElemento option[value="6"]').prop('disabled', false);
            $('#cboTipoElemento option[value="7"]').prop('disabled', false);
            $('#cboTipoElemento option[value="8"]').prop('disabled', false);
            $('#cboTipoElemento option[value="9"]').prop('disabled', true);
            $('#cboTipoElemento option[value="10"]').prop('disabled', false);
            $('#cboTipoElemento option[value="11"]').prop('disabled', false);
            $('#cboTipoElemento option[value="12"]').prop('disabled', false);
            $('#cboTipoElemento option[value="13"]').prop('disabled', false);
            $('#cboTipoElemento option[value="14"]').prop('disabled', false);
            $('#cboTipoElemento option[value="15"]').prop('disabled', false);
            $('#cboTipoElemento option[value="16"]').prop('disabled', false);
            $('#cboTipoElemento option[value="17"]').prop('disabled', false);
            $('#cboTipoElemento option[value="18"]').prop('disabled', true);
            //
            $("#modalFormularioElemento").modal("show");
        });
        $(document).on('click', '.btn-guardar-elemento', function () {
            // $("#form_elemento").submit();
            var sec_id=$("#fk_seccion").val();
            var dataForm = $('#form_elemento').serializeFormJSON();
            var url = '';
            if ($("#elem_id").val() == 0) {
                url = 'IntranetElemento/IntranetElementoInsertarJson';
            }
            else{
                url='IntranetElemento/IntranetElementoEditarJson';
            }
            var fk_tipo_elemento=$("#cboTipoElemento").val();
            if(fk_tipo_elemento!=''){
                responseSimple({
                    url: url,
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        // PanelSecciones.init_ListarSecciones();
                        if(response.respuesta){
                            $(".details-control"+sec_id).trigger('click');
                            $(".details-control"+sec_id).trigger('click');
                        }
                        $("#modalFormularioElemento").modal("hide");
                    }
                });
            }
            else{
                messageResponse({
                    text:"Debe Seleccionar un Tipo de Elemento",
                    type:"error",
                })
            }
            
        });
        $(document).on('click', '.btn_eliminar_elemento', function () {
            var elem_id = $(this).data("id");
            var fk_seccion=$(this).data("fkseccion");
            var dataForm = {
                elem_id: elem_id,
            }
            messageConfirmation({
                content: '¿Esta seguro de ELIMINAR este Elemento?',
                callBackSAceptarComplete: function () {
                    responseSimple({
                        url: 'IntranetElemento/IntranetElementoElementoEliminarJson',
                        refresh: false,
                        data: JSON.stringify(dataForm),
                        callBackSuccess: function (response) {
                            console.log(response);
                            if (response.respuesta) {
                                $(".details-control"+fk_seccion).trigger('click');
                                $(".details-control"+fk_seccion).trigger('click');
                            }
                        },
                    })
                }
            });

            
        });
        $(document).on('click','.btn_editar_elemento',function(){
            var elem_id=$(this).data('id');
            var dataForm={
                elem_id:elem_id,
            }
            responseSimple({
                url:'IntranetElemento/IntranetElementoIdObtenerJson',
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    if(response.respuesta){
                        var elemento=response.data;
                        $("#div_tipo_elemento").hide();
                        $("#fk_seccion").val(elemento.fk_seccion);
                        $("#elem_id").val(elemento.elem_id);
                        $("#elem_titulo").val(elemento.elem_titulo);
                        $("#elem_estado").val(elemento.elem_estado);
                        $("#cboTipoElemento").val(elemento.fk_tipo_elemento);
                        $("#elem_orden").val(elemento.elem_orden);
                        $(".elem-orden").show();
                        $("#modalFormularioElemento").modal("show");
                    }
                }
            })
        })
        //Imagen o boton con Texto, si es boton con texto, input de imagen debe ocultarse
        $(document).on('change', '#cboOpcion', function () {
            if ($(this).val() == 1) {
                $(".detel-imagen").hide();
            } else {
                $(".detel-imagen").show();
            }
        })
        $(document).on('change','#cboOpcionElemModal',function(){
            if($(this).val()==1){
                $(".detelm-imagen").hide();
            }
            else{
                $(".detelm-imagen").show();
            }
        })

        //Botones para abrir modal de nuevo detalle Elemento (Caso listas de texto, citas, imagenes) y evento para agregar un nuevo Detalle de Elemento
        $(document).on('click', '.btn-nuevo-detalle-elemento', function () {
            var elem_id = $(this).data("id");
            var tipo_elemento = $(this).data("tipo");
            $("#fk_elemento").val(elem_id);
            $("#detel_id").val(0);
            $("#tituloModalDetalleElemento").text("Nuevo ");
            $("#detel_descripcion").val("");
            $("#detel_estado").val("A");

            $(".detel-imagen").show();
            $(".detel-nombre").show();
            $(".detel-posicion").show();
            $(".detel-opcion").show();
            $(".detel-estado").show();
            $(".detel-url").show();
            $(".detel-blank").show();
            $(".detel-orden").show();
            $("#detel_orden").val("");
            $("#detel_nombre_imagen_modal").text("");
            $("#detel_nombre_imagen").val("");
            $("#detel_nombre").val("");
            $("#detel_url").val("");
            $("#cboPosicion").val("");
            $("#cboOpcion").val("1");
            $("#spandetel").html("");
            $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Icono');
            $("#divdetel").hide();
            // $(".detel-descripcion").show();
            $("#detel_nombre_imagen").val("");
            if (tipo_elemento == 5 || tipo_elemento == 6) {
                $("#fk_seccion_elemento").val(2);
                $(".detel-imagen").hide();
                $("#divdetel").hide();
                $(".detel-nombre").hide();
                $(".detel-posicion").hide();
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide(); 
                
            }

            //Abren Modales fk_seccion_elemento en int_detalle_elemento debe tener un id
            else if (tipo_elemento==8||tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15) {
                //va a abrir modal
                $("#fk_seccion_elemento").val(1);
                $(".detel-imagen").hide();
                $("#divdetel").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $('#cboPosicion option[value="C"]').prop('disabled', true);
            }
            else if(tipo_elemento==16){
                $("#fk_seccion_elemento").val(1);
                $(".detel-imagen").hide();
                $("#divdetel").hide();
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $('#cboPosicion option[value="C"]').prop('disabled', false);
            }
            else if(tipo_elemento==11){
                $("#fk_seccion_elemento").val(1);
                $(".detel-posicion").hide();
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $("#detel_posicion").val("");
            }
            else if(tipo_elemento==12||tipo_elemento==17){
                $("#fk_seccion_elemento").val(0);
                $(".detel-posicion").hide();
                $("#detel_posicion").val("");
                $(".detel-opcion").hide();

            }
            else {
                $("#fk_seccion_elemento").val(0);
                $(".detel-posicion").hide();
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
            }
            $(".detel-orden").hide();
            $("#modalFormularioDetalleElemento").modal("show");
        })
        $(document).on('click', '.btn-guardar-detalle-elemento', function () {
            //$("#form_detalle_elemento").submit();
            var elem_id=$("#fk_elemento").val();
            var dataForm = new FormData(document.getElementById("form_detalle_elemento"));
            var url = '';
            if ($("#detel_id").val() == 0) {
                url = 'IntranetDetalleElemento/IntranetDetalleElementoInsertarJson';
            }
            else{
                url='IntranetDetalleElemento/IntranetDetalleElementoEditarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $("#modalFormularioDetalleElemento").modal("hide");
                    }
                    $(".detalle-elemento"+elem_id).trigger('click');
                    $(".detalle-elemento"+elem_id).trigger('click');
                }
            })
        });
        $(document).on('click', '.btn-eliminar-detalle-elemento', function () {
            var fk_elemento=$(this).data("fkelemento");
            var detel_id = $(this).data("id");
            var dataForm = {
                detel_id: detel_id,
            }
            messageConfirmation({
                content: '¿Esta seguro de ELIMINAR este Detalle?',
                callBackSAceptarComplete: function () {
                    responseSimple({
                        url: 'IntranetDetalleElemento/IntranetDetalleElementoEliminarJson',
                        refresh: false,
                        data: JSON.stringify(dataForm),
                        callBackSuccess: function (response) {
                            if (response.respuesta) {
                                $(".detalle-elemento"+fk_elemento).trigger('click');
                                $(".detalle-elemento"+fk_elemento).trigger('click');
                            }
                        },
                    })
                }
            });
           
        })

        $(document).on('click','.btn_editar_detalle_elemento',function(){
            var detel_id=$(this).data('id');
            var dataForm={
                detel_id:detel_id,
            }
            responseSimple({
                url:'IntranetDetalleElemento/IntranetDetalleElementoIdObtenerJson',
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    var data=response.data;
                    console.log(data);
                    if(response.respuesta){
                        $("#divdetel").hide();
                        $('#detel_id').val(data.detel_id);
                        $('#fk_elemento').val(data.fk_elemento)
                        $("#spandetel").html("");
                        $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Icono');
                        if(data.detel_extension!=""){
                            $("#detel_nombre_imagen_modal").text("Nombre: " + data.detel_nombre+"."+data.detel_extension);
                            $("#detel_nombre_imagen").val(data.detel_nombre+"."+data.detel_extension);
                            // $("#detel_fecha_imagen").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#icono_actual_detel").attr("src", "data:image/gif;base64," + data.detel_nombre_imagen);
                            $("#divdetel").show();
                            $(".detel-imagen").show();
                            $(".detel-nombre").show();
                            $(".detel-posicion").hide();
                            $("#detel_posicion").val();
                            $(".detel-opcion").hide();
                            if(data.fk_tipo_elemento==12||data.fk_tipo_elemento==17){
                                $(".detel-url").show();
                                $(".detel-blank").show();
                            }
                            else{
                                $(".detel-url").hide();
                                $(".detel-blank").hide();
                            }
                        }
                        else{
                            $(".detel-imagen").hide();
                            $(".detel-nombre").hide();
                            $(".detel-posicion").hide();
                            $(".detel-opcion").hide();
                            $(".detel-url").hide();
                            $(".detel-blank").hide();
                            $("#detel_nombre_imagen_modal").text("");
                            $("#detel_nombre_imagen").val("");
                            $("#detel_url").val("");
                        }
                        if(data.detel_posicion!=''&&data.detel_extension==""){
                            $(".detel-posicion").show();
                        }
                        $("#detel_orden").val(data.detel_orden);
                        $("#cboPosicion").val(data.detel_posicion);
                        $("#tituloModalDetalleElemento").text("Editar ");
                        $("#detel_descripcion").val(data.detel_descripcion);
                        $("#detel_estado").val(data.detel_estado);
                        $("#detel_url").val(data.detel_url);
                        $("#detel_blank").val(data.detel_blank);
                        $(".detel-orden").show();
                        $('#modalFormularioDetalleElemento').modal('show');
                    }
                },
            })
            
        })

        //Botones para abrir modal de Nuevo Elemento Modal y evento para agregar un Nuevo elemento al Modal
        $(document).on('click', '.btn-nuevo-elemento-modal', function () {
            var fk_seccion_elemento = $(this).data('seccionelemento');
            $("#fk_seccion_elemento_modal").val(fk_seccion_elemento);
            $("#emod_id").val(0);
            $("#div_tipo_elemento_modal").show();
            $("#emod_titulo").val("");
            $(".emod-orden").hide();
            $("#emod_orden").val("");
            $("#cboTipoElementoModal").select2('destroy');
            $("#cboTipoElementoModal").select2();
            $('#cboTipoElementoModal option[value="5"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="6"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="7"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="8"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="9"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="10"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="11"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="12"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="13"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="14"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="15"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="16"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="17"]').prop('disabled', false);
            $('#cboTipoElementoModal option[value="18"]').prop('disabled', true);

            $("#modalFormularioElementoModal").modal("show");
            
        });
        $(document).on('click', '.btn-guardar-elemento-modal', function () {
            //$("#form_elemento_modal").submit();
            // $("#fk_seccion_elemento").val();
            var fk_seccion_elemento=$("#fk_seccion_elemento_modal").val();
            var dataForm = $("#form_elemento_modal").serializeFormJSON();
            var url = '';
            if ($("#emod_id").val() == 0) {
                url = 'IntranetElementoModal/IntranetElementoModalInsertarJson';
            }
            else{
                url='IntranetElementoModal/IntranetElementoModalEditarJson';
            }
            var fk_tipo_elemento_modal=$("#cboTipoElementoModal").val();
            if(fk_tipo_elemento_modal!=""){
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            $("#modalFormularioElementoModal").modal("hide");
                        }
                        $(".elemento-modal"+fk_seccion_elemento).trigger('click');
                        $(".elemento-modal"+fk_seccion_elemento).trigger('click');
                        //PanelSecciones.init_ListarSecciones();
                    },
                });
            }
            else{
                messageResponse({
                    text:"Debe Seleccionar un Tipo de Elemento",
                    type:"error",
                })
            }
            
        });
        $(document).on('click', '.btn-eliminar-elemento-modal', function () {
            var fk_seccion_elemento=$(this).data("fkseccionelemento");
            var emod_id = $(this).data("id");
            var dataForm = {
                emod_id: emod_id,
            }
            messageConfirmation({
                content: '¿Esta seguro de ELIMINAR este Elemento Modal?',
                callBackSAceptarComplete: function () {
                    responseSimple({
                        url: 'IntranetElementoModal/IntranetElementoModalEliminarJson',
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            if (response.respuesta) {
                                $(".elemento-modal"+fk_seccion_elemento).trigger('click');
                                $(".elemento-modal"+fk_seccion_elemento).trigger('click');
                            }
                        }
                    })
                }
            });
            
        });
        $(document).on('click','.btn_editar_elemento_modal',function(){
            var emod_id=$(this).data('id');
            var dataForm={
                emod_id:emod_id,
            }
            responseSimple({
                url:'IntranetElementoModal/IntranetElementoModalIdObtenerJson',
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    if(response.respuesta){
                        var data=response.data;
                        if(data.fk_tipo_elemento==1||data.fk_tipo_elemento==2||data.fk_tipo_elemento==3||data.fk_tipo_elemento==4){
                            $("#div_tipo_elemento_modal").show();
                            $("#cboTipoElementoModal").select2('destroy');
                            $("#fk_seccion_elemento_modal").val(data.fk_seccion_elemento);
                            $("#cboTipoElementoModal").select2();
                            $('#cboTipoElementoModal option[value="5"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="6"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="7"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="9"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="10"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="11"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="12"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="17"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="18"]').prop('disabled', true);
                            //abren Elemento Modales
                            $('#cboTipoElementoModal option[value="8"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="13"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="14"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="15"]').prop('disabled', true);
                            $('#cboTipoElementoModal option[value="16"]').prop('disabled', true);

                        }
                        else{
                            $("#div_tipo_elemento_modal").hide();
                            $("#fk_seccion_elemento_modal").val(data.fk_seccion_elemento);
                        }
                        $("#emod_titulo").val(data.emod_titulo);
                        $("#emod_id").val(data.emod_id);
                        $("#emod_estado").val(data.emod_estado);
                        $(".emod-orden").show();
                        $("#emod_orden").val(data.emod_orden);
                        $("#cboTipoElementoModal").val(data.fk_tipo_elemento)
           
                        $("#modalFormularioElementoModal").modal('show');
                    }
                }
            })
        });
        //Botones para agregar un nuevo Detalle de Elemento al Modal(caso listas de texto, citas o Imagenes) y evento para guardar ese detalle
        $(document).on('click', '.btn-nuevo-detalle-elemento-modal', function () {
            var emod_id = $(this).data("id");
            var tipo_elemento = $(this).data("tipo");

            $("#fk_elemento_modal").val(emod_id);
            $("#detelm_id").val(0);
            $(".detelm-imagen").show();
            $(".detelm-nombre").show();
            $(".detelm-posicion").show();
            $(".detelm-opcion").show();
            $("#spandetelm").html("");
            $("#spandetelm").append('<i class="fa fa-upload"></i>  Subir Icono');
            $("#divdetelm").hide();
            // $("#cboTipoElementoModal").select2('destroy');
            // $("#cboTipoElementoModal").select2();
            // $('#cboTipoElementoModal option[value="9"]').prop('disabled', true);
            // $('#cboTipoElementoModal option[value="11"]').prop('disabled', true);
            // $('#cboTipoElementoModal option[value="10"]').prop('disabled', true);
            // $('#cboTipoElementoModal option[value="18"]').prop('disabled', true);
            if (tipo_elemento == 5 || tipo_elemento == 6) {
                $("#fk_seccion_elemento_modal_").val(2);
                $(".detelm-imagen").hide();
                $(".detelm-nombre").hide();
                $(".detelm-posicion").hide();
                $(".detelm-opcion").hide();
            }
            else if (tipo_elemento == 8 || tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15) {
                //va a abrir modal
                $("#fk_seccion_elemento_modal_").val(1);
                $(".detelm-imagen").hide();
                $("#divdetelm").hide();
            }
            else if(tipo_elemento==16){
                $("#fk_seccion_elemento_modal_").val(1);
                $(".detelm-imagen").hide();
                $("#divdetelm").hide();
                $(".detelm-opcion").hide();
            }
            else {
                $("#fk_seccion_elemento_modal_").val(0);
                $(".detelm-opcion").hide();
                $(".detelm-posicion").hide();
            }
            $(".detelm-orden").hide();
            $("#detelm_orden").val("");
            $("#modalFormularioDetalleElementoModal").modal("show");
            
        })
        $(document).on('click', '.btn-guardar-detalle-elemento-modal', function () {
            var emod_id=$("#fk_elemento_modal").val();
            //$("#form_detalle_elemento_modal").submit();
            var dataForm = new FormData(document.getElementById("form_detalle_elemento_modal"));
            var url = '';
            if ($("#detelm_id").val() == 0) {
                url = 'IntranetDetalleElementoModal/IntranetDetalleElementoModalInsertarJson';
            }
            else{
                url='IntranetDetalleElementoModal/IntranetDetalleElementoModalEditarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    
                    console.log(response);
                    if (response.respuesta) {
                        $("#modalFormularioDetalleElementoModal").modal("hide");
                    }
                    $(".detalle-elemento-modal"+emod_id).trigger('click');
                    $(".detalle-elemento-modal"+emod_id).trigger('click');
                   
                    //PanelSecciones.init_ListarSecciones();
                }
            })
        })
        $(document).on('click', '.btn-editar-detalle-elemento-modal', function () {
            var detelm_id = $(this).data("id");
            var dataForm = {
                detelm_id:detelm_id,
            }
            responseSimple({
                url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalIdObtenerJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if(response.respuesta){
                        var data=response.data;
                        $("#divdetelm").hide();
                        $('#detelm_id').val(data.detelm_id);
                        $('#fk_elemento_modal').val(data.fk_elemento_modal)
                        $("#spandetelm").html("");
                        $("#spandetelm").append('<i class="fa fa-upload"></i>  Subir Icono');
                        $(".detelm-orden").show();
                        if(data.detelm_extension!=""){
                            $("#detelm_nombre_imagen_modal").text("Nombre: " + data.detelm_nombre+"."+data.detelm_extension);
                            $("#detelm_nombre_imagen").val(data.detelm_nombre+"."+data.detelm_extension);
                            // $("#detel_fecha_imagen").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#icono_actual_detelm").attr("src", "data:image/gif;base64," + data.detelm_nombre_imagen);
                            $("#divdetelm").show();
                            $(".detelm-imagen").show();
                            $(".detelm-nombre").show();
                            $(".detelm-posicion").hide();
                            $(".detelm-opcion").hide();
                        }
                        else{
                            $(".detelm-imagen").hide();
                            $(".detelm-nombre").hide();
                            $(".detelm-posicion").hide();
                            $(".detelm-opcion").hide();
                        }
                        if(data.detelm_posicion!=''&&data.detelm_extension==""){
                            $(".detelm-posicion").show();
                        }
                       
                        $("#detelm_orden").val(data.detelm_orden);
                        $("#detelm-posicion").val(data.detelm_posicion);
                        $("#tituloModalDetalleElementoModal").text("Editar ");
                        $("#detelm_descripcion").val(data.detelm_descripcion);
                        $("#detelm_estado").val(data.detelm_estado);
                        $("#modalFormularioDetalleElementoModal").modal('show');
                    }
                }
            })
        })
        $(document).on('click', '.btn-eliminar-detalle-elemento-modal', function () {
            var emod_id=$(this).data("emodid");
            var detelm_id = $(this).data("id");
            var dataForm = {
                detelm_id: detelm_id,
            }
            messageConfirmation({
                content: '¿Esta seguro de ELIMINAR este Detalle de Elemento Modal?',
                callBackSAceptarComplete: function () {
                    responseSimple({
                        url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalEliminarJson',
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            $(".detalle-elemento-modal"+emod_id).trigger('click');
                            $(".detalle-elemento-modal"+emod_id).trigger('click');
                        }
                    })
                }
            });
            
        })

        //Change en Imagenes
        //imagen de Detalle Elemento
        $(document).on("change", "#detel_nombre", function () {
            var _image = $('#detel_nombre')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension != "jpg" && extension != "png"&&extension!="jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg"||actualicon=='jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetel").html("");
                    $("#spandetel").append(icon + " " + nombre + "... ." + actualicon);
                    //$("#img_ubicacion").val(nombre + "." + actualicon);
                    $("#spandetel").css({ 'font-size': '10px' });  
                }
            }
            else {
                $("#spandetel").html("");
                $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Icono');
            }
        })

        //imagen de Detalle Elemento Modal
        $(document).on("change", "#detelm_nombre", function () {
            var _image = $('#detelm_nombre')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension != "jpg" && extension != "png"&&extension!="jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg"||actualicon=="jpeg") {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetelm").html("");
                    $("#spandetelm").append(icon + " " + nombre + "... ." + actualicon);
                    $("#detel_nombre_imagen").val(nombre + "." + actualicon);
                    $("#spandetelm").css({ 'font-size': '10px' });  
                }
            }
            else {
                $("#spandetelm").html("");
                $("#spandetelm").append('<i class="fa fa-upload"></i>  Subir Icono');
            }
        })
        $(document).on('input','.input-number',function(){
            this.value = this.value.replace(/[^0-9]/g,'');
        })
    };

    var _metodos = function () {
      
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _metodos();
            
        },
        init_ListarSecciones: function (menu_id) {
            _ListarSecciones(menu_id);
        }
    }
}();

function MostrarDetalle(row) {
    var table = '';
    if (row.length > 0) {
        table += '<table class="table table-bordered table-sm"><tr><th colspan="7"><legend>Elementos</legend></th></tr><tr class="thead-dark">';
        table += '<th>Detalle</th>';
        table += '<th>ID</th>';
        table += '<th>titulo</th>';
        table += '<th>Estado</th>';
        table += '<th>Orden</th>';
        table += '<th>Tipo</th>';
        table += '<th>Acciones</th></tr>';
        $.each(row, function (index, value) {
            var fk_tipo_elemento = value.fk_tipo_elemento;
            var spanNuevoElemento = '';
            var spanDetalleElemento = '';
            var spanEditarElemento = '';
            var spanEliminarElemento = '';
            if (fk_tipo_elemento == 1 || fk_tipo_elemento == 2 || fk_tipo_elemento == 3 || fk_tipo_elemento == 4) {
                spanNuevoElemento = '';
                spanDetalleElemento = '<tr><td></td>';
                spanEditarElemento = '<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                spanEliminarElemento = '<a  href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-fkseccion="'+value.fk_seccion+'" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
            }
            else if (fk_tipo_elemento == 8 || fk_tipo_elemento == 13 || fk_tipo_elemento == 14 || fk_tipo_elemento == 15 || fk_tipo_elemento == 16) {
                spanNuevoElemento = '<a href="javascript:void(0);" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.elem_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo Detalle</a>';
                spanDetalleElemento += '<tr class="elemento' + value.elem_id + ' detalle-oculto"><td data-id="' + value.elem_id + '" class="detalle-elemento detalle-elemento'+value.elem_id+'">' + '<a href="javascript:void(0);" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>' + '</td>';
                spanEliminarElemento = '<a  href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-fkseccion="'+value.fk_seccion+'" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
                spanEditarElemento = '<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="Editar">Editar</a>';
            }
            else {
                spanNuevoElemento = '<a href="javascript:void(0);" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.elem_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo Detalle</a>';
                spanDetalleElemento += '<tr class="elemento' + value.elem_id + ' detalle-oculto"><td data-id="' + value.elem_id + '" class="detalle-elemento detalle-elemento'+value.elem_id+'">' + '<a href="javascript:void(0);" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>' + '</td>';
                spanEditarElemento = '<a href="javascript:void(0);" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                spanEliminarElemento = '<a  href="javascript:void(0);" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-fkseccion="'+value.fk_seccion+'" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
            }

            table += spanDetalleElemento;
            table += '<td>' + value.elem_id + '</td>';
            table += '<td>' + value.elem_titulo + '</td>';
            table += '<td>' + (value.elem_estado == 'A' ? 'Activo' : 'Inactivo') + '</td>';
            table += '<td>' + value.elem_orden + '</td>';
            table += '<td>' + value.tipo_nombre + '</td>';
            table += '<td>' + spanEditarElemento +spanNuevoElemento + spanEliminarElemento + '</td></tr>';
        });
        table += '</table>';

    }
    return table;
}
document.addEventListener('DOMContentLoaded', function () {
    PanelSecciones.init();
});     