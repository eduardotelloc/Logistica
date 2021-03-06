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

    loaddata();

});

function loaddata() {
    
    $("#tbldetalle").html('');

    $("#tbldetalle").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col width="25%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tbldetalle").append($('<thead/>').append(
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

                $("#tbldetalle").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.fecha_tipo_cambio).attr('align', 'center'))
                    .append($('<td />').html(val.cambio_venta).attr('align', 'right'))
                    .append($('<td />').html(val.cambio_compra).attr('align', 'right'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"modificar('" + val.id + "','" + val.fecha_tipo_cambio + "','" + val.cambio_venta + "','" + val.cambio_compra + "')\">Editar</a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"eliminar('" + val.id + "')\">Eliminar</a>").attr('align', 'center'))

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

function grabar()
{
    var fecha_tipo_cambio = $("#fecha_tipo_cambio").val();
    var cambio_venta = $("#cambio_venta").val();
    var cambio_compra = $("#cambio_compra").val();
    var id = $("#id").val();
    

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

    $("#dialog").dialog("close");
}

function modificar(id, fecha_tipo_cambio, cambio_venta, cambio_compra) {
    $("#fecha_tipo_cambio").val(fecha_tipo_cambio);
    $("#cambio_venta").val(cambio_venta);
    $("#cambio_compra").val(cambio_compra);
    $("#id").val(id);

    $("#dialog").dialog("open");
}

function eliminar(id)
{
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

function nuevo() {
    $("#id").val('0');
    //$("#fecha_tipo_cambio").val('');
    $("#cambio_venta").val('0.00');
    $("#cambio_compra").val('0.00');
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

