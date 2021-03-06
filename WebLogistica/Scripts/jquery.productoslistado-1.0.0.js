$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
    Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(onRequestEnd)
    $("#tabs").tabs();

    LoadSelectItem('Operacion.aspx?ope=301&nombre=', 'cbopresentacion');
    LoadSelectItem('Operacion.aspx?ope=401&nombre=', 'cbocategoria');

    

    tips = $(".validateTips");

    $("#oidserie").val(LoadDataText('Operacion.aspx?ope=106'));

    $('#ochkserie').change(function () {

        if (this.checked) {
            $("#oserie").val($("#oidserie").val());
        }
        else {
            $("#oserie").val('');
        }
        
    });

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    //$("#filename").change(function (e) {

    //    $("#tblMasivo > tbody").empty();

    //    var ext = $("input#filename").val().split(".").pop().toLowerCase();


    //    if ($.inArray(ext, ["csv"]) == -1) {
    //        alert('Upload CSV');
    //        return false;
    //    }


    //    if (e.target.files != undefined) {
    //        var reader = new FileReader();
    //        reader.onload = function (e) {
    //            var lines = e.target.result.split('\r\n');

    //            for (i = 0; i < lines.length; ++i) {

    //                if (i > 0) {
    //                    var rowContent = lines[i].split(/,(?=(?:(?:[^"]*"){2})*[^"]*$)/);

    //                    if (rowContent != "") {

    //                        var codigo = rowContent[0].replaceAll('"', '');
    //                        var nombre = rowContent[1].replaceAll('"', '');
    //                        var serie = rowContent[2].replaceAll('"', '');
    //                        var stock = rowContent[3].replaceAll('"', '');
    //                        var precio = rowContent[4].replaceAll('"', '');
    //                        var bonificacion = rowContent[5].replaceAll('"', '');

    //                        var newRowContent = "<tr><td>" + (i + 1) + "</td><td>" + codigo + "</td><td>" + nombre + "</td><td>" + serie + "</td><td>" + stock + "</td><td>" + precio + "</td><td>" + ((bonificacion == 1) ? 'SI' : 'NO') + "</td></tr>";
    //                        $("#tblMasivo tbody").append(newRowContent);
    //                    }
    //                }
    //            }

    //        };
    //        reader.readAsText(e.target.files.item(0));
    //    }

    //    return false;
    //});

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

    $("#dialog-Presentacion").dialog({
        autoOpen: false
    });

    $("#dialog-Categoria").dialog({
        autoOpen: false
    });


    $("#dialog-Producto").dialog({
        autoOpen: false
    });










    var customCellWriter = function (column, record) {
        var html = column.attributeWriter(record),
            td = '<td';
        if (column.hidden || column.textAlign) {
            td += ' style="';
            if (column.hidden) {
                td += 'display: none;';
            }
            if (column.textAlign) {
                td += 'text-align: ' + column.textAlign + ';';
            }
            td += '"';
        }
        return td + '><div>' + html + '<\/td>';
    };

    var makeElementEditable = function (element) {
        $('div', element).attr('contenteditable', true);

        $(element).focusout(function () {
            $('div', element).attr('contenteditable', false);
        });

        $(element).keydown(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $('div', element).attr('contenteditable', false);
                $(document).focus();
            }
        });

        $('div', element).on('paste', function (e) {
            e.preventDefault();
        });
    };

    $(document).on('keypress', 'textarea#jsonDataDump', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });

    $(document).on('keypress', 'textarea#excelPasteBox', function (e) {
        if (e.ctrlKey !== true && e.key != 'v') {
            e.preventDefault();
            e.stopPropagation();
        }
    });

    $(document).on('paste', 'textarea#excelPasteBox', function (e) {
        e.preventDefault();
        var cb;
        var clipText = '';

        if (window.clipboardData && window.clipboardData.getData) {
            cb = window.clipboardData;
            clipText = cb.getData('Text');
        } else if (e.clipboardData && e.clipboardData.getData) {
            cb = e.clipboardData;
            clipText = cb.getData('text/plain');
        } else {
            cb = e.originalEvent.clipboardData;
            clipText = cb.getData('text/plain');
        }

        var clipRows = clipText.split('\n');

        for (i = 0; i < clipRows.length; i++) {
            clipRows[i] = clipRows[i].split('\t');
            
        }

        var jsonObj = [];

        for (i = 0; i < clipRows.length - 1; i++) {
            var item = {};

            for (j = 0; j < clipRows[i].length; j++) {
                if (clipRows[i][j] != '\r') {
                    if (clipRows[i][j].length !== 0) {
                        item[j] = clipRows[i][j];
                    }
                }
            }
            
            jsonObj.push(item);
        }



        $('textarea#jsonDataDump').val('');
        var tablePlaceHolder = document.getElementById('output');
        tablePlaceHolder.innerHTML = '';
        var table = document.createElement('table');
        table.id = 'excelDataTable';
        table.className = 'table';
        var header = table.createTHead();
        var row = header.insertRow(0);
        var keys = [];

        

        for (var i = 0; i < jsonObj.length; i++) {
            var obj = jsonObj[i];
            for (var j in obj) {
                if ($.inArray(j, keys) == -1) {
                    keys.push(j);
                }
            }
        }

        //console.log(keys);

        keys.forEach(function (value, index) {

            //console.log(value);

            var headerCell = document.createElement('th');
            headerCell.innerHTML = value;
            $(headerCell).click(function () {
                makeElementEditable(this);
            });

            $(headerCell).keyup(function (e) {
                var ignoredClass = 'ignored';
                var ignoredAttr = 'data-attr-ignore';
                var columnCells = $('td, th', table).filter(':nth-child(' + ($(this).index() + 1) + ')');
                $(this).removeAttr(ignoredAttr);

                $(columnCells).each(function () {
                    $(this).removeClass(ignoredClass);
                    $(this).removeAttr(ignoredAttr);
                });

                if ($(this).is(':empty') || $(this).text().trim() === '') {
                    $(this).attr(ignoredAttr, '');
                    $(columnCells).each(function () {
                        $(this).addClass(ignoredClass);
                        $(this).attr(ignoredAttr, '');
                    });
                }
            });

            var cell = row.insertCell(index);
            cell.parentNode.insertBefore(headerCell, cell);
            cell.parentNode.removeChild(cell);
        });

        //console.log(table);

        tablePlaceHolder.appendChild(table);
        var excelDynaTable = $('table#excelDataTable').dynatable({
            features: {
                paginate: false,
                search: false,
                recordCount: true,
                sort: false
            },
            dataset: {
                records: jsonObj
            },
            writers: {
                _cellWriter: customCellWriter
            }
        });
        $(document).on('click', 'table#excelDataTable td', function () {
            makeElementEditable(this);
        });
    });
});


function exportdata() {
    $("#tblProducto").tableToCSV();
}

function mgrabar() {

    var bnlrows = false;

    $("#excelDataTable > tr").each(function (index, tr) {

        if (index != 0) {
            bnlrows = true;
            var numero = $(this).find("td").eq(0).text();
            var codigo = $(this).find("td").eq(1).text();
            var nombre = $(this).find("td").eq(2).text();
            var serie = $(this).find("td").eq(3).text();
            var stock = $(this).find("td").eq(4).text();
            var precio = $(this).find("td").eq(5).text();
            var bonificacion = $(this).find("td").eq(6).text();
            bonificacion = ((bonificacion == 'SI') ? '1' : '0');

            var url = 'Operacion.aspx?ope=4001&codigo=' + codigo + '&nombre=' + nombre + '&serie=' + serie + '&stock=' + stock + '&precio=' + precio + '&bonificacion=' + bonificacion;
            console.log(url);

            $.ajax({
                url: url,
                async: false,
                success: function (data) {
                }
            });
        }

        
    });

    if (bnlrows) {
        alert('Proceso finalizado');
    }

}

function loaddataPresentacion() {
    var presentacion = $("#presentacion").val();
    
    $("#tblPresentacion").html('');

    $("#tblPresentacion").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="40%"/>').html(''))
           .append($('<col width="40%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblPresentacion").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Descripcion'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))   
            ));

    var fila = 0;

   
    $("#tblPresentacion").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=301&nombre=' + presentacion,
        async: true,
        beforeSend: function () {
            BlockBegin('Searching ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);
            
            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblPresentacion").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.descripcion).attr('align', 'left'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"pmodificar('" + val.id + "','" + val.nombre + "','" + val.descripcion + "')\"><img src=\"Img/edit.png\" title=\"Editar\" /></a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"peliminar('" + val.id + "')\"><img src=\"Img/delete.png\" title=\"Eliminar\" /></a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#registrosPresentacion").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function pgrabar()
{
    var pnombre = $("#pnombre").val();
    var pdescripcion = $("#pdescripcion").val();
    var pid = $("#pid").val();
    

    $.ajax({
        url: 'Operacion.aspx?ope=302&id=' + pid + '&nombre=' + pnombre + '&descripcion=' + pdescripcion,
        async: true,
        beforeSend: function () {
            BlockBegin('Updating ...');
        },
        success: function (data) {

            loaddataPresentacion();
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog-Presentacion").dialog("close");
}

function pmodificar(id, nombre, descripcion) {
    $("#pnombre").val(nombre);
    $("#pdescripcion").val(descripcion);
    $("#pid").val(id);

    $("#dialog-Presentacion").dialog("open");
}

function peliminar(id)
{
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=303&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Updating ...');
            },
            success: function (data) {

                loaddataPresentacion();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function pnuevo() {
    $("#pid").val('0');
    $("#pnombre").val('');
    $("#pdescripcion").val('');
    $("#dialog-Presentacion").dialog("open");
}



function loaddataCategoria() {
    var categoria = $("#categoria").val();

    $("#tblCategoria").html('');

    $("#tblCategoria").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="40%"/>').html(''))
           .append($('<col width="40%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblCategoria").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Nombre'))
            .append($('<th />').html('Descripcion'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $("#tblCategoria").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=401&nombre=' + categoria,
        async: true,
        beforeSend: function () {
            BlockBegin('Searching ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblCategoria").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    .append($('<td />').html(val.descripcion).attr('align', 'left'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"cmodificar('" + val.id + "','" + val.nombre + "','" + val.descripcion + "')\"><img src=\"Img/edit.png\" title=\"Editar\" /></a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"celiminar('" + val.id + "')\"><img src=\"Img/delete.png\" title=\"Eliminar\" /></a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#registrosCategoria").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function cgrabar() {
    var cnombre = $("#cnombre").val();
    var cdescripcion = $("#cdescripcion").val();
    var cid = $("#cid").val();


    $.ajax({
        url: 'Operacion.aspx?ope=402&id=' + cid + '&nombre=' + cnombre + '&descripcion=' + cdescripcion,
        async: true,
        beforeSend: function () {
            BlockBegin('Updating ...');
        },
        success: function (data) {

            loaddataCategoria();
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog-Categoria").dialog("close");
}

function cmodificar(id, nombre, descripcion) {
    $("#cnombre").val(nombre);
    $("#cdescripcion").val(descripcion);
    $("#cid").val(id);

    $("#dialog-Categoria").dialog("open");
}

function celiminar(id) {
    if (confirm('Esta seguro de eliminar el registro?')) {


        $.ajax({
            url: 'Operacion.aspx?ope=403&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Updating ...');
            },
            success: function (data) {

                loaddataCategoria();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}



function cnuevo() {
    $("#cid").val('0');
    $("#cnombre").val('');
    $("#cdescripcion").val('');
    $("#dialog-Categoria").dialog("open");
}

function loaddataProducto() {
    var producto = $("#producto").val();

    $("#tblProducto").html('');

    $("#tblProducto").append(
            $('<colgroup/>')
           .append($('<col />').html(''))
           .append($('<col width="10%"/>').html(''))
           .append($('<col width="20%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col width="30%"/>').html(''))
           //.append($('<col width="5%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col width="5%"/>').html(''))
           .append($('<col />').html(''))
           .append($('<col />').html(''))
           );

    $("#tblProducto").append($('<thead/>').append(
            $('<tr/>')
            .append($('<th />').html('N°'))
            .append($('<th />').html('Presentacion'))
            .append($('<th />').html('Categoria'))
            .append($('<th />').html('Serie'))
            .append($('<th />').html('Codigo'))
            .append($('<th />').html('Nombre'))
            //.append($('<th />').html('Descripcion'))
            .append($('<th />').html('Stock'))
            .append($('<th />').html('Precio Unit'))
            .append($('<th />').html('Precio Tot'))
            .append($('<th />').html('Bonificación'))
            .append($('<th />').html(''))
            .append($('<th />').html(''))
            ));

    var fila = 0;

    $("#tblProducto").append($('<tbody/>'));

    $.ajax({
        url: 'Operacion.aspx?ope=601&nombre=' + producto,
        async: true,
        beforeSend: function () {
            BlockBegin('Searching ...');
        },
        success: function (data) {

            datos = $.parseJSON(data);

            fila = 0;
            var numero = 1;

            $.each(datos, function (i, val) {

                $("#tblProducto").append($('<tr/>')
                    .append($('<td />').html(numero).attr('align', 'center'))
                    .append($('<td />').html(val.presentacion).attr('align', 'left'))
                    .append($('<td />').html(val.categoria).attr('align', 'left'))
                    .append($('<td />').html(val.serie).attr('align', 'center'))
                    .append($('<td />').html(val.codigo).attr('align', 'center'))
                    .append($('<td />').html(val.nombre).attr('align', 'left'))
                    //.append($('<td />').html(val.descripcion).attr('align', 'left'))
                    .append($('<td />').html(val.stock).attr('align', 'right'))
                    .append($('<td />').html(val.precio).attr('align', 'right'))
                    .append($('<td />').html(val.total).attr('align', 'right'))
                    .append($('<td />').html( ((val.bonificacion=='1')?'SI':'NO') ).attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"omodificar('" + val.id + "','" + val.codigo + "','" + val.serie + "','" + val.nombre + "','" + val.descripcion + "','" + val.idpresentacion + "','" + val.idcategoria + "','" + val.stock + "','" + val.precio + "','" + val.total + "','" + val.bonificacion + "')\"><img src=\"Img/edit.png\" title=\"Editar\" /></a>").attr('align', 'center'))
                    .append($('<td />').html("<a href=\"#\" style=\"color:#545454\" onClick=\"oeliminar('" + val.id + "')\"><img src=\"Img/delete.png\" title=\"Eliminar\" /></a>").attr('align', 'center'))

                );

                fila++;
                numero++;
            });

            $("#registrosProducto").html('<b>' + (fila) + ' registros encontrados</b>');
        },
        complete: function () {
            BlockEnd();
        }
    });
}

function ograbar() {
    var ocodigo = $("#ocodigo").val();
    var idpresentacion = $("#cbopresentacion option:selected").val();
    var idcategoria = $("#cbocategoria option:selected").val();
    var oserie = $("#oserie").val();
    var onombre = $("#onombre").val();
    var odescripcion = $("#odescripcion").val();
    var ostock = $("#ostock").val();
    var oprecio = $("#oprecio").val();
    var ototal = $("#ototal").val();
    var obonificacion = ($('#ochkbonificacion').is(":checked") ? '1' : '0');
    var oid = $("#oid").val();

    if (idpresentacion == "-1" || idcategoria == "-1")
    {
        alert('Seleccione una presentacion o categoria');
        return;
    }


    $.ajax({
        url: 'Operacion.aspx?ope=602&id=' + oid + '&codigo=' + ocodigo + '&serie=' + oserie + '&nombre=' + onombre + '&descripcion=' + odescripcion + '&idpresentacion=' + idpresentacion + '&idcategoria=' + idcategoria + '&stock=' + ostock + '&precio=' + oprecio + '&total=' + ototal + '&bonificacion=' + obonificacion,
        async: true,
        beforeSend: function () {
            BlockBegin('Updating ...');
        },
        success: function (data) {

            loaddataProducto();
        },
        complete: function () {
            BlockEnd();
        }
    });

    $("#dialog-Producto").dialog("close");
}

function omodificar(id, codigo, serie, nombre, descripcion, idpresentacion, idcategoria, stock, precio, total, bonificacion) {
    $("#ocodigo").val(codigo);
    $("#oserie").val(serie);
    $("#onombre").val(nombre);
    $("#odescripcion").val(descripcion);

    $("#cbopresentacion option[value=" + idpresentacion + "]").prop("selected", true);
    $("#cbocategoria option[value=" + idcategoria + "]").prop("selected", true);

    $('#ochkserie').prop("checked", ((serie == '') ? false : true));
    $('#ochkbonificacion').prop("checked", ((bonificacion == '1') ? true:false));
   
    $("#ostock").val(stock);
    $("#oprecio").val(precio);
    $("#ototal").val(total);
    $("#oid").val(id);

    $("#dialog-Producto").dialog("open");
}

function oeliminar(id) {

    if (confirm('Esta seguro de eliminar el registro?')) {
        $.ajax({
            url: 'Operacion.aspx?ope=603&id=' + id,
            async: true,
            beforeSend: function () {
                BlockBegin('Updating ...');
            },
            success: function (data) {

                loaddataProducto();
            },
            complete: function () {
                BlockEnd();
            }
        });
    }
}

function onuevo() {
    $("#oid").val('0');
    $("#ocodigo").val('');
    $("#onombre").val('');
    $("#odescripcion").val('');
    $("#cbopresentacion option[value=-1]").prop("selected", true);
    $("#cbocategoria option[value=-1]").prop("selected", true);
    $("#ostock").val('1');
    $("#oprecio").val('0.00');
    $("#ototal").val('0.00');
    $("#ochkserie").prop("checked", false);
    $("#ochkbonificacion").prop("checked", false);
    $("#dialog-Producto").dialog("open");
}

function plantilladata() {
    window.location.href = "Template/producto.xlsx";
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


