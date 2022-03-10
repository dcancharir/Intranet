var LayoutVista = function () {
    var _Layout = function () {
        //console.log(menu_id);
       
        var meses = ["ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO",
            "JULIO", "AGOSTO", "SETIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"
        ];
        dias = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
        var diahoy = new Date();
        var menuInicio = menu_id;
        //console.log(meses[diahoy.getMonth()]);
        responseSimple({
            url: "IntranetPJ/ListarDataJson",
            data: JSON.stringify({ menu_id: menuInicio }),
            refresh: false,
            callBackSuccess: function (response) {
                var dataMenus = response.dataMenus;
                var dataSaludos = response.dataSaludos;
                var dataCumpleanios = response.dataCumpleanios;
                var dataActividades = response.dataActividades;
                var listaNoticias = response.listaNoticias;
                var secciones = response.dataSecciones;
                var dataFooter=response.dataFooter;
                //Salas y ApuestasDeportivas
                $("#cantidadApuestasDeportivas").text(response.cantidadApuestasDeportivas);
                $("#cantidadSalas").text(response.cantidadSalas);
                //Creacion de Menus
                if (dataMenus.length > 0) {
                    $("#menuIntranet").html("");
                    $("#barnav").html("");
                    var appendMenuLateral = "";
                    var appendMenuPrincipal = "";
                    $.each(dataMenus, function (index, menu) {
                        appendMenuLateral += '<li><a href="' + basePath + 'intranetPJ/index?menu=' + menu.menu_id + '">' + menu.menu_titulo + '</a></li>';
                        var clase = "";
                        if (menuInicio == 0) {
                            clase = (index == 0) ? "active" : "";
                        }
                        else {
                            clase = (menu.menu_id == menuInicio) ? "active" : "";
                        }

                        appendMenuPrincipal += '<li class="' + clase + '"><a href="' + basePath + 'intranetPJ/index?menu=' + menu.menu_id + '" data-id="' + menu.menu_id + '" class="sf-with-ul">' + menu.menu_titulo + '</a><ul></ul></li>';
                        //noticia.push(menu.menu_titulo)
                    });
                    appendMenuLateral += '<li><a href="#" class="btnCerrarSesion">Cerrar Sesion</a></li>';
                    $("#menuIntranet").html(appendMenuLateral);
                    $("#barnav").html(appendMenuPrincipal);
                }
                if (dataSaludos.length > 0) {
                    $("#saludos_li").html("");
                    var appendSaludos = "";
                    $.each(dataSaludos, function (index, saludo) {
                        appendSaludos += '<li>' +
                            '<div class="cthumb">' +
                            '<a href="#" class="img_saludo"><img src="' + basePath + 'Content/intranet/images/faces/' + saludo.sld_avatar + '" alt=""></a>' +
                            '</div>' +
                            '<div class="dcomment">' +
                            '<a href="#"><span style="font-size:11px;">De:</span> ' + saludo.per_saluda + ' ' + saludo.apelpat_per_saluda + ', <span style="font-size:11px;">Para:</span> ' + saludo.per_saludada + ' ' + saludo.apelpat_per_saludada + ',<br> <span style="font-size:11px;">Mensaje: </span>' + saludo.sld_cuerpo + '</a>' +
                            '<div class="fecha_registro_saludo">10-11-2019</div>' +
                            '</div>' +
                            '</li>';
                    });
                    $("#saludos_li").html(appendSaludos);
                }
                if(dataFooter.length>0){
                    $.each(dataFooter,function(inde,footer){
                        var append='';
                        if(footer.foot_posicion=='I'){
                            $("#titulo_izquierda").html(footer.foot_descripcion);
                            append+='<img style="width:100%;height:260px;" src="'+basePath+"IntranetFiles/Footer/"+footer.foot_imagen+'"></img>';
                            $("#foto_izquierda").append(append);
                        }
                        else{
                            $("#titulo_derecha").html(footer.foot_descripcion);
                            append+='<img style="width:100%;height:260px;" src="'+basePath+"IntranetFiles/Footer/"+footer.foot_imagen+'"></img>';
                            $("#foto_derecha").append(append);
                        }
                    })
                }

                //Creacion de Aside para Cumpleaños
                if (dataCumpleanios.length > 0) {
                    $("#cumpleaniosIntranet").html("");
                    var appendCumpleanios = '';
                    appendCumpleanios += '<h3 class="blocktitle"><img src="' + basePath + 'Content/intranet/images/cake.png" class="img_title"/> CUMPLEAÑOS DE ' + meses[diahoy.getMonth()] + ' </h3><div class="getcat"><ul class="catlist">';
                    $.each(dataCumpleanios, function (index, cumpleanios) {
                        if(index<=9){
                            var diaCumpleanios = new Date(moment(cumpleanios.per_fechanacimiento).format('YYYY-MM-DD'));
                            diaCumpleanios.setMinutes(diaCumpleanios.getMinutes() + diaCumpleanios.getTimezoneOffset());
                            var diaCumpleaniosModal = diaCumpleanios.getDate() + " de " + meses[diahoy.getMonth()];
                            appendCumpleanios += '<li class="_cumple" data-id="' + cumpleanios.per_id + '" data-direccionenvio="' + cumpleanios.per_correoelectronico + '" data-diacumple="' + diaCumpleaniosModal + '" data-numdoc="' + cumpleanios.per_numdoc + '"><a href = "javascript:void(0);" ><img src="' + basePath + 'Content/intranet/images/png/calendar.png" /><div class="spannumber">' + (diaCumpleanios.getDate()) + '</div><p class="meta-date">' + meses[diahoy.getMonth()] + ' ' + (diaCumpleanios.getDate()) + ', ' + diahoy.getFullYear() + '</p><h2 class="wtitle">' + cumpleanios.per_nombre.toUpperCase() + ' ' + cumpleanios.per_apellido_pat.toUpperCase() + ' ' + cumpleanios.per_apellido_mat.toUpperCase() + '</h2></a >    </li >';
                        }
                        else{
                            return false;
                        }
                        
                    });
                    appendCumpleanios += '</ul></div >';
                    $("#cumpleaniosIntranet").html(appendCumpleanios);
                }
                else {
                    $("#cumpleaniosIntranet").html("");
                    var appendCumpleanios = '';
                    appendCumpleanios += '<h3 class="blocktitle"><img src="' + basePath + 'Content/intranet/images/cake.png" class="img_title"/> CUMPLEAÑOS DE ' + meses[diahoy.getMonth()] + ' </h3><div class="getcat">';
                    appendCumpleanios += '<ul class="catlist"><li><a href="javascript:void(0);"><img src="' + basePath + 'Content/intranet/images/cake.png"><div class="spannumber"></div><p class="meta-date"></p><h2 class="wtitle">No se encontraron mas cumpleaños hasta fin de mes</h2></a></li></ul>';
                    $("#cumpleaniosIntranet").append(appendCumpleanios);
                }

                //Creacion de Aside Para Actividades
                if (dataActividades.length > 0) {
                    $("#actividadesMes").html("");
                    var appendActividades = '';
                    appendActividades += '<h3 class="blocktitle"><img src="' + basePath + 'Content/intranet/images/actividad.png" class="img_title"/> ACTIVIDADES DE ' + meses[diahoy.getMonth()] + '</h3><div class="getcat"> <ul class="catlist">';
                    $.each(dataActividades, function (index, actividad) {
                        var diaActividad = new Date(moment(actividad.act_fecha).format('YYYY-MM-DD'));
                        appendActividades += '<li><a href="javascript:void(0);"><img src="'+basePath+"IntranetFiles/Actividades/" + actividad.act_imagen + '" class="img_title" /><p class="meta-date" >' + meses[diaActividad.getMonth()] + ' ' + (diaActividad.getDate() + 1) + ', ' + diaActividad.getFullYear() + ' - ' + moment(actividad.act_fecha).format('hh-mm A') + '</p><h2 class="wtitle">' + actividad.act_descripcion + '</h2> </a></li>';
                    });
                    appendActividades += '</ul></div >';
                    $("#actividadesMes").append(appendActividades);
                }
                else {
                    $("#actividadesMes").html("");
                    var appendActividades = '';
                    appendActividades += '<h3 class="blocktitle"><img src="' + basePath + 'Content/intranet/images/actividad.png" class="img_title"/> ACTIVIDADES DE ' + meses[diahoy.getMonth()] + '</h3><div class="getcat"> <ul class="catlist">';
                    appendActividades += '<li><a href="javascript:void(0);"><img src="' + basePath + 'Content/intranet/images/actividad.png" class="img_title"/><p class="meta-date"></p><h2 class="wtitle">No hay Actividades Registradas</h2></a></li></ul></div>';
                    $("#actividadesMes").append(appendActividades);
                }
                //Listado en Seccion de Noticias
                if (listaNoticias.length > 0) {
                    $("#ticker01").html("");
                    var appendNoticias = '';
                    $.each(listaNoticias, function (index, noticia) {
                        var fechaNoticiaBD = new Date(moment(noticia.Item1).format('YYYY-MM-DD'));
                        fechaNoticiaBD.setMinutes(fechaNoticiaBD.getMinutes() + fechaNoticiaBD.getTimezoneOffset());
                        var fechaNoticia = (fechaNoticiaBD.getDate()) + " DE " + meses[fechaNoticiaBD.getMonth()];
                        var comentarioExtra = noticia.Item3;
                        appendNoticias += '<li><span>' + fechaNoticia + '</span><a href="#">' + comentarioExtra + noticia.Item2 + '</a></li>';
                    })
                    $("#ticker01").html(appendNoticias);
                    $("ul#ticker01").liScroll().css({ 'opacity': 1 });
                }

                //listado secciones
                if (secciones.length > 0) {
                    console.log(secciones);
                    //console.log(secciones)
                    if(menu_id==0){
                        return false;
                    }
                    $("#content").html("");
                    var appendSeccion = "";
                    slider = false;
                    silertrans = false;
                    mansonery = false;
                    $.each(secciones, function (index, seccion) {
                        var elementos = seccion.elementos;
                        var appendElementos = "";
                        if (elementos.length > 0) {
                            $.each(elementos, function (index, elemento) {
                                if (elemento.fk_tipo_elemento == 1) {
                                    appendElementos += '<header>' +
                                        '<div class="loverate"><a href="#"><i class="fa fa-exclamation-circle"></i></a></div>' +
                                        '<h1>' + elemento.elem_titulo + '</h1>' +
                                        '</header>';
                                }

                                if (elemento.fk_tipo_elemento == 2) {
                                    appendElementos += '<header>' +
                                        '<div class="postmeta">' +
                                        '<div class="meta-date">' + elemento.elem_titulo + '</div>' +
                                        '</div>' +
                                        '</header>';
                                }

                                if (elemento.fk_tipo_elemento == 3) {
                                    appendElementos +=
                                        '<div class="divsinglepost"> ' + elemento.elem_titulo + '</div>';

                                }

                                if (elemento.fk_tipo_elemento == 4) {
                                    appendElementos += '<article>' +
                                        '<h4 class="lista_titulo">' + elemento.elem_titulo + '</h4>' +
                                        '<div class="clear"></div>' +
                                        '</article>';
                                }

                                if (elemento.fk_tipo_elemento == 5) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendLista = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendLista += '<li>' + detalleelemento.detel_descripcion + '</li>';

                                        });

                                        appendElementos += '<ul class="listas">' +
                                            appendLista +
                                            '</ul>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 6) {
                                    appendElementos += '<div class="divsinglepost"> <blockquote>' + elemento.elem_titulo + '</blockquote></div>';
                                }

                                if (elemento.fk_tipo_elemento == 7) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendElementos += '<article>' +
                                                '<div class="imgpost th">' +
                                                '<img src = "'+basePath+ detalleelemento.detel_nombre + '" alt = "">' +
                                                '</div>' +
                                                '</article>';
                                        });
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 8) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    "<div class='ctitle wt" + (index + 2) + "'>" + detalleelemento.detel_descripcion + " </div></li>";
                                            }
                                            else {
                                                appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="' +basePath+ detalleelemento.detel_nombre + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="six columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '<div class="six columns">' +
                                            '<div class="sponsor">' +
                                            appendsDerecha +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 9) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendContenido = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendContenido += '<article><div class="post-authorthumb th">'+
                                                '<div class="intern"><img src="'+basePath+detalleelemento.detel_nombre + '" alt= "padding:0px" style=""></div>'+
		  					                                            '</div>' +
                                                '<div class="post-content">' +
                                                detalleelemento.detel_descripcion +'</div></article>'
                                        });

                                        appendElementos += '<div class="singlepost_">' + appendContenido+'</div>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 10) {
                                    silertrans = true;
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendslider = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendslider += '<div class="contentBox">' +
                                                '<img src = "' +basePath+detalleelemento.detel_nombre + '" alt = "Field" />' +
                                                '<div class="overlay">' +
                                                '<div class="title-carousel">' +
                                                '<a href="javascript:void(0);">' +
                                                '<div class="ticarousel"> ' + detalleelemento.detel_descripcion + '</div>' +
                                                '</a>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>';

                                        });

                                        appendElementos += '<article><div class="post-content" ><div class="row">' +
                                            '<div class="twelve columns">' +
                                            '<div id="mixedContent" class="mixedContent onlyimgblank slidercontent" style="padding-left: 10px;padding-right: 10px;">' +
                                            appendslider +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div></article>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 11) {
                                    slider = true;
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendslider = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendslider += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                '<div class="row" style="margin-right:0px !important;margin-left:0px !important;">' +
                                                '<div class="twelve columns">' +
                                                '<div class="itemcatslide">' +
                                                '<div class="catf">' +
                                                '<div class="catf-format">' +
                                                //'<div class="fdate">15<br /><span>Dec</span></div>' +
                                                '</div>' +
                                                '<div class="imgslide">' +
                                                '<img src="'+basePath+detalleelemento.detel_nombre + '" alt="" title="" class="slidefeatured">' +
                                                '</div>' +
                                                //'<div class="catf-caption">'+
                                                //    '<h2>Star Trek Into Darkness is a 2013 American</h2>'+
                                                //    '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>'+
                                                //'</div>'+
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</li>';

                                        });

                                        appendElementos += '<article>' +
                                            '<div class="post-content">' +
                                            '<div class="">' +
                                            '<div class="twelve columns">' +
                                            '<div class="catslide">' +
                                            '<ul class="slider" data-orbit>' +
                                            appendslider +
                                            '</ul>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="clear"></div>' +
                                            '</article>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 12) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendDetalleElementoheader = "";
                                    var appendDetalleElementobody = "";
                                    if (detalleElementolista.length > 0) {
                                        var i = 0;
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (index == 0) {
                                                appendDetalleElementoheader += '<div class="featured abrir_pagina abrir_pagina_izquierda" data-blank="' + detalleelemento.detel_blank + '" data-url="' + detalleelemento.detel_url + '" style="border: 4px solid #4e4c4c;padding-right: 20px;padding-top: 20px;">' +
                                                    '<div class="thumb foto_cuadro_izquiera">' +
                                                    ' <img src="'+basePath+detalleelemento.detel_nombre + '" alt="">' +
                                                    '<div class="overlay">' +
                                                    '<div class="title-carousel">' +
                                                    '<div class="ticarousel titulo_izquierda"> ' + detalleelemento.detel_descripcion + '</div>' +
                                                    '</div>' +
                                                    '</div>' +
                                                    ' </div>' +
                                                    '<div class="excerpt">' +
                                                    '<div class="desc">' +
                                                    '<p class="pcats subtitulo_izquierda" style="padding-left: 0px !important;padding-right: 0px !important;"> ACCEDER A : ' + detalleelemento.detel_descripcion + '</p>' +

                                                    '</div>' +
                                                    '</div>' +
                                                    '</div>';
                                            }
                                            else {
                                                i++;
                                                if (i < 4) {
                                                    appendDetalleElementobody += '<li>' +
                                                        '<div class="octhumb">' +
                                                        '<a href="javascript:void(0);" class="foto_cuadro_derecha" data-id="'+detalleelemento.detel_id+'"><img src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>' +
                                                        '</div>' +
                                                        '<div class="desc">' +
                                                        '<a href="javascript:void(0);" class="abrir_pagina titulo_derecha'+detalleelemento.detel_id+'" data-blank="' + detalleelemento.detel_blank + '" data-url="' + detalleelemento.detel_url + '">' + detalleelemento.detel_descripcion + '</a>' +
                                                        '<h4><a href="javascript:void(0);" class="abrir_pagina subtitulo_derecha'+detalleelemento.detel_id+'" data-blank="' + detalleelemento.detel_blank + '" data-url="' + detalleelemento.detel_url + '"> ACCEDER A : ' + detalleelemento.detel_descripcion + '</a></h4>' +
                                                        '</div>' +
                                                        '</li>';
                                                }

                                            }
                                        });
                                    }


                                    appendElementos += '<article>' +
                                        '<div class="post-content">' +
                                        '<section id="cat2news">' +
                                        appendDetalleElementoheader +
                                        '<div class="othercat">' +
                                        '<ul class="oc-horizon">' +
                                        appendDetalleElementobody +
                                        '</ul>' +
                                        '</div>' +
                                        '</section>' +
                                        '</div>' +
                                        '<div class="clear"></div>' +
                                        '</article >';
                                }

                                if (elemento.fk_tipo_elemento == 13) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + (index + 2) + '" > ' + detalleelemento.detel_descripcion + ' </div ></li > ';
                                            }
                                            else {
                                                appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="six columns">' +
                                            '<div class="sponsor">' +
                                            appendsIzquierda +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="six columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsDerecha +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '</div>' +
                                            '</div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 14) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                            }
                                            else {
                                                appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="eight columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '<div class="four columns">' +
                                            '<div class="sponsor">' +
                                            appendsDerecha +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 15) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                            }
                                            else {
                                                appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="four columns">' +
                                            '<div class="sponsor">' +
                                            appendsDerecha +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="eight columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +

                                            '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 16) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsCentro = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        iz = 1, ce = 1, r = 1;
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + iz + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                iz++;
                                            }
                                            if (detalleelemento.detel_posicion == "C") {
                                                appendsCentro += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + ce + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                ce++;
                                            }
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += "<li class='modal_o' data-sece='" + JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'") + "'>" +
                                                    '<div class="ctitle wt' + r + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                r++;
                                            }
                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsCentro +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsDerecha +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 17) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendimageneslistasimple = "";
                                    var float_ = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (index == (detalleElementolista.length - 1)) {

                                                float_ = 'style="float:left;"';
                                            }
                                            appendimageneslistasimple += ' <div class="three tilesli columns abrir_pagina" ' + float_ + ' data-blank="' + detalleelemento.detel_blank + '" data-url="' + detalleelemento.detel_url + '">' +
                                                '<div class="space_" >' +
                                                '<div class="itemblog">' +
                                                '<div class="clear"></div>' +
                                                '<div class="thumb">' +
                                                '<a href="javascript:void(0);" class="" ><img src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>' +
                                                '</div>' +
                                                '<div class="clear"></div>' +
                                                '<div class="excerpt">' +
                                                '<h2 class="ntitle"><a href="javascript:void(0);">' + detalleelemento.detel_descripcion + '</a></h2>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>';
                                        });

                                        appendElementos += '<article>' +
                                            '<div class="post-content">' +
                                            appendimageneslistasimple +
                                            '</div>' +
                                            '<div class="clear"></div>' +
                                            '</article>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 18) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendContenido = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendContenido += '<li>' +
                                                '<div class="">' +
                                                '<div class="itemblog">' +
                                                '<div class="clear"></div>' +
                                                '<div class="lthumb">' +
                                                '<a href="javascript:void(0);"><img src="'+basePath+detalleelemento.detel_nombre + '" alt=""></a>' +
                                                '</div>' +
                                                '<div class="clear"></div>' +
                                                '<div class="excerpt">' +
                                                '<h2 class="ntitle"><a href="javascript:void(0);">' + detalleelemento.detel_descripcion + '</a></h2>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</li>';
                                        });
                                        appendElementos += '<div class="masonrystyle"><ul class="tiles">' + appendContenido + '</ul></div>';
                                    }
                                    mansonery = true;
                                }
                            })
                        }

                        appendSeccion += '<section id="singlepost">' + appendElementos + '</section><div class="separador"></div>';

                    })
                    $("#content").html(appendSeccion);
                    if (slider) {
                        $('.slider').orbit({
                            animation: 'fade',
                            timer_speed: 10000,
                            pause_on_hover: true,
                            resume_on_mouseout: false,
                            swipe: true,
                            animation_speed: 500
                        });
                    }

                    if (silertrans) {
                        LayoutVista.divscrool();
                    }

                    if (mansonery) {
                        $('.tiles').imagesLoaded(function () {
                            // Prepare layout options.
                            var options = {
                                autoResize: true, // This will auto-update the layout when the browser window is resized.
                                container: $('.masonrystyle'), // Optional, used for some extra CSS styling
                                offset: 20, // Optional, the distance between grid items
                                outerOffset: 20, // Optional, the distance to the containers border
                                itemWidth: 354 // Optional, the width of a grid item
                            };

                            // Get a reference to your grid items.
                            var handler = $('.tiles li');

                            // Call the layout function.
                            handler.wookmark(options);

                            // Capture clicks on grid items.
                            handler.click(function () {
                                // Randomize the height of the clicked item.
                                var newHeight = $('img', this).height() + Math.round(Math.random() * 300 + 30);
                                $(this).css('height', newHeight + 'px');

                                // Update the layout.
                                handler.wookmark();
                            });
                        });
                        $("a[data-rel^='prettyPhoto']").prettyPhoto();
                    }
                }
            }
        });
    };
    var _componentes = function () {

        $(document).on('click', 'a.foto_cuadro_derecha', function () {

            // info del lado derecho
            var data_derecha=$(this).data("id");
            var imgderecha = $(this);
            var imgder = imgderecha.find("img").attr('src');
            var titulo_derecha=$(".titulo_derecha"+data_derecha).text();
            var subtitulo_derecha=$(".subtitulo_derecha"+data_derecha).text();
            var data_blank_derecha=$(".titulo_derecha"+data_derecha).data("blank");
            var data_url_derecha=$(".titulo_derecha"+data_derecha).data("url");
    
            //info del lado izquiero

            var imgizquierda = $("div.foto_cuadro_izquiera").find("img").attr('src');
            var data_blank_izquierda=$(".abrir_pagina_izquierda").data("blank");
            var data_url_izquierda=$(".abrir_pagina_izquierda").data("url");
            var titulo_izquierda=$(".titulo_izquierda").text();
            var subtitulo_izquierda=$(".subtitulo_izquierda").text();

            // Hacer los cambios
             // lado derecho
             imgderecha.find("img").attr("src",imgizquierda);
             $(".titulo_derecha"+data_derecha).text(titulo_izquierda);
             $(".subtitulo_derecha"+data_derecha).text(subtitulo_izquierda);
             $(".titulo_derecha"+data_derecha).data("blank",data_blank_izquierda);
             $(".titulo_derecha"+data_derecha).data("url",data_url_izquierda);
            // lado izquierdo
            $("div.foto_cuadro_izquiera").find("img").attr("src",imgder);
            $(".abrir_pagina_izquierda").data("url",data_url_derecha);
            $(".abrir_pagina_izquierda").data("blank",data_blank_derecha);
            $(".titulo_izquierda").text(titulo_derecha);
            $(".subtitulo_izquierda").text(subtitulo_derecha);

            // $("div.foto_cuadro_izquiera").find("img").attr('src', imgder);
            // imgderecha.find("img").attr('src', imgizquierda);
        });

        $(document).on('click', 'li.modal_o', function () {
            var my_object2='';
            var dataseccion = $(this).data("sece");
            var formattedJson = JSON.stringify(dataseccion);
            var my_object = JSON.parse(formattedJson);
            if (my_object.length > 0) {
                var seccion = my_object[0];
                var elementos = seccion.elemento_modal;
                if(elementos==null){
                    my_object2 = JSON.parse(dataseccion);
                    seccion=my_object2[0];
                    elementos=seccion.elemento_modal;
                }
                $("#contenido_modal").html("");
                var appendSeccion = "";
                slider = false;
                silertrans = false;
                if (elementos.length > 0) {

                    var appendElementos = "";
                    $.each(elementos, function (index, elemento) {

                        if (elemento.fk_tipo_elemento == 1) {
                            appendElementos += '<header>' +
                                '<div class="loverate" style="right:-1px !important;"><a href="#"><i class="fa fa-exclamation-circle"></i></a></div>' +
                                '<h1>' + elemento.emod_titulo + '</h1>' +
                                '</header>';
                        }

                        if (elemento.fk_tipo_elemento == 2) {
                            appendElementos += '<header>' +
                                '<div class="postmeta">' +
                                '<div class="meta-date">' + elemento.emod_titulo + '</div>' +
                                '</div>' +
                                '</header>';
                        }

                        if (elemento.fk_tipo_elemento == 3) {
                            appendElementos +=
                                '<div class="divsinglepost"> ' + elemento.emod_titulo + '</div>';

                        }

                        if (elemento.fk_tipo_elemento == 4) {
                            appendElementos += '<article>' +
                                '<h4 class="lista_titulo">' + elemento.emod_titulo + '</h4>' +
                                '<div class="clear"></div>' +
                                '</article>';
                        }

                        if (elemento.fk_tipo_elemento == 5) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendLista = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendLista += '<li>' + detalleelemento.detelm_descripcion + '</li>';

                                });

                                appendElementos += '<ul class="listas">' +
                                    appendLista +
                                    '</ul>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 6) {
                            appendElementos += '<div class="divsinglepost"> <blockquote>' + elemento.emod_titulo + '</blockquote></div>';
                        }

                        if (elemento.fk_tipo_elemento == 7) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendElementos += '<article>' +
                                        '<div class="imgpost th">' +
                                        '<img src = "'+basePath+detalleelemento.detelm_nombre + '" alt = "">' +
                                        '</div>' +
                                        '</article>';

                                });
                            }

                        }

                        if (elemento.fk_tipo_elemento == 8) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += "<li class='modal_o'>" +
                                            "<div class='ctitle wt" + (index + 2) + "'>" + detalleelemento.detelm_descripcion + " </div></li>";
                                    }
                                    else {
                                        appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="six columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="six columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 9) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendContenido = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendContenido += '<article><div class="post-authorthumb th">' +
                                        '<div class="intern"><img src="'+basePath+detalleelemento.detelm_nombre + '" alt= "" style="padding:0px"></div>' +
                                        '</div>' +
                                        '<div class="post-content">' +
                                        detalleelemento.detelm_descripcion + '</div></article>'
                                });
                                appendElementos += '<div class="singlepost_">' + appendContenido + '</div>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 10) {
                            silertrans = true;
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendslider = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendslider += '<div class="contentBox">' +
                                        '<img src = "'+basePath+detalleelemento.detelm_nombre + '" alt = "Field" />' +
                                        '<div class="overlay">' +
                                        '<div class="title-carousel">' +
                                        '<a href="javascript:void(0);">' +
                                        '<div class="ticarousel"> ' + detalleelemento.detelm_descripcion + '</div>' +
                                        '</a>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>';

                                });

                                appendElementos += '<article><div class="post-content" ><div class="row">' +
                                    '<div class="twelve columns">' +
                                    '<div id="slidercontent" class="mixedContent onlyimgblank slidercontent" style="padding-left: 10px;padding-right: 10px;">' +
                                    appendslider +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div></article>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 11) {
                            slider = true;
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendslider = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendslider += '<li data-orbit-slide="headline-' + (index + 1) + '" style="width:100%">' +
                                        '<div class="row" style="padding-right:0px !important;padding-left:0px !important;">' +
                                        '<div class="twelve columns">' +
                                        '<div class="itemcatslide">' +
                                        '<div class="catf">' +
                                        '<div class="catf-format">' +
                                        //'<div class="fdate">15<br /><span>Dec</span></div>' +
                                        '</div>' +
                                        '<div class="imgslide">' +
                                        '<img src="'+basePath+detalleelemento.detelm_nombre + '" alt="" title="" class="slidefeatured">' +
                                        '</div>' +
                                        //'<div class="catf-caption">'+
                                        //    '<h2>Star Trek Into Darkness is a 2013 American</h2>'+
                                        //    '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>'+
                                        //'</div>'+
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</li>';

                                });

                                appendElementos += '<article>' +
                                    '<div class="post-content">' +
                                    '<div class="">' +
                                    '<div class="twelve columns">' +
                                    '<div class="catslide">' +
                                    '<ul class="slider" data-orbit>' +
                                    appendslider +
                                    '</ul>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="clear"></div>' +
                                    '</article>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 12) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendDetalleElementoheader = "";
                            var appendDetalleElementobody = "";
                            if (detalleElementolista.length > 0) {
                                var i = 0;
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (index == 0) {
                                        appendDetalleElementoheader += '<div class="featured" style="border: 4px solid #4e4c4c;padding-right: 20px;padding-top: 20px;">' +
                                            '<div class="thumb foto_cuadro_izquiera">' +
                                            ' <img src="'+basePath+detalleelemento.detelm_nombre + '" alt="">' +
                                            '<div class="overlay">' +
                                            '<div class="title-carousel">' +
                                            '<div class="ticarousel"> ' + detalleelemento.detelm_descripcion + '</div>' +
                                            '</div>' +
                                            '</div>' +
                                            ' </div>' +
                                            '<div class="excerpt">' +
                                            '<div class="desc">' +
                                            '<p class="pcats" style="padding-left: 0px !important;padding-right: 0px !important;">uc option voluptaria ex, nec habeo viris ei. Ne qui tota legendos, nam at debitis tractatos.</p>' +
                                            '<a href="javascript:void(0);"><i class="fa fa-external-link"></i> LEER MAS </a>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div>';
                                    }
                                    else {
                                        i++;
                                        if (i < 4) {
                                            appendDetalleElementobody += '<li>' +
                                                '<div class="octhumb">' +
                                                '<a href="javascript:void(0);" class="foto_cuadro_derecha"><img src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>' +
                                                '</div>' +
                                                '<div class="desc">' +
                                                '<a href="javascript:void(0);">' + detalleelemento.detelm_descripcion + '</a>' +
                                                '<h4><a href="#"> 5 YouTube Exercises to Strengthen Your Core</a></h4>' +
                                                '</div>' +
                                                '</li>';
                                        }

                                    }
                                });
                            }


                            appendElementos += '<article>' +
                                '<div class="post-content">' +
                                '<section id="cat2news">' +
                                appendDetalleElementoheader +
                                '<div class="othercat">' +
                                '<ul class="oc-horizon">' +
                                appendDetalleElementobody +
                                '</ul>' +
                                '</div>' +
                                '</section>' +
                                '</div>' +
                                '<div class="clear"></div>' +
                                '</article >';
                        }

                        if (elemento.fk_tipo_elemento == 13) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="six columns">' +
                                    '<div class="sponsor">' +
                                    appendsIzquierda +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="six columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsDerecha +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '</div>' +
                                    '</div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 14) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="eight columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="four columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 15) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="four columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="eight columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +

                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 16) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsCentro = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                iz = 1, ce = 1, r = 1;
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += '<li><div class="ctitle wt' + iz + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                        iz++;
                                    }
                                    if (detalleelemento.detelm_posicion == "C") {
                                        appendsCentro += '<li><div class="ctitle wt' + ce + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                        ce++;
                                    }
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + r + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                        r++;
                                    }
                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsCentro +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsDerecha +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 17) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendimageneslistasimple = "";
                            var float_ = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (index == (detalleElementolista.length - 1)) {

                                        float_ = 'style="float:left;"';
                                    }
                                    appendimageneslistasimple += ' <div class="three tilesli columns" ' + float_ + '>' +
                                        '<div class="space_" >' +
                                        '<div class="itemblog">' +
                                        '<div class="clear"></div>' +
                                        '<div class="thumb">' +
                                        '<a href="javascript:void(0);"><img src="'+basePath+detalleelemento.detelm_nombre + '" alt=""></a>' +
                                        '</div>' +
                                        '<div class="clear"></div>' +
                                        '<div class="excerpt">' +
                                        '<h2 class="ntitle"><a href="javascript:void(0);">' + detalleelemento.detelm_descripcion + '</a></h2>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>';
                                });

                                appendElementos += '<article>' +
                                    '<div class="post-content">' +
                                    appendimageneslistasimple +
                                    '</div>' +
                                    '<div class="clear"></div>' +
                                    '</article>';
                            }
                        }


                    })
                    appendSeccion += '<section id="singlepost">' + appendElementos + '</section><div class="separador"></div>';
                    $("#contenido_modal").html(appendSeccion);

                    $.pgwModal({
                        target: '#modalContent',
                        title: 'CORPORACION PJ',
                        maxWidth: 800
                    });
                }
            }
        });
        // Get the modal
        var modal = document.getElementById("modalCumple");

        // Get the button that opens the modal
        //var btn = document.getElementById("myBtn");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];
        $(document).on('click', 'li._cumple', function () {
           
            var per_numdoc = $(this).data("numdoc");
            var nombrecumpleaniero = $(this).find("h2.wtitle").html();
            var per_id = $(this).data("id");
            var diaCumpleanios = $(this).data("diacumple");
            var span = "Este " + diaCumpleanios + " celebraremos el cumpleaños de nuestro colaborador " + nombrecumpleaniero + ", acompáñanos a celebrarlo y saludarlo.";
            $('#fk_persona_saludada').val(per_id);
            $("#fk_persona_que_saluda").val(persona.per_id);
            $("#direccion_envio").val($(this).data("direccionenvio"));
            $("#tituloCumpleanios").text(span);
            var text='<textarea rows="4" id="text_mensaje" name="text_mensaje" style="width:100%;margin-top:20px;" placeholder="Mensaje de Cumpleaños"></textarea>';
            $("#text_message").html("");
            $("#text_message").html(text);
            crearEditor();
            var dataForm = {
                dni: per_numdoc,
            }
            responseSimple({
                url: "IntranetPJ/IntranetObtenerAreadeTrabajoxUsuarioJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var data = response.data;
                        var span = "";
                        $("#informacion_puesto").html("");
                        span += "<strong>Área: </strong>" + ((data.DE_AREA == null) ? "" : data.DE_AREA) + "<br/><strong> Puesto:</strong> " + ((data.DE_PUES_TRAB == null) ? "" : data.DE_PUES_TRAB) + " <br/><strong>Oficina:</strong> " + ((data.DE_SEDE == null) ? "" : data.DE_SEDE) + "<br/>"
                        $("#informacion_puesto").html(span);
                    
                    };
                    $("body").addClass("openModal");
                    modal.style.display = "block";
                    modal.style.zIndex = 10000;
                    
                }
            })

            //responseSimple("")
            ////$("#modalCumple").css('display', 'block');
            ////$('#modalCumple').show();
            //$.pgwModal({
            //    target: '#modalCumple',
            //    title:'Cumpleaños de '+ nombrecumpleaniero,
            //    maxWidth: 800,
            //});
        });
        $(document).on('click', 'span.close', function () {
            modal.style.display = "none";
            $("body").removeClass("openModal");
        })
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
                $("body").removeClass("openModal");
            }
        }
        $(document).on('click', '.btn_enviar_saludo', function () {
            // var filepath = $(".select_img>img").attr('src');
            // var imagen = filepath.replace(/^.*[\\\/]/, '');
            var fk_persona_que_saluda = persona.per_id;
            var fk_persona_saludada = $("#fk_persona_saludada").val();
            var sld_cuerpo = $("#text_mensaje").val();
            var direccion_envio = $("#direccion_envio").val();
            var dataForm = {
                // sld_avatar: imagen,
                fk_persona_que_saluda: fk_persona_que_saluda,
                fk_persona_saludada: fk_persona_saludada,
                sld_cuerpo: sld_cuerpo,
                direccion_envio: direccion_envio
            };
            if (sld_cuerpo !== "") {
                responseSimple({
                    url: "IntranetSaludosCumpleanios/IntranetSaludoCumpleanioInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        refresh(true);
                    }
                })
            }
            else {
                messageResponse({
                    text: "El mensaje no puede estar vacio",
                    type: "error"
                });
            }
        });

        //$(document).on('keyup','#sld_cuerpo',function (e) {
        //    var text_length = $('#sld_cuerpo').val();
        //    console.log(text_length);
        //    e.preventDefault();
        //});


        $(document).on('click', 'ul#locales li', function () {
            var tipo = $(this).data("tipo");
            redirect({ site: 'intranetPJ/mapa?tipo=' + tipo, time: 0 })
        });
        $(document).on('click', 'ul#boletasGDT li', function () {
            // var tipo = $(this).data("tipo");
            redirect({ site: 'intranetPJ/MisBoletasGDT', time: 0 })
        });


        $(document).on('click', 'div.img_face', function () {
            var div = $(this);
            $("div.img_face").removeClass("select_img");
            div.addClass("select_img");
        });
        $(document).on('click', '.abrir_pagina', function () {
            var otra_pagina = $(this).data("blank");
            var url = $(this).data("url");
            console.log(otra_pagina);
            console.log(url);
            if (url != "") {
                if (otra_pagina=="blank") {
                    window.open(url, '_blank', '');
                }
                else {
                    window.location.href = url;
                }
            }
            else {
                messageResponse({
                    text: 'No se Ha declarado la Url para este Sitio',
                    type: 'error',
                })
            }
        })

    };
    //
    // Return objects assigned to module
    //
    var _smoot = function () {
        $(".slidercontent").smoothDivScroll({
            touchScrolling: true,
        });
        console.log("assii")
        $(".contentBox .overlay").mouseover(function () {
            $(this).prev().css({
                opacity: 1,
                WebkitTransition: 'opacity 1s ease-in-out',
                MozTransition: 'opacity 1s ease-in-out',
                MsTransition: 'opacity 1s ease-in-out',
                OTransition: 'opacity 1s ease-in-out',
                transition: 'opacity 1s ease-in-out'
            });
        });
        $(".contentBox .overlay").mouseleave(function () {
            $(this).prev().css({
                opacity: 0.5,
                WebkitTransition: 'opacity 1s ease-in-out',
                MozTransition: 'opacity 1s ease-in-out',
                MsTransition: 'opacity 1s ease-in-out',
                OTransition: 'opacity 1s ease-in-out',
                transition: 'opacity 1s ease-in-out'
            });
        });
    };

    return {
        init: function () {
            _Layout();
            _componentes();
        },
        divscrool:function() {
            _smoot();
        }

    }
}();

// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    LayoutVista.init();
});