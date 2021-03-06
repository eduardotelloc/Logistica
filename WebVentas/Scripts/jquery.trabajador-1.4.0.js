
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


    $("#fecha_nacimiento").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true
    }).datepicker("setDate", "0");



    tips = $(".validateTips");

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " +
              min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    LoadSelectItem('Operacion.aspx?ope=501', 'cbopuntos');

});

function loaddata(){
    var cliente = $("#cliente").val();
    
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
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $.ajax({
        url: 'Operacion.aspx?ope=901&nombre=' + cliente,
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
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
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"modificar('" + val.id + "','" + val.nombre + "','" + val.apellidos + "','" + val.sexo + "','" + val.fecha_nacimiento + "','" + val.tipo_documento + "','" + val.num_documento + "','" + val.direccion + "','" + val.telefono + "','" + val.email + "','" + val.acceso + "','" + val.usuario + "','" + val.password + "','" + val.serie + "','" + val.puntosid + "')\"><img src=\"Img/edit.png\" title=\"Editar\" /></a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"eliminar('" + val.id + "')\"><img src=\"Img/eliminar.png\" title=\"Eliminar\" /></a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}


function grabar() {
    var nombre = $("#nombre").val();
    var apellidos = $("#apellidos").val();
    var sexo = $("#cbosexo option:selected").val();
    var fecha_nacimiento = $("#fecha_nacimiento").val();
    var tipo_documento = $("#cbotipo_documento option:selected").val();
    var num_documento = $("#num_documento").val();
    var direccion = $("#direccion").val();
    var telefono = $("#telefono").val();
    var email = $("#email").val();
    var acceso = $("#cboacceso option:selected").val();
    var usuario = $("#usuario").val();
    var password = $("#password").val();
    var serie = $("#serie").val();
    var puntosid = $("#cbopuntos option:selected").val();

    var id = $("#id").val();


    $.ajax({
        url: 'Operacion.aspx?ope=902&id=' + id + '&nombre=' + nombre + '&apellidos=' + apellidos + '&sexo=' + sexo + '&fecha_nacimiento=' + fecha_nacimiento + '&tipo_documento=' + tipo_documento + '&num_documento=' + num_documento + '&direccion=' + direccion + '&telefono=' + telefono + '&email=' + email + '&acceso=' + acceso + '&usuario=' + usuario + '&password=' + password + '&serie=' + serie + '&puntosid=' + puntosid,
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

    $("#dialog").dialog("close");
}

function modificar(id, nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento, direccion, telefono, email, acceso, usuario, password, serie, puntosid) {

    $("#nombre").val(nombre);
    $("#apellidos").val(apellidos);
    $("#cbosexo option[value=" + sexo + "]").prop("selected", true);
    $("#fecha_nacimiento").val(fecha_nacimiento);
    $("#cbotipo_documento option[value=" + tipo_documento + "]").prop("selected", true);
    $("#cbotipo_documento option[value=-1]").prop("selected", true);
    $("#cboacceso option[value=" + acceso + "]").prop("selected", true);
    $("#num_documento").val(num_documento);
    $("#direccion").val(direccion);
    $("#telefono").val(telefono);
    $("#email").val(email);
    $("#acceso").val(acceso);
    $("#usuario").val(usuario);
    $("#password").val(password);
    $("#serie").val(serie);

    if (puntosid != '') { $("#cbopuntos option[value=" + puntosid + "]").prop("selected", true); }

    $("#id").val(id);

    $("#dialog").dialog("open");
}

function eliminar(id) {
    if (confirm('Esta seguro de eliminar el registro?')) {


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

function nuevo() {
    $("#id").val('0');
    $("#nombre").val('');
    $("#apellidos").val('');
    $("#num_documento").val('');
    $("#direccion").val('');
    $("#telefono").val('');
    $("#email").val('');
    $("#usuario").val('');
    $("#password").val('');
    $("#serie").val('');
    $("#cbotipo_documento option[value=-1]").prop("selected", true);
    $("#dialog").dialog("open");
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

