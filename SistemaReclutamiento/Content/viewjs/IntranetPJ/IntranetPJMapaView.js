var MapaVista = function () {
   
    var map = '';
    var _crearMapa = function (puntos) {
        $("#map").html("");
        var centro = (puntos.length > 0) ?
            { longitud: puntos[0].loc_longitud, latitud: puntos[0].loc_latitud }
            :
            { longitud: -77.0282400, latitud: -12.0431800 };
        //El centro sera el primer elemento del array, en caso este vacio sera un punto de LIMA
        map = new ol.Map({
            target: 'map',
            layers: [
                new ol.layer.Tile({
                    source: new ol.source.OSM()
                })
            ],
            view: new ol.View({
                projection: "EPSG:4326",
                //center: [parseFloat(response.data[0].loc_longitud), parseFloat(response.data[0].loc_latitud)],
                center: [centro.longitud, centro.latitud],
                zoom: 16,
                minzoom: 1,
                maxzoom: 18
            })
        });
        var washingtonWebMercator;
        if (puntos.length > 0) {
            $.each(puntos, function (index, value) {
                if(value.loc_latitud!=0&&value.loc_longitud!=0){
                    washingtonWebMercator = [parseFloat(value.loc_longitud), parseFloat(value.loc_latitud)];
                    $("#seccion-mapa").append('<div id="marker' + index + '" title="Marker"><img src="'+basePath+'Content/intranet/images/png/marker.png" /></div>');
                    $("#seccion-mapa").append('<div class="overlay" id="tittle' + index + '"><span class="label label-danger label-white middle">' + value.loc_nombre + '</span></div>');
                    var marker2 = new ol.Overlay({
                        position: washingtonWebMercator,
                        positioning: 'center-center',
                        element: document.getElementById('marker' + index),
                        stopEvent: false
                    });
                    map.addOverlay(marker2);
                    var tittle = new ol.Overlay({
                        position: washingtonWebMercator,
                        element: document.getElementById('tittle' + index)
                    });
                    map.addOverlay(tittle);
                }
            })
        }
    
    };
    var _inicio = function () {
        $("#tipo").val(tipo);
        responseSimple({
            url:"IntranetPJ/ListarLocalesporTipoJson",
            data:JSON.stringify({
                tipo: tipo
            }),
            refresh: false,
            callBackSuccess: function (response) {
                
                $("#resultados").html("");
                var puntos = response.data;
                $("#tipoBusqueda").html("");
                if(tipo=='ApuestasDeportivas'){
                    $("#tipoBusqueda").html("NOMBRE");
                }
                else{
                    $("#tipoBusqueda").html("DEPARTAMENTO");
                }
               
                _crearMapa(puntos);
                if (puntos.length > 0) {
                    var span = '';
                    var departamento='';
                    $("#span_total_puntos").html(puntos.length);
                    $.each(puntos, function (index, value) {
                        if(tipo=="Salas"){
                            departamento='<li><strong>Departamento: </strong> ' + (value.ubi_nombre==null?"":value.ubi_nombre) + '</li>';
                        }
                        else{
                            departamento='';
                        }
                        span += '<li class="cambiarCentro item" data-latitud="' + value.loc_latitud + '" data-longitud="' + value.loc_longitud + '"><h6 style="margin-bottom: 4px;margin-top: 4px;color: #d80000;">' + (index + 1) + '.- <span class="nombres" style="border-bottom:2px solid #d80000;">' + value.loc_nombre + '</span></h6><ul style="line-height: 1.2;margin-left: 37px;"><li><strong>Dirección: </strong> ' + value.loc_direccion + '</li>'+departamento+'</ul></li>';
                    })
                    $("#resultados").html(span);
                }
            }
        });
    };
    var _buscar=function(){
        //busquedaLocales
        let arrayPuntos = [];
        if(tipo=="Salas"){
            var dataForm='';
            if($("#nombre").val()!=""){
                dataForm = $('#busquedaLocales').serializeFormJSON();
            }
            else{
                dataForm={
                        tipo: tipo
                }
            }
            responseSimple({
                url: "IntranetPJ/ListarLocalesporTipoJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    $("#resultados").html("");
                    var puntos = response.data;
                    $("#map").html("");
                    _crearMapa(puntos);
                    if (puntos.length > 0) {
                        var span = '';
                        $("#span_total_puntos").html(puntos.length);
                        $.each(puntos, function (index, value) {
                            span += '<li class="cambiarCentro" data-latitud="' + value.loc_latitud + '" data-longitud="' + value.loc_longitud + '"><h6 style="margin-bottom: 4px;margin-top: 4px;color: #d80000;">' + (index + 1) + '.- <span style="border-bottom:2px solid #d80000;">' + value.loc_nombre + '</span></h6><ul style="line-height: 1.2;margin-left: 37px;"><li><strong>Dirección: </strong> ' + value.loc_direccion + '</li><li><strong>Departamento: </strong> ' + value.ubi_nombre + '</li></ul></li>';
                        })
                        $("#resultados").html(span);
                    }
                    else{
                        $("#span_total_puntos").html(0);    
                    }
                }
            });
        }
        else{
            var nombres = $('.nombres');
            var buscando=$("#nombre").val();
            var item='';

            for( var i = 0; i < nombres.length; i++ ){
                item = $(nombres[i]).html().toLowerCase();
            
                        if( buscando.length == 0 || item.indexOf( buscando ) > -1 ){
                            $(nombres[i]).parents('.item').show(); 
                        var latitud_api=$(nombres[i]).parents('.item').data('latitud');
                        var longitud_api=$(nombres[i]).parents('.item').data('longitud');
                        var nombre_api=$(nombres[i]).html();
                            var obj={
                                loc_latitud:latitud_api,
                                loc_longitud:longitud_api,
                                loc_nombre:nombre_api
                            }
                            arrayPuntos.push(obj);
                        }else{
                            $(nombres[i]).parents('.item').hide();
                        }
                
            }
            $("#span_total_puntos").html(arrayPuntos.length);
            _crearMapa(arrayPuntos);
        }
    };
    var _componentes = function () {
        $(document).on("click", ".btn-buscar", function () {
            _buscar();
        });

        $(document).on("click", "#btn_hide_show", function () {
            $('#scroll').toggle('slow');
        });
        $(document).on("click", ".cambiarCentro", function () {
            var latitud = $(this).data("latitud");
            var longitud = $(this).data("longitud");
            if(latitud==0||longitud==0){
                messageResponse({
                    text: "No se ha registrado una Latitud o Longitud para este punto",
                    type: "error"
                })
                return false;
            }
            map.setView(new ol.View({
                projection: "EPSG:4326",
                //center: [parseFloat(response.data[0].loc_longitud), parseFloat(response.data[0].loc_latitud)],
                center: [longitud, latitud],
                zoom: 16,
                minzoom: 1,
                maxzoom: 18
            }));
            //map.getView().setCenter(ol.proj.transform([longitud, latitud], 'EPSG:4326', 'EPSG:3857'));
        });
        $("#busquedaLocales").keypress(function(e) {
            if (e.which == 13) {
                _buscar();
                return false;
            }
        });
    };
    var _metodos = function () {
        // validar_Form({
        //     nameVariable: 'busquedaLocales',
        //     contenedor: '#busquedaLocales',
        //     rules: {
        //         nombre:
        //         {
        //             required: true,

        //         }
        //     },
        //     messages: {
        //         nombre:
        //         {
        //             required: 'Campo Obligatorio',
        //         }
        //     }
        // });
    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _metodos();
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    MapaVista.init();
});