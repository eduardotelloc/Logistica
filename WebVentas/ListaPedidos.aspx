<%@ Page Title="Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaPedidos.aspx.cs" Inherits="WebVentas.ListaPedidos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.listapedidos-1.0.0.js"></script>
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
            <span class="label1">Documentos</span> <span class="label2">/ Búsqueda</span>
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
        <div class="col-md-2">
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
                
        <div class="col-md-1">
            <div class="row"><b>Serie</b></div>
            <div class="row"><input id="serie" maxlength="4" class="form-control input-normal" style="width:80px" /></div>
        </div>

        <div class="col-md-1">
            <div class="row"><b>Correlativo</b></div>
            <div class="row"><input id="correlativo" maxlength="8" class="form-control input-normal" style="width:100px" onkeypress="return isNumberKey(event);" /></div>
        </div>

        

        <div class="col-md-1">
            <div class="row"><b>Estado</b></div>
            <div class="row">
                <select id="cboestado" class="form-control form-control-inline" style="width:80px">
                    <option value="00">[TODOS]</option>
                    <option value="ACT">ACT</option>
                    <option value="ANU">ANU</option>
                </select>
            </div>
        </div>

        <div class="col-md-1">
            <div class="row"><b>SUNAT</b></div>
            <div class="row">
                <select id="cbosunat" class="form-control form-control-inline" style="width:100px">
                    <option value="00">[TODOS]</option>
                    <option value="ACEPTADO">ACEPTADO</option>
                    <option value="NO ACEPTADO">NO ACEPTADO</option>
                </select>
            </div>
        </div>
        
        <div class="col-md-1">
            <div class="row">
                <br />
            </div>
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

        <div class="col-md-1">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <a href="#" onclick="exportdata();"><img src="Img/excel.png" /></a>
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

    <div id="ddocumento" class="dialog" title="Documento">
        <table border="0">
            <tr>
                <td>Cliente</td>
                <td><input type="text" name="idcliente" id="idcliente" style="width:90px" maxlength="11" value="0" class="text ui-widget-content ui-corner-all" ></td>
                <td><input type="text" name="cliente" id="cliente" style="width:200px" maxlength="50" class="text ui-widget-content ui-corner-all"></td>
                <td></td>
                <td><input type="button" class="btn btn-info" onclick="nuevocliente();" value=":.Cliente:."/></td>
                <td>Fecha</td>
                <td><input name="fecha" id="fecha" style="width:80px" readonly="true" maxlength="10" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Trabajador</td>
                <td><input type="text" name="idtrabajador" id="idtrabajador" style="width:90px" value="<%:this.ViewState["usuarioanum_documento"].ToString()%>"  maxlength="10" class="text ui-widget-content ui-corner-all"></td>
                <td><input type="text" name="trabajador" id="trabajador" style="width:200px" maxlength="50" class="text ui-widget-content ui-corner-all"></td>
                <td></td>
                <td><input type="button" class="btn btn-info" onclick="nuevoproducto();" value="Producto"/></td>
                <td>T.C</td>
                <td><input type="text" name="cambio_venta" id="cambio_venta" readonly="true" style="width:80px" maxlength="10" class="text ui-widget-content ui-corner-all"></td>
            </tr>
            <tr>
                <td>Tipo Doc</td>
                <td>
                    <select  name="cbotipo_documento" id="cbotipo_documento" class="text ui-widget-content ui-corner-all">
                      <%--  <option value="03">Boleta</option>
                        <option value="01">Factura</option>
                        <option value="07">Nota Cred</option>
                        <option value="08">Nota Deb</option>--%>
                    </select>
                    <select  name="cboindicador" id="cboindicador" class="text ui-widget-content ui-corner-all">
                       <%-- <option value="B">B(Electr)</option>
                        <option value="0">0(Manual)</option>--%>
                    </select>
                </td>
                <td>
                    <select  name="cboserie" id="cboserie" class="text ui-widget-content ui-corner-all">
                    </select>

                    <input type="number" name="dcorrelativo" id="dcorrelativo" style="width:60px"  class="text ui-widget-content ui-corner-all">

                </td>
                <td>
                    <input type="hidden" name="higv" id="higv"/>
                    Ref.
                </td>
                <td><input type="text" name="referencia" id="referencia" style="width:100px" maxlength="20" class="text ui-widget-content ui-corner-all"></td>
                <td>Moneda</td>
                <td>
                     <select  name="cbomoneda" id="cbomoneda" class="text ui-widget-content ui-corner-all" >
                        <option value="PEN">Soles</option>
                        <option value="USD">Dolares</option>
                    </select>
                </td>
            </tr>
           
            <tr>
                <td colspan="7">
                    <table id="tbldocumento" class="table">
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="7" style="text-align:right">
                    <input type="hidden" name="id" id="id"/>
                    <input type="button" class="btn btn-danger" onclick="grabar();" value="Grabar"/>
                </td>
            </tr>
        </table>
    </div>

    <div id="dcliente" class="dialog" title="Cliente">
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

    <div id="dproducto" title="Producto">
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
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    <input type="hidden" name="oid" id="oid" />
                    <input type="button" class="btn btn-danger" onclick="grabarproducto();" value="Agregar"/>
                </td>
            </tr>
        </table>
    </div>

    <div id="dpermiso" title="Autorizacion">
        <table border="0">
            <tr>
                <td colspan="2">
                    Hubo cambio de precio se requiere autorizacion
                </td>
            </tr>
            <tr>
                <td>
                    Usuario
                </td>
                <td>
                    <input type="text" name="pusuario" id="pusuario" value="" class="text ui-widget-content ui-corner-all">
                </td>
            </tr>
            <tr>
                <td>
                    Clave
                </td>
                <td>
                    <input type="password" name="pclave" id="pclave" value="" class="text ui-widget-content ui-corner-all">
                </td>
            </tr>
            
            <tr>
                <td colspan="2" style="text-align:right">
                    <input type="hidden" name="pid" id="pid" />
                    <input type="button" class="btn btn-danger" onclick="grabarpermiso();" value="Autorizar"/>
                </td>
            </tr>
        </table>
    </div>
 
</asp:Content>
