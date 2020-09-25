
$(function () {

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)


    //$("#dialog").dialog({
    //    autoOpen: false
    //});
    
});

//function validar() {

//    $("#dialog").dialog("open");
//}

function loaddata() 
{

    $.ajax({
        url: 'Operacion.aspx?ope=102',
        async: false,
        success: function (data) {
            
            datos = $.parseJSON(data);

            contpanel=1;
            contador=0;

            $.each(datos, function (i, val) {

                var name = "panel" + contpanel;
                var imagen = '';

                if (val.imagen != '') {
                    imagen = '<div>' +
                             '<table><tr>' +
                                '<td>' + val.texto + '</td>' +
                                '<td><img src="Img/Temp/' + val.imagen + '" style="width: 200px;height: 140px" /></td>' +
                             '</tr></table>' +
                             '</div>';
                }
                else {
                    imagen = '<div>' + val.texto + '</div>';
                }


                $("#" + name).append("<div id='window" + contador + "' style='height: 200px; width: 250px;'>" +
                                     "<div>" + val.titulo + "</div>" +
                                     imagen +
                                     "</div>");

                //console.log(val.titulo);
                //console.log(val.texto);

                contpanel++;
                contador++;

                if (contpanel == 4)
                    contpanel = 1;

            });

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








