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

   
    $("#fechainicio").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function () {
            //- get date from another datepicker without language dependencies
            var minDate = $('#fechainicio').datepicker('getDate');
            $("#fechafin").datepicker("change", { minDate: minDate });
        }
    }).datepicker("setDate", "0");

    $("#fechafin").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function () {
            //- get date from another datepicker without language dependencies
            var maxDate = $('#fechafin').datepicker('getDate');
            $("#fechainicio").datepicker("change", { maxDate: maxDate });
        }
    }).datepicker("setDate", "0");

    carga('', '', '', '', '', '', '', '100');

});

function exportdata() {
    $("#tbldetalle").tableToCSV();
}

function loaddata(){
    var tipo = $("#cbotipo option:selected").val();

    var estado = $("#cboestado option:selected").val();
    var sunat = $("#cbosunat option:selected").val();

    var serie = $("#serie").val();
    var correlativo = $("#correlativo").val();
    var fechainicio = $("#fechainicio").val();
    var fechafin = $("#fechafin").val();
    
    carga(tipo, estado, sunat, serie, correlativo, fechainicio, fechafin, '');
}


function carga(tipo, estado, sunat, serie, correlativo, fechainicio, fechafin, top) {
    
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
            .append($('<th />').html('DNI/RUC'))
            .append($('<th />').html('CLIENTE'))
            .append($('<th />').html('TIPO'))
            .append($('<th />').html('DOCUMENTO'))
            .append($('<th />').html('FECHA'))
            .append($('<th />').html('MONEDA'))
            .append($('<th />').html('MONTO'))
            .append($('<th />').html('ESTADO'))
            .append($('<th />').html(''))
            .append($('<th />').html('SUNAT'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=101&tipo=' + tipo + '&estado=' + estado + '&sunat=' + sunat + '&serie=' + serie + '&correlativo=' + correlativo + '&fechainicio=' + fechainicio + '&fechafin=' + fechafin + '&top=' + top,
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tbldetalle tbody").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.CLIENTE_RUC_DNI).attr('align', 'center'))
                    .append($('<td />').html(val.CLIENTE_NOMBRE).attr('align', 'left'))
                    .append($('<td />').html(val.TIPO_DOCU_DES).attr('align', 'center'))
                    .append($('<td />').html(val.DOCUMENTO).attr('align', 'center').attr('title', 'Última fecha de envío: ' + val.FECHA_ENVIADO).css('font-weight', 'bold'))
                    .append($('<td />').html(val.FECHA_DOCU_FORMATO).attr('align', 'center'))
                    .append($('<td />').html(val.MONEDA_DOCU).attr('align', 'center'))
                    .append($('<td />').html(val.IMPO_TOTAL).attr('align', 'right'))
                    .append($('<td />').html(val.ESTADO_DOCU).attr('align', 'center'))
                    .append($('<td />').html((val.ESTADO == 'ACEPTADO') ? '<img src=\"Img/verde.png\" title=\"Aceptado\" />' : (val.ESTADO == 'RECHAZADO') ? '<img src=\"Img/rojo.png\" title=\"' + (val.ERROR) + '\" />' : (val.ESTADO == '') ? '<img src=\"Img/ambar.png\" title=\"En proceso\" />' : '').attr('align', 'center'))
                    .append($('<td />').html(val.ESTADO).attr('align', 'center'))
                    .append($('<td />').html(((val.ESTADO == 'POR ACEPTAR' || val.ESTADO == 'ACEPTADO' || val.ESTADO == 'RECHAZADO' || val.ESTADO == 'ANULADO') || (val.ESTADO == 'PENDIENTE' && val.DOCUMENTO.substring(0,1) == 'B')) ? "<a href=\"#\" onClick=\"viewpdf('" + val.ID + "')\"><img src=\"Img/viewer.png\" title=\"Ver Documento\" /></a>" : "").attr('align', 'center'))
                    .append($('<td />').html((val.ESTADO_ENVIO_CORREO_CLIENTE == '') ? '' : (val.ESTADO_ENVIO_CORREO_CLIENTE == 'S') ? '<img src=\"Img/mail_ok.png\" title=\"Envío Satisfactorio: ' + val.CLIENTE_CORREO + '\" />' : '<img src=\"Img/mail_error.png\" title=\"Envío con Error: ' + val.CLIENTE_CORREO + '\" />').attr('align', 'center'))

                );


                fila++;
                numero++;
            });

            //$('#tbldetalle').DataTable({
            //    "language": {
            //        "sProcessing": "Procesando...",
            //        "sLengthMenu": "Mostrar _MENU_ registros",
            //        "sZeroRecords": "No se encontraron resultados",
            //        "sEmptyTable": "Ningún dato disponible en esta tabla",
            //        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            //        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            //        "sInfoPostFix": "",
            //        "sSearch": "Buscar:",
            //        "sUrl": "",
            //        "sInfoThousands": ",",
            //        "sLoadingRecords": "Cargando...",
            //        "oPaginate": {
            //            "sFirst": "Primero",
            //            "sLast": "Último",
            //            "sNext": "Siguiente",
            //            "sPrevious": "Anterior"
            //        },
            //        "oAria": {
            //            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            //            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            //        }
            //    }
            //});
        },
        complete: function () {
            BlockEnd();
        }
    });
}


function print(id) {
    var cadena = 'PrintTicket.aspx?id=' + id;
    PopupCenter(cadena, 'PrintTicket', '900', '500');
}

function viewpdf(id) {
    var cadena = 'ViewTicket.aspx?id=' + id;
    PopupCenter(cadena, 'ViewTicket', '900', '500');
}

function descarga(tipo,id) {

    if (tipo==1)
        location.href = 'Documentos.aspx?id=' + id;
    else
        location.href = 'DocumentosAdmin.aspx?tipo=' + tipo + '&id=' + id;
    //location.href = 'Documentos.aspx?tipo=1&id=' + id;
    //location.href = 'Documentos.aspx?tipo=2&id=' + id;

    //window.open('Documentos.aspx?tipo=1&id=' + id);
    //window.open('Documentos.aspx?tipo=2&id=' + id);

    //var link1 = document.createElement("a");
    //link1.href = 'Documentos.aspx?tipo=1&id=' + id;
    //link1.click();

    //var link2 = document.createElement("a");
    //link2.href = 'Documentos.aspx?tipo=2&id=' + id;
    //link2.click();
}

//function descargar(pdf, xml, tipo, codigo) {
//    var cadena1 = 'pdf/' + pdf;
//    var link1 = document.createElement("a");
//    link1.download = pdf;
//    link1.href = cadena1;
//    link1.click();

//    var cadena2 = 'xml/' + xml;
//    var link2 = document.createElement("a");
//    link2.download = xml;
//    link2.href = cadena2;
//    link2.click();

//}



function PopupCenter(url, title, w, h) {
    // Fixes dual-screen position                         Most browsers      Firefox
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

    width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = ((height / 2) - (h / 2)) + dualScreenTop;
    var newWindow = window.open(url, title, 'location=no,toolbar=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

    // Puts focus on the newWindow
    if (window.focus) {
        newWindow.focus();
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

