var datos_head;
var datos;
var regresivo = 0;
var historialid = 0;
var registros = 0;

$(function () {

    LoadAutocomplete('Operacion.aspx?pagina=504', 'txtuser');
    
    LoadSelectItem('Operacion.aspx?pagina=902', 'cborol');
    LoadSelectItem('Operacion.aspx?pagina=902', 'cborol2');


    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        height: "300"
    });




});

function loaddata() {

    var login = $("#txtuser").val();
    var rolid = ($("#cborol2 option:selected").val() != '-1') ? $("#cborol2 option:selected").val() : '';
    
    $("#tbldetalle").html('');

    $("#tbldetalle").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('RUC/DNI'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Rol'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;



    $.ajax({
        url: 'Operacion.aspx?pagina=501&login=' + login + '&rolid=' + rolid,
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
                .append($('<td />').html(val.cliente_ruc_dni).attr('align', 'left'))
                .append($('<td />').html(val.cliente_nombre).attr('align', 'left'))
                .append($('<td />').html(val.rol).attr('align', 'left'))
                .append($('<td />').html('<img src="Img/edit.png" alt="Edit" title="Edit" onclick="Edit(\'' + val.codigo + '\',\'' + val.cliente_ruc_dni + '\',\'' + val.cliente_nombre + '\',\'' + val.rol_codigo + '\');"  style=\"cursor: pointer;\" />').attr('align', 'center'))
                .append($('<td />').html('<img src="Img/delete.png" alt="Delete" title="Delete"  onclick="Delete(\'' + val.codigo + '\');" style=\"cursor: pointer;\"/>').attr('align', 'center'))
                );

                fila++;
                numero++;
            });

            $("#registros").html('<b>' + (fila) + ' matches found</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
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

function Add() {
    $("#hcodigo").val('');
    $("#txtcliente_ruc_dni").val('');
    $("#txtcliente_nombre").val('');
    $("#txtcliente_clave").val('');
    $("#divclave").show();
    $("#txtcliente_ruc_dni").prop('readonly', false);
    $("#txtcliente_nombre").prop('readonly', false);
    $("#dialogDetail").modal('show');
}

function Edit(codigo, documento, nombre, rolid) {
    $("#hcodigo").val(codigo);
    $("#txtcliente_ruc_dni").val(documento);
    $("#txtcliente_nombre").val(nombre);
    $('#cborol option[value="' + rolid + '"]').attr("selected", "selected");
    $("#divclave").hide();
    $("#txtcliente_ruc_dni").prop('readonly', true);
    $("#txtcliente_nombre").prop('readonly', true);
    $("#dialogDetail").modal('show');
}

function Save() {

    var codigo = $("#hcodigo").val();
    var rolid = $("#cborol").val();

    if (codigo == "") {

        var documento=$("#txtcliente_ruc_dni").val();
        var nombre= $("#txtcliente_nombre").val();
        var clave = $("#txtcliente_clave").val();

        if (documento == "" || documento.length < 5) {
            alert("Debe ingresar un dni/ruc codigo (min 5 caracteres)");
            return;
        }

        if (nombre == "" || nombre.length < 5) {
            alert("Debe ingresar un nombre (min 5 caracteres)");
            return;
        }

        if (clave == "" || clave.length < 6)
        {
            alert("Debe ingresar una clave");
            return; alert("Debe ingresar una clave (min 6 caracteres)");
            return;
        }

        $.ajax({
            url: 'Operacion.aspx?pagina=505&documento=' + documento + '&nombre=' + nombre + '&clave=' + clave + '&rolid=' + rolid,
            success: function (data) {
                $("#dialogDetail").modal('hide');
                alert('Update successful!!');
                loaddata();
            }
        });
    }
    else {

        $.ajax({
            url: 'Operacion.aspx?pagina=502&codigo=' + codigo + '&rolid=' + rolid,
            success: function (data) {
                $("#dialogDetail").modal('hide');
                alert('Update successful!!');
                loaddata();
            }
        });
    }
}

function Delete(codigo) {

    if (confirm("Are you sure you want to delete?")) {
        $.ajax({
            url: 'Operacion.aspx?pagina=503&codigo=' + codigo,
            success: function (data) {
                alert('Delete successful!!');
                loaddata();
            }
        });
    }
}






