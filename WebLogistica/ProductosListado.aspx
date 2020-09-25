<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductosListado.aspx.cs" Inherits="WebVentas.ProductosListado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
	<title>Producto Listado</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.productoslistado-1.0.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.3.0.js"></script>
    <script src="Scripts/jquery.maskedinput.js"></script>

    <script src="Scripts/jquery.tabletoCSV.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <link type="text/css" href="DataTables/datatables.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="DataTables/datatables.min.js"></script>

    <style type="text/css">
        table {
            border: 1px solid #545454;
            border-collapse: collapse; 
            font-size:11.5px;
        }

        table td, table th {
            border: 1px solid #545454;
            vertical-align:middle;
            font-size: 13px;
            font-family:Calibri ;
        }

        .table > thead > tr > th {
            border: 1px solid #545454;
        }
        
        table th {
	        color: #000;
	        text-align: center;
	        background-color: #FFC312;
        }

        table td {
	        color: #000;
            height:18px;
            padding-left: 2px;
            padding-right: 2px;
        }

        .text {
            height:18px;
        }

        .totheright{ 
            text-align:right;
        }

        tr:nth-child(odd) {
            background-color:#f2f2f2;
        }
        tr:nth-child(even) {
            background-color: #FFF;
        }

        .label1 {
            
            color:  #545454;
            font-size: 28px;/* Aproximación debida a la sustitución de la fuente */
            font-weight: 400;
            line-height: 28px;/* Aproximación debida a la sustitución de la fuente */
            text-align: left;
        }

    </style>
</head>
<body>
    <form id="formdocument" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 

        <div class="row">
            <div class="col-md-12">
               <br />
            </div>
        </div>

	    <div id="tabs">
          <ul>
            <li><a href="#tabs-Producto">Producto</a></li>
            <li><a href="#tabs-Categoria">Categoria</a></li>
            <li><a href="#tabs-Presentacion">Presentacion</a></li>
            <li><a href="#tabs-Masivo">Carga Masiva</a></li>
          </ul>
      
          <div id="tabs-Producto">
        
   
            <div class="row">
                <div class="col-md-8">
                    <div class="row"><b>Nombre</b></div>
                    <div class="row"><input id="producto" maxlength="100" class="form-control input-normal" style="width:400px" /></div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="loaddataProducto();" value="Buscar" />
                    </div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="onuevo();" value="Nuevo" />
                    </div>
                </div>

                <div class="col-md-1">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <a href="#" onclick="exportdata();"><img src="Img/excel_32.png" title="Exportar" /></a>
                    </div>
                </div>
                <div class="col-md-1" style="text-align:right">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <a href="#" onclick="window.location.href='Menu.aspx'" title="Volver al Menu"><img src="Img/back.png"/></a>
                    </div>
                </div>
            </div>
       
            <div class="row">
               <div class="col-md-12">
                    <br />
                </div>
            </div>

            <div class="row" style="width:90%">
               <div class="col-md-12" style="text-align:left">
                    <b><p id="registrosProducto"></p></b>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tblProducto" class="table"></table>
            </div>    

            <div id="dialog-Producto" title="Producto">
                <table border="0">
                    <tr>
                        <td>
                            Presentacion
                        </td>
                        <td>
                            <select id="cbopresentacion" class="form-control form-control-inline" style="width:160px"></select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Categoria
                        </td>
                        <td>
                            <select id="cbocategoria" class="form-control form-control-inline" style="width:160px"></select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Serie
                        </td>
                        <td>
                            <input type="text" name="oserie" id="oserie" value="" readonly="readonly" class="text ui-widget-content ui-corner-all" style="width:40px">
                            <input type="checkbox" id="ochkserie" name="ochkserie" /><label>Habilitar</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Codigo
                        </td>
                        <td>
                            <input type="text" name="ocodigo" id="ocodigo" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nombre
                        </td>
                        <td>
                            <input type="text" name="onombre" id="onombre" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Descripcion
                        </td>
                        <td>
                            <input type="text" name="odescripcion" id="odescripcion" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Stock
                        </td>
                        <td>
                            <input type="number" name="ostock" id="ostock" value="1" class="text ui-widget-content ui-corner-all" min="1" max="1000000" step="1">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Precio Unit
                        </td>
                        <td>
                            <input type="number" name="oprecio" id="oprecio" value="0" class="text ui-widget-content ui-corner-all" min="0.00" max="10000.00" step="0.01">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Precio Total
                        </td>
                        <td>
                            <input type="number" name="ototal" id="ototal" value="0" class="text ui-widget-content ui-corner-all" min="0.00" max="10000.00" step="0.01">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bonificacion
                        </td>
                        <td>
                             <input type="checkbox" id="ochkbonificacion" name="ochkbonificacion" />
                        </td>
                    </tr>
               
                     <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:right">
                            <input type="hidden" name="oidserie" id="oidserie" />
                            <input type="hidden" name="oid" id="oid" />
                            <input type="button" class="btn btn-danger" onclick="ograbar();" value="Grabar" />
                        </td>
                    </tr>
                </table>
            </div>

          </div>

          <div id="tabs-Categoria">
       
      
            <div class="row">
                <div class="col-md-9">
                    <div class="row"><b>Nombre</b></div>
                    <div class="row"><input id="categoria" maxlength="100" class="form-control input-normal" style="width:400px" /></div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="loaddataCategoria();" value="Buscar" />
                    </div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="cnuevo();" value="Nuevo" />
                    </div>
                </div>
                <div class="col-md-1" style="text-align:right">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <a href="#" onclick="window.location.href='Menu.aspx'" title="Volver al Menu"><img src="Img/back.png"/></a>
                    </div>
                </div>
            </div>
       
            <div class="row">
               <div class="col-md-12">
                    <br />
                </div>
            </div>

            <div class="row" style="width:60%">
               <div class="col-md-12" style="text-align:left">
                    <b><p id="registrosCategoria"></p></b>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tblCategoria" class="table" style="width:60%"></table>
            </div>    

            <div id="dialog-Categoria" title="Categoria">
                <table border="0">
                    <tr>
                        <td>
                            Nombre
                        </td>
                        <td>
                            <input type="text" name="cnombre" id="cnombre" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Descripcion
                        </td>
                        <td>
                            <input type="text" name="cdescripcion" id="cdescripcion" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:right">
                            <input type="hidden" name="cid" id="cid"/>
                            <input type="button" class="btn btn-danger" onclick="cgrabar();" value="Agregar" />
                        </td>
                    </tr>
                </table>
            </div>
          </div>

          <div id="tabs-Presentacion">
            <div class="row">
                <div class="col-md-9">
                    <div class="row"><b>Nombre</b></div>
                    <div class="row"><input id="presentacion" maxlength="100" class="form-control input-normal" style="width:400px" /></div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="loaddataPresentacion();" value="Buscar" />
                    </div>
                </div>

                <div class="col-md-1" style="text-align:center">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="pnuevo();" value="Nuevo" />
                    </div>
                </div>
                <div class="col-md-1" style="text-align:right">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <a href="#" onclick="window.location.href='Menu.aspx'" title="Volver al Menu"><img src="Img/back.png"/></a>
                    </div>
                </div>
            </div>
       
            <div class="row">
               <div class="col-md-12">
                    <br />
                </div>
            </div>

            <div class="row" style="width:60%">
               <div class="col-md-12" style="text-align:left">
                    <b><p id="registrosPresentacion"></p></b>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tblPresentacion" class="table" style="width:60%"></table>
            </div>    

            <div id="dialog-Presentacion" title="Presentacion">
                <table border="0">
                    <tr>
                        <td>
                            Nombre
                        </td>
                        <td>
                            <input type="text" name="pnombre" id="pnombre" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Descripcion
                        </td>
                        <td>
                            <input type="text" name="pdescripcion" id="pdescripcion" value="" class="text ui-widget-content ui-corner-all">
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:right">
                            <input type="hidden" name="pid" id="pid"/>
                            <input type="button" class="btn btn-danger" onclick="pgrabar();" value="Agregar" />
                        </td>
                    </tr>
                </table>
            </div>

         </div>

          <div id="tabs-Masivo">

            <table id="tblupdate" class="table" style="width:40%">
                <tr>
                    <td style="width:80%">
                        <%--<input type="file" name="filename" id="filename">--%>
                        <textarea rows="1" id="excelPasteBox" placeholder="Pegar datos de excel aqui..." style="width:100%"></textarea>
                    </td>
                    <td>
                        <input type="button" class="btn btn-danger" onclick="mgrabar();" value="Agregar" />  
                    </td>
                    <td style="text-align:center">
                        <a href="#" onclick="plantilladata();" title="Plantilla"><img src="Img/excel_32.png" /></a>
                    </td>
                </tr>
            </table>

            <div id="output">
              <table id="excelDataTable"></table>
            </div>

        </div>

        </div>

        
    </form>
</body>
</html>

