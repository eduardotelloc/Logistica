var array_id_producto;
var array_des_producto;
var vnumero = 15;


$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)

   

    var id = $(this).val();

    $("#divmesas").html('');

    $.ajax({
        url: 'Operacion.aspx?ope=502',
        async: false,
        beforeSend: function () {
            BlockBegin('Updating ...');
        },
        success: function (data) {

            var test = data.split("|");
            var ini = test[0];
            var fin = test[1];

            for(i=ini; i<=fin;i++){
                var enter = (i % 8 == 0) ? '<br />' : '';
                $("#divmesas").append('<input type=\"hidden" name=\"hid' + i + '\" id=\"hid' + i + '\" value=\"N\"/><a href=\"#\" ondblclick=\"editar(\'' + i +'\')\" onClick=\"marcar(\'' + i + '\')\"><div id=\"divmesas' + i + '" class=\"circle\"  contenteditable=\"false\">' + i + '</div></a>' + enter);

            }

        },
        complete: function () {
            BlockEnd();
            loaddatamesero();
        }
    });



    $.ajax({
        url: 'Operacion.aspx?ope=214',
        async: true,
        beforeSend: function () {
            BlockBegin('Searching ...');
        },
        success: function (data) {

            try {

                datos = $.parseJSON(data);

                $.each(datos, function (i, val) {
                    //val.mesa
                    $("#hid" + val.mesa).val("S");
                    $("#divmesas" + val.mesa).css('background-color', '#DB1A0D');
                });

            }
            catch (err) {
                //alert("No existen registros!");
            }
        },
        complete: function () {
            BlockEnd();
        }
    });
       
});



function loaddatamesero() {
    
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
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Apellidos'))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $.ajax({
        url: 'Operacion.aspx?ope=503',
        async: true,
        beforeSend: function () {
            BlockBegin('Searching ...');
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
                    .append($('<td />').html('<a href=\"#\" onClick=\"next(\'' + val.num_documento + '\')\"><img src=\"Img/next.png\" title=\"Siguiente\" width=\"40px\" height=\"40px\" /></a>').attr('align', 'center'))

                );

                fila++;
                numero++;
            });

        },
        complete: function () {
            BlockEnd();
        }
    });
}

function marcar(i) {

    //#F9EA0E

    var valor = $("#hid" + i).val();

    if (valor == 'N') {

        var id = $("#hid").val();

        if (i == id) {
            $("#hid").val("");
            $("#divmesas" + i).css('background-color', '#40a977');
        }
        else {
            if (id == "") {
                $("#hid").val(i);
                $("#divmesas" + i).css('background-color', '#F9EA0E');
            }
        }
    }
}

function editar(i) {

    var valor = $("#hid" + i).val();

    if (valor == 'S') {

        var url = "Pedidos.aspx?ope=1&mesa=" + i + "&num_documento=00000000";
        window.location.href = url;
    }
}

function next(num_documento) {

    var id = $("#hid").val();

    if (id == "") {
        alert("Debe seleccionar una mesa!");
        return;
    }

    var url = "Pedidos.aspx?ope=0&mesa=" + id + "&num_documento=" + num_documento;
    window.location.href = url;

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
