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
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tbldetalle").append($('<thead/>').append(
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

                $("#tbldetalle").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.tipodes).attr('align', 'left'))
                    .append($('<td />').html(val.indicadordes).attr('align', 'left'))
                    .append($('<td />').html(val.serie).attr('align', 'center'))
                    .append($('<td />').html(val.correlativo).attr('align', 'right'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"modificar('" + val.id + "','" + val.tipo + "','" + val.indicador + "','" + val.serie + "','" + val.correlativo + "')\">Editar</a>").attr('align', 'center'))
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
    var tipo = $("#cbotipo option:selected").val();
    var indicador = $("#cboindicador option:selected").val();
    var serie = $("#serie").val();
    var correlativo = $("#correlativo").val();
    var id = $("#id").val();
    

    $.ajax({
        url: 'Operacion.aspx?ope=912&id=' + id + '&tipo=' + tipo + '&indicador=' + indicador + '&serie=' + serie + '&correlativo=' + correlativo,
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

function modificar(id, tipo, indicador, serie, correlativo) {
    $("#cbotipo option[value=" + tipo + "]").prop("selected", true);
    $("#cboindicador option[value=" + indicador + "]").prop("selected", true);
    $("#serie").val(serie);
    $("#correlativo").val(correlativo);
    $("#id").val(id);

    $("#dialog").dialog("open");
}

function eliminar(id)
{
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=913&id=' + id,
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
    $("#serie").val('001');
    $("#correaltivo").val('0');
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

