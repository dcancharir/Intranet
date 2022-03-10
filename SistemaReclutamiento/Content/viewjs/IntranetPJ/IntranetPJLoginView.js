var LoginRegisterView = function () {
    //
    // Setup module components
    //
    var _componentes = function () {
        $(document).on("click", ".btn_ingresar", function (e) {
            $("#login-form").submit();
            if (_objetoForm_frmLogin.valid()) {
                var dataForm = $('#login-form').serializeFormJSON();
                responseSimple({
                    url: "IntranetPJ/IntranetLoginValdidarCredencialesJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        if (response.respuesta) {
                            redirect({ site: "IntranetPJ/index?menu=1" });
                        }
                        else {
                            $("#usu_password").val("");
                        }
                    },
                    time: 500
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).keypress(function (event) {
            var keycode = event.keyCode || event.which;
            if (keycode == '13') {
                $(".btn_ingresar").click();
            }
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmLogin',
            contenedor: '#login-form',
            rules: {
                usu_login: {
                    required: true,
                },
                usu_password: {
                    required: true
                },
            },
            messages: {
                usu_login: {
                    required: 'Campo Obligatorio',
                },
                usu_password: {
                    required: 'Campo Obligatorio'
                },
            }
        });
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _componentes();
            _metodos();

        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    LoginRegisterView.init();
});