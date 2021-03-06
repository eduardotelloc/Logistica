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

    carga('', '', '', '', '', '', '', '100');



    //hotkeys('ctrl+b,f2,ctrl+s', function (event, handler) {
    //    switch (handler.key) {
    //        case "ctrl+b":
    //            loaddata();
    //            break;
    //        case "f2":
    //            nuevo();
    //            break;
    //        case "ctrl+s":
    //            if ($('#dialog').dialog('isOpen')) {
    //                grabar();
    //            }

    //            break;
    //    }
    //});

    LoadComboNormal('Operacion.aspx?ope=915', 'cbotipo_documento');

    array_des_producto = LoadAutocompleteData('Operacion.aspx?ope=604');
    array_id_producto = LoadAutocompleteData('Operacion.aspx?ope=606');
    vnumero = LoadDataText('Operacion.aspx?ope=933')
    console.log(vnumero);

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



    $("#ddocumento").dialog({

        width: "750px",
        //height: "650px",
        autoOpen: false
    });

    $("#dcliente").dialog({
        autoOpen: false
    });

    $("#dproducto").dialog({
        autoOpen: false
    });

    $("#dpermiso").dialog({
        autoOpen: false
    });
    

    LoadSelectItem('Operacion.aspx?ope=301&nombre=', 'cbopresentacion');
    LoadSelectItem('Operacion.aspx?ope=401&nombre=', 'cbocategoria');

    LoadAutocomplete('Operacion.aspx?ope=704', 'idcliente');
    LoadAutocomplete('Operacion.aspx?ope=706', 'cliente');
    LoadAutocomplete('Operacion.aspx?ope=904', 'idtrabajador');

    $('#idcliente').blur(function () {

        var codigo = $(this).val();;

        if (codigo != '') {
            var url = 'Operacion.aspx?ope=705&numero_documento=' + codigo;
            var data = LoadDataText(url);

            $('#cliente').val(data);
        }
    });

    $('#cliente').blur(function () {

        var nombre = $(this).val();
        nombre = encodeURIComponent(nombre);

        if (nombre != '') {
            var url = 'Operacion.aspx?ope=707&nombre=' + nombre;
            var data = LoadDataText(url);

            $('#idcliente').val(data);
        }
    });

    $('#num_documento').blur(function () {

        var tipo = $("#cbotipo_documento_cliente").val();
        var numero = $(this).val();

      

        if (tipo == "RUC") {
            if (rucValido(numero)) {

                $.ajax({
                    url: 'Operacion.aspx?ope=2000&ruc=' + numero,
                    async: false,
                    beforeSend: function () {
                        BlockBegin('Actualizando ...');
                    },
                    success: function (data) {

                        console.log(data);

                        datos = $.parseJSON(data);


                        $('#nombre').val(datos.nombre);
                        $('#departamento').val(datos.departamento);
                        $('#provincia').val(datos.provincia);
                        $('#distrito').val(datos.distrito);
                        $('#direccion').val(datos.direccion);
                        $('#ubigeo').val(datos.ubigeo);
                        $('#sestado').text(datos.estado);
                    },
                    complete: function () {
                        BlockEnd();
                    }
                });


            }
        }

        if (tipo == "DNI") {
            if (numero.length != 8) {
                alert('El DNI no puede ser diferente de 8 caracteres');
                return;
            }
            else {

                $.ajax({
                    url: 'Operacion.aspx?ope=2001&dni=' + numero,
                    async: false,
                    beforeSend: function () {
                        BlockBegin('Actualizando ...');
                    },
                    success: function (data) {

                        console.log(data);

                        var datos = data.split('|');

                        if (datos[0] != "")
                            $('#nombre').val(datos[0] + ' ' + datos[1] + ' ' + datos[2]);
                        //datos = $.parseJSON(data);

                    },
                    complete: function () {
                        BlockEnd();
                    }
                });

            }
        }
    });

    $('#idtrabajador').prop("readonly", true);

    $('#idtrabajador').blur(function () {

        var codigo = $(this).val();

        if (codigo != '') {
            var url = 'Operacion.aspx?ope=905&numero_documento=' + codigo;
            var data = LoadDataText(url);

            $('#trabajador').val(data);
        }
    });

    $('#cbotipo_documento').change(function () {

        var codigo = $(this).val();
        $('#cboindicador').empty();

        if (codigo == "03") {
            $('#cboindicador').append($('<option>', { value: 'B' }).text('B(Electr)'));
            $('#cboindicador').append($('<option>', { value: '0' }).text('0(Manual)'));
        }

        if (codigo == "01") {
            $('#cboindicador').append($('<option>', { value: 'F' }).text('F(Electr)'));
        }

        if (codigo == "07" || codigo == "08") {
            $('#cboindicador').append($('<option>', { value: 'B' }).text('B(Electr)'));
            $('#cboindicador').append($('<option>', { value: 'F' }).text('F(Electr)'));
        }

        var indicador = $("#cboindicador option:selected").val();
        var tipo = codigo;
        LoadComboNormal('Operacion.aspx?ope=914&tipo=' + tipo + '&indicador=' + indicador, 'cboserie');
        $('#dcorrelativo').val(0);
    });

    $('#cbotipo_documento').blur(function () {

        var codigo = $(this).val();
        $('#cboindicador').empty();

        if (codigo == "03") {
            $('#cboindicador').append($('<option>', { value: 'B' }).text('B(Electr)'));
            $('#cboindicador').append($('<option>', { value: '0' }).text('0(Manual)'));
        }

        if (codigo == "01") {
            $('#cboindicador').append($('<option>', { value: 'F' }).text('F(Electr)'));
        }

        if (codigo == "07" || codigo == "08") {
            $('#cboindicador').append($('<option>', { value: 'B' }).text('B(Electr)'));
            $('#cboindicador').append($('<option>', { value: 'F' }).text('F(Electr)'));
        }

        var indicador = $("#cboindicador option:selected").val();
        var tipo = codigo;
        LoadComboNormal('Operacion.aspx?ope=914&tipo=' + tipo + '&indicador=' + indicador, 'cboserie');
        $('#dcorrelativo').val(0);
    });


    $('#cboindicador').blur(function () {
        $('#cboserie').empty();
        var indicador = $(this).val();
        var tipo = $("#cbotipo_documento option:selected").val();
        var url = 'Operacion.aspx?ope=914&tipo=' + tipo + '&indicador=' + indicador;
        LoadComboNormal(url, 'cboserie');
    });

    //$('#cboserie').blur(function () {
    //    var correlativo = $("#cboserie option:selected").val();
    //    $('#dcorrelativo').val(correlativo);
    //});



    $('#cambio_venta').val(LoadDataText('Operacion.aspx?ope=804'));
    $('#higv').val(LoadDataText('Operacion.aspx?ope=921&nombre=igv'));


    var indicador = $("#cboindicador option:selected").val();
    var tipo = $("#cbotipo_documento option:selected").val();
    var url = 'Operacion.aspx?ope=914&tipo=' + tipo + '&indicador=' + indicador;
    LoadComboNormal(url, 'cboserie');


});

function grabarpermiso() {

    var pusuario = $("#pusuario").val();
    var pclave = $("#pclave").val();


    if (pusuario == "" || pclave == "") {
        alert('debe ingresar un valor en usuario y clave');
        return;
    }


    $.ajax({
        url: 'Operacion.aspx?ope=114&usuario=' + pusuario + '&clave=' + pclave,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {
            console.log(data);

            if (data == "1") {
                $("#pid").val(data);
            }
            else {
                alert('Error en credenciales');
            }
        },
        complete: function () {
            BlockEnd();
        }
    });


}




function nuevocliente() {
    $("#cid").val('0');
    $("#nombre").val('');
    $("#num_documento").val('');
    $("#sestado").text('');
    $("#departamento").val('');
    $("#provincia").val('');
    $("#distrito").val('');
    $("#ubigeo").val('');
    $("#direccion").val('');
    $("#telefono").val('');
    $("#email").val('');
    $("#dcliente").dialog("open");
}

function grabarproducto() {
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

function nuevoproducto() {
    $("#oid").val('0');
    $("#ocodigo").val('');
    $("#onombre").val('');
    $("#odescripcion").val('');
    $("#cbopresentacion option[value=-1]").prop("selected", true);
    $("#cbocategoria option[value=-1]").prop("selected", true);
    $("#ostock").val('1');
    $("#oprecio").val('0.00');
    $("#ototal").val('0.00');
    $("#dproducto").dialog("open");
}

function grabarcliente() {
    var nombre = $("#nombre").val();

    var departamento = $("#departamento").val();
    var provincia = $("#provincia").val();
    var distrito = $("#distrito").val();
    var ubigeo = $("#ubigeo").val();


    var sexo = $("#cbosexo option:selected").val();
    var fecha_nacimiento = $("#fecha_nacimiento").val();
    var tipo_documento = $("#cbotipo_documento_cliente option:selected").val();
    var num_documento = $("#num_documento").val();
    var direccion = $("#direccion").val();
    var telefono = $("#telefono").val();
    var email = $("#email").val();
    var id = $("#cid").val();


    $.ajax({
        url: 'Operacion.aspx?ope=702&id=' + id + '&nombre=' + nombre + '&sexo=' + sexo + '&fecha_nacimiento=' + fecha_nacimiento +
            '&tipo_documento=' + tipo_documento + '&num_documento=' + num_documento +
            '&departamento=' + departamento + '&provincia=' + provincia + '&distrito=' + distrito + '&direccion=' + direccion +
            '&ubigeo=' + ubigeo + '&telefono=' + telefono + '&email=' + email,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            if (data != "")
                alert(data);
            else {
                alert("Registro satisfactorio!");

                LoadAutocomplete('Operacion.aspx?ope=704', 'idcliente');
                LoadAutocomplete('Operacion.aspx?ope=706', 'cliente');


                $("#dcliente").dialog("close");
            }
        },
        complete: function () {
            BlockEnd();
        }
    });

}

function grabar() {

    var idcliente = $("#idcliente").val();
    var idtrabajador = $("#idtrabajador").val();
    var fecha = $("#fecha").val();
    var cambio_venta = $("#cambio_venta").val();
    var moneda = $("#cbomoneda option:selected").val();
    var tipo = $("#cbotipo_documento option:selected").val();
    var indicador = $("#cboindicador option:selected").val();
    var serie = $("#cboserie option:selected").text();
    var correlativo = $("#dcorrelativo").val();
    var referencia = $("#referencia").val();

    var valorventa = $("#valorventa").val();
    var igv = $("#igv").val();
    var total = $("#totalsoles").val();

    if (tipo == "01" || tipo == "07" || tipo == "08") {
        if (indicador == "F") {
            if (idcliente.length != 11) {
                alert('El Id del cliente debe ser un RUC');
                return;
            }
        }

    }

    if (tipo == "07" || tipo == "08") {
        if (referencia == "") {
            alert('Debe ingresar una referencia');
            return;
        }
    }


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

    var url = 'Operacion.aspx?ope=110&idcliente=' + idcliente + '&idtrabajador=' + idtrabajador + '&fecha=' + fecha +
            '&cambio_venta=' + cambio_venta + '&moneda=' + moneda + '&tipo=' + tipo + '&indicador=' + indicador +
            '&serie=' + serie + '&correlativo=' + correlativo +
            '&referencia=' + referencia + '&valorventa=' + valorventa + '&igv=' + igv + '&total=' + total;

    console.log(url);

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

            console.log('items:' + itemsjson);

            $.ajax({
                url: 'Operacion.aspx?ope=111&json=' + encodeURIComponent(itemsjson),
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
                    url: 'Operacion.aspx?ope=112&codigo=' + codigo,
                    async: false,
                    success: function (data) {
                        console.log('complete:' + data);
                        documento = data;
                    }
                });

                alert('Proceso realizado satisfactoriamente ' + documento + ' !');
                $("#ddocumento").dialog("close");
            }
            else
                alert('Hubo un error en el proceso' + documento + ' !');

        },
        complete: function () {
            BlockEnd();
        }
    });





}

function nuevo() {
    $("#id").val('0');
    $("#pid").val('0');
    $("#idcliente").val('0');
    $("#cliente").val('');

    //$("#idtrabajor").val('');
    $("#dcorrelativo").val('');
    $("#referencia").val('');
    $("#trabajor").val('');


    loadconfig();

    $("#ddocumento").dialog("open");
}

function exportdata() {
    $("#tbldetalle").tableToCSV();
}

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
            .append($('<td />').html('<input type="text"     onblur="getpro2(' + numero + ')"                       name="idproducto' + numero + '"     id="idproducto' + numero + '"               style="width:60px;text-align:center;"   maxlength="15"                                               class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"     onblur="getpro(' + numero + ')"                        name="producto' + numero + '"       id="producto' + numero + '"                 style="width:350px"                     maxlength="100"                                             class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"                                        readonly="true"     name="unidad' + numero + '"         id="unidad' + numero + '"                   style="width:60px;text-align:center;"                                                               class="text ui-widget-content ui-corner-all">').attr('align', 'center'))
            .append($('<td />').html('<input type="numeric"  onblur="getcal(' + numero + ')"                        name="cantidad' + numero + '"       id="cantidad' + numero + '"                 style="width:40px;text-align:right;"    maxlength="9"   value="0"      min="1" max="10000" step="1" class="text ui-widget-content ui-corner-all"><span name="preciototal' + numero + '" id="preciototal' + numero + '">').attr('align', 'center'))
            .append($('<td />').html('<input type="text"     onblur="getcal(' + numero + ')"                        name="preciounit' + numero + '"     id="preciounit' + numero + '"               style="width:60px;text-align:right;"                    value="0"                                   class="text ui-widget-content ui-corner-all">').attr('align', 'center'))

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
    nombre = encodeURIComponent(nombre);

    if (nombre != '') {
        var datos = LoadDataText('Operacion.aspx?ope=605&nombre=' + nombre).split('|');

        console.log(datos);

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

function getpro2(id) {
    var objeto = 'idproducto' + id;
    var codigo = $("#" + objeto).val();

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

function loaddata() {
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
            .append($('<th />').html('DNI/RUC'))
            .append($('<th />').html('CLIENTE'))
            .append($('<th />').html('TIPO'))
            .append($('<th />').html('DOCUMENTO'))
            .append($('<th />').html('RESUMEN'))
            .append($('<th />').html('FECHA'))
            .append($('<th />').html('MONEDA'))
            .append($('<th />').html('MONTO'))
            .append($('<th />').html('ESTADO'))
            .append($('<th />').html(''))
            .append($('<th />').html('SUNAT'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
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
            url: 'Operacion.aspx?ope=114&empresa=' + empresa + '&codigo=' + codigo,
            async: false,
            success: function (data) {
                console.log(data);
                alert('Proceso realizado');
            }
        });
    }
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

