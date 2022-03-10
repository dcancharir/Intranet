let PanelEmpresas = function () {
 
    let _inicio= function(){
        $(".mySelect").append('<option value="">---Seleccione---</option>');
        $(".mySelect").select2({
            multiple: false, placeholder: "--Seleccione--"
        })
        responseSimple({
            url:'IntranetPJBoletasGDT/BolEmpresaListarJson',
            refresh:false,
            callBackSuccess:function(response){
               if(response.respuesta){
                if(response.data.length>0){
                    $.each(response.data, function (index, value) {
                        $("#cboEmpresa").append('<option value="' + value.emp_id + '" data-codofisis="'+value.emp_co_ofisis+'">' + value.emp_nomb + '</option>');
                    });  
                }
               }
            }
        })
    }
    let _metodos= function(){
        $(document).on('change','#cboEmpresa',function(e){
            e.preventDefault()
            let emp_id = $("#cboEmpresa option:selected").val()
            $("#det_empr_id").val(emp_id)
            ObtenerInformacionEmpresa(emp_id)
        })
        $(document).on('click','#btnNuevoCertificado',function(e){
            e.preventDefault()
            $("#modalFormularioCertificado").modal('show')
        })
        $(document).on('click','#btnSincronizar',function(e){
            e.preventDefault()
            responseSimple({
                url:'IntranetPJBoletasGDT/BolEmpresaSincronizarJson',
                refresh:false,
                callBackSuccess:function(response){
                    window.location.reload()
                }
            })
        })
        $(document).on('click','#btnGuardarCertificado',function(e){
            e.preventDefault();
            
            let selected = $("#cboEmpresa").find('option:selected');
            let codofisis = selected.data('codofisis')
            let empresa=selected.text()
            let emp_id = selected.val()

            let file = $('#certificado')[0].files[0];
            if(file==null){
                messageResponse({
                    text: "Debe Seleccionar Un Archivo",
                    type: "error"
                })
                return false;
            }
            else{
              
                let image_arr = file.name.split(".");
                let extension = image_arr.pop().toLowerCase();
                url='IntranetPJBoletasGDT/BolDetCertEmpresaInsertarJson';
                let dataForm=new FormData();
                dataForm.append('file',file);
                dataForm.append('emp_co_ofisis',codofisis)
                dataForm.append('emp_nomb',empresa)
                dataForm.append('det_empr_id',$("#det_empr_id").val())
                dataForm.append('det_pass_cert',$("#det_pass_cert").val())
                if (extension != 'pfx') {
                    messageResponse({
                        text: 'Sólo Se Permite formato PFX(.pfx)',
                        type: "warning"
                    });
                  
                }
                else {
                    responseFileSimple({
                        url:url,
                        data:dataForm,
                        refresh:false,
                        callBackSuccess:function(response){
                            if(response.respuesta){
                                ObtenerInformacionEmpresa(emp_id)
                                $("#modalFormularioCertificado").modal('hide')
                            }
                        }
                    })
                }
            }
        })
        $(document).on('change','.selectEnUso',function(e){
            e.preventDefault()
            let det_empr_id=$(this).data("emprid")
            let det_id=$(this).data("id")
            let det_en_uso=$(this).val()
            let dataForm={
                det_empr_id: det_empr_id,
                det_id: det_id,
                det_en_uso: det_en_uso,
            }
            responseSimple({
                url:'IntranetPJBoletasGDT/BolDetCertEmpresaEditarUsoJson',
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    setTimeout(function(){
                        ObtenerInformacionEmpresa(det_empr_id)
                    },2000)
                }
            })
        })
        $(document).on('click','.btnEliminarCertificado',function(e){
            e.preventDefault()
            let selected = $("#cboEmpresa").find('option:selected');
            let codofisis = selected.data('codofisis')
            let empresa=selected.text()
            let emp_id = selected.val()
            let det_id=$(this).data("id");
            let certificado =$(this).data('certificado')

            let dataForm={
                det_id: det_id,
                emp_co_ofisis:codofisis,
                emp_nomb:empresa,
                det_empr_id:emp_id,
                det_ruta_cert:certificado,
            }
            let url='IntranetPJBoletasGDT/BolDetCertEmpresaEliminarJson'
            responseSimple({
                url:url,
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    if(response.respuesta){
                        ObtenerInformacionEmpresa(emp_id)
                    }
                }
            })

        })
        $(document).on('click','#btnEditarEmpresa',function(e){
            e.preventDefault()
            let selected = $("#cboEmpresa").find('option:selected');
            let emp_id = selected.val()
            let dataForm={
                emp_id: emp_id,
            }
            responseSimple({
                url:'IntranetPJBoletasGDT/BolEmpresaIdObtenerJson',
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    if(response.respuesta){
                        let empresa=response.data
                        $("#emp_id").val(empresa.emp_id)
                        $("#emp_nomb").val(empresa.emp_nomb)
                        $("#emp_nomb_corto").val(empresa.emp_nomb_corto)
                        $("#emp_rucs").val(empresa.emp_rucs)
                        $("#cboFirmaVisible").val(empresa.emp_firma_visible)
                        $("#emp_firma_img").val(empresa.emp_firma_img)
                        $("#emp_nom_rep_legal").val(empresa.emp_nom_rep_legal)
                        $("#emp_co_ofisis").val(empresa.emp_co_ofisis)
                        $("#modalFormularioEmpresa").modal('show')
                    }
                }
            })
        })
        $(document).on('click','#btnGuardarEmpresa',function(e){
            e.preventDefault()
            let file = $('#firma')[0].files[0];
            let validarFile=false;
            if(file!=null){
                let image_arr = file.name.split(".");
                let extension = image_arr.pop().toLowerCase();
                if (extension != 'jpg' && extension != 'png' && extension != 'jpeg') {
                    messageResponse({
                        text: 'Sólo Se Permite formato de Imagen(.jpg,.png,.jpeg)',
                        type: "warning"
                    });
                }
                else{
                    validarFile=true;
                }
            }
            else{
                validarFile=true;
            }
            if(validarFile){
                let emp_id=$("#emp_id").val()
                url='IntranetPJBoletasGDT/BolEmpresaEditarJson';
                let dataForm=new FormData();
                dataForm.append('file',file);
                dataForm.append('emp_id',emp_id)
                dataForm.append('emp_img_firma',$("#emp_img_firma").val())
                dataForm.append('emp_nomb',$("#emp_nomb").val())
                dataForm.append('emp_nomb_corto',$("#emp_nomb_corto").val())
                dataForm.append('emp_nom_rep_legal',$("#emp_nom_rep_legal").val())
                dataForm.append('emp_rucs',$("#emp_rucs").val())
                dataForm.append('emp_firma_visible',$("#cboFirmaVisible").val())
                dataForm.append('emp_co_ofisis',$("#emp_co_ofisis").val())

                responseFileSimple({
                    url:url,
                    data:dataForm,
                    refresh:false,
                    callBackSuccess:function(response){
                        if(response.respuesta){
                            $("#modalFormularioEmpresa").modal('hide')
                            ObtenerInformacionEmpresa(emp_id)
                        }
                    }
                })
            }
        })
    }
    let ObtenerInformacionEmpresa=function(emp_id){
        let dataForm={
            emp_id:emp_id,
        }
        responseSimple({
            url:'IntranetPJBoletasGDT/BolEmpresaIdObtenerJson',
            refresh:false,
            data: JSON.stringify(dataForm),
            callBackSuccess:function(response){
                console.log(response)
              if(response.respuesta){
                    $("#divInfoEmpresa").show()
                    let empresa=response.data
                    $("#txtRazonSocial").val(empresa.emp_nomb)
                    $("#txtNombreCorto").val(empresa.emp_nomb_corto)
                    $("#txtRepresentanteLegal").val(empresa.emp_nom_rep_legal)
                    $("#txtRucs").val(empresa.emp_rucs)
                    $("#txtUbicacion").val(empresa.emp_prov+ " - " + empresa.emp_depa+" - "+empresa.emp_pais)
                    $("#txtFirmaVisible").val(empresa.emp_firma_visible==1?"SI":"NO")
                    if(empresa.emp_firma_img_base64!="" && empresa.emp_firma_img_base64!=null){
                        $("#divEmpFirmaBase64").show()
                        $("#imgEmpFirmaBase64").attr('src', 'data:image/png;base64,'+empresa.emp_firma_img_base64)
                    }
                    else{
                        $("#divEmpFirmaBase64").hide()
                    }
                    var addtabla = $("#divDataTableDetalle");
                    $(addtabla).empty();
                    if(empresa.DetalleCerts.length>0){
                        $("#divInfoCertificados").show()
                        $(addtabla).append('<table id="dataTableDetalle" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                        objetodatatable = $("#dataTableDetalle").DataTable({
                            "bDestroy": true,
                            "bSort": false,
                            "scrollCollapse": true,
                            "scrollX": false,
                            "sScrollX": "100%",
                            "paging": true,
                            "autoWidth": false,
                            "bAutoWidth": true,
                            "bProcessing": true,
                            "bDeferRender": true,
                            data: empresa.DetalleCerts,
                            columns: [
                                { 
                                    data: null, title: "Nombre" ,"render":function(value,type,row){
                                        return row.det_nomb_cert
                                    }
                                },
                                {
                                    data: null, title: "Certificado",
                                    "render": function (value,type,row) {
                                        return row.det_ruta_cert
                                    }
                                },
                                {
                                    data: null, title: "En uso",
                                    "render": function (value,type,row) {
                                        let select='<select class="form-control input-sm selectEnUso" data-emprid="'+row.det_empr_id+'" data-id="'+row.det_id+'" style="width:100%">'
                                        if(row.det_en_uso==1){
                                            select+='<option value="1" selected>SI</option><option value="0">NO</option>'
                                        }
                                        else{
                                            select+='<option value="1">SI</option><option value="0" selected>NO</option>'
                                        }
                                        select+='</select>'
                                        return select
                                    }
                                },
                                   { 
                                    data: null, title: "Acciones" ,"render":function(value,type,row){
                                        return '<a class="btn btn-white btn-danger btn-round btnEliminarCertificado" style="padding-top: 3px;padding-bottom: 3px;" data-id="'+row.det_id+'" data-certificado="'+row.det_ruta_cert+'">Eliminar</a>'
                                    }
                                },
                            ]
                        });
                    }
                    else {
                        $("#divInfoCertificados").hide()
                    }
              }
              else{
                $("#divInfoEmpresa").hide()
                $("#divInfoCertificados").hide()
              }
            }
        })
    }
    return {
        init:function(){
            _inicio()
            _metodos()
        }
    }
}()
document.addEventListener("DOMContentLoaded", function () {
    PanelEmpresas.init()
})