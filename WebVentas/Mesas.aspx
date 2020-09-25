<%@ Page Title="Documentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="WebVentas.Mesas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <script src="Scripts/jquery.mesas-1.1.0.js"></script>
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
            font-size: 24px;
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
            height:48px;
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
       
       .circle { 
            display: table-cell; 
            padding: 40px; 
            width: 120px; 
            height: 120px; 
            border-radius: 50%; 
            font-size:24px; 
            color: #fff; 
            line-height: 50px; 
            text-align: center; 
            background: #40a977; 
            word-break: break-all; 
            justify-content: center; 
            overflow: hidden; 
        } 
       
         
    </style>

    <br />

    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <input type="hidden" name="hid" id="hid" />
                <div id="divmesas" ></div>
            </div>
        </div>
                
        <div class="col-md-4">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <div class="table-responsive">
                    <table id="tbldetalle" class="table"></table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
