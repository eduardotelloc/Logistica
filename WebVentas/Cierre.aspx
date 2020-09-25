<%@ Page Title="Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cierre.aspx.cs" Inherits="WebVentas.Cierre" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.cierre-1.0.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.3.0.js"></script>
   <%-- <script src="Scripts/jquery.hotkeys.js"></script>--%>
    <script src="Scripts/jquery.tabletoCSV.js"></script>
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <link type="text/css" href="DataTables/datatables.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="DataTables/datatables.min.js"></script>

    <style type="text/css">
      

        .modal .modal-dialog { width: 95%; }

        /*span { color:#f00}*/
        
        table {border-collapse: collapse; font-size:11.5px;}

        table td, table th {
            border: 1px solid #545454;
            vertical-align:middle;
            font-size: 13px;
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

    <br />

    <div class="row">
        <div class="col-md-6">
            <span class="label1">Cierre Caja</span> <span class="label2">/ Búsqueda</span>
        </div>
        <div class="col-md-6" style="text-align:right">
            <%--<img src="Img/Empresa/<% =Session["empresaid"].ToString() %>_corto.png" class="img-responsive" style="text-align:left;float: right"/>--%>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
           <br />
        </div>
    </div>

   
    <div class="row">
       
        <div class="col-md-8">
        </div>

        <div class="col-md-1">
            <div class="row"><b>Fecha Ini</b></div>
            <div class="row"><input id="fechainicio" maxlength="8" class="form-control input-normal" style="width:100px" readonly="true"/></div>
        </div>
        
        <div class="col-md-1">
            <div class="row"><b>Fecha Fin</b></div>
            <div class="row"><input id="fechafin" maxlength="8" class="form-control input-normal" style="width:100px" readonly="true" /></div>
        </div>
                
        <div class="col-md-1">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <input type="button" class="btn btn-danger" onclick="loaddata();" value="Buscar"/>
            </div>
        </div>

         <div class="col-md-1" style="text-align:center">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <input type="button" class="btn btn-danger" onclick="nuevo();" value="Nuevo"/>
            </div>
        </div>
       
    </div>
       
    <div class="row">
       <div class="col-md-12">
            <br />
        </div>
    </div>

    <div class="row">
       <div class="col-md-12" style="text-align:left">
            <b><p id="registros"></p></b>
        </div>
    </div>

    <div class="container">
        <table id="tbldetalle" class="table"></table>
    </div>

  
     <div id="dCierre" class="dialog" title="Cierre">
        <table border="0">
            <tr>
                <td>Tipo Doc</td>
                <td><select  name="cbotipo_documento_cliente" id="cbotipo_documento_cliente">
                    <option value="RUC">RUC</option>
                    <option value="DNI">DNI</option>
                    </select></td>
            </tr>
            <tr>
                <td>Numero</td>
                <td><input type="text" name="num_documento" id="num_documento" maxlength="11" class="text ui-widget-content ui-corner-all" style="width:100px">
                    <br />
                    <b><span id="sestado"></span></b>
                </td>
            </tr>
            <tr>
                <td>Nombre</td>
                <td><textarea name="nombre" id="nombre" maxlength="100" class="ui-widget-content ui-corner-all" style="resize:none"></textarea></td>
            </tr>
           
            <tr>
                <td>Sexo</td>
                <td><select  name="cbosexo" id="cbosexo">
                    <option value="M">M</option>
                    <option value="F">F</option>
                    </select></td>
            </tr>
            <tr>
                <td>Fecha Nac</td>
                <td><input name="fecha_nacimiento" id="fecha_nacimiento" style="width:80px" readonly="true" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Departamanto</td>
                <td><input type="text" name="departamento" id="departamento" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
             <tr>
                <td>Provincia</td>
                <td><input type="text" name="provincia" id="provincia" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
             <tr>
                <td>Distrito</td>
                <td><input type="text" name="distrito" id="distrito" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Direccion</td>
                <td><textarea name="direccion" id="direccion" maxlength="200" class="ui-widget-content ui-corner-all" style="resize:none"></textarea></td>
            </tr>
            <tr>
                <td>Ubigeo Inei</td>
                <td><input type="text" name="ubigeo" id="ubigeo" maxlength="6" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Telefono</td>
                <td><input type="text" name="telefono" id="telefono" maxlength="12"  class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><input type="email" name="email" id="email" maxlength="50" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr><td colspan="2"><br /></td></tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    <input type="hidden" name="cid" id="cid"/>
                    <input type="button" class="btn btn-danger" onclick="grabarcliente();" value="Agregar"/>
                </td>
            </tr>
        </table>
    </div>   

    <div id="dEgreso" class="dialog" title="Egreso">
        <table border="0">
            <tr>
                <td>Tipo Doc</td>
                <td><select  name="cbotipo_documento_cliente" id="cbotipo_documento_cliente">
                    <option value="RUC">RUC</option>
                    <option value="DNI">DNI</option>
                    </select></td>
            </tr>
            <tr>
                <td>Numero</td>
                <td><input type="text" name="num_documento" id="num_documento" maxlength="11" class="text ui-widget-content ui-corner-all" style="width:100px">
                    <br />
                    <b><span id="sestado"></span></b>
                </td>
            </tr>
            <tr>
                <td>Nombre</td>
                <td><textarea name="nombre" id="nombre" maxlength="100" class="ui-widget-content ui-corner-all" style="resize:none"></textarea></td>
            </tr>
           
            <tr>
                <td>Sexo</td>
                <td><select  name="cbosexo" id="cbosexo">
                    <option value="M">M</option>
                    <option value="F">F</option>
                    </select></td>
            </tr>
            <tr>
                <td>Fecha Nac</td>
                <td><input name="fecha_nacimiento" id="fecha_nacimiento" style="width:80px" readonly="true" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Departamanto</td>
                <td><input type="text" name="departamento" id="departamento" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
             <tr>
                <td>Provincia</td>
                <td><input type="text" name="provincia" id="provincia" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
             <tr>
                <td>Distrito</td>
                <td><input type="text" name="distrito" id="distrito" maxlength="100" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Direccion</td>
                <td><textarea name="direccion" id="direccion" maxlength="200" class="ui-widget-content ui-corner-all" style="resize:none"></textarea></td>
            </tr>
            <tr>
                <td>Ubigeo Inei</td>
                <td><input type="text" name="ubigeo" id="ubigeo" maxlength="6" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Telefono</td>
                <td><input type="text" name="telefono" id="telefono" maxlength="12"  class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><input type="email" name="email" id="email" maxlength="50" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr><td colspan="2"><br /></td></tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    <input type="hidden" name="cid" id="cid"/>
                    <input type="button" class="btn btn-danger" onclick="grabarcliente();" value="Agregar"/>
                </td>
            </tr>
        </table>
    </div>   

   
 
</asp:Content>
