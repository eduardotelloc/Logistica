<%@ Page Title="Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="WebVentas.Pedidos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.pedidos-1.0.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.3.0.js"></script>
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <link type="text/css" href="DataTables/datatables.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="DataTables/datatables.min.js"></script>

    <style type="text/css">
      

        table {border-collapse: collapse; font-size:11.5px;}

        table td, table th {
            border: 1px solid #545454;
            vertical-align:middle;
            font-size: 18px;
            font-weight: bold;
            font-family:Calibri ;
        }

        .table > thead > tr > th {border: 1px solid #D11925;}
        
        
        
        table th {
	        color: #FFF;
	        text-align: center;
	        background-color: #ff002e;
        }

        table td {
	        color: #000;
            height:22px;
            padding-left: 2px;
            padding-right: 2px;
        }

        .text {
            height:22px;
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
         
    </style>

    <br />

    <div class="row">
        <div class="col-md-6">
            <span class="label1">Pedidos</span> <span class="label2"> - Mesa N° </span> <span class="label2" id="lblmesa"></span>
        </div>
        <div class="col-md-6" style="text-align:right">
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
           <br />
        </div>
    </div>

   

    <table border="0" style="width:80%">
        <tr>
            <td>Trabajador</td>
            <td><input type="text" name="idtrabajador" id="idtrabajador" style="width:90px" value="<%:this.ViewState["usuarioanum_documento"].ToString()%>"  maxlength="10" class="text ui-widget-content ui-corner-all"></td>
            <td><input type="text" name="trabajador" id="trabajador" style="width:200px" maxlength="50" class="text ui-widget-content ui-corner-all"></td>
            <td>T.C</td>
            <td>
                <input type="hidden" name="higv" id="higv"/>
                <input type="text" name="cambio_venta" id="cambio_venta" readonly="true" style="width:80px" maxlength="10" class="text ui-widget-content ui-corner-all">
            </td>
            
            <td>Fecha</td>
            <td><input name="fecha" id="fecha" style="width:100px" readonly="true" maxlength="10" class="text ui-widget-content ui-corner-all"></td>
        </tr>
           
        <tr>
            <td colspan="7">
                <table id="tbldocumento" class="table">
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4"> </td>
            <td colspan="1" style="text-align:center">
                <input type="button" name="btnfacturar" id="btnfacturar" class="btn btn-danger" onclick="facturar();" value="Facturar"/>
            </td>
            <td colspan="1" style="text-align:center">
                <input type="button" name="btneliminar" id="btneliminar" class="btn btn-danger" onclick="eliminar();" value="Eliminar"/>
            </td>
            <td colspan="1" style="text-align:center">
                <input type="hidden" name="id" id="id"/>
                <input type="button" name="btngrabar" id="btngrabar" class="btn btn-danger" onclick="grabar();" value="Grabar"/>
            </td>
        </tr>
    </table>
   

    
 
</asp:Content>
