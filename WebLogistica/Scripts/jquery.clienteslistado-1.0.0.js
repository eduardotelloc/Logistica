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


    $('#num_documento').blur(function () {
        var tipo = $("#cbotipo_documento").val();
        var numero = $(this).val();


        $('#nombre').val('');
        $('#departamento').val('');
        $('#provincia').val('');
        $('#distrito').val('');
        $('#direccion').val('');
        $('#ubigeo').val('');
        $('#sestado').text('');
       
        if (tipo == "RUC") {
            if (rucValido(numero)) {
                
                $.ajax({
                    url: 'Operacion.aspx?ope=2000&ruc=' + numero,
                    async: false,
                    beforeSend: function () {
                        BlockBegin('Updating ...');
                    },
                    success: function (data) {
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

                //$.ajax({
                //    url: 'Operacion.aspx?ope=2001&dni=' + numero,
                //    async: false,
                //    beforeSend: function () {
                //        BlockBegin('Updating ...');
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
    });
    

    


    $("#fecha_nacimiento").datepicker({
        beforeShow: function (i) {
            if ($(i).attr('readonly')) { return false; }
        },
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true
    }).datepicker("setDate", "0").mask('99/99/9999');

    


    tips = $(".validateTips");

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " +
              min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    $("#dialog").dialog({
        autoOpen: false
    });


});



function loaddata(){

    
    var cliente = $("#cliente").val();
    var top = '';

    $("#registros").html("");
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
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $("#tbldetalle").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=701&nombre=' + cliente + '&top=' + top,
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
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.sexo).attr('align', 'center'))
                    .append($('<td />').html(val.fecha_nacimiento).attr('align', 'center'))
                    .append($('<td />').html(val.tipo_documento).attr('align', 'center'))
                    .append($('<td />').html(val.num_documento).attr('align', 'left'))
                    .append($('<td />').html(val.direccion).attr('align', 'left'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"modificar('" + val.id + "','" + val.nombre + "','" + val.sexo + "','" + val.fecha_nacimiento + "','" + val.tipo_documento + "','" + val.num_documento + "','" + val.departamento + "','" + val.provincia + "','" + val.distrito + "','" + val.direccion + "','" + val.ubigeo + "','" + val.telefono + "','" + val.email + "')\"><img src=\"Img/edit.png\" title=\"Editar\" /></a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"eliminar('" + val.id + "')\"><img src=\"Img/delete.png\" title=\"Eliminar\" /></a>").attr('align', 'center'))
                );

                fila++;
                numero++;
            });

            $("#registros").html('<b>' + (fila) + ' registros encontrados</b>');

            //$('#tbldetalle').DataTable({
            //    destroy: true,
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


function grabar() {
    var nombre = $("#nombre").val();
    nombre = encodeURIComponent(nombre);

    var departamento = $("#departamento").val();
    var provincia = $("#provincia").val();
    var distrito = $("#distrito").val();
    var ubigeo = $("#ubigeo").val();

    
    var sexo = $("#cbosexo option:selected").val();
    var fecha_nacimiento = $("#fecha_nacimiento").val();
    var tipo_documento = $("#cbotipo_documento option:selected").val();
    var num_documento = $("#num_documento").val();
    var direccion = $("#direccion").val();
    var telefono = $("#telefono").val();
    var email = $("#email").val();
    var id = $("#id").val();

    if (tipo_documento == "RUC")
    {
        if (num_documento.length != 11)
        {
            alert('El RUC debe contar con 11 caracteres');
            return;
        }
    }

    if (tipo_documento == "DNI") {
        if (num_documento.length != 8) {
            alert('El RUC debe contar con 8 caracteres');
            return;
        }
    }


    $.ajax({
        url: 'Operacion.aspx?ope=702&id=' + id + '&nombre=' + nombre + '&sexo=' + sexo + '&fecha_nacimiento=' + fecha_nacimiento +
            '&tipo_documento=' + tipo_documento + '&num_documento=' + num_documento +
            '&departamento=' + departamento + '&provincia=' + provincia + '&distrito=' + distrito + '&direccion=' + direccion +
            '&ubigeo=' + ubigeo + '&telefono=' + telefono + '&email=' + email,
        async: true,
        beforeSend: function () {
            BlockBegin('Updating ...');
        },
        success: function (data) {

            if (data != "")
                alert(data);
            else {
                alert("Registro satisfactorio!");
                loaddata();
            }
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog").dialog("close");
}

function modificar(id, nombre, sexo, fecha_nacimiento, tipo_documento, num_documento, departamento, provincia, distrito, direccion, ubigeo, telefono, email) {
    $("#sestado").text('');
    $("#nombre").val(nombre);
    $("#cbosexo option[value=" + sexo + "]").prop("selected", true);
    $("#fecha_nacimiento").val(fecha_nacimiento);
    $("#cbotipo_documento option[value=" + tipo_documento + "]").prop("selected", true);
    $("#num_documento").val(num_documento);
    $("#departamento").val(departamento);
    $("#provincia").val(provincia);
    $("#distrito").val(distrito);
    $("#direccion").val(direccion);
    $("#ubigeo").val(ubigeo);

    $("#telefono").val(telefono);
    $("#email").val(email);

    $("#id").val(id);

    $("#dialog").dialog("open");
}

function eliminar(id) {
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=703&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Updating ...');
            },
            success: function (data) {

                loaddata();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function nuevo() {
    $("#id").val('0');
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
    $("#dialog").dialog("open");
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

