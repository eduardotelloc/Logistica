var temporizador = 20;//expresado en segundos

function LoadData(url) {

    var datos;

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            datos = $.parseJSON(data);
        }
    });

    return datos;

}

function LoadDataText(url) {

    var datos;

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            datos = data;
        }
    });

    return datos;

}

function LoadComboNormal(url, combo) {


    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            var datos = $.parseJSON(data);

            $('#' + combo).empty();



            var entro = 0;

            $.each(datos, function (index, val) {
                $('#' + combo)
                        .append($('<option>', { value: val.id, selected: ((entro == 0) ? true : false) })
                        .text(val.name));

                entro = 1;
            });
        }
    });


}

function LoadComboSeleccionar(datos, combo) {

    $('#' + combo).empty();
    $('#' + combo)
                .append($('<option>', { value: 0 })
                .text('[All]'));

    var entro = 0;

    $.each(datos, function (index, val) {
        $('#' + combo)
                .append($('<option>', { value: val.id, selected: ((entro == 0) ? true : false) })
                .text(val.name));

        entro = 1;
    });
}

function LoadCombo_Datos_Seleccionar(url, combo) {

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            datos = $.parseJSON(data);

            $('#' + combo).empty();

            if (datos.length > 0) {
                $('#' + combo)
                            .append($('<option>', { value: 0 })
                            .text('[All]'));

                var entro = 0;

                $.each(datos, function (index, val) {


                    $('#' + combo)
                            .append($('<option>', { value: val.id, selected: ((entro == 0) ? true : false) })
                            //.append($('<option>', { value: val.id })
                            .text(val.name));

                    entro = 1;
                });
            }
            else {
                $('#' + combo)
                            .append($('<option>', { value: -1 })
                            .text('[Nothing]'));
            }

        }
    });
}

function LoadSelectText(url, cbo) {

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            var datos = $.parseJSON(data);
            var combo = $('#' + cbo);

            combo.empty().append('<option selected="selected" value="-1">[Select]</option>');
            //combo.append($('<option>', { value: -1 }).text('[Seleccionar]'));

            $.each(datos, function (i, val) {
                combo.append($('<option>', { value: 0 }).text(val.nombre));
            });
        }
    });
}

function LoadSelectItem(url, cbo) {
    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            var rangos = $.parseJSON(data);

            var combo = $('#' + cbo);
            combo.empty().append('<option selected="selected" value="-1">[Select]</option>');

            $.each(rangos, function (i, val) {
                combo.append($('<option>', { value: val.id }).text(val.nombre));
            });
        }
    });
}

function LoadAutocomplete(url, input) {

    var textbox = $('#' + input);

    if (textbox.hasClass('ui-autocomplete-input')) {
        console.log('ui-autocomplete destroy');
        textbox.autocomplete("destroy");
    }

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            var rangos = $.parseJSON(data);
            var availableTags = [];



            $.each(rangos, function (i, val) {
                availableTags.push(val.nombre);
            });

            textbox.autocomplete({
                minLength: 4,
                source: availableTags
            });
        }
    });
}

function LoadAutocompleteData(url) {

    var availableTags = [];

    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            var rangos = $.parseJSON(data);


            $.each(rangos, function (i, val) {
                availableTags.push(val.nombre);
            });


        }
    });

    return availableTags;
}

function LoadAutocompleteAsign(availableTags, input) {

    var textbox = $('#' + input);
    textbox.autocomplete({
        source: availableTags,
        minLength: 4
    });
}

function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';
    //Set Report title in first row or line

    //CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "Dispositivos_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function BlockBegin(texto) {

    $.blockUI({
        theme: false,
        message: "<div class='row'><div class='col-md-12'><img src='Img/loading.gif' class='img-thumbnail'/></div></div>",
        css: {
            border: 'none',
            backgroundColor: '#fff',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px'
        }
    });
}

function BlockEnd() {
    $.unblockUI({ timeout: 100 });
}

function getDateTime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    if (month.toString().length == 1) {
        var month = '0' + month;
    }
    if (day.toString().length == 1) {
        var day = '0' + day;
    }
    if (hour.toString().length == 1) {
        var hour = '0' + hour;
    }
    if (minute.toString().length == 1) {
        var minute = '0' + minute;
    }
    if (second.toString().length == 1) {
        var second = '0' + second;
    }
    var dateTime = year + '/' + month + '/' + day + ' ' + hour + ':' + minute + ':' + second;
    return dateTime;
}

function rucValido(ruc) {
   
    //11 dígitos y empieza en 10,15,16,17 o 20
    if (!(ruc >= 1e10 && ruc < 11e9
       || ruc >= 15e9 && ruc < 18e9
       || ruc >= 2e10 && ruc < 21e9))
        return false;
    else
        return true;

    //for (var suma = -(ruc % 10 < 2), i = 0; i < 11; i++, ruc = ruc / 10 | 0)
    //    suma += (ruc % 10) * (i % 7 + (i / 7 | 0) + 1);
    //return suma % 11 === 0;

}

String.prototype.replaceAll = function (str1, str2, ignore) {
    return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
} 


