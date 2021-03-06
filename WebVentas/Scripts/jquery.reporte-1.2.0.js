
$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)

    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
        ,colors: ['#da0604','#194f12']
    });

    var fecha = new Date();
    var ano = fecha.getFullYear();
    var anoini = ano - 10;
    var i=0;

    $("#cboanio").empty();

    for (i = ano; i > anoini ; i--) {
        $("#cboanio").append('<option value=' + i + '>' + i + '</option>');
    }
});


function descargar() {

    var anio = $("#cboanio").val();
    var mes = $("#cbomes").val();
    location.href = 'Reporte.aspx?ope=01&anio=' + anio + '&mes=' + mes;
}

function buscar() {

    var anio = $("#cboanio").val();
    var mes = $("#cbomes").val();

    $("#tbldetalle").html('');

    $("#tbldetalle").append(
           $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tbldetalle").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('TIPO'))
            .append($('<th />').html('ESTADO'))
            .append($('<th />').html('SOLES'))
            .append($('<th />').html('DOLARES'))
            .append($('<th />').html('TRANSACCIONES'))
            ));

    var fila = 0;

    $.ajax({
        url: 'Operacion.aspx?ope=981&anio=' + anio + '&mes=' + mes,
        async: true,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;
            var transacciones = 0.00;
            var soles = 0.00;
            var dolares = 0.00;

            $.each(datos, function (i, val) {

                $("#tbldetalle").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.Tipo_Desc).attr('align', 'left'))
                    .append($('<td />').html(((val.Estado_Docu == 'ACT') ? 'ACTIVO' : 'ANULADO')).attr('align', 'center'))
                    .append($('<td />').html(formato(val.Soles)).attr('align', 'right'))
                    .append($('<td />').html(formato(val.Dolares)).attr('align', 'right'))
                    .append($('<td />').html(val.Transacciones).attr('align', 'right'))
                );

                if (val.Estado_Docu == 'ACT') {

                    if (val.Tipo_Desc == 'FACTURA') {
                        soles = parseFloat(soles) + parseFloat(val.Soles);
                        dolares = parseFloat(dolares) + parseFloat(val.Dolares);
                    }
                    if (val.Tipo_Desc == 'BOLETA') {
                        soles = parseFloat(soles) + parseFloat(val.Soles);
                        dolares = parseFloat(dolares) + parseFloat(val.Dolares);
                    }
                    if (val.Tipo_Desc == 'NOTA CREDITO') {
                        soles = parseFloat(soles) - parseFloat(val.Soles);
                        dolares = parseFloat(dolares) - parseFloat(val.Dolares);
                    }
                    if (val.Tipo_Desc == 'NOTA DEBITO') {
                        soles = parseFloat(soles) + parseFloat(val.Soles);
                        dolares = parseFloat(dolares) + parseFloat(val.Dolares);
                    }
                }

                transacciones = parseFloat(transacciones) + parseFloat(val.Transacciones);
                

                fila++;
                numero++;
            });

            $("#tbldetalle").append($('<tr/>')
                    .append($('<td colspan=3/>').html('<b>TOTAL</b>').attr('align', 'center'))
                    .append($('<td />').html('<b>' + formato(soles) + '</b>').attr('align', 'right'))
                    .append($('<td />').html('<b>' + formato(dolares) + '</b>').attr('align', 'right'))
                    .append($('<td />').html('<b>' + transacciones + '</b>').attr('align', 'right'))
                );

            $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });


    $("#container_1").html('');
    $("#container_2").html('');
    $("#container_3").html('');

    load101(anio);
    //load102(anio, semestre);
    //load103(anio, semestre);

}


function load101(periodo) {

    var $data;
    
    $.ajax({
        url: 'Operacion.aspx?ope=3001&periodo=' + periodo,
        async: false,
        success: function (data) {
            $data = $.parseJSON(data);

        }
    });


    var chardata = {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Ventas por Mes del ' + periodo
        },       
        xAxis: {
            categories: $data.map(x=>x.name),
            crosshair: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Venta'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                         '<td style="padding:0"><b>{point.y:,.0f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
       
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:.2f}'
                }
              
            }
        },
        plotOptions: {
            column: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>{point.y:,.0f}',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{
            showInLegend: false,  
            name:'Venta',
            data: $data.map(x=>x.amount)
        }]
    };

    Highcharts.chart('container_1', chardata);
}

function formato(valueString) {
    var amount=parseFloat(valueString).toFixed(2);
    var formattedString = amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return formattedString;
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
