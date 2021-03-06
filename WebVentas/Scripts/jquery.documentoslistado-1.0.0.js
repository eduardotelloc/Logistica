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

    $('#ribbon').ribbon();

    var dateini = new Date();
    dateini.setDate(dateini.getDate() - 31);

    var datefin = new Date();

    $("#fechainicio").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function () {
            //- get date from another datepicker without language dependencies
            var minDate = $('#fechainicio').datepicker('getDate');
            $("#fechafin").datepicker("change", { minDate: minDate });
        }
    }).datepicker("setDate", dateini);

    $("#fechafin").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function () {
            //- get date from another datepicker without language dependencies
            var maxDate = $('#fechafin').datepicker('getDate');
            $("#fechainicio").datepicker("change", { maxDate: maxDate });
        }
    }).datepicker("setDate", datefin);

    $('.ribbon-button').click(function () {

        if (this.isEnabled()) {

            switch ($(this).attr('id')) {
                case "add-page-btn":
                    nuevo();
                    break;

                case "open-page-btn":
                    modificar();
                    break;

                case "del-page-btn":
                    eliminar();
                    break;

                case "search-btn":
                    buscar();
                    break;

                case "retornar-btn":
                    retornar();
                    break;

                case "exportar-page-btn":
                    exportar();
                    break;
            }
        }
    });

    loadinicio();
});


function exportar() {
    $("#tbldetalle").table2excel({
        name: "Table2Excel",
        filename: "Documentos",
        fileext: ".xls"
    });
}


function retornar() {
    window.location.href = 'Menu.aspx';
}

function loadinicio() {

    $("#registros").html("");
    $("#tbldetalle").html('');

    $("#tbldetalle").append(
        $('<colgroup/>')
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            .append($('<col />').html(''))
            //.append($('<col />').html('').attr('display', 'none'))
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
            //.append($('<th />').html('RESUMEN'))
            .append($('<th />').html('FECHA'))
            .append($('<th />').html('MONEDA'))
            .append($('<th />').html('MONTO'))
            .append($('<th />').html('ESTADO'))
            .append($('<th />').html('SUNAT'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
    ));


}


function buscar() {

    var tipo = $("#cbotipo option:selected").val();
    var estado = $("#cboestado option:selected").val();
    var sunat = $("#cbosunat option:selected").val();

    var serie = $("#serie").val();
    var correlativo = $("#correlativo").val();
    var fechainicio = $("#fechainicio").val();
    var fechafin = $("#fechafin").val();


    $("#registros").html("");
    $("#tbldetalle tbody").remove();
    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=101&tipo=' + tipo + '&estado=' + estado + '&sunat=' + sunat + '&serie=' + serie + '&correlativo=' + correlativo + '&fechainicio=' + fechainicio + '&fechafin=' + fechafin,
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            var fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tbldetalle tbody").append($('<tr/>')
					.append($('<td />').html(numero).attr('align', 'center'))
					.append($('<td />').html(val.CLIENTE_RUC_DNI).attr('align', 'center'))
					.append($('<td />').html(val.CLIENTE_NOMBRE).attr('align', 'left'))
					.append($('<td />').html(val.TIPO_DOCU_DES).attr('align', 'center'))
					.append($('<td />').html(val.DOCUMENTO).attr('align', 'center').attr('title', 'Última fecha de envío: ' + val.FECHA_ENVIADO).css('font-weight', 'bold'))
                    //.append($('<td />').html(val.ARCHIVO_RESU).attr('align', 'center'))
					.append($('<td />').html(val.FECHA_DOCU_FORMATO).attr('align', 'center'))
					.append($('<td />').html(val.MONEDA_DOCU).attr('align', 'center'))
					.append($('<td />').html(val.IMPO_TOTAL).attr('align', 'right'))
					.append($('<td />').html(val.ESTADO_DOCU).attr('align', 'center'))
					.append($('<td />').html((val.ESTADO == 'ACEPTADO') ? '<img src=\"Img/verde.png\" title=\"Aceptado\" />' : (val.ESTADO == 'RECHAZADO') ? '<img src=\"Img/rojo.png\" title=\"' + (val.ERROR) + '\" />' : (val.ESTADO == '') ? '<img src=\"Img/ambar.png\" title=\"En proceso\" />' : '').attr('align', 'center'))
					.append($('<td />').html('<a href=\"#\" onClick=\"imprimir(\'' + val.EMPRESA_CODIGO + '\',\'' + val.CODIGO + '\',\'' + val.DOCUMENTO + '\')\"><img src=\"Img/print.png\" title=\"Enviar Impresion\" /></a>').attr('align', 'center'))
					.append($('<td />').html("<a href=\"#\" onClick=\"viewticket('" + val.ID + "')\"><img src=\"Img/ticket-16.png\" title=\"Ver Ticket\" /></a>").attr('align', 'center'))
					.append($('<td />').html("<a href=\"#\" onClick=\"viewpdf('" + val.ID + "')\"><img src=\"Img/viewer.png\" title=\"Ver A4\" /></a>").attr('align', 'center'))
					.append($('<td />').html((val.ESTADO == 'ACEPTADO') ? "<a href=\"#\" onClick=\"descarga(1,'" + val.ID + "')\"><img src=\"Img/zip.png\" title=\"Descargar Zip\" /></a>" : "").attr('align', 'center'))
					.append($('<td />').html((val.ESTADO_ENVIO_CORREO_CLIENTE == '') ? '' : (val.ESTADO_ENVIO_CORREO_CLIENTE == 'S') ? '<img src=\"Img/mail_ok.png\" title=\"Envío Satisfactorio: ' + val.CLIENTE_CORREO + '\" />' : '<img src=\"Img/mail_error.png\" title=\"Envío con Error: ' + val.CLIENTE_CORREO + '\" />').attr('align', 'center'))
                    .append($('<td />').html('<input type="radio" value="' + val.ID + '" id="r' + val.ID + '" name="radio" onclick="seleccion(\'' + val.ID + '\')" />').attr('align', 'center'))
				);


                fila++;
                numero++;
            });

            $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');

            if (fila > 0)
                $('#exportar-page-btn').enable();
            else
                $('#exportar-page-btn').disable();


            $('#tbldetalle tbody tr').on("click", function (e) {
                var id = $(this).find("input[type=radio]").val();
                $("#hdocumento").val($(this).find("td:nth-child(5)").text());
                seleccion_row(id);
            });

        },
        complete: function () {
            BlockEnd();
        }
    });
}

function seleccion(id) {
    $('#del-page-btn').enable();
    $("#hid").val(id);
}

function seleccion_row(id) {

    $("#r" + id).prop('checked', true);
    $('#del-page-btn').enable();
    $("#hid").val(id);
}

function viewpdf(id) {
    var cadena = 'ViewPDF.aspx?id=' + id;
    PopupCenter(cadena, 'ViewPdf', '900', '500');
}

function viewticket(id) {
    var cadena = 'ViewTicket.aspx?id=' + id;
    PopupCenter(cadena, 'ViewPdf', '900', '500');
}

function descarga(tipo, id) {

    location.href = 'DocumentosAdmin.aspx?tipo=' + tipo + '&id=' + id;
}

function imprimir(empresa, codigo, documento) {

    if (confirm('Esta seguro de imprimir el documento ' + documento + '?')) {
        $.ajax({
            url: 'Operacion.aspx?ope=104&empresa=' + empresa + '&codigo=' + codigo,
            async: false,
            success: function (data) {
                alert('Proceso realizado');
            }
        });
    }
}

function eliminar(empresa, codigo, documento) {

    var documento = $("#hdocumento").val();
    var id = $("#hid").val();

    if (confirm('Esta seguro de anular el documento ' + documento + '?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=113&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Actualizando ...');
            },
            success: function (data) {
                alert('Proceso realizado');
                loaddata();
            },
            complete: function () {
                BlockEnd();
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
