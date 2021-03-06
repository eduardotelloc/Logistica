$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)
   
    $('#ribbon').ribbon();

    $("#dialog").dialog({
        autoOpen: false,
        maxWidth: 680,
        maxHeight: 300,
        width: 680,
        height: 300,
        modal: true,
    });
    
    loadinicio();

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

function retornar() {
    window.location.href = 'Menu.aspx';
}

function cancelar() {
    $("#dialog").dialog("close");
}


function loadinicio() {

    $("#registros").html('');
    $("#tbldetalle").html('');

    $("#tbldetalle").append(
        $('<colgroup/>')
            .append($('<col />').html(''))
            .append($('<col width="20%"/>').html(''))
            .append($('<col width="20%"/>').html(''))
            .append($('<col width="20%"/>').html(''))
            .append($('<col width="20%"/>').html(''))
            .append($('<col />').html(''))
    );

    $("#tbldetalle").append($('<thead/>').append(
        $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Tipo'))
            .append($('<th />').html('Indicador'))
            .append($('<th />').html('Serie'))
            .append($('<th />').html('Correaltivo'))
            .append($('<th />').html(''))
    ));

    $("#registros").html('<b>0 registros encontrados</b>');
}

function buscar() {

    var nombre = $("#nombre").val().trim();

    $("#registros").html("");
    $("#tbldetalle tbody").remove();
    $("#tbldetalle").append($('<tbody/>'));
   
    $.ajax({
        url: 'Operacion.aspx?ope=911&serie=' + nombre,
        async: false,
        beforeSend: function () {
            BlockBegin('Buscando ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);
            
            var fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tbldetalle").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.tipodes).attr('align', 'left'))
                    .append($('<td />').html(val.indicadordes).attr('align', 'left'))
                    .append($('<td />').html(val.serie).attr('align', 'center'))
                    .append($('<td />').html(val.correlativo).attr('align', 'right'))
                    .append($('<td />').html("<input type='radio' value='" + val.id + "' id='r" + val.id + "' name='radio' onclick='seleccion(\"" + val.id + "\",\"" + val.nombre + "\")' />").attr('align', 'center'))
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
    $("#serie").val 
    $("#correaltivo").val('0');
    $("#serie").prop("disabled", false);

    $("#dialog").dialog("open");
}

function grabar()
{
    var tipo = $("#cbotipo option:selected").val();
    var indicador = $("#cboindicador option:selected").val();
    var serie = $("#serie").val();
    var correlativo = $("#correlativo").val();
    var id = $("#hid").val();

    if (serie == "") {
        alert('La serie no debe ser un valor vacio');
        return;
    }

    if (correlativo == "") {
        alert('El correlativo no debe ser un valor vacio');
        return;
    }
    

    $.ajax({
        url: 'Operacion.aspx?ope=912&id=' + id + '&tipo=' + tipo + '&indicador=' + indicador + '&serie=' +
                                            serie + '&correlativo=' + correlativo,
        async: false,
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
    $("#serie").prop("disabled", true);


    
    $.ajax({
        url: 'Operacion.aspx?ope=910&codigo=' + id,
        async: false,
        success: function (data) {

            datos = $.parseJSON(data);

            $.each(datos, function (i, val) {
                $("#cbotipo option[value=" + val.tipo + "]").prop("selected", true);
                $("#cboindicador option[value=" + val.indicador + "]").prop("selected", true);
                $("#serie").val(val.serie);
                $("#correlativo").val(val.correlativo);

            });
        }
    });

    $("#dialog").dialog("open");
}

function eliminar()
{
    var id = $("#hid").val();
    var nombre = $("#hnombre").val();

    if (confirm('Esta seguro de eliminar la serie ' + nombre + ' ?')) {

        $.ajax({
            url: 'Operacion.aspx?ope=913&id=' + id,
            async: false,
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