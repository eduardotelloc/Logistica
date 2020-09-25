<%@ Page Title="Reporte" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="WebVentas.Reporte" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.reporte-1.3.0.js"></script>
    <script src="Scripts/jquery.blockUI.js"></script>
    <script src="Scripts/jquery.general-1.2.0.js"></script>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-3d.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
      
        .modal .modal-dialog { width: 95%; }
        
        table {border-collapse: collapse; font-size:11.5px;}

        table td, table th {
            border: 1px solid #545454;
            vertical-align:middle;
            font-size: 13px;
            font-family:Calibri ;
        }

        .table > thead > tr > th {border: 1px solid #545454;}
        
        table th {
	        color: #545454;
	        text-align: center;
	        background-color: #fbe607;
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
            <span class="label1">Reporte</span> 
        </div>
        <div class="col-md-6" style="text-align:right">
            <%--<img src="Img/Empresa/<% =Session["empresaid"].ToString() %>_corto.png" class="img-responsive" style="text-align:left;float: right"/>--%>
        </div>
    </div>

    <div class="row">
        <div class="col-md-10">
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
        <div class="col-md-12">
           <br />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
   
            <div class="row">
                <div class="col-md-3">
                    <div class="row"><b>Año</b></div>
                    <div class="row">
                        <select id="cboanio" class="form-control form-control-inline" style="width:100px">
                        </select>
                    </div>
                </div>
                
                <div class="col-md-3">
                    <div class="row"><b>Mes</b></div>
                    <div class="row">
                        <select id="cbomes" class="form-control form-control-inline" style="width:150px">
                            <option value="01">Enero</option>
                            <option value="02">Febrero</option>
                            <option value="03">Marzo</option>
                            <option value="04">Abril</option>
                            <option value="05">Mayo</option>
                            <option value="06">Junio</option>
                            <option value="07">Julio</option>
                            <option value="08">Agosto</option>
                            <option value="09">Setiembre</option>
                            <option value="10">Octubre</option>
                            <option value="11">Noviembre</option>
                            <option value="12">Diciembre</option>
                        </select>
                    </div>
                </div>
        
                <div class="col-md-3">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="buscar();" value="Buscar"/>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <input type="button" class="btn btn-danger" onclick="descargar();" value="Xml"/>
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
        </div>
        <div class="col-md-6">
            <div id="container_1">

            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div id="container_2">

            </div>
        </div>
    
        <div class="col-md-6">
            <div id="container_3">

            </div>
        </div>
    </div>
</asp:Content>
