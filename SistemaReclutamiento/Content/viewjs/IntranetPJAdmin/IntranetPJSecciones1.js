
var PanelContenido = function () {
    var urislista = [basePath + '/intranetPJ/agenda', basePath + '/intranetPJ/descargas'];
    var _sort_seccion = function (menu_id) {
        $('#accordion_' + menu_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('#accordion_' + menu_id +' div.panel-default')
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        sec_id: $(this).data("id"),
                        sec_orden: (index + 1)
                    });
                    $(this).find("span.sec_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "IntranetSeccion/IntranetSeccionEditarOrdenJson",
                    data: JSON.stringify({ arraySecciones: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarSecciones(menu_id);
                        }
                    }
                });
            }
        });
    };

    var _sort_elemento = function (seccion_id) {
        $('.tbody_elemento_' + seccion_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_elemento_' + seccion_id + ' tr.sec_' + seccion_id);
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        elem_id: $(this).data("id"),
                        elem_orden: (index + 1)
                    });
                    $(this).find("span.elem_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "IntranetElemento/IntranetElementoEditarOrdenJson",
                    data: JSON.stringify({ arrayElementos: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarSecciones(menu_id);
                        }
                    }
                });
            }
        });
    };

    var _sort_detalle_elemento = function (elemento_id) {
        $('.tbody_detalle_elemento_' + elemento_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_detalle_elemento_' + elemento_id + ' tr.elem_' + elemento_id);
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        detel_id: $(this).data("id"),
                        detel_orden: (index + 1)
                    });
                    $(this).find("span.detelem_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "IntranetDetalleElemento/IntranetDetalleElementoEditarOrdenJson",
                    data: JSON.stringify({ arrayDetElemento: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarSecciones(menu_id);
                        }
                    }
                });
            }
        });
    };

    var _sort_elemento_modal = function (detal_elem_id) {
        $('.tbody_elemento_modal_' + detal_elem_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_elemento_modal_' + detal_elem_id + ' tr.elem_modal_' + detal_elem_id);
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        emod_id: $(this).data("id"),
                        emod_orden: (index + 1)
                    });
                    $(this).find("span.elem_modal_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "IntranetElementoModal/IntranetElementoModalEditarOrdenJson",
                    data: JSON.stringify({ arrayElementoModal: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarSecciones(menu_id);
                        }
                    }
                });
            }
        });
    };
    
    var _sort_detalle_elemento_modal = function (elementom_id) {
        $('.tbody_detalle_elemento_modal_' + elementom_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_detalle_elemento_modal_' + elementom_id + ' tr');
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        detelm_id: $(this).data("id"),
                        detelm_orden: (index + 1)
                    });
                    $(this).find("span.det_elem_modal_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "IntranetDetalleElementoModal/IntranetDetalleElementoModalEditarOrdenJson",
                    data: JSON.stringify({ arrayDetElementoModal: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarSecciones(menu_id);
                        }
                    }
                });
            }
        });
    };

    var _ListarMenus = function () {
        $("#tabmenu").html("");
        $("#tabcontenido").html("");
        responseSimple({
            url: "IntranetMenu/IntranetMenuSeccionListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var li = "";
                var div = "";
                var activo = "";
                var contenidoDiv = "";
                var menu_id_ = 0;
                $.each(data, function (index, value) {
                    activo = (index == 0) ? "in active" : "";
                    
                    li += '<li class="' + activo + '" data-id="' + value.menu_id +'">' +
                        '<a data-toggle="tab" href="#_tab_contenido_' + value.menu_id + '"> ' +
                                '<i class="red ace-icon fa fa-tachometer bigger-110"></i> '+
                                value.menu_titulo+
                            '</a >' +
                        '</li >';
                    
                    if (index == 0) {
                        var data_seccion = value.secciones;
                        var contentd = "";
                        $.each(data_seccion, function (index_, value_) {
                            var estadoText = (value_.sec_estado == "A") ? "Activo" : "Inactivo";
                            var estadoClase = (value_.sec_estado == "A") ? "success" : "danger";
                            var estadoActivo = (value_.sec_estado == "A") ? "selected" : "";
                            var estadoInActivo = (value_.sec_estado == "I") ? "selected" : "";
                            contentd += '<div class="panel panel-default" data-id="' + value_.sec_id + '">' +
                                            '<div class="panel-heading">'+
                                                '<h4 class="panel-title">'+
                                                        '<a class="accordion-toggle cabecera-seccion collapsed" data-id="' + value_.sec_id +'" data-toggle="collapse" data-parent="accordion_' + value.menu_id + '" href="#collapse_' + value_.sec_id + '">'+
                                                        '<i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                '<span class="sec_orden label label-default label-white middle">'+(index_ +1)+ '</span> - Seccion ID_' + value_.sec_id + ' <div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;"><span id="span_estado_' + value_.sec_id + '" class="label label-' + estadoClase+' label-white middle">' + estadoText+'</span></div>'+
									                '</a>'+
                                                '</h4>'+
								            '</div >'+

                                            '<div class="panel-collapse collapse " id="collapse_' + value_.sec_id + '">' +
                                               '<div class="panel-body" id="menu_panel' + value_.sec_id + '">' +
                                                    '<div class="row">'+
                                                        '<div class="col-md-12"><div class="row">' +
                                                                '<div class="col-md-2">' +
                                                                    '<div class="input-group">' +
                                                                        '<span class="input-group-addon">' +
                                                                        'Estado' +
                                                                        '</span>' +
                                '<select data-id="' + value_.sec_id + '" data-menu_id="' + value.menu_id + '" class="form-control input-sm select-estado-seccion"><option value="A" ' + estadoActivo + '>Activo</option><option value="I" ' + estadoInActivo +'>Inactivo</option></select>' +
                                                                    '</div>' +
                                                                '</div>' +
                                '<div class="col-md-10"><div class="row">' +
                                '<div class="col-md-3 col-md-offset-4 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_elemento_seccion" data-id="' + value_.sec_id + '" data-rel="tooltip" title="Ordenar Elementos"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                                '<div class="col-md-3 col-sm-5 col-xs-12"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento_seccion" data-id="' + value_.sec_id + '" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Elemento </button></div>' +
                                '<div class="col-md-2 col-sm-5 col-xs-12"><button class="btn btn-white btn-danger btn-sm btn-block btn-round btn-eliminar-seccion" data-id="' + value_.sec_id + '" data-menu_id="' + value.menu_id + '" data-rel="tooltip" title="Eliminar Seccion"><i class="ace-icon fa fa-trash"></i> Eliminar</button></div>' +
                                                                '</div></div>' +
                                                        '</div></div>' +
                                                        '<div class="col-md-12"><div class="hr hr8 hr-double"></div><div class="" id="seccion_lista_elemento_' + value_.sec_id+'"></div></div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>'+
							            '</div>';
                        });
                        contenidoDiv += '<div id="accordion_' + value.menu_id + '" class="accordion-style1 panel-group">' + contentd + '</div>';
                    }
                    else {
                        contenidoDiv = "";
                    }
                    div += '<div id="_tab_contenido_' + value.menu_id + '" class="tab-pane ' + activo + '">' +
                        '<div class="row" style="margin-bottom:8px">' +
                        '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nueva_seccion" data-id="' + value.menu_id + '" data-rel="tooltip" title="Nueva Seccion"><i class="ace-icon fa fa-file"></i> Nueva Seccion </button></div>' +
                        '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_seccion" data-id="' + value.menu_id + '" data-rel="tooltip" title="Ordenar Seccion(es)"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar Seccion(es) </button></div>' +
                        '</div>' +
                                contenidoDiv +
                        '</div>';
                });
                $("#tabmenu").html(li);
                $("#tabcontenido").html(div);
                
            }
        });
    };

    var _ListarSecciones = function (menu_id) {
        $("#_tab_contenido_" + menu_id).html("");
        var dataForm = {
            menu_id: menu_id
        };
        responseSimple({
            url: "IntranetSeccion/IntranetSeccionListarTodoxMenuIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var contentd = "";
                $.each(data, function (index, value_) {
                    var estadoText = (value_.sec_estado == "A") ? "Activo" : "Inactivo";
                    var estadoClase = (value_.sec_estado == "A") ? "success" : "danger";
                    var estadoActivo = (value_.sec_estado == "A") ? "selected" : "";
                    var estadoInActivo = (value_.sec_estado == "I") ? "selected" : "";
                    contentd += '<div class="panel panel-default" data-id="' + value_.sec_id + '">' +
                        '<div class="panel-heading">' +
                        '<h4 class="panel-title">' +
                        '<a class="accordion-toggle cabecera-seccion collapsed" data-id="' + value_.sec_id + '" data-toggle="collapse" data-parent="accordion_' + menu_id + '" href="#collapse_' + value_.sec_id + '">' +
                        '<i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>' +
                        '<span class="sec_orden label label-default label-white middle">'+(index + 1) + '</span> - Seccion ID_' + value_.sec_id + ' <div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;"><span id="span_estado_' + value_.sec_id + '" class="label label-' + estadoClase + ' label-white middle">' + estadoText + '</span></div>' +
                        '</a>' +
                        '</h4>' +
                        '</div >' +

                        '<div class="panel-collapse collapse " id="collapse_' + value_.sec_id + '">' +
                        '<div class="panel-body" id="menu_panel' + value_.sec_id + '">' +
                        '<div class="row">' +
                        '<div class="col-md-12"><div class="row">' +
                        '<div class="col-md-2">' +
                        '<div class="input-group">' +
                        '<span class="input-group-addon">' +
                        'Estado' +
                        '</span>' +
                        '<select data-id="' + value_.sec_id + '" data-menu_id="' + menu_id + '" class="form-control input-sm select-estado-seccion"><option value="A" ' + estadoActivo + '>Activo</option><option value="I" ' + estadoInActivo+'>Inactivo</option></select>' +
                        '</div>' +
                        '</div>' +
                        '<div class="col-md-10"><div class="row">' +
                        '<div class="col-md-3 col-md-offset-4 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_elemento_seccion" data-id="' + value_.sec_id + '" data-rel="tooltip" title="Ordenar Elementos"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                        '<div class="col-md-3 col-sm-5 col-xs-12"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento_seccion" data-id="' + value_.sec_id + '" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Elemento </button></div>' +
                        '<div class="col-md-2 col-sm-5 col-xs-12"><button class="btn btn-white btn-danger btn-sm btn-block btn-round btn-eliminar-seccion" data-id="' + value_.sec_id + '" data-menu_id="' + menu_id + '" data-rel="tooltip" title="Eliminar Seccion"><i class="ace-icon fa fa-trash"></i> Eliminar</button></div>' +
                        
                        '</div></div>' +
                        '</div></div>' +
                        '<div class="col-md-12"><div class="hr hr8 hr-double"></div><div class="" id="seccion_lista_elemento_' + value_.sec_id + '"></div></div>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                });
                contentd = '<div class="row" style="margin-bottom:8px">' +
                    '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nueva_seccion" data-id="' + menu_id + '" data-rel="tooltip" title="Nueva Seccion"><i class="ace-icon fa fa-file"></i> Nueva Seccion </button></div>' +
                    '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_seccion" data-id="' + menu_id + '" data-rel="tooltip" title="Ordenar Seccion(es)"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar Seccion(es) </button></div>' +
                    '</div>' +
                    '<div id="accordion_' + menu_id + '" class="accordion-style1 panel-group">' + contentd + '</div>';

                $("#_tab_contenido_" + menu_id).html(contentd);

            }
        });
    };

    var _ListarElementos = function (sec_id) {
        var dataForm = {
            sec_id: sec_id
        };
        $("#seccion_lista_elemento_" + sec_id).html("");
        responseSimple({
            url: "IntranetElemento/IntranetElementoListarxSeccionIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var rows = response.data;
                var tr = "";
                $.each(rows, function (index, value) {
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_elemento" data-id="' + value.elem_id + '" data-seccion_id="' + sec_id +'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_elemento" data-id="' + value.elem_id + '" data-seccion_id="' + sec_id+'"><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.elem_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var tipo = value.fk_tipo_elemento;
                    var clasedetalle = "blue";
                    var clasedetalleboton = "btn_detalle_elemento";
                    if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                        clasedetalle = "grey";
                        clasedetalleboton = "";
                    }

                    var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.elem_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                    tr += '<tr id="elemento_' + value.elem_id+'" data-id="' + value.elem_id + '" data-orden="' + value.elem_orden + '" class="sec_' + sec_id + '" data-tipo="' + value.fk_tipo_elemento + '"><td class="center">' + detalle + '</td><td><span class="elem_orden label label-default label-white middle">' + (index + 1) + '</span> ' + value.tipo_nombre + '</td><td>' + value.elem_titulo + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });
                if (rows.length == 0) {
                    tr = '<tr><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr>';
                }


                tr = '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody class="tbody_elemento_' + sec_id+'">' + tr + '</tbody></table>';
                $("#seccion_lista_elemento_" + sec_id).html(tr);
            }
        })
    };

    var _ListarDetalleElementos = function (elemento_id) {
        var dataForm = {
            elem_id: elemento_id
        };
        var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");
        responseSimple({
            url: "IntranetDetalleElemento/IntranetDetalleElementoListarxElementoIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var rows = response.data;
                var tr = "";
                $.each(rows, function (index, value) {
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + elemento_id +'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + elemento_id+'"><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.detel_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var posicion = '';
                    if (value.detel_posicion == 'L') {
                        posicion = 'Izquierda';
                    }
                    else if (value.detel_posicion == 'C') {
                        posicion = 'Centro';
                    }
                    else if (value.detel_posicion == 'R') {
                        posicion = 'Derecha';
                    }
                    else {
                        posicion = '';
                    };

                    var tipo = $("#elemento_" + elemento_id).data("tipo");
                    var clasedetalle = "blue";
                    var clasedetalleboton = "btn_elemento_modal";
                    var columan_imagen = "";
                    var columna_ubicacion = "";
                    if (tipo == 17 || tipo == 12 || tipo == 5 || tipo == 7 || tipo == 9 || tipo == 18) {
                        clasedetalle = "grey";
                        clasedetalleboton = "";
                    }

                    if (tipo == 5) {
                        columan_imagen = "hidden";
                        columna_ubicacion = "hidden";
                    }

                    if (tipo == 16) {
                        columan_imagen = "hidden";
                    }

                    if (tipo == 17 || tipo == 12 || tipo == 11 || tipo == 7 || tipo == 9 || tipo == 18) {
                        columna_ubicacion = "hidden";
                        if (tipo == 11|| tipo==7) {
                            value.detel_descripcion = "IMAGEN";
                        }
                    }

                    

                    var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.detel_id + '" data-seccion="' + value.fk_seccion_elemento + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                    var nombre = value.detel_nombre + '.' + value.detel_extension;
                    if (value.detel_extension == "") {
                        nombre = "";
                    }
                    if ((tipo == 8 || tipo == 14 )&& value.detel_posicion=="L") {
                        nombre = "TEXTO";
                    }
                    if ((tipo == 8 || tipo == 14) && value.detel_posicion == "R") {
                        value.detel_descripcion = "IMAGEN";
                    }

                    if ((tipo == 13 || tipo == 15) && value.detel_posicion == "L") {
                        value.detel_descripcion = "IMAGEN";
                    }
                    if ((tipo == 13 || tipo == 15) && value.detel_posicion == "R") {
                        nombre = "TEXTO";
                    }

                    tr += '<tr class="elem_' + elemento_id + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' + (index + 1) + '</span> ' + value.detel_descripcion + '</td><td class="' + columan_imagen + '">' + nombre + '</td><td class="' + columna_ubicacion + '" >' + posicion + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });

                var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                    '<div class="col-md-3 col-md-offset-6 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Ordenar Detalle Elemento"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                    '<div class="col-md-3 col-sm-5 col-xs-12"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Detalle Elemento </button></div>' +
                    
                    '</div>';

                
                var columan_imagen = "";
                var columna_ubicacion = "";
                if (tipo_elemento == 5) {
                    columan_imagen = "hidden";
                    columna_ubicacion = "hidden";
                }

                if (tipo_elemento == 17 || tipo_elemento == 12 || tipo_elemento == 11 || tipo_elemento == 7 || tipo_elemento == 9 || tipo_elemento == 18) {
                    columna_ubicacion = "hidden";
                }

                if (tipo_elemento == 16) {
                    columan_imagen = "hidden";
                }

                if (rows.length > 0) {
                    tr = '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_ubicacion + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' + elemento_id + '">' + tr + '</tbody></table>';
                    $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + boton_nuevo + '' + tr + '</div></div></td>');
                }
                else {
                    tr = '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_ubicacion + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody></table>';
                    $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5"><div class="table-detail">' + boton_nuevo + ''+tr+'</div></td>');
                }
            }
        });
    };

    var _ListarElementosModal = function (fk_seccion_elemento, detalle_elemento_id) {
        var dataForm = {
            fk_seccion_elemento: fk_seccion_elemento
        };

        responseSimple({
            url: "IntranetElementoModal/IntranetElementoModalListarxSeccionElementoJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var rows = response.data;
                console.log(rows.length);
                var tr = "";
                $.each(rows, function (index, value) {
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_elemento_modal" data-id=' + value.emod_id + ' data-detal_elem_id="' + detalle_elemento_id+'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_elemento_modal" data-id=' + value.emod_id + ' data-seccion_id="' + fk_seccion_elemento + '" data-detal_elem_id="' + detalle_elemento_id +'"><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.emod_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };


                    var tipo = value.fk_tipo_elemento;
                    var clasedetalle = "blue";
                    var clasedetalleboton = "btn_detalle_elemento_modal";
                    if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                        clasedetalle = "grey";
                        clasedetalleboton = "";
                    }

                    var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.emod_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                    tr += '<tr class="elem_modal_' + detalle_elemento_id + '" id="elemento_modal_' + value.emod_id + '" data-id="' + value.emod_id + '" data-orden="' + value.emod_orden + '" data-tipo="' + value.fk_tipo_elemento + '"><td class="center">' + detalle + '</td><td><span class="elem_modal_orden label label-white middle label-default">' + (index + 1) + '</span> '  + value.tipo_nombre + '</td><td>' + value.emod_titulo + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });

                var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                    '<div class="col-md-3 col-md-offset-6 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_elemento_modal" data-id="' + detalle_elemento_id + '" data-rel="tooltip" title="Ordenar  Elemento"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                    '<div class="col-md-3 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento_modal" data-seccion="' + fk_seccion_elemento + '" data-id="' + detalle_elemento_id + '" data-rel="tooltip" title="Nuevo Elemento Modal"> <i class="fa fa-file"></i> Nuevo Elemento Modal</button></div>' +
                    '</div>';

                tr = boton_nuevo + '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody class="tbody_elemento_modal_' + detalle_elemento_id + '">' + tr + '</tbody></table>';
    
                if (rows.length > 0) {
                    $('#tr_elemento_contenido_modal_' + detalle_elemento_id).html('<td colspan="6" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                }
                else {
                    var tr = boton_nuevo + '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody></table>';
                    $('#tr_elemento_contenido_modal_' + detalle_elemento_id).html('<td colspan="6" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr +'</div></div></td>');
                }
            }
        });
    };

    var _ListarDetalleElementosModal = function (elemento_modal_id) {
        var dataForm = {
            fk_elemento_modal: elemento_modal_id
        };
        var tipo_elemento = $("#elemento_modal_" + elemento_modal_id).data("tipo");
        responseSimple({
            url: "IntranetDetalleElementoModal/IntranetDetalleElementoModalListarxElementoModalJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var rows = response.data;
                var tr = "";
                $.each(rows, function (index, value) {
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento_modal" data-id=' + value.detelm_id + '><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento_modal" data-elemento_id="' + elemento_modal_id+'" data-id=' + value.detelm_id + '><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.detelm_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var posicion = '';
                    if (value.detelm_posicion == 'L') {
                        posicion = 'Izquierda';
                    }
                    else if (value.detelm_posicion == 'C') {
                        posicion = 'Centro';
                    }
                    else if (value.detelm_posicion == 'R') {
                        posicion = 'Derecha';
                    }
                    else {
                        posicion = '';
                    };



                    var tipo = $("#elemento_modal_" + elemento_modal_id).data("tipo");
                   
                    var columan_imagen = "";
                    var columna_ubicacion = "";

                    if (tipo == 5) {
                        columan_imagen = "hidden";
                        columna_ubicacion = "hidden";
                    }

                    if (tipo == 16) {
                        columan_imagen = "hidden";
                    }

                    if (tipo == 17 || tipo == 12 || tipo == 11 || tipo == 7 || tipo == 9 || tipo == 18) {
                        columna_ubicacion = "hidden";
                        if (tipo == 11 || tipo == 7) {
                            value.detelm_descripcion = "IMAGEN";
                        }
                    }

                    var nombre = value.detelm_nombre + '.' + value.detelm_extension;
                    if (value.detelm_extension == "") {
                        nombre = "";
                    }
                    if ((tipo == 8 || tipo == 14) && value.detelm_posicion == "L") {
                        nombre = "TEXTO";
                    }
                    if ((tipo == 8 || tipo == 14) && value.detelm_posicion == "R") {
                        value.detelm_descripcion = "IMAGEN";
                    }

                    if ((tipo == 13 || tipo == 15) && value.detelm_posicion == "L") {
                        value.detelm_descripcion = "IMAGEN";
                    }
                    if ((tipo == 13 || tipo == 15) && value.detelm_posicion == "R") {
                        nombre = "TEXTO";
                    }

                    tr += '<tr  data-id="' + value.detelm_id + '" data-orden="' + value.detelm_orden + '"><td><span class="det_elem_modal_orden label label-default label-white middle">' + (index + 1) + '</span> ' + value.detelm_descripcion + '</td><td class="' + columan_imagen + '">' + nombre + '</td><td class="' + columna_ubicacion + '">' + posicion + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });

                var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                    '<div class="col-md-3 col-md-offset-4 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_detalle_elemento_modal" data-id="' + elemento_modal_id + '" data-rel="tooltip" title="Ordenar  Elemento"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                    '<div class="col-md-5 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento_modal" data-id="' + elemento_modal_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento Modal"> <i class="fa fa-file"></i> Nuevo Detalle Elemento Modal</button></div>' +
                    '</div>';

                var columan_imagen = "";
                var columna_ubicacion = "";
                if (tipo_elemento == 5) {
                    columan_imagen = "hidden";
                    columna_ubicacion = "hidden";
                }

                if (tipo_elemento == 17 || tipo_elemento == 12 || tipo_elemento == 11 || tipo_elemento == 7 || tipo_elemento == 9 || tipo_elemento == 18) {
                    columna_ubicacion = "hidden";
                }

                if (tipo_elemento == 16) {
                    columan_imagen = "hidden";
                }

                tr = boton_nuevo + '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th>Texto</th><th class="' + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_ubicacion + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_modal_' + elemento_modal_id + '">' + tr + '</tbody></table>';
                if (rows.length > 0) {
                    $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                }
                else {
                    tr = boton_nuevo + '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th>Texto</th><th class="' + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_ubicacion + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody><td colspan="5" ><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tbody></table>';
                    $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                }

            }
        });
    };

    var _componentes = function () {
        /////////////////////////////listar segmentos
        $(document).on('click', '#tabmenu li', function () {
            var menu_id = $(this).data("id");
            PanelContenido.init_ListarSecciones(menu_id);
        });

        $(document).on('click', '.cabecera-seccion', function () {
            var sec_id = $(this).data("id");
            if (!$(this).hasClass("collapsed")) {
                if (sec_id > 0 || sec_id != "") {
                    PanelContenido.init_ListarElementos(sec_id);
                }
            }
        });

        $(document).on('click', '.btn_detalle_elemento', function (e) {
            e.preventDefault();
            var elemento_id = $(this).data("id");
            var act_tr = $(this).closest("tr");
            var tipo = act_tr.data("tipo");
            if (tipo == 1 || tipo==2 || tipo==3 || tipo==6) {
                return false;
            }
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_' + elemento_id).hasClass("open")) {
                $('#tr_elemento_contenido_' + elemento_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_' + elemento_id).remove();
                $('<tr id="tr_elemento_contenido_' + elemento_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);
                PanelContenido.init_ListarDetalleElementos(elemento_id);
            }
        });

        $(document).on('click', '.btn_elemento_modal', function (e) {
            e.preventDefault();
            var detalle_elemento_id = $(this).data("id");
            var fk_seccion_elemento = $(this).data("seccion");
            var act_tr = $(this).closest("tr");
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_modal_' + detalle_elemento_id).hasClass("open")) {
                $('#tr_elemento_contenido_modal_' + detalle_elemento_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_modal_' + detalle_elemento_id).remove();
                $('<tr id="tr_elemento_contenido_modal_' + detalle_elemento_id + '" class="detail-row open"><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);

                PanelContenido.init_ListarElementosModal(fk_seccion_elemento, detalle_elemento_id);
            }
        });

        $(document).on('click', '.btn_detalle_elemento_modal', function (e) {
            e.preventDefault();
            var elemento_modal_id = $(this).data("id");
            var act_tr = $(this).closest("tr");
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).hasClass("open")) {
                $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).remove();
                $('<tr id="tr_elemento_contenido_detalle_modal' + elemento_modal_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);

                PanelContenido.init_ListarDetalleElementoModal(elemento_modal_id);
            }
        });

        //////////////////////////////////seccion

        $(document).on('click', '.btn_ordenar_seccion', function (e) {
            var menu_id = $(this).data("id");
            var spans = $(".sec_orden");
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Seccion(menu_id);
        });

        $(document).on('click', '.btn_nueva_seccion', function (e) {
            var menu_id = $(this).data("id");
            $("#fk_menu").val(menu_id);
            $("#sec_id").val(0);
            $("#modalFormularioSeccion").modal("show");
        });

        $(document).on('click', '.btn_guardar_seccion', function () {
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
                        var menu_id = $("#fk_menu").val();
                        PanelContenido.init_ListarSecciones(menu_id);
                    }
                    $("#modalFormularioSeccion").modal("hide");
                },
            })
        });

        $(document).on("click", ".btn-eliminar-seccion", function (e) {
            var seccion_id = $(this).data("id");
            var menu_id = $(this).data("menu_id");
            if (seccion_id != "" || seccion_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR esta Seccion?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSeccion/IntranetSeccionEliminarJson",
                            data: JSON.stringify({ sec_id: seccion_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarSecciones(menu_id);
                                }
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

        $(document).on('input', '.input-number', function () {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
        
        $(document).on('change', '.select-estado-seccion', function () {
            var seccion_id = $(this).data("id");
            var sec_estado = $(this).val();
            var menu_id = $(this).data("menu_id");
            var dataForm = {
                sec_id: seccion_id,
                sec_estado: sec_estado
            }
            responseSimple({
                url: "IntranetSeccion/IntranetSeccionEditarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (!response.respuesta) {
                        PanelContenido.init_ListarSecciones(menu_id);
                    }
                    else {
                        var texto = (sec_estado == "A") ? "Activo" : "Inactivo";
                        var clasetexto = (sec_estado == "A") ? "label label-success label-white middle" : "label label-danger label-white middle";
                        $("#span_estado_" + seccion_id).text(texto);
                        $("#span_estado_" + seccion_id).removeClass();
                        $("#span_estado_" + seccion_id).addClass(clasetexto);
                    }
                }
            });
        });


        ///////////////////////////////////////////////////////////////////////elemento

        $(document).on('click', '.btn_ordenar_elemento_seccion', function (e) {
            var seccion_id = $(this).data("id");
            var spans = $('tbody.tbody_elemento_' + seccion_id + ' tr span.elem_orden');
            console.log(spans)
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Elemento(seccion_id);
        });

        $(document).on('change', '#fk_tipo_elemento', function (e) {
            var estado = $("#tituloModalElemento").text();
            if (estado == "Nuevo") {
                var input = '<input type="text" name="elem_titulo" id="elem_titulo" class="form-control" placeholder="Texto">';
                var textarea = '<textarea name="elem_titulo" id="elem_titulo" class="form-control"></textarea>';
                var tipo = $(this).val();
                if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                    if (tipo == 3) {
                        $("#parrafo_elemento").text("Parrafo");
                        $("#contenido_input").html(textarea);
                        $('#elem_titulo').richText({
                            imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                            fileUpload: false, urls: false
                        });
                        $("a.richText-help").hide();
                    } else {
                        $("#parrafo_elemento").text("Texto");
                        $("#contenido_input").html(input);
                    };
                    $("div.titulo_elemento").show();
                    $("#elem_titulo").val("");

                }
                else {
                    $("#contenido_input").html(input);
                    $("#parrafo_elemento").text("Texto");
                    $("div.titulo_elemento").hide();
                    $("#elem_titulo").val($("#fk_tipo_elemento option:selected").text());
                }
            }
        });

        $(document).on('click', '.btn_nuevo_elemento_seccion', function (e) {
            $("#tituloModalElemento").text("Nuevo");
            var seccion_id = $(this).data("id");
            $("#fk_seccion").val(seccion_id);
            $("#elem_id").val(0);
            $("#elem_titulo").val("");
            $("#div_fk_tipo_elemento").removeClass("hidden");
            $("#div_texto_fk_tipo_elemento").addClass("hidden");
            $("#fk_tipo_elemento").val("").trigger('change');
            _objetoForm_form_elemento.resetForm();
           
            $("#modalFormularioElemento").modal("show");
        });

        $(document).on('click', '.btn_editar_elemento', function () {
            $("#tituloModalElemento").text("Editar");
            var elem_id = $(this).data('id');
            var dataForm = {
                elem_id: elem_id,
            }
            $("#div_fk_tipo_elemento").addClass("hidden");
            $("#div_texto_fk_tipo_elemento").removeClass("hidden");
            responseSimple({
                url: 'IntranetElemento/IntranetElementoIdObtenerJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var elemento = response.data;
                        var input = '<input type="text" name="elem_titulo" id="elem_titulo" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="elem_titulo" id="elem_titulo" class="form-control"></textarea>';
                        var tipo = elemento.fk_tipo_elemento;
                        $("#fk_tipo_elemento").val(elemento.fk_tipo_elemento).trigger('change');
                        $("#fk_seccion").val(elemento.fk_seccion);
                        $("#elem_id").val(elemento.elem_id);
                        $("#elem_orden").val(elemento.elem_orden);
                        $("#elem_estado").val(elemento.elem_estado);
                        if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                            if (tipo == 3) {
                                $("#parrafo_elemento").text("Parrafo");
                                $("#contenido_input").html(textarea);
                                $("#elem_titulo").val(elemento.elem_titulo);
                                $('#elem_titulo').richText({
                                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                    fileUpload: false, urls: false
                                });
                                $("a.richText-help").hide();
                            } else {
                                $("#parrafo_elemento").text("Texto");
                                $("#contenido_input").html(input);
                                $("#elem_titulo").val(elemento.elem_titulo);
                            };
                            $("div.titulo_elemento").show();
                        }
                        else {
                            $("#contenido_input").html(input);
                            $("#parrafo_elemento").text("Texto");
                            $("div.titulo_elemento").hide();
                            $("#elem_titulo").val(elemento.elem_titulo);
                        }
                        $("#texto_fk_tipo_elemento").html($("#fk_tipo_elemento option:selected").text());
                        $("#modalFormularioElemento").modal("show");
                    }
                }
            })
        })

        $(document).on('click', '.btn_guardar_elemento', function () {
            $("#form_elemento").submit();
            if (_objetoForm_form_elemento.valid()) {
                var dataForm = $('#form_elemento').serializeFormJSON();
                var url = "";
                if ($("#elem_id").val() == 0) {
                    url = 'IntranetElemento/IntranetElementoInsertarJson';
                }
                else {
                    url = 'IntranetElemento/IntranetElementoEditarJson';
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        //console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            var seccion_id = $("#fk_seccion").val();
                            PanelContenido.init_ListarElementos(seccion_id);
                            $("#modalFormularioElemento").modal("hide");
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

        });

        $(document).on("click", ".btn_eliminar_elemento", function (e) {
            var elemento_id = $(this).data("id");
            var seccion_id = $(this).data("seccion_id");
            if (elemento_id != "" || elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetElemento/IntranetElementoElementoEliminarJson",
                            data: JSON.stringify({ elem_id: elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarElementos(seccion_id);
                                }
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

        ////////////////////////////////////////////////////////////////////detalleelemento

        $(document).on('click', '.btn_ordenar_detalle_elemento', function (e) {
            var elemento_id = $(this).data("id");
            var spans = $('tbody.tbody_detalle_elemento_' + elemento_id + ' tr span.detelem_orden');
            console.log(spans)
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Detalle_Elemento(elemento_id);
        });

        $(document).on("click", ".btn_eliminar_detalle_elemento", function (e) {
            var det_elemento_id = $(this).data("id");
            var elemento_id = $(this).data("elemento_id");
            if (det_elemento_id != "" || det_elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Detalle de Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetDetalleElemento/IntranetDetalleElementoEliminarJson",
                            data: JSON.stringify({ detel_id: det_elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarDetalleElementos(elemento_id);
                                }
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

        $(document).on('change', '#cboOpcion', function (e) {
            var valor = $(this).val();
            var elemento_id = $("#fk_elemento").val();
            var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");
            if (valor == 1) {
                $(".detel-imagen").hide();
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboPosicion").val("L");
                }
                if (tipo_elemento == 13 || tipo_elemento == 15) {
                    $("#cboPosicion").val("R");
                }
                if (tipo_elemento == 5) {
                    $("#cboPosicion").val("");
                }
            }
            else {
                $(".detel-imagen").show();
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboPosicion").val("R");
                }
                if (tipo_elemento == 13 || tipo_elemento == 15) {
                    $("#cboPosicion").val("L");
                }
                if (tipo_elemento == 17 || tipo_elemento==12) {
                    $("#cboPosicion").val("");
                }
                if (tipo_elemento == 7 || tipo_elemento == 11) {
                    $("#cboPosicion").val("");
                }
            }
            
        });

        $(document).on('click', '.btn_nuevo_detalle_elemento', function (e) {
            var elemento_id = $(this).data("id");
            var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");
            $("#div_parrafo_detalleelemento").html("");
            var input = '<input type="text" name="detel_descripcion" id="detel_descripcion" class="form-control" placeholder="Texto">';
            var textarea = '<textarea name="detel_descripcion" id="detel_descripcion" class="form-control"></textarea>';
            $("#div_parrafo_detalleelemento").html(input);

            console.log(tipo_elemento);
            $("#tituloModalDetalleElemento").text("Nuevo");
            $("#fk_elemento").val(elemento_id);
            $("#detel_id").val(0);
            $("#detel_nombre_imagen_modal").text("");
            $("#detel_nombre_imagen").val("");
            $("#spandetel").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#cboOpcion").val(1).change();
            $("#cboPosicion").val("L");
            $("#detel_descripcion").val("");
            $("#detel_nombre").val("");
            $("#detel_url").val("");
            $("#detel_blank").val("false");
            $("#detel_estado").val("A");
            $("#divdetel").hide();
            $(".detel-listaDirecciones").hide();
            $("#div_lista_uris").html("");
            if (urislista.length>0) {
                $.each(urislista, function (index, value) {
                    $("#div_lista_uris").append('<div>URL: '+value+'</div>');
                });
            }

            if (tipo_elemento == 5) {
                $(".detel-opcion").hide();
                $(".detel-descripcion").show();
                $(".detel-imagen").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $(".detel-posicion").hide();
                $(".detel-estado").show();
                $("#fk_seccion_elemento").val(2);
                $("#cboOpcion").val(1).change();
                $("#cboPosicion").val("");
            }
            else if (tipo_elemento == 7 || tipo_elemento == 11 || tipo_elemento == 10) {
                $(".detel-opcion").hide();
                $(".detel-descripcion").hide();
                $(".detel-imagen").show();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $(".detel-posicion").hide();
                $(".detel-estado").show();
                $("#fk_seccion_elemento").val(1);
                $("#cboOpcion").val(2).change();
                $("#cboPosicion").val("");
            }
            else if (tipo_elemento == 9 || tipo_elemento == 18) {

                $("#div_parrafo_detalleelemento").html(textarea);
                $('#detel_descripcion').richText({
                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                    fileUpload: false, urls: false
                });
                $("a.richText-help").hide();

                $(".detel-opcion").hide();
                $(".detel-descripcion").show();
                $(".detel-imagen").show();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $(".detel-posicion").hide();
                $(".detel-estado").show();
                $("#fk_seccion_elemento").val(1);
                $("#cboOpcion").val(2).change();
                $("#cboPosicion").val("");
            }

            else if (tipo_elemento == 8 || tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15) {
                //va a abrir modal
                $("#fk_seccion_elemento").val(1);
                $(".detel-imagen").hide();
                $("#divdetel").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $(".detel-opcion").show();
                $(".detel-posicion").hide();
                $(".detel-descripcion").show();
                
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboOpcion").val(1).change();
                    $("#cboPosicion").val("L");
                }
                else {
                    $("#cboOpcion").val(1).change();
                    $("#cboPosicion").val("R");
                }                   
            } else if (tipo_elemento == 16) {
                $("#fk_seccion_elemento").val(1);
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
                $("#cboOpcion").val(1).change();
                $(".detel-posicion").show();
            }
            else if (tipo_elemento == 17 || tipo_elemento==12) {
                $("#fk_seccion_elemento").val(1);
                $(".detel-opcion").hide();
                $(".detel-url").show();
                $(".detel-blank").show();
                $("#cboOpcion").val(2).change();
                $(".detel-posicion").hide();
                if (urislista.length>0) {
                    $(".detel-listaDirecciones").show();
                }
               
            }
            else {
                $("#fk_seccion_elemento").val(0);
                $(".detel-posicion").hide();
                $(".detel-opcion").hide();
                $(".detel-url").hide();
                $(".detel-blank").hide();
            }
            $("#modalFormularioDetalleElemento").modal("show");
        });

        $(document).on('click', '.btn_editar_detalle_elemento', function (e) {
            var det_elemento_id = $(this).data("id");
            $("#tituloModalDetalleElemento").text("Editar");
            $(".detel-listaDirecciones").hide();
            $("#div_lista_uris").html("");
            if (urislista.length > 0) {
                $.each(urislista, function (index, value) {
                    $("#div_lista_uris").append('<div>URL: ' + value + '</div>');
                });
            };
            $("#detel_url").val("");
            var dataForm = {
                detel_id: det_elemento_id,
            }
            responseSimple({
                url: 'IntranetDetalleElemento/IntranetDetalleElementoIdObtenerJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    var data = response.data;
                    console.log(data);
                    if (response.respuesta) {
                        $("#detel_nombre").val("");
                        $("#divdetel").hide();
                        $('#detel_id').val(data.detel_id);
                        $('#fk_elemento').val(data.fk_elemento)
                        $("#spandetel").html("");
                        $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
                        if (data.detel_extension != "") {
                            $("#detel_nombre_imagen_modal").text("Nombre: " + data.detel_nombre + "." + data.detel_extension);
                            $("#detel_nombre_imagen").val(data.detel_nombre + "." + data.detel_extension);
                            // $("#detel_fecha_imagen").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#icono_actual_detel").attr("src", basePath+"IntranetFiles/" + data.detel_nombre_imagen);
                            $("#divdetel").show();
                            $(".detel-imagen").show();
                            $(".detel-nombre").show();
                            $(".detel-posicion").hide();
                            $(".detel-opcion").hide();
                            if (data.fk_tipo_elemento == 12 || data.fk_tipo_elemento == 17) {
                                if (urislista.length > 0) {
                                    $(".detel-listaDirecciones").show();
                                }
                                $(".detel-url").show();
                                $(".detel-blank").show();
                            }
                            else {
                                $(".detel-url").hide();
                                $(".detel-blank").hide();
                            }
                        }
                        else {
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

                        if (data.fk_tipo_elemento == 8 || data.fk_tipo_elemento == 14 || data.fk_tipo_elemento == 13 || data.fk_tipo_elemento == 15 || data.fk_tipo_elemento == 7 || data.fk_tipo_elemento==11) {
                            $(".detel-posicion").hide();
                        }
                        if (data.fk_tipo_elemento == 16) {
                            $(".detel-posicion").show();
                        }

                        $("#div_parrafo_detalleelemento").html("");
                        var input = '<input type="text" name="detel_descripcion" id="detel_descripcion" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="detel_descripcion" id="detel_descripcion" class="form-control"></textarea>';
                        $("#div_parrafo_detalleelemento").html(input);

                        if (data.fk_tipo_elemento == 9 || data.fk_tipo_elemento == 18) {
                            $("#div_parrafo_detalleelemento").html(textarea);
                            $("#detel_descripcion").val(data.detel_descripcion);
                            $('#detel_descripcion').richText({
                                imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                fileUpload: false, urls: false
                            });
                            $("a.richText-help").hide();
                        }

                        $("#detel_orden").val(data.detel_orden);
                        $("#cboPosicion").val(data.detel_posicion);
                        $("#detel_descripcion").val(data.detel_descripcion);
                        $("#detel_estado").val(data.detel_estado);
                        $("#detel_url").val(data.detel_url);
                        $(".detel-orden").show();
                        $('#modalFormularioDetalleElemento').modal('show');
                    }
                },
            })

        });

        $(document).on('click', '.btn_guardar_detalle_elemento', function () {
            //$("#form_detalle_elemento").submit();
            var elemento_id = $("#fk_elemento").val();
            var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");

            if (tipo_elemento == 5) {
                if ($("#detel_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 9 || tipo_elemento == 18) {
                if ($("#detel_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                }

                if ($("#detel_nombre").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });

                    return false;
                }
            }

            if (tipo_elemento == 7) {
                if ($("#detel_nombre").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 8 || tipo_elemento == 14) {
                if ($("#cboOpcion").val() == 1 && $("#detel_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                };
                

                if ($("#cboOpcion").val() == 2 && $("#detel_nombre").val() == "") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 17 || tipo_elemento==12) {
                if ($("#detel_descripcion").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });

                    return false;
                }

                if ($("#detel_nombre").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                   
                    return false;
                }
                
            }
            var dataForm = new FormData(document.getElementById("form_detalle_elemento"));
            var url = '';
            if ($("#detel_id").val() == 0) {
                url = 'IntranetDetalleElemento/IntranetDetalleElementoInsertarJson';
            }
            else {
                url = 'IntranetDetalleElemento/IntranetDetalleElementoEditarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        PanelContenido.init_ListarDetalleElementos(elemento_id);
                        $("#modalFormularioDetalleElemento").modal("hide");
                    }
                }
            })
        });

        $(document).on("change", "#detel_nombre", function () {
            var _image = $('#detel_nombre')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension!= "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
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
                $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        });

        ////////////////////////////////////////////////////elemento modal

        $(document).on('click', '.btn_ordenar_elemento_modal', function (e) {
            var detal_elem_id = $(this).data("id");
            var spans = $('tbody.tbody_elemento_modal_' + detal_elem_id + ' tr span.elem_modal_orden');
            console.log(spans)
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Elemento_modal(detal_elem_id);
        });

        $(document).on('change', '#fk_tipo_elemento_modal', function (e) {
            var estado = $("#tituloModalElementoModal").text();
            if (estado == "Nuevo") {
                var input = '<input type="text" name="emod_titulo" id="emod_titulo" class="form-control" placeholder="Texto">';
                var textarea = '<textarea name="emod_titulo" id="emod_titulo" class="form-control"></textarea>';
                var tipo = $(this).val();
                if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                    if (tipo == 3) {
                        $("#parrafo_elemento_modal").text("Parrafo");
                        $("#contenido_input_modal").html(textarea);
                        $('#emod_titulo').richText({
                            imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                            fileUpload: false, urls: false
                        });
                        $("a.richText-help").hide();
                    } else {
                        $("#parrafo_elemento_modal").text("Texto");
                        $("#contenido_input_modal").html(input);
                    };
                    $("div.titulo_elemento_modal").show();
                    $("#emod_titulo").val("");

                }
                else {
                    $("#contenido_input_modal").html(input);
                    $("#parrafo_elemento_modal").text("Texto");
                    $("div.titulo_elemento_modal").hide();
                    $("#emod_titulo").val($("#fk_tipo_elemento_modal option:selected").text());
                }
            }
        });

        $(document).on('click', '.btn_nuevo_elemento_modal', function (e) {
            $("#tituloModalElementoModal").text("Nuevo");
            var fk_seccion_elemento_modal = $(this).data("seccion");
            var detal_elem_id = $(this).data("id");
            $("#fk_seccion_elemento_modal").val(fk_seccion_elemento_modal);
            $("#detal_elem_id").val(detal_elem_id);
            $("#emod_id").val(0);
            $("#emod_titulo").val("");

            $("#fk_tipo_elemento_modal").select2("destroy");
            $("#fk_tipo_elemento_modal option[value='10']").remove();
            $("#fk_tipo_elemento_modal option[value='11']").remove();
            $("#fk_tipo_elemento_modal option[value='18']").remove();
            $("#fk_tipo_elemento_modal").select2({width:"100%"});

            $("#div_fk_tipo_elemento_modal").removeClass("hidden");
            $("#div_texto_fk_tipo_elemento_modal").addClass("hidden");
            $("#fk_tipo_elemento_modal").val("").trigger('change');
            _objetoForm_form_elemento_modal.resetForm();

            $("#modalFormularioElementoModal").modal("show");
        });

        $(document).on('click', '.btn_editar_elemento_modal', function () {
            $("#tituloModalElementoModal").text("Editar");
            var emod_id = $(this).data('id');
            var dataForm = {
                emod_id: emod_id,
            }
            _objetoForm_form_elemento_modal.resetForm();
            $("#div_fk_tipo_elemento_modal").addClass("hidden");
            $("#div_texto_fk_tipo_elemento_modal").removeClass("hidden");
            responseSimple({
                url: 'IntranetElementoModal/IntranetElementoModalIdObtenerJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var elemento = response.data;
                        var input = '<input type="text" name="emod_titulo" id="emod_titulo" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="emod_titulo" id="emod_titulo" class="form-control"></textarea>';
                        var tipo = elemento.fk_tipo_elemento;
                        $("#fk_tipo_elemento_modal").val(elemento.fk_tipo_elemento).trigger('change');
                        $("#fk_seccion_elemento_modal").val(elemento.fk_seccion_elemento);
                        $("#emod_id").val(elemento.emod_id);
                        $("#emod_orden").val(elemento.emod_orden);
                        $("#emod_estado").val(elemento.emod_estado);
                        if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                            if (tipo == 3) {
                                $("#parrafo_elemento_modal").text("Parrafo");
                                $("#contenido_input_modal").html(textarea);
                                $("#emod_titulo").val(elemento.emod_titulo);
                                $('#emod_titulo').richText({
                                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                    fileUpload: false, urls: false
                                });
                                $("a.richText-help").hide();
                            } else {
                                $("#parrafo_elemento_modal").text("Texto");
                                $("#contenido_input_modal").html(input);
                                $("#emod_titulo").val(elemento.emod_titulo);
                            };
                            $("div.titulo_elemento_modal").show();
                        }
                        else {
                            $("#contenido_input_modal").html(input);
                            $("#parrafo_elemento_modal").text("Texto");
                            $("div.titulo_elemento_modal").hide();
                            $("#emod_titulo").val(elemento.emod_titulo);
                        }
                        $("#texto_fk_tipo_elemento_modal").html($("#fk_tipo_elemento_modal option:selected").text());
                        $("#modalFormularioElementoModal").modal("show");
                    }
                }
            })
        });

        $(document).on('click', '.btn_guardar_elemento_modal', function () {
            $("#form_elemento_modal").submit();
            if (_objetoForm_form_elemento_modal.valid()) {
                var dataForm = $('#form_elemento_modal').serializeFormJSON();
                var url = "";
                if ($("#emod_id").val() == 0) {
                    url = 'IntranetElementoModal/IntranetElementoModalInsertarJson';
                }
                else {
                    url = 'IntranetElementoModal/IntranetElementoModalEditarJson';
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        //console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            var seccion_id = $("#fk_seccion_elemento_modal").val();
                            var detal_elem_id = $("#detal_elem_id").val();
                            PanelContenido.init_ListarElementosModal(seccion_id, detal_elem_id);
                            $("#modalFormularioElementoModal").modal("hide");
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

        });

        $(document).on("click", ".btn_eliminar_elemento_modal", function (e) {
            var emod_id = $(this).data("id");
            var seccion_id = $(this).data("seccion_id");
            var detal_elem_id = $(this).data("detal_elem_id");
            if (emod_id != "" || emod_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetElementoModal/IntranetElementoModalEliminarJson",
                            data: JSON.stringify({ emod_id: emod_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarElementosModal(seccion_id, detal_elem_id);
                                }
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

        //////////////////////////////////////////////detalle elemento modal

        $(document).on('click', '.btn_ordenar_detalle_elemento_modal', function (e) {
            var elementom_id = $(this).data("id");
            var spans = $('tbody.tbody_detalle_elemento_modal_' + elementom_id + ' tr span.det_elem_modal_orden');
            console.log(spans)
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Detalle_Elemento_modal(elementom_id);
        });

        $(document).on("click", ".btn_eliminar_detalle_elemento_modal", function (e) {
            var det_elemento_id = $(this).data("id");
            var elementom_id = $(this).data("elemento_id");
            if (det_elemento_id != "" || det_elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Detalle de Elemento Modal?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetDetalleElementoModal/IntranetDetalleElementoModalEliminarJson",
                            data: JSON.stringify({ detelm_id: det_elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarDetalleElementoModal(elementom_id);
                                }
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

        $(document).on('change', '#cboOpcionElemModal', function (e) {
            var valor = $(this).val();
            var elemento_id = $("#fk_elemento_modal").val();
            var tipo_elemento = $("#elemento_modal_" + elemento_id).data("tipo");
            if (valor == 1) {
                $(".detelm-imagen").hide();
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboPosicionElemModal").val("L");
                }
                if (tipo_elemento == 13 || tipo_elemento == 15) {
                    $("#cboPosicionElemModal").val("R");
                }
                if (tipo_elemento == 5) {
                    $("#cboPosicionElemModal").val("");
                }
            }
            else {
                $(".detelm-imagen").show();
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboPosicionElemModal").val("R");
                }
                if (tipo_elemento == 13 || tipo_elemento == 15) {
                    $("#cboPosicionElemModal").val("L");
                }
                if (tipo_elemento == 17 || tipo_elemento == 12) {
                    $("#cboPosicionElemModal").val("");
                }
                if (tipo_elemento == 7 || tipo_elemento == 11) {
                    $("#cboPosicionElemModal").val("");
                }
            }

        });

        $(document).on('click', '.btn_nuevo_detalle_elemento_modal', function (e) {

            var elemento_id = $(this).data("id");
            var tipo_elemento = $("#elemento_modal_" + elemento_id).data("tipo");

            $("#div_parrafo_detalleelemento_modal").html("");
            var input = '<input type="text" name="detelm_descripcion" id="detelm_descripcion" class="form-control" placeholder="Texto">';
            var textarea = '<textarea name="detelm_descripcion" id="detelm_descripcion" class="form-control"></textarea>';
            $("#div_parrafo_detalleelemento_modal").html(input);

            console.log(tipo_elemento);
            $("#tituloModalDetalleElementoModal").text("Nuevo");
            $("#fk_elemento_modal").val(elemento_id);
            $("#detelm_id").val(0);
            $("#detelm_nombre_imagen_modal").text("");
            $("#detelm_nombre_imagen").val("");
            $("#spandetelm").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#cboOpcionElemModal").val(1).change();
            $("#cboPosicionElemModal").val("L");
            $("#detelm_descripcion").val("");
            $("#detelm_nombre").val("");
            $("#detelm_url").val("");
            $("#detelm_blank").val("false");
            $("#detelm_estado").val("A");

            $("#divdetelm").hide();


            if (tipo_elemento == 5) {
                $(".detelm-opcion").hide();
                $(".detelm-descripcion").show();
                $(".detelm-imagen").hide();
                $(".detelm-url").hide();
                $(".detelm-blank").hide();
                $(".detelm-posicion").hide();
                $(".detelm-estado").show();
                $("#fk_seccion_det_elemento_modal").val(2);
                $("#cboOpcionElemModal").val(1).change();
                $("#cboPosicionElemModal").val("");
            }
            else if (tipo_elemento == 7 || tipo_elemento == 11 || tipo_elemento == 10) {
                $(".detelm-opcion").hide();
                $(".detelm-descripcion").hide();
                $(".detelm-imagen").show();
                $(".detelm-url").hide();
                $(".detelm-blank").hide();
                $(".detelm-posicion").hide();
                $(".detelm-estado").show();
                $("#fk_seccion_det_elemento_modal").val(1);
                $("#cboOpcionElemModal").val(2).change();
                $("#cboPosicionElemModal").val("");
            }
            else if (tipo_elemento == 9 || tipo_elemento == 18) {

                $("#div_parrafo_detalleelemento_modal").html(textarea);
                $('#detelm_descripcion').richText({
                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                    fileUpload: false, urls: false
                });
                $("a.richText-help").hide();

                $(".detelm-opcion").hide();
                $(".detelm-descripcion").show();
                $(".detelm-imagen").show();
                $(".detelm-url").hide();
                $(".deteml-blank").hide();
                $(".detelm-posicion").hide();
                $(".detelm-estado").show();
                $("#fk_seccion_det_elemento_modal").val(1);
                $("#cboOpcionElemModal").val(2).change();
                $("#cboPosicionElemModal").val("");
            }
            else if (tipo_elemento == 8 || tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15) {
                //va a abrir modal
                $("#fk_seccion_det_elemento_modal").val(1);
                $(".detelm-imagen").hide();
                $("#divdetelm").hide();
                $(".detelm-url").hide();
                $(".detelm-blank").hide();
                $(".detelm-opcion").show();
                $(".detelm-posicion").hide();
                $(".detelm-descripcion").show();
                if (tipo_elemento == 8 || tipo_elemento == 14) {
                    $("#cboOpcionElemModal").val(1).change();
                    $("#cboPosicionElemModal").val("L");
                }
                else {
                    $("#cboOpcionElemModal").val(1).change();
                    $("#cboPosicionElemModal").val("R");
                }
            } else if (tipo_elemento == 16) {
                $("#fk_seccion_det_elemento_modal").val(1);
                $(".detelm-opcion").hide();
                $(".detelm-url").hide();
                $(".detelm-blank").hide();
                $("#cboOpcionElemModal").val(1).change();
                $(".detelm-posicion").show();
            }
            else if (tipo_elemento == 17 || tipo_elemento == 12) {
                $("#fk_seccion_det_elemento_modal").val(1);
                $(".detelm-opcion").hide();
                $(".detelm-url").show();
                $(".detelm-blank").show();
                $("#cboOpcionElemModal").val(2).change();
                $(".detelm-posicion").hide();
            }
            else {
                $("#fk_seccion_det_elemento_modal").val(0);
                $(".detelm-posicion").hide();
                $(".detelm-opcion").hide();
                $(".detelm-url").hide();
                $(".detelm-blank").hide();
            }
            $("#modalFormularioDetalleElementoModal").modal("show");
        });

        $(document).on('click', '.btn_editar_detalle_elemento_modal', function (e) {
            var det_elemento_id = $(this).data("id");
            $("#tituloModalDetalleElementoModal").text("Editar");

            var dataForm = {
                detelm_id: det_elemento_id,
            }
            responseSimple({
                url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalIdObtenerJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    var data = response.data;
                    console.log(data);
                    if (response.respuesta) {
                        $("#detelm_nombre").val("");
                        $("#divdetelm").hide();
                        $('#detelm_id').val(data.detelm_id);
                        $('#fk_elemento_modal').val(data.fk_elemento_modal)
                        $("#spandetelm").html("");
                        $("#spandetelm").append('<i class="fa fa-upload"></i>  Subir Icono');
                        if (data.detelm_extension != "") {
                            $("#detelm_nombre_imagen_modal").text("Nombre: " + data.detelm_nombre + "." + data.detelm_extension);
                            $("#detelm_nombre_imagen").val(data.detelm_nombre + "." + data.detelm_extension);
                            // $("#detel_fecha_imagen").text("Fecha Subida: " + moment(actividad.act_fecha).format("YYYY-MM-DD hh:mm A"));
                            $("#icono_actual_detelm").attr("src", basePath+"IntranetFiles/" + data.detelm_nombre_imagen);
                            $("#divdetelm").show();
                            $(".detelm-imagen").show();
                            $(".detelm-nombre").show();
                            $(".detelm-posicion").hide();
                            $(".detelm-opcion").hide();
                            if (data.fk_tipo_elemento == 12 || data.fk_tipo_elemento == 17) {
                                $(".detelm-url").show();
                                $(".detelm-blank").show();
                            }
                            else {
                                $(".detelm-url").hide();
                                $(".detelm-blank").hide();
                            }
                        }
                        else {
                            $(".detelm-imagen").hide();
                            $(".detelm-nombre").hide();
                            $(".detelm-posicion").hide();
                            $(".detelm-opcion").hide();
                            $(".detelm-url").hide();
                            $(".detelm-blank").hide();
                            $("#detelm_nombre_imagen_modal").text("");
                            $("#detelm_nombre_imagen").val("");
                            $("#detelm_url").val("");
                        }

                        if (data.fk_tipo_elemento == 8 || data.fk_tipo_elemento == 14 || data.fk_tipo_elemento == 13 || data.fk_tipo_elemento == 15 || data.fk_tipo_elemento == 7 || data.fk_tipo_elemento == 11) {
                            $(".detelm-posicion").hide();
                        }
                        if (data.fk_tipo_elemento == 16) {
                            $(".detelm-posicion").show();
                        }

                        $("#div_parrafo_detalleelemento_modal").html("");
                        var input = '<input type="text" name="detelm_descripcion" id="detelm_descripcion" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="detelm_descripcion" id="detelm_descripcion" class="form-control"></textarea>';
                        $("#div_parrafo_detalleelemento_modal").html(input);

                        if (data.fk_tipo_elemento == 9 || data.fk_tipo_elemento == 18) {
                            $("#div_parrafo_detalleelemento_modal").html(textarea);
                            $("#detelm_descripcion").val(data.detel_descripcion);
                            $('#detelm_descripcion').richText({
                                imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                fileUpload: false, urls: false
                            });
                            $("a.richText-help").hide();
                        }


                        $("#detelm_orden").val(data.detelm_orden);
                        $("#cboPosicionElemModal").val(data.detelm_posicion);
                        $("#detelm_descripcion").val(data.detelm_descripcion);
                        $("#detelm_estado").val(data.detelm_estado);
                        $(".detelm-orden").show();
                        $('#modalFormularioDetalleElementoModal').modal('show');
                    }
                },
            })

        });

        $(document).on('click', '.btn_guardar_detalle_elemento_modal', function () {
            //$("#form_detalle_elemento").submit();
            var elemento_id = $("#fk_elemento_modal").val();
            var tipo_elemento = $("#elemento_modal_" + elemento_id).data("tipo");

            if (tipo_elemento == 5) {
                if ($("#detelm_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 7) {
                if ($("#detelm_nombre").val() == "" && $("#tituloModalDetalleElementoModal").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 9 || tipo_elemento == 18) {
                if ($("#detelm_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                }

                if ($("#detelm_nombre").val() == "" && $("#tituloModalDetalleElementoModal").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });

                    return false;
                }
            }

            if (tipo_elemento == 8 || tipo_elemento == 14) {
                if ($("#cboOpcionElemModal").val() == 1 && $("#detelm_descripcion").val() == "") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });
                    return false;
                };


                if ($("#cboOpcionElemModal").val() == 2 && $("#detelm_nombre").val() == "") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 17 || tipo_elemento == 12) {
                if ($("#detelm_descripcion").val() == "" && $("#tituloModalDetalleElementoModal").text() == "Nuevo") {
                    messageResponse({
                        text: 'Contenido es obligatorio',
                        type: "error"
                    });

                    return false;
                }

                if ($("#detelm_nombre").val() == "" && $("#tituloModalDetalleElementoModal").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });

                    return false;
                }

            }
            var dataForm = new FormData(document.getElementById("form_detalle_elemento_modal"));
            var url = '';
            if ($("#detelm_id").val() == 0) {
                url = 'IntranetDetalleElementoModal/IntranetDetalleElementoModalInsertarJson';
            }
            else {
                url = 'IntranetDetalleElementoModal/IntranetDetalleElementoModalEditarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        PanelContenido.init_ListarDetalleElementoModal(elemento_id);
                        $("#modalFormularioDetalleElementoModal").modal("hide");
                    }
                }
            })
        });

        $(document).on("change", "#detelm_nombre", function () {
            var _image = $('#detelm_nombre')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension != "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetelm").html("");
                    $("#spandetelm").append(icon + " " + nombre + "... ." + actualicon);
                    //$("#img_ubicacion").val(nombre + "." + actualicon);
                    $("#spandetelm").css({ 'font-size': '10px' });
                }
            }
            else {
                $("#spandetelm").html("");
                $("#spandetelm").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        })
    };

    var _metodos = function () {

        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "fk_tipo_elemento",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            closeMessages: true,
        });

        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "fk_tipo_elemento_modal",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            closeMessages: true,
        });

        validar_Form({
            nameVariable: 'form_seccion',
            contenedor: '#form_seccion',
            rules: {
                sec_estado:
                {
                    required: true,

                }
            },
            messages: {
                sec_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });

        validar_Form({
            nameVariable: 'form_elemento',
            contenedor: '#form_elemento',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                elem_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                elem_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });

        validar_Form({
            nameVariable: 'form_detalle_elemento',
            contenedor: '#form_detalle_elemento',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                elem_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                elem_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });

        validar_Form({
            nameVariable: 'form_elemento_modal',
            contenedor: '#form_elemento_modal',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                emod_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                emod_estado:
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
            _ListarMenus();
            _componentes();
            _metodos();

        },
        init_Sort_Seccion: function (menu_id) {
            _sort_seccion(menu_id);
        },
        init_Sort_Elemento: function (seccion_id) {
            _sort_elemento(seccion_id);
        },
        init_Sort_Detalle_Elemento: function (elemento_id) {
            _sort_detalle_elemento(elemento_id);
        },
        init_Sort_Elemento_modal: function (detal_elemento_id) {
            _sort_elemento_modal(detal_elemento_id);
        },
        init_Sort_Detalle_Elemento_modal: function (elementom_id) {
            _sort_detalle_elemento_modal(elementom_id);
        },
        init_ListarMenus: function () {
            _ListarMenus();
        },
        init_ListarSecciones: function (menu_id) {
            _ListarSecciones(menu_id);
        },
        init_ListarElementos: function (seccion_id) {
            _ListarElementos(seccion_id);
        },
        init_ListarDetalleElementos: function (elemento_id) {
            _ListarDetalleElementos(elemento_id);
        },
        init_ListarElementosModal: function (fk_seccion_elemento, detalle_elemento_id) {
            _ListarElementosModal(fk_seccion_elemento, detalle_elemento_id);
        },
        init_ListarDetalleElementoModal: function (elemento_modal_id) {
            _ListarDetalleElementosModal(elemento_modal_id);
        },
        
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelContenido.init();
});