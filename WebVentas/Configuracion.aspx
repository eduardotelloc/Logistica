<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="WebVentas.Configuracion" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.Configuracion-1.3.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.2.0.js"></script>
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
      

        .modal .modal-dialog { width: 95%; }

        /*span { color:#f00}*/
        
        table {border-collapse: collapse; font-size:11.5px;}

        table td, table th {
            border: 1px solid #545454;
            vertical-align:middle;
            font-size: 14px;
            font-family:Calibri ;
        }

        .table > thead > tr > th {border: 1px solid #545454;}
        
        table th {
	        color: #000;
	        text-align: center;
	        background-color: #fbe607;
        }

        table td {
	        color: #000;
            height:24px;
            padding-left: 5px;
            padding-right: 5px;
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
            color:  #ff002e;
            font-size: 28px;/* Aproximación debida a la sustitución de la fuente */
            font-weight: 400;
            line-height: 28px;/* Aproximación debida a la sustitución de la fuente */
            text-align: left;
        }

        .label2 {          
            color:  #000000;
            font-size: 28px;/* Aproximación debida a la sustitución de la fuente */
            font-weight: 400;
            line-height: 28px;/* Aproximación debida a la sustitución de la fuente */
            text-align: left;
        }    
        
        #dialog { display:none;}
    </style>

    <div class="row">
        <div class="col-md-10" style="text-align:center">
            <div class="row">
                <br />
            </div>
        </div>
        <div class="col-md-2" style="text-align:right">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <a href="#" onclick="window.location.href='Menu.aspx'" title="Volver al Menu"><img src="Img/back.png"/></a>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12" style="text-align:center">
            <div class="row">
                <br />
            </div>
        </div>
    </div>

    <div id="tabs">
      <ul>
        <li><a href="#tabs-Serie">Serie</a></li>
        <li><a href="#tabs-TipoCambio">Tipo de Cambio</a></li>
        <li><a href="#tabs-Variables">Variables</a></li>
        
      </ul>
      <div id="tabs-Serie">

        <div class="row">
            <div class="col-md-12">
                <br />
            </div>
        </div>

       
    
        <div class="row" style="width:50%">
            <div class="col-md-10" style="text-align:left">
                <b><p id="sregistros"></p></b>
            </div>
            <div class="col-md-2" style="text-align:right">
                <div class="row">
                    <input type="button" class="btn btn-danger" onclick="snuevo();" value="Nuevo"/>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <br />
            </div>
        </div>

        <div class="table-responsive">
            <table id="tblSerie" class="table" style="width:50%"></table>
        </div>    

        <div id="dialog-Serie" title="Serie">
            <table border="0">
                <tr>
                    <td>
                        Tipo
                    </td>
                    <td>
                        <select  name="cbotipo" id="cbotipo">
                            <option value="01">Factura</option>
                            <option value="03">Boleta</option>
                            <option value="07">Nota Credito</option>
                            <option value="08">Nota Debito</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Indicador
                    </td>
                    <td>
                        <select  name="cboindicador" id="cboindicador">
                        <option value="F">F (Electronico)</option>
                        <option value="B">B (Electronico)</option>
                        <option value="0">Manual</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Serie
                    </td>
                    <td>
                        <input type="number" name="serie" id="serie" value="001" class="text ui-widget-content ui-corner-all" min="001" max="999" step="001">
                    </td>
                </tr>
                <tr>
                    <td>
                        Correlativo
                    </td>
                    <td>
                        <input type="number" name="correlativo" id="correlativo" value="0" class="text ui-widget-content ui-corner-all" min="0" max="999999" step="1">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right">
                        <input type="hidden" name="sid" id="sid"/>
                        <input type="button" class="btn btn-danger" onclick="sgrabar();" value="Agregar"/>
                    </td>
                </tr>
            </table>
        </div>
       </div>


      <div id="tabs-TipoCambio">
    
        <div class="row" style="width:50%">
            <div class="col-md-10" style="text-align:left">
                <b><p id="tregistros"></p></b>
            </div>
            <div class="col-md-2" style="text-align:right">
                <div class="row">
                    <input type="button" class="btn btn-danger" onclick="tnuevo();" value="Nuevo"/>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <br />
            </div>
        </div>

        <div class="table-responsive">
            <table id="tblTipoCambio" class="table" style="width:50%"></table>
        </div>    

        <div id="dialog-TipoCambio" title="Tipo Cambio">
            <table border="0">
                <tr>
                    <td>
                        Fecha
                    </td>
                    <td><input name="fecha_tipo_cambio" id="fecha_tipo_cambio" maxlength="10" class="form-control input-normal"></td>
                </tr>
                <tr>
                    <td>
                        Monto de Venta
                    </td>
                    <td>
                        <input type="number" name="cambio_venta" id="cambio_venta" value="0" class="text ui-widget-content ui-corner-all" min="0.001" max="10000.000" step="0.001">
                    </td>
                </tr>
                <tr>
                    <td>
                        Monto de Compra
                    </td>
                    <td>
                        <input type="number" name="cambio_compra" id="cambio_compra" value="0" class="text ui-widget-content ui-corner-all" min="0.001" max="10000.000" step="0.001">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right">
                        <input type="hidden" name="tid" id="tid"/>
                        <input type="button" class="btn btn-danger" onclick="tgrabar();" value="Agregar"/>
                    </td>
                </tr>
            </table>
        </div>
      </div>

      <div id="tabs-Variables">
       
        <div class="row">
            <div class="col-md-12">
                <br />
            </div>
        </div>
   
    
        <div class="row" style="width:50%">
            <div class="col-md-10" style="text-align:left">
                <b><p id="vregistros"></p></b>
            </div>
            <div class="col-md-2" style="text-align:right">
                <div class="row">
                    <%--<input type="button" class="btn btn-danger" onclick="vnuevo();" value="Nuevo"/>--%>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <br />
            </div>
        </div>

        <div class="table-responsive">
            <table id="tblVariable" class="table" style="width:30%"></table>
        </div>    

        <div id="dialog-Variable" title="Variables">
            <table border="0">
                
                <tr>
                    <td>
                        Nombre
                    </td>
                    <td>
                        <input type="text" name="vnombre" id="vnombre" readonly="true" class="text ui-widget-content ui-corner-all" >
                    </td>
                </tr>

                <tr>
                    <td>
                        Valor
                    </td>
                    <td>
                        <input type="number" name="vvalor" id="vvalor" value="0" class="text ui-widget-content ui-corner-all" min="0" max="100" step="1">
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right">
                        <input type="hidden" name="vid" id="vid"/>
                        <input type="button" class="btn btn-danger" onclick="vgrabar();" value="Agregar"/>
                    </td>
                </tr>
            </table>
        </div>
      </div>
    </div>
   
      
</asp:Content>