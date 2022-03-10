var PanelFooter = function () {
   
    var _inicio=function(){
        responseSimple({
            url:"IntranetFooter/IntranetFooterObtenerImagenes",
            refresh:false,
            callBackSuccess:function(response){
                console.log(response);
                var data=response.data;
                if(data.length>0){
                    $.each(data, function (index, value) {
                       if(value.foot_posicion=="D"){
                            $("#ruta_anterior_derecha").val(value.ruta_anterior);
                            $("#estado_derecha").val(value.foot_estado);
                            $("#foot_descripcion_derecha").val(value.foot_descripcion);
                            //Insertar Imagen Actual
                            let preview=document.getElementById('imagen_actual_derecha'),
                            image=document.createElement('img');
                            image.src=basePath+"IntranetFiles/Footer/"+value.foot_imagen;
                            image.style.width="100%";
                            image.style.height="100%";
                            preview.innerHTML='';
                            preview.append(image);
                       }
                       else{
                        $("#ruta_anterior_izquierda").val(value.ruta_anterior);
                        $("#estado_izquierda").val(value.foot_estado);
                        $("#foot_descripcion_izquierda").val(value.foot_descripcion);
                          //Insertar Imagen Actual
                          let preview=document.getElementById('imagen_actual_izquierda'),
                          image=document.createElement('img');
                          image.src=basePath+"IntranetFiles/Footer/"+value.foot_imagen;
                          image.style.width="100%";
                          image.style.height="100%";
                          preview.innerHTML='';
                          preview.append(image);
                       }
                    });
                }
            }
        })        
    };
    var _componentes = function () {
        $(document).on('change', '#file_izquierda', function (e) {
            let reader = new FileReader();
            reader.onload = function () {
                let preview = document.getElementById('imagen_actual_izquierda'),
                    image = document.createElement('img');
                image.src = reader.result;
                image.style.width = "100%";
                image.style.height = "100%";
                preview.innerHTML = '';
                preview.append(image);
            }
            reader.readAsDataURL(e.target.files[0]);
        });

        $(document).on('change', '#file_derecha', function (e) {
            let reader = new FileReader();
            reader.onload = function () {
                let preview = document.getElementById('imagen_actual_derecha'),
                    image = document.createElement('img');
                image.src = reader.result;
                image.style.width = "100%";
                image.style.height = "100%";
                preview.innerHTML = '';
                preview.append(image);
            }
            reader.readAsDataURL(e.target.files[0]);
        });

        $(document).on('click','.btn-guardar-footer-izquierda',function(){
            $("#form_footer_izquierda").submit();
            if (_objetoForm_form_footer_izquierda.valid()) {
                var dataForm = new FormData(document.getElementById("form_footer_izquierda"));
                var url = "IntranetFooter/IntranetFooterInsertarJson";
                responseFileSimple({
                    url: url,
                    data: dataForm,
                    refresh: false,
                    callBackSuccess: function (response) {
                        var value=response.data;
                        if(response.respuesta==true){
                            let preview=document.getElementById('imagen_actual_izquierda');
                            preview.innerHTML="";
                            image=document.createElement('img');
                            image.src=basePath+"IntranetFiles/Footer/"+value.foot_imagen;
                            image.style.width="100%";
                            image.style.height="100%";
                            preview.innerHTML='';
                            preview.append(image);
                            //let preview2=document.getElementById('preview_izquierda');
                            //preview2.innerHTML="<h1>Pre - Vista</h1>";
                            $("#ruta_anterior_izquierda").val(value.ruta_anterior);
                            $("#estado_izquierda").val(value.foot_estado);
                            $("#foot_descripcion_izquierda").val(value.foot_descripcion);
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
        $(document).on('click','.btn-guardar-footer-derecha',function(){
            $("#form_footer_derecha").submit();
            if (_objetoForm_form_footer_izquierda.valid()) {
                var dataForm = new FormData(document.getElementById("form_footer_derecha"));
                var url = "IntranetFooter/IntranetFooterInsertarJson";
                responseFileSimple({
                    url: url,
                    data: dataForm,
                    refresh: false,
                    callBackSuccess: function (response) {
                        var value=response.data;
                        if(response.respuesta==true){
                            let preview=document.getElementById('imagen_actual_derecha');
                            preview.innerHTML="";
                            image=document.createElement('img');
                            image.src=basePath+"IntranetFiles/Footer/"+value.foot_imagen;
                            image.style.width="100%";
                            image.style.height="100%";
                            preview.innerHTML='';
                            preview.append(image);
                            //let preview2=document.getElementById('preview_derecha');
                            //preview2.innerHTML="<h1>Pre - Vista</h1>";
                            $("#ruta_anterior_derecha").val(value.ruta_anterior);
                            $("#estado_derecha").val(value.foot_estado);
                            $("#foot_descripcion_derecha").val(value.foot_descripcion);
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
    }

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_footer_izquierda',
            contenedor: '#form_footer_izquierda',
            rules: {
                foot_posicion:
                {
                    required: false,

                },
                foot_imagen:
                {
                    required: false,

                }

            },
            messages: {
                foot_posicion:
                {
                    required: 'Campo Obligatorio',
                },
                foot_imagen:
                {
                    required: 'Campo Obligatorio',
                }

            }
        });
        validar_Form({
            nameVariable: 'form_footer_derecha',
            contenedor: '#form_footer_derecha',
            rules: {
                foot_posicion:
                {
                    required: false,

                },
                foot_imagen:
                {
                    required: false,

                }

            },
            messages: {
                foot_posicion:
                {
                    required: 'Campo Obligatorio',
                },
                foot_imagen:
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
            _inicio();
            _componentes();
            _metodos();

        },
        init_ListarFooters: function () {
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFooter.init();
});