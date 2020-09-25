<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Errores.aspx.cs" Inherits="WebLogistica.Errores" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.errores-1.2.0.js"></script>
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

        .table > thead > tr > th {border: 1px solid #D11925;}
        
        
        
        table th {
	        color: #FFF;
	        text-align: center;
	        background-color: #ff002e;
        }

        table td {
	        color: #000;
            height:24px;
            padding-left: 5px;
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
        <div class="col-md-12">
            <span class="label1">Errores</span> <span class="label2">/ Listado</span>
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

   
    <div class="table-responsive">
        <table id="tbldetalle" class="table"></table>
    </div>    
      
</asp:Content>
