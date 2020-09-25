<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentosListado.aspx.cs" Inherits="WebVentas.DocumentosListado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
	<title>Documento Listado</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.documentoslistado-1.0.0.js"></script>
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

        <input type="hidden" name="hid" id="hid"/>
        <input type="hidden" name="hdocumento" id="hdocumento"/>

        <div id="ribbon">
		    <span class="ribbon-window-title">Mantenimieto de Documentos</span>
		    

		    <div class="ribbon-tab" id="operacion-tab">
			    <span class="ribbon-title">Operación</span>
			    
			    <div class="ribbon-section">
				    <div class="ribbon-button ribbon-button-large disabled" id="add-page-btn">   
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
					    <img class="ribbon-icon ribbon-normal" src="icons/normal/return.png" />
				    </div>
			    </div>

		    </div>
		    <div class="ribbon-tab" id="busqueda-tab">
			    <span class="ribbon-title">Busqueda</span>
			    <div class="ribbon-section">
				    <div class="ribbon-button ribbon-button-large">
                         <div class="row">
                            <div class="col-sm-2">
                                <div class="row"><b>Tipo</b></div>
                                <div class="row">
					                <select id="cbotipo" class="form-control form-control-inline" style="width:160px">
                                        <option value="00">[TODOS]</option>
                                        <option value="01">FACTURA</option>
                                        <option value="03">BOLETA</option>
                                        <option value="07">NOTA DE CREDITO</option>
                                        <option value="08">NOTA DE DEBITO</option>
                                        <option value="09">GUIA</option>
                                        <option value="20">RETENCION</option>
                                        <option value="40">PERCEPCION</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <div class="row"><b>Serie</b></div>
                                <div class="row">
                                    <input id="serie" maxlength="4" class="form-control input-normal" style="width:100px" />
                                </div>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1">
                                <div class="row"><b>Correlativo</b></div>
                                <div class="row">
                                    <input id="correlativo" maxlength="8" class="form-control input-normal" style="width:80px" onkeypress="return isNumberKey(event);" />
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="row"><b>Estado</b></div>
                                <div class="row">
                                    <select id="cboestado" class="form-control form-control-inline" style="width:100px">
                                        <option value="00">[TODOS]</option>
                                        <option value="ACT">ACT</option>
                                        <option value="ANU">ANU</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <div class="row"><b>SUNAT</b></div>
                                <div class="row">
                                    <select id="cbosunat" class="form-control form-control-inline" style="width:100px">
                                        <option value="00">[TODOS]</option>
                                        <option value="ACEPTADO">ACEPTADO</option>
                                        <option value="NO ACEPTADO">NO ACEPTADO</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1">
                                <div class="row"><b>Inicio</b></div>
                                <div class="row">
                                    <input id="fechainicio" maxlength="8" class="form-control input-normal" style="width:100px" readonly="true"/>
                                </div>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-1">
                                <div class="row"><b>Fin</b></div>
                                <div class="row">
                                    <input id="fechafin" maxlength="8" class="form-control input-normal" style="width:100px" readonly="true" />
                                </div>
                            </div>
                        </div>
				    </div>
                </div>
                <div class="ribbon-section">
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

    </form>
</body>
</html>

