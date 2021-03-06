$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)
    $("#tabs").tabs();
    tips = $(".validateTips");
    
    $.datepicker.regional['es'] = {
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mc', 'Ju', 'Vi', 'Sa']
    }

    $.datepicker.setDefaults($.datepicker.regional['es']);


    $("#fecha_tipo_cambio").datepicker({
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

    $("#dialog-Serie").dialog({
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

    $("#dialog-TipoCambio").dialog({
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

    $("#dialog-Variable").dialog({
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

    loaddataSerie();
    loaddataTipoCambio();
    loaddataVariable();

});

function loaddataSerie() {
    
    $("#tblSerie").html('');

    $("#tblSerie").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblSerie").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Tipo'))
            .append($('<th />').html('Indicador'))
            .append($('<th />').html('Serie'))
            .append($('<th />').html('Correaltivo'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

   

    $.ajax({
        url: 'Operacion.aspx?ope=911',
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);
            
            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblSerie").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.tipodes).attr('align', 'left'))
                    .append($('<td />').html(val.indicadordes).attr('align', 'left'))
                    .append($('<td />').html(val.serie).attr('align', 'center'))
                    .append($('<td />').html(val.correlativo).attr('align', 'right'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"smodificar('" + val.id + "','" + val.tipo + "','" + val.indicador + "','" + val.serie + "','" + val.correlativo + "')\">Editar</a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"seliminar('" + val.id + "')\">Eliminar</a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#sregistros").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function sgrabar()
{
    var tipo = $("#cbotipo option:selected").val();
    var indicador = $("#cboindicador option:selected").val();
    var serie = $("#serie").val();
    var correlativo = $("#correlativo").val();
    var id = $("#sid").val();
    

    $.ajax({
        url: 'Operacion.aspx?ope=912&id=' + id + '&tipo=' + tipo + '&indicador=' + indicador + '&serie=' + serie + '&correlativo=' + correlativo,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            loaddataSerie();
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog-Serie").dialog("close");
}

function smodificar(id, tipo, indicador, serie, correlativo) {
    $("#cbotipo option[value=" + tipo + "]").prop("selected", true);
    $("#cboindicador option[value=" + indicador + "]").prop("selected", true);
    $("#serie").val(serie);
    $("#correlativo").val(correlativo);
    $("#sid").val(id);

    $("#dialog-Serie").dialog("open");
}

function seliminar(id)
{
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=913&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Actualizando ...');
            },
            success: function (data) {

                loaddataSerie();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function snuevo() {
    $("#sid").val('0');
    $("#serie").val('001');
    $("#correaltivo").val('0');
    $("#dialog-Serie").dialog("open");
}


function loaddataTipoCambio() {

    $("#tblTipoCambio").html('');

    $("#tblTipoCambio").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblTipoCambio").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Fecha'))
            .append($('<th />').html('Monto Venta'))
            .append($('<th />').html('Monto Compra'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;



    $.ajax({
        url: 'Operacion.aspx?ope=801',
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblTipoCambio").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.fecha_tipo_cambio).attr('align', 'center'))
                    .append($('<td />').html(val.cambio_venta).attr('align', 'right'))
                    .append($('<td />').html(val.cambio_compra).attr('align', 'right'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"tmodificar('" + val.id + "','" + val.fecha_tipo_cambio + "','" + val.cambio_venta + "','" + val.cambio_compra + "')\">Editar</a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"teliminar('" + val.id + "')\">Eliminar</a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#tregistros").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function tgrabar() {
    var fecha_tipo_cambio = $("#fecha_tipo_cambio").val();
    var cambio_venta = $("#cambio_venta").val();
    var cambio_compra = $("#cambio_compra").val();
    var id = $("#tid").val();


    $.ajax({
        url: 'Operacion.aspx?ope=802&id=' + id + '&fecha_tipo_cambio=' + fecha_tipo_cambio + '&cambio_venta=' + cambio_venta + '&cambio_compra=' + cambio_compra,
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

    $("#dialog-TipoCambio").dialog("close");
}

function tmodificar(id, fecha_tipo_cambio, cambio_venta, cambio_compra) {
    $("#fecha_tipo_cambio").val(fecha_tipo_cambio);
    $("#cambio_venta").val(cambio_venta);
    $("#cambio_compra").val(cambio_compra);
    $("#tid").val(id);

    $("#dialog-TipoCambio").dialog("open");
}

function teliminar(id) {
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=803&id=' + id,
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

function tnuevo() {
    $("#tid").val('0');
    //$("#fecha_tipo_cambio").val('');
    $("#cambio_venta").val('0.00');
    $("#cambio_compra").val('0.00');
    $("#dialog-TipoCambio").dialog("open");
}



function loaddataVariable() {

    $("#tblVariable").html('');

    $("#tblVariable").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblVariable").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Valor'))
            .append($('<th />').html(''))
            ));

    var fila = 0;



    $.ajax({
        url: 'Operacion.aspx?ope=931',
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblVariable").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.valor).attr('align', 'right'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"vmodificar('" + val.id + "','" + val.nombre + "','" + val.valor + "')\">Editar</a>").attr('align', 'center'))
     
                );

                fila++;
                numero++;
            });

            $("#vregistros").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function vgrabar() {
    var valor = $("#vvalor").val();
    var id = $("#vid").val();


    $.ajax({
        url: 'Operacion.aspx?ope=932&id=' + id + '&valor=' + valor,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            loaddataVariable();
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog-Variable").dialog("close");
}

function vmodificar(id, nombre, valor) {
    $("#vnombre").val(nombre);
    $("#vvalor").val(valor);
    $("#vid").val(id);

    $("#dialog-Variable").dialog("open");
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

