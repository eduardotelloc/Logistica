var array_id_producto;
var array_des_producto;
var vnumero = 15;
var mesa = 0;
var ope = 0;

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

    ope = getParameterByName('ope');
    mesa = getParameterByName('mesa');
    var num_documento = getParameterByName('num_documento');

    $("#lblmesa").html(mesa);
  

    $("#id").val('0');
    $("#pid").val('0');
   
    

    array_des_producto = LoadAutocompleteData('Operacion.aspx?ope=604');
    array_id_producto = LoadAutocompleteData('Operacion.aspx?ope=606');
    vnumero = LoadDataText('Operacion.aspx?ope=933')
   

    $.datepicker.setDefaults($.datepicker.regional['es']);

    $("#fecha_nacimiento").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true
    }).datepicker("setDate", "0");



    $("#fecha").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function () {

        }
    }).datepicker("setDate", "0");

   



    LoadSelectItem('Operacion.aspx?ope=301&nombre=', 'cbopresentacion');
    LoadSelectItem('Operacion.aspx?ope=401&nombre=', 'cbocategoria');

    LoadAutocomplete('Operacion.aspx?ope=704', 'idcliente');
    LoadAutocomplete('Operacion.aspx?ope=706', 'cliente');
    LoadAutocomplete('Operacion.aspx?ope=904', 'idtrabajador');

    $('#cambio_venta').val(LoadDataText('Operacion.aspx?ope=804'));

    loadconfig();

    if (ope == "0") {

        $("#btnfacturar").toggle(false);
        $("#btneliminar").toggle(false);

        $('#idtrabajador').val(num_documento);
        var url = 'Operacion.aspx?ope=905&numero_documento=' + num_documento;
        var data = LoadDataText(url);
        $('#trabajador').val(data);
        $('#higv').val(LoadDataText('Operacion.aspx?ope=921&nombre=igv'));
    }

    if (ope == "1") {

        $.ajax({
            url: 'Operacion.aspx?ope=215&mesa=' + mesa,
            async: false,
            beforeSend: function () {
                BlockBegin('Buscando ...');
            },
            success: function (data) {

                datos = $.parseJSON(data);

                $.each(datos, function (i, val) {

                    $('#id').val(val.id);
                    $('#idtrabajador').val(val.num_documento);
                    $('#trabajador').val(val.trabajador);
                    $('#fecha').val(val.fecha);
                    $('#higv').val(val.itbis);
                    $('#valorventa').val(val.valor_venta);
                    $('#igv').val(val.igv);
                    $('#totaldolares').val('0.00');
                    $('#totalsoles').val(val.total);
                });

            },
            complete: function () {
                BlockEnd();
            }
        });

        

        $.ajax({
            url: 'Operacion.aspx?ope=216&id=' + $('#id').val(),
            async: false,
            beforeSend: function () {
                BlockBegin('Buscando ...');
            },
            success: function (data) {

                datos = $.parseJSON(data);

                var numero = 1;

                $.each(datos, function (i, val) {

                    $('#numero' + numero).val(numero);
                    $('#idproducto' + numero).val(val.codarticulo);
                    $('#producto' + numero).val(val.articulo);
                    $('#unidad' + numero).val(val.unidad);
                    $('#cantidad' + numero).val(val.cantidad);
                    $('#preciounit' + numero).val(val.preciounit);
                    $('#precioventa' + numero).val(val.precio_venta);

                    numero++;
                });

            },
            complete: function () {
                BlockEnd();
            }
        });
    }



    $('#idtrabajador').prop("readonly", true);
    $('#trabajador').prop("readonly", true);


    $('#idproducto1').focus().select();
    
    
});

function loadconfig() {

    $("#tbldocumento").html('');

    $("#tbldocumento").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tbldocumento").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Cod'))
            .append($('<th />').html('Descripcion'))
            .append($('<th />').html('Unidad'))
            .append($('<th />').html('Cantidad'))
            .append($('<th />').html('Precio Unit'))
            .append($('<th />').html('Precio Venta'))
            .append($('<th />').html(''))
            ));

    $("#tbldocumento").append($('<tbody/>'));


    for (numero = 1; numero <= vnumero; numero++) {

        $("#tbldocumento tbody").append($('<tr/>')
            .append($('<td />').html(numero).attr('align', 'center'))
            .append($('<td />').html('<input type="text"     onblur="getpro2(' + numero + ')"                       name="idproducto' + numero + '"     id="idproducto' + numero + '"               style="width:90px;text-align:center;"   maxlength="15"                                              class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"     onblur="getpro(' + numero + ')"                        name="producto' + numero + '"       id="producto' + numero + '"                 style="width:600px"                     maxlength="200"                                             class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"                                        readonly="true"     name="unidad' + numero + '"         id="unidad' + numero + '"                   style="width:60px;text-align:center;"                                                               class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="numeric"  onblur="getcal(' + numero + ')"                        name="cantidad' + numero + '"       id="cantidad' + numero + '"                 style="width:40px;text-align:right;"    maxlength="9"   value="0"      min="1" max="10000" step="1" class="text ui-widget-content ui-corner-all"><span name="preciototal' + numero + '" id="preciototal' + numero + '">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"     onblur="getcal(' + numero + ')"    readonly="true"     name="preciounit' + numero + '"     id="preciounit' + numero + '"               style="width:60px;text-align:right;"                    value="0"                                   class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"                                        readonly="true"     name="precioventa' + numero + '"    id="precioventa' + numero + '"              style="width:60px;text-align:right;"                    value="0"                                   class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="button"   onclick="deleteitem(' + numero + ')"                   name="btnitem' + numero + '"        id="btnitem' + numero + '"                  style="height:18px;text-align:center;"                  value="-">').attr('align', 'center'))
        );


        LoadAutocompleteAsign(array_des_producto, 'producto' + numero);
        LoadAutocompleteAsign(array_id_producto, 'idproducto' + numero);

    }

    $("#tbldocumento tbody").append($('<tr/>')
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('Valor venta').attr('align', 'right'))
            .append($('<td />').html('<input type="text" readonly="true"    name="valorventa"   id="valorventa"     style="width:80px;text-align:right;"     value="0"             class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
        );

    $("#tbldocumento tbody").append($('<tr/>')
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('IGV').attr('align', 'right'))
            .append($('<td />').html('<input type="text" readonly="true"    name="igv"   id="igv"     style="width:80px;text-align:right;"     value="0"             class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
        );

    $("#tbldocumento tbody").append($('<tr/>')
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
            .append($('<td />').html('Total $').attr('align', 'right'))
            .append($('<td />').html('<input type="text" readonly="true"    name="totaldolares" id="totaldolares"   style="width:80px;text-align:right;"    value="0"              class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('Total S/').attr('align', 'right'))
            .append($('<td />').html('<input type="text" readonly="true"    name="totalsoles"   id="totalsoles"     style="width:80px;text-align:right;"     value="0"             class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('').attr('align', 'center'))
        );

}


function facturar() {
    if (confirm('Esta seguro de generar el documento electrónico de la mesa: ' + mesa + '?')) {
        window.location.href = "DocumentoElectronico.aspx?id=" + $('#id').val();
    }
}

function eliminar() {
    if (confirm('Esta seguro de anular el pedido de la mesa: ' + mesa + '?')) {
        $.ajax({
            url: 'Operacion.aspx?ope=213&id=' + $('#id').val(),
            async: false,
            success: function (data) {
                console.log(data);
                alert('Proceso realizado');
                window.location.href = "Mesas.aspx";
                
            }
        });
    }
}

function grabar() {

    if ($('#id').val() != "0") {
        $.ajax({
            url: 'Operacion.aspx?ope=213&id=' + $('#id').val(),
            async: false,
            success: function (data) {
            }
        });
    }

    var idtrabajador = $("#idtrabajador").val();
    var fecha = $("#fecha").val();
    
    var valorventa = $("#valorventa").val();
    var igv = $("#igv").val();
    var total = $("#totalsoles").val();

   
    var detalle = false;

    for (numero = 1; numero <= vnumero; numero++) {

        if ($("#idproducto" + numero).val() != "") {
            detalle = true;
        }
    }


    if (!detalle) {
        alert('Debe ingresar un item');
        return;
    }

    var url = 'Operacion.aspx?ope=210&idtrabajador=' + idtrabajador + '&fecha=' + fecha + '&mesa=' + mesa + '&valorventa=' + valorventa + '&igv=' + igv + '&total=' + total;


    $.ajax({
        url: url,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            var codigo = data;
            console.log(codigo);
            var items = [];

            for (numero = 1; numero <= vnumero; numero++) {

                if ($("#idproducto" + numero).val() != "") {

                    var idproducto = $("#idproducto" + numero).val();
                    var producto = $("#producto" + numero).val();
                    var unidad = $("#unidad" + numero).val();
                    var cantidad = $("#cantidad" + numero).val();
                    var preciounit = $("#preciounit" + numero).val();
                    var precioventa = $("#precioventa" + numero).val();

                    items.push(
                        "{\"codigo\":\"" + codigo + "\"," +
                        "\"idproducto\":\"" + idproducto + "\"," +
                        "\"producto\":\"" + producto.replace('"', '#') + "\"," +
                        "\"unidad\":\"" + unidad + "\"," +
                        "\"cantidad\":\"" + cantidad + "\"," +
                        "\"preciounit\":\"" + preciounit + "\"," +
                        "\"precioventa\":\"" + precioventa + "\"}"
                    );
                }
            }


            var itemsjson = "[" + items.join(",") + "]";
            //var itemsjson = JSON.stringify(items);

            //console.log('items:' + itemsjson);

            $.ajax({
                url: 'Operacion.aspx?ope=211&json=' + encodeURIComponent(itemsjson),
                type: 'POST',
                async: false,
                success: function (data) {
                    console.log('detalle:' + data);
                    detalle = data;
                },
                error: function (request, error) {
                    console.log(arguments);
                    alert(" Can't do because: " + error);
                }
            });

            var documento = '';

            if (detalle != 'ERROR') {

                $.ajax({
                    url: 'Operacion.aspx?ope=212&codigo=' + codigo,
                    async: false,
                    success: function (data) {
                        console.log('complete:' + data);
                        documento = data;
                    }
                });
                
                alert('Proceso realizado satisfactoriamente ' + documento + ' !');
                
                window.location.href = "Mesas.aspx";
            }
            else
                alert('Hubo un error en el proceso' + documento + ' !');

        },
        complete: function () {
            BlockEnd();
        }
    });





}

function calculartotal() {
    var total = 0.00;
    var totalsoles = 0.00;
    var valorventa = 0.00;

    var totaldolares = 0.00;
    var cambio_venta = parseFloat($("#cambio_venta").val());
    var igv = 1 + (parseFloat($("#higv").val()) / 100);


    for (numero = 1; numero <= vnumero; numero++) {
        total = Number(total) + Number($("#precioventa" + numero).val());
    }

    totalsoles = parseFloat(total).toFixed(2);
    totaldolares = parseFloat(total / cambio_venta).toFixed(2);
    valorventa = parseFloat(totalsoles / igv).toFixed(2);
    igv = parseFloat(totalsoles - valorventa).toFixed(2);

    $("#valorventa").val(valorventa);
    $("#igv").val(igv);
    $("#totalsoles").val(totalsoles);
    $("#totaldolares").val(totaldolares);
}

function deleteitem(id) {

    $("#idproducto" + id).val('');
    $("#producto" + id).val('');
    $("#unidad" + id).val('');
    $("#cantidad" + id).val('0');
    $("#preciounit" + id).val('0');
    $("#preciototal" + id).text('');
    $("#precioventa" + id).val('0');
    calculartotal();
}

function getcal(id) {
    var objeto = 'cantidad' + id;
    var cantidad = $("#" + objeto).val();
    var preciounit = $('#preciounit' + id).val();
    var precioventa = cantidad * preciounit;
    $('#precioventa' + id).val(precioventa);
    calculartotal();
}

function getpro(id) {
    var objeto = 'producto' + id;
    var nombre = $("#" + objeto).val();
    var cantidad = $('#cantidad' + id).val();

    if (cantidad == '0') {

        nombre = encodeURIComponent(nombre);

        if (nombre != '') {
            var datos = LoadDataText('Operacion.aspx?ope=605&nombre=' + nombre).split('|');

            $('#idproducto' + id).val(datos[0]);
            $('#cantidad' + id).val(datos[2]);
            $('#preciounit' + id).val(datos[3]);

            if (datos[3] != datos[4])
                $('#preciototal' + id).text('(' + datos[4] + ')');

            $('#unidad' + id).val(datos[5]);
            $('#cantidad' + id).focus();
            $('#cantidad' + id).select();
        }
    }
}

function getpro2(id) {
    var objeto = 'idproducto' + id;
    var codigo = $("#" + objeto).val();

    var cantidad = $('#cantidad' + id).val();

    if (cantidad == '0') {

        if (codigo != '') {
            var datos = LoadDataText('Operacion.aspx?ope=607&codigo=' + codigo).split('|');

            console.log(datos);

            $('#idproducto' + id).val(datos[0]);
            $('#producto' + id).val(datos[1]);
            $('#cantidad' + id).val(datos[2]);
            $('#preciounit' + id).val(datos[3]);

            if (datos[3] != datos[4])
                $('#preciototal' + id).text('(' + datos[4] + ')');

            $('#unidad' + id).val(datos[5]);
            $('#cantidad' + id).focus();
            $('#cantidad' + id).select();
        }
    }
}

function getParameterByName(name) {
    var regexS = "[\\?&]" + name + "=([^&#]*)",
  regex = new RegExp(regexS),
  results = regex.exec(window.location.search);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
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

