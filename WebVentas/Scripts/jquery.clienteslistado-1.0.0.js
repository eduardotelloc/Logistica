var ubigeo;

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
    //$('#myTab li:last-child a').tabs('show')

    $("#fecha_nacimiento").datepicker({
        beforeShow: function (i) {
            if ($(i).attr('readonly')) { return false; }
        },
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        maxDate: '+0m +0d',
    }).datepicker("setDate", "0").mask('99/99/9999');


    $("#dialog").dialog({
        autoOpen: false,
        maxWidth: 680,
        maxHeight: 640,
        width: 680,
        height: 640,
        modal: true,
    });

    
    ubigeo = LoadData('Operacion.aspx?ope=002');

    
    loadinicio();


    $('#cbodepartamento').on('change', function () {

        $("#cboprovincia").children().remove().end();
        $("#cboprovincia").append('<option value="00">[Seleccinar]</option>');
        var departamento = this.value;
       
        $.each(ubigeo, function (i, val) {

            if (val.departamento == departamento && val.provincia != '00' && val.distrito == '00') {
                $("#cboprovincia").append('<option value="' + val.provincia + '">' + val.nombre + '</option>');
            }
        });

        $("#cbodistrito").children().remove().end();
        $("#cbodistrito").append('<option selected value="00">[Ninguno]</option>');
    });


    $('#cboprovincia').on('change', function () {
        $("#cbodistrito").children().remove().end();
        $("#cbodistrito").append('<option value="00">[Seleccinar]</option>');
        var departamento = $("#cbodepartamento").val();
        var provincia = this.value;

        $.each(ubigeo, function (i, val) {
            if (val.departamento == departamento && val.provincia == provincia && val.distrito != '00') {
                $("#cbodistrito").append('<option value="' + val.distrito + '">' + val.nombre + '</option>');
            }
        });
    });

    $('#cbodistrito').on('change', function () {
        var departamento = $("#cbodepartamento").val();
        var provincia = $("#cboprovincia").val();
        var distrito = this.value;
        $("#ubigeo").val(departamento + provincia + distrito);
    });


    $('#longitud').blur(function () {
        mapa();
    });

    $('#numerodocumento').blur(function () {

        var id = $("#hid").val();

        if (id == '0') {
            var tipo = $("#cbotipo_documento").val();
            var numero = $(this).val();


            $('#nombre').val('');
            $('#cboprovincia').val('');
            $('#cbodistrito').val('');
            $('#direccion').val('');
            $('#ubigeo').val('');
            $('#sestado').text('');

            if (tipo == "RUC") {
                if (rucValido(numero)) {

                    $.ajax({
                        url: 'Operacion.aspx?ope=2000&ruc=' + numero,
                        async: false,
                        beforeSend: function () {
                            BlockBegin('Actualizando ...');
                        },
                        success: function (data) {
                            datos = $.parseJSON(data);
                            $('#nombre').val(datos.nombre);
                            $('#direccion').val(datos.direccion);
                            $('#sestado').text(datos.estado);

                            var tmpubigeo = datos.ubigeo;
                            $('#ubigeo').val(tmpubigeo);

                            var tmpdepartamento = tmpubigeo.substring(0, 2);
                            var tmpprovincia = tmpubigeo.substring(2, 4);
                            var tmpdistrito = tmpubigeo.substring(4, 6);

                            $("#cbodepartamento").val(tmpdepartamento).trigger('change');
                            $("#cboprovincia ").val(tmpprovincia).trigger('change');
                            $("#cbodistrito").val(tmpdistrito);
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

                    //$.ajax({
                    //    url: 'Operacion.aspx?ope=2001&dni=' + numero,
                    //    async: false,
                    //    beforeSend: function () {
                    //        BlockBegin('Actualizando ...');
                    //    },
                    //    success: function (data) {

                    //        console.log(data);

                    //        var datos = data.split('|');

                    //        if (datos[0]!="")
                    //            $('#nombre').val(datos[0] + ' ' + datos[1] + ' ' + datos[2]);
                    //        //datos = $.parseJSON(data);

                    //    },
                    //    complete: function () {
                    //        BlockEnd();
                    //    }
                    //});

                }

            }
        }
    });

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

});

function mapa() {

    var latitud = $('#latitud').val();
    var longitud = $('#longitud').val();

    var myLatlng = new google.maps.LatLng(latitud, longitud)
    var mapOptions = {
        center: myLatlng,
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        marker: true
    };

    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: 'Location'
    });

    marker.setIcon('Img/green-dot.png')
}

function cancelar() {
    $("#dialog").dialog("close");
}

function exportar() {
    $("#tbldetalle").table2excel({
        name: "Table2Excel",
        filename: "Clientes",
        fileext: ".xls"
    });
}


function retornar() {
    window.location.href = 'Menu.aspx';
}

function loadinicio() {

    $("#registros").html('');
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
    );

    $("#tbldetalle").append($('<thead/>').append(
        $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Sexo'))
            .append($('<th />').html('Fecha Nac'))
            .append($('<th />').html('Tipo'))
            .append($('<th />').html('Numero'))
            .append($('<th />').html('Direccion'))
            .append($('<th />').html('Distrito'))
            .append($('<th />').html('Teléfono'))
            .append($('<th />').html('Correo'))
            .append($('<th />').html(''))
    ));

    $("#registros").html('<b>0 registros encontrados</b>');


    loadubigeo();
}

function loadubigeo() {
    $("#registros").html('<b>0 registros encontrados</b>');

    $("#cbodepartamento").children().remove().end();
    $("#cbodepartamento").append('<option value="00">[Seleccinar]</option>');

    $("#cboprovincia").children().remove().end();
    $("#cboprovincia").append('<option selected value="00">[Ninguno]</option>');

    $("#cbodistrito").children().remove().end();
    $("#cbodistrito").append('<option selected value="00">[Ninguno]</option>');

    $.each(ubigeo, function (i, val) {
        if (val.provincia == '00' && val.distrito == '00') {
            $("#cbodepartamento").append('<option value="' + val.departamento + '">' + val.nombre + '</option>');
        }
    });
}

function buscar(){

    var cliente = $("#cliente").val().trim();

    if (cliente.length < 2) {
        alert('Debe ingresar al menos 2 caracteres de busqueda');
        return;
    }


    $("#registros").html("");
    $("#tbldetalle tbody").remove();
    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=701&nombre=' + encodeURIComponent(cliente),
        async: false,
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
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.sexo).attr('align', 'center'))
                    .append($('<td />').html(val.fecha_nacimiento).attr('align', 'center'))
                    .append($('<td />').html(val.tipo_documento).attr('align', 'center'))
                    .append($('<td />').html(val.num_documento).attr('align', 'left'))
                    .append($('<td />').html(val.direccion).attr('align', 'left'))
                    .append($('<td />').html(val.distrito).attr('align', 'left'))
                    .append($('<td />').html(val.telefono).attr('align', 'left'))
                    .append($('<td />').html(val.email).attr('align', 'left'))
                    .append($('<td />').html("<input type='radio' value='" + val.id + "' id='r" + val.id + "' name='radio' onclick='seleccion(\"" + val.id + "\",\"" + val.nombre  +"\")' />").attr('align', 'center'))
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
                $("#hnombre").val($(this).find("td:nth-child(2)").text());
                seleccion_row(id);
            });

            $('#tbldetalle tbody tr').on("dblclick", function (e) {
                var id = $(this).find("input[type=radio]").val();
                $("#hnombre").val($(this).find("td:nth-child(2)").text());
                seleccion_row(id);
                modificar();
            });

        },
        complete: function () {
            BlockEnd();
        }
    });
}

function seleccion_row(id) {
    $("#r" + id).prop('checked', true);
    $('#open-page-btn').enable();
    $('#del-page-btn').enable();
    $("#hid").val(id);
}

function seleccion(id, nombre) {
    $('#open-page-btn').enable();
    $('#del-page-btn').enable();
    $("#hnombre").val(nombre);
    $("#hid").val(id);
}

function nuevo() {
    $("#hid").val('0');
    $("#hnombre").val('');

    $("#nombre").val('');
    $("#numerodocumento").val('');
    $("#sestado").text('');
    $("#cbodepartamento").val('00');
    $("#cboprovincia").val('00');
    $("#cbodistrito").val('00');
    $("#ubigeo").val('');
    $("#direccion").val('');
    $("#telefono").val('');
    $("#email").val('');
    $('#latitud').val('');
    $('#longitud').val('');
    mapa();

    $("#nombre").prop("disabled", false);
    $("#numerodocumento").prop("disabled", false);

    $("#dialog").dialog("open");
}


function grabar() {

    var id = $("#hid").val();
    var nombre = $("#nombre").val().trim();
    var sexo = $("#cbosexo option:selected").val();
    var fecha_nacimiento = $("#fecha_nacimiento").val();
    var tipo_documento = $("#cbotipo_documento option:selected").val();
    var numerodocumento = $("#numerodocumento").val().trim();
    var direccion = $("#direccion").val().trim();
    var departamento = $("#cbodepartamento option:selected").text();
    var provincia = $("#cboprovincia option:selected").text();
    var distrito = $("#cbodistrito option:selected").text();
    var ubigeo = $("#ubigeo").val();
    var telefono = $("#telefono").val().trim();
    var email = $("#email").val().trim();
    var latitud = $('#latitud').val();
    var longitud = $('#longitud').val();
    

    if (tipo_documento == "RUC")
    {
        if (numerodocumento.length != 11)
        {
            alert('El RUC debe contener con 11 caracteres');
            return;
        }
    }

    if (tipo_documento == "DNI") {
        if (numerodocumento.length != 8) {
            alert('El RUC debe contener con 8 caracteres');
            return;
        }
    }

    if (nombre.length < 2) {
        alert('El nombre debe contener mas de 2 caracteres');
        return;
    }


    $.ajax({
        url: 'Operacion.aspx?ope=702&id=' + id + '&nombre=' + nombre + '&sexo=' + sexo + '&fecha_nacimiento=' + fecha_nacimiento +
            '&tipo_documento=' + tipo_documento + '&num_documento=' + numerodocumento +
            '&departamento=' + departamento + '&provincia=' + provincia + '&distrito=' + distrito + '&direccion=' + direccion +
            '&ubigeo=' + ubigeo + '&telefono=' + telefono + '&email=' + email + '&latitud=' + latitud + '&longitud=' + longitud,
        async: true,
        beforeSend: function () {
            BlockBegin('Actualizando ...');
        },
        success: function (data) {

            if (data != "")
                alert(data);
            else {
                alert("La transacción se realizó satisfactoriamente!");
            }
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog").dialog("close");
}

function modificar() {

    var id = $("#hid").val();
    $("#nombre").prop("disabled", true);
    $("#numerodocumento").prop("disabled", true);

    $.ajax({
        url: 'Operacion.aspx?ope=700&codigo=' + id,
        async: false,
        success: function (data) {

            datos = $.parseJSON(data);

            $.each(datos, function (i, val) {

                $("#sestado").text('');
                $("#nombre").val(val.nombre);
                $("#cbosexo option[value=" + val.sexo + "]").prop("selected", true);
                $("#fecha_nacimiento").val(val.fecha_nacimiento);
                $("#cbotipo_documento option[value=" + val.tipo_documento + "]").prop("selected", true);
                $("#numerodocumento").val(val.num_documento);

                loadubigeo();

                if (val.departamento != '') {
                    $("#cbodepartamento option:contains(" + val.departamento + ")").attr('selected', 'selected').trigger('change');
                    if (val.provincia != '') {
                        $("#cboprovincia option:contains(" + val.provincia + ")").attr('selected', 'selected').trigger('change');
                        if (val.distrito != '') {
                            $("#cbodistrito option:contains(" + val.distrito + ")").attr('selected', 'selected');
                        }
                    }
                }

                $("#direccion").val(val.direccion);
                $("#ubigeo").val(val.ubigeo);
                $("#telefono").val(val.telefono);
                $("#email").val(val.email);

                $("#latitud").val(val.latitud);
                $("#longitud").val(val.longitud);
                mapa();
                
            });
        }
    });

    $("#dialog").dialog('open');
}

function eliminar() {

    var nombre=$("#hnombre").val();

    if (confirm('Esta seguro de eliminar el cliente ' + nombre  + ' ?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=703&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Actualizando ...');
            },
            success: function (data) {
                alert("La transacción se realizó satisfactoriamente!");
                buscar();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function rucValido(ruc) {
    //11 dígitos y empieza en 10,15,16,17 o 20
    if (!(ruc >= 1e10 && ruc < 11e9
        || ruc >= 15e9 && ruc < 18e9
        || ruc >= 2e10 && ruc < 21e9))
        return false;

    for (var suma = -(ruc % 10 < 2), i = 0; i < 11; i++, ruc = ruc / 10 | 0)
        suma += (ruc % 10) * (i % 7 + (i / 7 | 0) + 1);
    return suma % 11 === 0;

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
