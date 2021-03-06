var array_id_producto;
var array_des_producto;
var vnumero = 15;


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

    carga( '', '', '10');


   
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

    $("#dCierre").dialog({
        width: "800px",
        autoOpen: false
    });

    $("#dEgreso").dialog({
        width: "800px",
        autoOpen: false
    });

   
    //$('#cbotipocliente').focus();

});



function grabar() {
    var ocodigo = $("#ocodigo").val();
    var idpresentacion = $("#cbopresentacion option:selected").val();
    var idcategoria = $("#cbocategoria option:selected").val();
    var onombre = $("#onombre").val();
    var odescripcion = $("#odescripcion").val();
    var ostock = $("#ostock").val();
    var oprecio = $("#oprecio").val();
    var ototal = $("#ototal").val();
    var oid = $("#oid").val();

    if (idpresentacion == "-1" || idcategoria == "-1") {
        alert('Seleccione una presentacion o categoria');
        return;
    }


    $.ajax({
        url: 'Operacion.aspx?ope=602&id=' + oid + '&codigo=' + ocodigo + '&nombre=' + onombre + '&descripcion=' + odescripcion + '&idpresentacion=' + idpresentacion + '&idcategoria=' + idcategoria + '&stock=' + ostock + '&precio=' + oprecio + '&total=' + ototal,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            array_des_producto = LoadAutocompleteData('Operacion.aspx?ope=604');
            array_id_producto = LoadAutocompleteData('Operacion.aspx?ope=606');

            for (numero = 1; numero <= vnumero; numero++) {
                LoadAutocompleteAsign(array_des_producto, 'producto' + numero);
                LoadAutocompleteAsign(array_id_producto, 'idproducto' + numero);
            }


            $("#dproducto").dialog("close");
        },
        complete: function () {
            BlockEnd();
        }
    });


}

function nuevo() {
    $("#oid").val('0');
    //$("#ocodigo").val('');
    //$("#onombre").val('');
    //$("#odescripcion").val('');
    //$("#cbopresentacion option[value=-1]").prop("selected", true);
    //$("#cbocategoria option[value=-1]").prop("selected", true);
    //$("#ostock").val('1');
    //$("#oprecio").val('0.00');
    //$("#ototal").val('0.00');
    $("#dCierre").dialog("open");
}


function loaddata() {
  
    var fechainicio = $("#fechainicio").val();
    var fechafin = $("#fechafin").val();

    carga(fechainicio, fechafin, '');
}

function carga( fechainicio, fechafin, top) {

    $("#tbldetalle").html('');

    $("#tbldetalle").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html('').attr('display', 'none'))
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
            .append($('<th />').html('Fecha Cierre'))
            .append($('<th />').html('Contado'))
            .append($('<th />').html('Visa'))
            .append($('<th />').html('MasterCard'))
            .append($('<th />').html('Egreso'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=6001&fechainicio=' + fechainicio + '&fechafin=' + fechafin + '&top=' + top,
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            try {



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
                        .append($('<td />').html(val.ARCHIVO_RESU).attr('align', 'center'))
						.append($('<td />').html(val.FECHA_DOCU_FORMATO).attr('align', 'center'))
						.append($('<td />').html(val.MONEDA_DOCU).attr('align', 'center'))
						.append($('<td />').html(val.IMPO_TOTAL).attr('align', 'right'))
						.append($('<td />').html(val.ESTADO_DOCU).attr('align', 'center'))
						.append($('<td />').html((val.ESTADO == 'ACEPTADO') ? '<img src=\"Img/verde.png\" title=\"Aceptado\" />' : (val.ESTADO == 'RECHAZADO') ? '<img src=\"Img/rojo.png\" title=\"' + (val.ERROR) + '\" />' : (val.ESTADO == '') ? '<img src=\"Img/ambar.png\" title=\"En proceso\" />' : '').attr('align', 'center'))
						.append($('<td />').html(val.ESTADO).attr('align', 'center'))
                        .append($('<td />').html('<a href=\"#\" onClick=\"imprimir(\'' + val.EMPRESA_CODIGO + '\',\'' + val.CODIGO + '\',\'' + val.DOCUMENTO + '\')\"><img src=\"Img/print.png\" title=\"Enviar Impresion\" /></a>').attr('align', 'center'))
						.append($('<td />').html(((val.ESTADO == 'POR ACEPTAR' || val.ESTADO == 'ACEPTADO' || val.ESTADO == 'RECHAZADO' || val.ESTADO == 'ANULADO') || (val.ESTADO == 'PENDIENTE' && val.DOCUMENTO.substring(0, 1) == 'B')) ? "<a href=\"#\" onClick=\"viewticket('" + val.ID + "')\"><img src=\"Img/ticket-16.png\" title=\"Ver Ticket\" /></a>" : "").attr('align', 'center'))
						.append($('<td />').html(((val.ESTADO == 'POR ACEPTAR' || val.ESTADO == 'ACEPTADO' || val.ESTADO == 'RECHAZADO' || val.ESTADO == 'ANULADO') || (val.ESTADO == 'PENDIENTE' && val.DOCUMENTO.substring(0, 1) == 'B')) ? "<a href=\"#\" onClick=\"viewpdf('" + val.ID + "')\"><img src=\"Img/viewer.png\" title=\"Ver A4\" /></a>" : "").attr('align', 'center'))
						.append($('<td />').html((val.ESTADO == 'ACEPTADO') ? "<a href=\"#\" onClick=\"descarga(1,'" + val.ID + "')\"><img src=\"Img/zip.png\" title=\"Descargar Zip\" /></a>" : "").attr('align', 'center'))
						.append($('<td />').html((val.ESTADO_ENVIO_CORREO_CLIENTE == '') ? '' : (val.ESTADO_ENVIO_CORREO_CLIENTE == 'S') ? '<img src=\"Img/mail_ok.png\" title=\"Envío Satisfactorio: ' + val.CLIENTE_CORREO + '\" />' : '<img src=\"Img/mail_error.png\" title=\"Envío con Error: ' + val.CLIENTE_CORREO + '\" />').attr('align', 'center'))
						.append($('<td />').html('<a href=\"#\" onClick=\"eliminar(\'' + val.EMPRESA_CODIGO + '\',\'' + val.CODIGO + '\',\'' + val.DOCUMENTO + '\')\"><img src=\"Img/delete.png\" title=\"Anulacion\" /></a>').attr('align', 'center'))

					);


                    fila++;
                    numero++;
                });

                $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');

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


            }
            catch (err) {  //We can also throw from try block and catch it here
                alert("No existen registros!");
            }
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function eliminar(empresa, codigo, documento) {

    if (confirm('Esta seguro de anular el documento ' + documento + '?')) {
        $.ajax({
            url: 'Operacion.aspx?ope=113&empresa=' + empresa + '&codigo=' + codigo,
            async: false,
            success: function (data) {
                console.log(data);
                alert('Proceso realizado');
            }
        });
    }
}

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

