<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpleadosListado.aspx.cs" Inherits="WebVentas.EmpleadosListado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
	<title>Empleado Listado</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.empleadoslistado-1.0.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.3.0.js"></script>
    <script src="Scripts/jquery.maskedinput.js"></script>

    <script src="Scripts/jquery.tabletoCSV.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <link type="text/css" href="DataTables/datatables.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="DataTables/datatables.min.js"></script>

    <link href="ribbon/ribbon-dark.css" rel="stylesheet" type="text/css" />
	<link href="soft_button.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="ribbon/ribbon.js"></script>
    <script src="Scripts/jquery.table2excel.min.js"></script>

    <link href="Content/table.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="~/favicon.ico" />
</head>
<body>
    <form id="formdocument" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 

        <div id="ribbon">
		    <span class="ribbon-window-title">Mantenimieto de Empleados</span>
		    

		    <div class="ribbon-tab" id="operacion-tab">
			    <span class="ribbon-title">Operación</span>
			    
			    <div class="ribbon-section">
				    <div class="ribbon-button ribbon-button-large" id="add-page-btn">   
					    <span class="button-title">Nuevo</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/new-page.png" />
					    <img class="ribbon-icon ribbon-hot" src="icons/hot/new-page.png" />
					    <img class="ribbon-icon ribbon-disabled" src="icons/disabled/new-page.png" />
				    </div>
				    <div class="ribbon-button ribbon-button-large disabled" id="open-page-btn">
					    <span class="button-title">Editar</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/open-page.png" />
					    <img class="ribbon-icon ribbon-hot" src="icons/hot/open-page.png" />
					    <img class="ribbon-icon ribbon-disabled" src="icons/disabled/open-page.png" />
				    </div>
				    <div class="ribbon-button ribbon-button-large disabled" id="del-page-btn">
					    <span class="button-title">Eliminar</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/delete-page.png" />
					    <img class="ribbon-icon ribbon-hot" src="icons/hot/delete-page.png" />
					    <img class="ribbon-icon ribbon-disabled" src="icons/disabled/delete-page.png" />
				    </div>
                    <div class="ribbon-button ribbon-button-large disabled" id="exportar-page-btn">
                        <span class="button-title">Save</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/excel.png" alt="Save" />
					    <img class="ribbon-icon ribbon-disabled" src="icons/normal/excel.png" alt="Save" />
				    </div>
			    </div>
			
			
			    <div class="ribbon-section">
				    <div class="ribbon-button ribbon-button-large" id="retornar-btn">
					    <span class="button-title">Retornar</span>
					    <span class="button-help">This button will repeat something.</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/return.png" />
				    </div>
			    </div>

		    </div>
		    <div class="ribbon-tab" id="busqueda-tab">
			    <span class="ribbon-title">Busqueda</span>
			    <div class="ribbon-section">
				    <div class="ribbon-button ribbon-button-large">
					    <span class="button-title">Nombre o DNI</span>
					    <input id="empleado" maxlength="100" class="form-control input-normal" style="width:200px"/>
				    </div>
				    <div class="ribbon-button ribbon-button-large" id="search-btn">
					    <span class="button-title">Buscar</span>
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/search.png" />
				    </div>
                </div>
                <div class="ribbon-section">
                    <div class="ribbon-button ribbon-button-large" id="resultado-btn">
                        <br />
                        <b><p style="color:black;font-size:small" id="registros"></p></b>
                    </div>
			    </div>
		    </div>
	    </div>
   
        <div class="table-responsive">
            <table id="tbldetalle" class="table"></table>
        </div>    

        <div id="dialog" title="Empleado" >
            <div class="container">
                <div class="row">
                    <div class="col-lg-2">
                        <b>Nombres</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="nombre" id="nombre" maxlength="40" class="form-control" required="required"/>
                    </div>
                
                    <div class="col-lg-2">
                        <b>Apellidos</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="apellidos" id="apellidos" maxlength="40" class="form-control" required="required"/>
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Sexo</b>
                    </div>
                    <div class="col-lg-4">
                        <select  name="cbosexo" id="cbosexo" class="form-control">
                            <option value="M">M</option>
                            <option value="F">F</option>
                        </select>
                    </div>
                    <div class="col-lg-2">
                        <b>Fec Nac</b>
                    </div>
                    <div class="col-lg-4">
                        <input name="fecha_nacimiento" id="fecha_nacimiento" maxlength="10" class="form-control input-normal" style="width:100px"/>
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Tipo</b>
                    </div>
                    <div class="col-lg-4">
                        <select  name="cbotipo_documento" id="cbotipo_documento" class="form-control">
                            <option value="DNI">DNI</option>
                        </select>
                    </div>
                    <div class="col-lg-2">
                        <b>Numero</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="numerodocumento" id="numerodocumento" maxlength="8" class="form-control" onkeypress="return isNumberKey(event);"/>
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2"><b>Departamento</b></div>
                    <div class="col-lg-4">
                        <select id="cbodepartamento" name="cbodepartamento" class="form-control"></select>
                    </div>
                    <div class="col-lg-2"><b>Provincia</b></div>
                    <div class="col-lg-4">
                        <select id="cboprovincia" name="cboprovincia" class="form-control"></select>
                    </div>
                 </div>
                 <div class="row">
                    <br />
                 </div>
                 <div class="row">
                    <div class="col-lg-2"><b>Distrito</b></div>
                    <div class="col-lg-4">
                        <select id="cbodistrito" name="cbodistrito" class="form-control"></select>
                    </div>
                    <div class="col-lg-6"></div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Dirección</b>
                    </div>
                    <div class="col-lg-10">
                        <textarea name="direccion" id="direccion" maxlength="200" class="form-control" style="resize:none" rows="2"></textarea>
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Telefono</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="telefono" id="telefono" maxlength="12"  class="form-control" />
                    </div>
                    <div class="col-lg-2">
                        <b>Email</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="email" name="email" id="email" maxlength="50" class="form-control" />
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Acceso</b>
                    </div>
                    <div class="col-lg-4">
                        <select  name="cboacceso" id="cboacceso" class="form-control">
                            <option value="Administrador">Administrador</option>
                            <option value="Vendedor">Vendedor</option>
                            <option value="Cajero">Cajero</option>
                            <option value="Mesero">Mesero</option>
                        </select>
                    </div>
                    <div class="col-lg-2">
                        <b>(Cajero)</b>
                    </div>
                    <div class="col-lg-4">
                        <select  name="cbopuntos" id="cbopuntos" class="form-control"></select>
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Usuario</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="usuario" id="usuario" maxlength="15" class="form-control" required="required" />
                    </div>
                    <div class="col-lg-2">
                        <b>Contraseña</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="password" id="password" maxlength="12" class="form-control" required="required" />
                    </div>
                </div>
                <div class="row">
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-2">
                        <b>Serie</b>
                    </div>
                    <div class="col-lg-4">
                        <input type="text" name="serie" id="serie" maxlength="3" class="form-control" onkeypress="return isNumberKey(event);"/>
                    </div>
                    <div class="col-lg-6"></div>
                </div>
                <div class="row">
                    <div class="col-lg-9">
                        <input type="hidden" name="hid" id="hid"/>
                        <input type="hidden" name="hid" id="hnombre"/>
                    </div>
                    <div class="col-lg-3">
                        <input type="button" class="btn btn-primary" onclick="grabar();" value="Grabar" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

