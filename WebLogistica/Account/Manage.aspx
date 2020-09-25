<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="WebLogistica.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    
    <style type="text/css">
       
        span { color:#f00}
        

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
        <div class="col-md-12">
            <br />
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <span class="label1">Manage</span> <span class="label2">/ Account</span>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <br />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div>
                <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                    <h3><p class="text-success"><%: SuccessMessage %></p></h3>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>

    <%--<div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Change your account settings</h4>
                <hr />
                <dl class="dl-horizontal">
                    <dt>Password:</dt>
                    <dd>
                        <asp:HyperLink ForeColor="#00aefe" NavigateUrl="~/Account/ManagePassword" Text="[Change]" Visible="false" ID="ChangePassword" runat="server" />
                    </dd>
                </dl>
            </div>
        </div>
    </div>--%>

</asp:Content>
