<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeriesListado.aspx.cs" Inherits="WebVentas.SeriesListado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
	<title>Cliente Listado</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.serieslistado-1.0.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.3.0.js"></script>
    <script src="Scripts/jquery.maskedinput.js"></script>
    <script src="Scripts/bootstrap.js"></script>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBJN04gt3w85bnsJ_381PyjvqC1RDoAtVw&sensor=false">


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
		    <span class="ribbon-window-title">Mantenimieto de Serie</span>
		    

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
					    <span class="button-title">Serie</span>
					    <input id="nombre" maxlength="100" class="form-control input-normal" style="width:200px"/>
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

        <div id="dialog" title="Serie" >
           <div class="content">
               <div class="row">
                    <br />
               </div>
              <div class="row">
                  <div class="col-lg-3">
                      <b>Tipo</b>
                  </div>
                  <div class="col-lg-3">
                     <select  name="cbotipo" id="cbotipo" class="form-control">
                        <option value="01">Factura</option>
                        <option value="03">Boleta</option>
                        <option value="07">Nota Credito</option>
                        <option value="08">Nota Debito</option>
                    </select>
                  </div>
                  <div class="col-lg-3">
                      <b>Indicador</b>
                  </div>
                  <div class="col-lg-3">
                    <select  name="cboindicador" id="cboindicador" class="form-control">
                        <option value="F">F (Electronico)</option>
                        <option value="B">B (Electronico)</option>
                        <option value="0">Manual</option>
                    </select>
                  </div>
               </div>
               <div class="row">
                    <br />
                </div>
               <div class="row">
                  <div class="col-lg-3">
                      <b>Serie</b>
                  </div>
                  <div class="col-lg-3">
                     <input type="text" name="serie" id="serie" value="001" class="form-control" onkeypress="return isNumberKey(event);"  required="required"/>
                  </div>
                  <div class="col-lg-3">
                      <b>Correlativo</b>
                  </div>
                  <div class="col-lg-3">
                    <input type="text" name="correlativo" id="correlativo" value="0" class="form-control" onkeypress="return isNumberKey(event);" required="required"/>
                  </div>
               </div>
               <div class="row">
                    <br />
                </div>
            </div>
            <div class="footer">
               <div class="row">
                    <div class="col-lg-6">
                        <input type="hidden" name="hid" id="hid"/>
                        <input type="hidden" name="hid" id="hnombre"/>
                    </div>
                     <div class="col-lg-3">
                        <input type="button" class="btn btn-export" onclick="cancelar();" value="cancelar" title="Cancelar" />
                    </div>
                    <div class="col-lg-3">
                        <input type="button" class="btn btn-primary" onclick="grabar();" value="Grabar" title="Grabar" />
                    </div>
                </div>
           </div>
        </div>
    </form>
</body>
</html>

