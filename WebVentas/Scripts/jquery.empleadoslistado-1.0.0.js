var ubigeo;

$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)

    $.datepicker.regional['es'] = {
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mc', 'Ju', 'Vi', 'Sa']
    }

    $.datepicker.setDefaults($.datepicker.regional['es']);

    $('#ribbon').ribbon();

    $("#fecha_nacimiento").datepicker({
        beforeShow: function (i) {
            if ($(i).attr('readonly')) { return false; }
        },
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        maxDate: '+0m +0d',
    }).datepicker("setDate", "0").mask('99/99/9999');;


    $("#dialog").dialog({
        autoOpen: false,
        maxWidth: 680,
        maxHeight: 640,
        width: 680,
        height: 640,
        modal: true,
    });

    ubigeo = LoadData('Operacion.aspx?ope=002');

    loadinicio();

    $('#cbodepartamento').on('change', function () {

        $("#cboprovincia").children().remove().end();
        $("#cboprovincia").append('<option value="00">[Seleccinar]</option>');
        var departamento = this.value;

        $.each(ubigeo, function (i, val) {

            if (val.departamento == departamento && val.provincia != '00' && val.distrito == '00') {
                $("#cboprovincia").append('<option value="' + val.provincia + '">' + val.nombre + '</option>');
            }
        });

        $("#cbodistrito").children().remove().end();
        $("#cbodistrito").append('<option selected value="00">[Ninguno]</option>');
    });


    $('#cboprovincia').on('change', function () {
        $("#cbodistrito").children().remove().end();
        $("#cbodistrito").append('<option value="00">[Seleccinar]</option>');
        var departamento = $("#cbodepartamento").val();
        var provincia = this.value;

        $.each(ubigeo, function (i, val) {
            if (val.departamento == departamento && val.provincia == provincia && val.distrito != '00') {
                $("#cbodistrito").append('<option value="' + val.distrito + '">' + val.nombre + '</option>');
            }
        });
    });

    $('#cbodistrito').on('change', function () {
        var departamento = $("#cbodepartamento").val();
        var provincia = $("#cboprovincia").val();
        var distrito = this.value;
        $("#ubigeo").val(departamento + provincia + distrito);
    });

    
    LoadSelectItem('Operacion.aspx?ope=501', 'cbopuntos');

    $('.ribbon-button').click(function () {

        if (this.isEnabled()) {

            switch ($(this).attr('id')) {
                case "add-page-btn":
                    nuevo();
                    break;

                case "open-page-btn":
                    modificar();
                    break;

                case "del-page-btn":
                    eliminar();
                    break;

                case "search-btn":
                    buscar();
                    break;

                case "retornar-btn":
                    retornar();
                    break;

                case "exportar-page-btn":
                    exportar();
                    break;
            }
        }
    });

});

function exportar() {
    $("#tbldetalle").table2excel({
        name: "Table2Excel",
        filename: "Empleados",
        fileext: ".xls"
    });
}


function retornar() {
    window.location.href = 'Menu.aspx';
}

function loadinicio() {

    $("#registros").html("");
    $("#tbldetalle").html('');

    $("#tbldetalle").append(
        $('<colgroup/>')
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
    );

    $("#tbldetalle").append($('<thead/>').append(
        $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Apellidos'))
            .append($('<th />').html('Sexo'))
            .append($('<th />').html('Fecha Nac'))
            .append($('<th />').html('Tipo'))
            .append($('<th />').html('Numero'))
            .append($('<th />').html('Direccion'))
            .append($('<th />').html('Acceso'))
            .append($('<th />').html('Usuario'))
            .append($('<th />').html('Serie'))
            .append($('<th />').html(''))
    ));

    
    loadubigeo();
}

function loadubigeo() {
    $("#registros").html('<b>0 registros encontrados</b>');

    $("#cbodepartamento").children().remove().end();
    $("#cbodepartamento").append('<option value="00">[Seleccinar]</option>');

    $("#cboprovincia").children().remove().end();
    $("#cboprovincia").append('<option selected value="00">[Ninguno]</option>');

    $("#cbodistrito").children().remove().end();
    $("#cbodistrito").append('<option selected value="00">[Ninguno]</option>');

    $.each(ubigeo, function (i, val) {
        if (val.provincia == '00' && val.distrito == '00') {
            $("#cbodepartamento").append('<option value="' + val.departamento + '">' + val.nombre + '</option>');
        }
    });
}

function buscar() {

    var empleado = $("#empleado").val().trim();

    $("#registros").html("");
    $("#tbldetalle tbody").remove();
    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=901&nombre=' + encodeURIComponent(empleado),
        async: false,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            var fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tbldetalle").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.apellidos).attr('align', 'left'))
                    .append($('<td />').html(val.sexo).attr('align', 'center'))
                    .append($('<td />').html(val.fecha_nacimiento).attr('align', 'center'))
                    .append($('<td />').html(val.tipo_documento).attr('align', 'center'))
                    .append($('<td />').html(val.num_documento).attr('align', 'left'))
                    .append($('<td />').html(val.direccion).attr('align', 'left'))
                    .append($('<td />').html(val.acceso).attr('align', 'left'))
                    .append($('<td />').html(val.usuario).attr('align', 'left'))
                    .append($('<td />').html(val.serie).attr('align', 'center'))
                    .append($('<td />').html("<input type='radio' value='" + val.id + "' id='r" + val.id + "' name='radio' onclick='seleccion(" + val.id + ")' />").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');

            if (fila > 0)
                $('#exportar-page-btn').enable();
            else
                $('#exportar-page-btn').disable();


            $('#tbldetalle tbody tr').on("click", function (e) {
                var id = $(this).find("input[type=radio]").val();
                $("#hnombre").val($(this).find("td:nth-child(2)").text());
                seleccion_row(id);
            });

            $('#tbldetalle tbody tr').on("dblclick", function (e) {
                var id = $(this).find("input[type=radio]").val();
                $("#hnombre").val($(this).find("td:nth-child(2)").text());
                seleccion_row(id);
                modificar();
            });
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function seleccion_row(id) {

    $("#r" + id).prop('checked', true);
    $('#open-page-btn').enable();
    $('#del-page-btn').enable();
    $("#hid").val(id);
}

function seleccion(id) {
    $('#open-page-btn').enable();
    $('#del-page-btn').enable();
    $("#hid").val(id);
}

function nuevo() {
    $("#id").val('0');
    $("#hnombre").val('');

    $("#nombre").val('');
    $("#apellidos").val('');
    $("#numerodocumento").val('');
    $("#cbodepartamento").val('00');
    $("#cboprovincia").val('00');
    $("#cbodistrito").val('00');
    $("#direccion").val('');
    $("#telefono").val('');
    $("#email").val('');
    $("#usuario").val('');
    $("#password").val('');
    $("#serie").val('');
    $("#cbotipo_documento option[value=-1]").prop("selected", true);

    $("#dialog").dialog("open");
}


function grabar() {

    var id = $("#hid").val();
    var nombre = $("#nombre").val();
    var apellidos = $("#apellidos").val();
    var sexo = $("#cbosexo option:selected").val();
    var fecha_nacimiento = $("#fecha_nacimiento").val();
    var tipo_documento = $("#cbotipo_documento option:selected").val();
    var numerodocumento = $("#numerodocumento").val();
    var direccion = $("#direccion").val().trim();
    var departamento = $("#cbodepartamento option:selected").text();
    var provincia = $("#cboprovincia option:selected").text();
    var distrito = $("#cbodistrito option:selected").text();
    var telefono = $("#telefono").val();
    var email = $("#email").val();
    var acceso = $("#cboacceso option:selected").val();
    var usuario = $("#usuario").val();
    var password = $("#password").val();
    var serie = $("#serie").val();
    var puntosid = $("#cbopuntos option:selected").val();

    if (nombre.length < 2) {
        alert('El nombre debe contener mas de 2 caracteres');
        return;
    }

    if (apellidos.length < 2) {
        alert('El apellido debe contener mas de 2 caracteres');
        return;
    }

    if (usuario.length < 6) {
        alert('El usuario debe contener mas de 6 caracteres');
        return;
    }

    if (password.length < 6) {
        alert('La contraseña debe contener mas de 6 caracteres');
        return;
    }

    $.ajax({
        url: 'Operacion.aspx?ope=902&id=' + id + '&nombre=' + nombre + '&apellidos=' + apellidos + '&sexo=' + sexo +
            '&fecha_nacimiento=' + fecha_nacimiento + '&tipo_documento=' + tipo_documento + '&num_documento=' + numerodocumento +
            '&departamento=' + departamento + '&provincia=' + provincia + '&distrito=' + distrito +
            '&direccion=' + direccion + '&telefono=' + telefono + '&email=' + email + '&acceso=' + acceso +
            '&usuario=' + usuario + '&password=' + password + '&serie=' + serie + '&puntosid=' + puntosid,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            if (data != "")
                alert(data);
            else {
                alert("La transacción se realizó satisfactoriamente!");
            }
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog").dialog("close");
}

function modificar() {

    var id = $("#hid").val();

    $.ajax({
        url: 'Operacion.aspx?ope=900&codigo=' + id,
        async: false,
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#nombre").val(val.nombre);
                $("#apellidos").val(val.apellidos);
                $("#cbosexo option[value=" + val.sexo + "]").prop("selected", true);
                $("#fecha_nacimiento").val(val.fecha_nacimiento);
                $("#cbotipo_documento option[value=" + val.tipo_documento + "]").prop("selected", true);
                $("#cbotipo_documento option[value=-1]").prop("selected", true);
                $("#cboacceso option[value=" + val.acceso + "]").prop("selected", true);
                $("#numerodocumento").val(val.num_documento);

                loadubigeo();

                if (val.departamento != '') {
                    $("#cbodepartamento option:contains(" + val.departamento + ")").attr('selected', 'selected').trigger('change');
                    if (val.provincia != '') {
                        $("#cboprovincia option:contains(" + val.provincia + ")").attr('selected', 'selected').trigger('change');
                        if (val.distrito != '') {
                            $("#cbodistrito option:contains(" + val.distrito + ")").attr('selected', 'selected');
                        }
                    }
                }


                $("#direccion").val(val.direccion);
                $("#telefono").val(val.telefono);
                $("#email").val(val.email);
                $("#acceso").val(val.acceso);
                $("#usuario").val(val.usuario);
                $("#password").val(val.password);
                $("#serie").val(val.serie);

                if (val.puntosid != '') { $("#cbopuntos option[value=" + val.puntosid + "]").prop("selected", true); }
            });
        }
    });

    $("#dialog").dialog('open');
}

function eliminar() {

    var nombre = $("#hnombre").val();

    if (confirm('Esta seguro de eliminar al empleado ' + nombre + ' ?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=903&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Actualizando ...');
            },
            success: function (data) {

                loaddata();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function onRequestStart(sender, args) {

    var elem = sender._postBackSettings.asyncTarget;

    if (typeof elem != "undefined") {

        BlockBegin('Buscando');
    }
}

function onRequestEnd(sender, args) {
    var elem = sender._postBackSettings.asyncTarget;

    BlockEnd();
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    return !(charCode > 31 && (charCode < 48 || charCode > 57));
}
