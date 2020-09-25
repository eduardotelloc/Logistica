<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebLogistica.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <style type="text/css">
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
    <div class="row">
        <div class="col-md-10" style="text-align:right">
            <div class="row">
                <br />
            </div>
            <div class="row">
                <a href="#" onclick="window.location.href='Menu.aspx'" title="Volver al Menu"><img src="Img/back.png"/></a>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>

    <div class="row">
            <div class="col-md-2"></div>
            <div class="col-md-8">
       
                <div>
                    <br />
                    <span class="label1">Contact</span>
                    <br />
                    <br />
                    <address>
                        Ingenia IT<br />
                        Av Riva Aguero N° 2020 - San Miguel - Lima<br />
                        <abbr title="Phone">Phone (51)-9474-00306</abbr></address>
                    <address>
                        <strong>Service Management:</strong> <a style="color:#ff002e" href="mailto:soporte@ingenia-it.com">soporte@ingenia-it.com</a><br />
                        <strong>Developer:</strong> <a style="color:#ff002e" href="mailto:luis.pariatanta@ingenia-it.com">luis.pariatanta@ingenia-it.com</a><br />
                    </address>
                </div>
            </div>
            <div class="col-md-2"></div>  
    </div> 
</asp:Content>
