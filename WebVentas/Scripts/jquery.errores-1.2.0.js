$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)

    loaddata();
});

function loaddata(){
    
    $("#tbldetalle").html('');

    $("#tbldetalle").append(
            $('<colgroup/>')
            .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tbldetalle").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('FECHA'))
            .append($('<th />').html('PROYECTO'))
            .append($('<th />').html('CLASE'))
            .append($('<th />').html('METODO'))
            .append($('<th />').html('DESCRIPCION'))
            ));

    var fila = 0;

    
    $.ajax({
        url: 'Operacion.aspx?ope=104',
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
                    .append($('<td />').html(val.fecha).attr('align', 'left'))
                    .append($('<td />').html(val.proyecto).attr('align', 'left'))
                    .append($('<td />').html(val.clase).attr('align', 'left'))
                    .append($('<td />').html(val.metodo).attr('align', 'left'))
                    .append($('<td />').html(val.descripcion).attr('align', 'left'))
                    
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

