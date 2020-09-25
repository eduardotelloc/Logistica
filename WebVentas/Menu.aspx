<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WebVentas.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Menu Ventas</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
	<script src="Scripts/jquery.validate.min.js"></script>
	<script src="Scripts/jquery.default-1.0.0.js"></script>
	<link href="Content/bootstrap.4.1.3.min.css" rel="stylesheet" />
	<link href="Content/fontawesome.5.3.1.min.css" rel="stylesheet" />
    <link href="defaultstyles.css" rel="stylesheet" />
    <link rel="shortcut icon" type="image/x-icon" href="~/favicon.ico" />
</head>
<body>
    <form id="formdocument" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
	    <div class="container">
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-8">
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="text-align:center">
                            <p class="text" style="color:white">
                                <asp:Literal runat="server" ID="lblEmpresa"   /> 
                            </p>
                        </div>
                    </div> 
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="text-align:center">
                                <a id="A5" runat="server" href="~/DocumentosVenta">
                                    <input type="button" class= "btn float-xl-none menu_btn" title="Documentos"
                                    style="background-image:url(Img/punto_venta_black.png); background-repeat:no-repeat; background-position:center" />
                                </a>
                                <p class="text" style="color:#FFC312">
                                    Punto de Venta
                                </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            <a id="A2" runat="server" href="~/DocumentosListado">
                                <input type="button" class= "btn float-xl-none menu_btn" title="Documentos"
                                style="background-image:url(Img/document_black.png); background-repeat:no-repeat; background-position:center" />
                            </a>
                            <p class="text" style="color:#FFC312">
                                    Documentos
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            <a id="A12" runat="server" href="~/ClientesListado"><input type="button" class= "btn float-xl-none menu_btn" title="Clientes"
                                style="background-image:url(Img/customer_black.png); background-repeat:no-repeat; background-position:center"/>
                            </a>
                            <p class="text" style="color:#FFC312">
                                Clientes
                            </p>
                        </div>
                    </div>
                    <div class="row"> 
                        <br /> 
                    </div>  
                    <div class="row">
                        <div class="col-md-4" style="text-align:center">
                            <a id="A14" runat="server" href="~/EmpleadosListado"> <input type="button" class= "btn float-xl-none menu_btn" title="Empleados"
                                style="background-image:url(Img/employee_black.png); background-repeat:no-repeat; background-position:center"<%
                                        if (this.ViewState["acceso"].ToString() == "Vendedor")
                                        { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                                Empleado
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            <a id="A1" runat="server" href="~/LocalesListado"> <input type="button" class= "btn float-xl-none menu_btn" title="Empleados"
                                style="background-image:url(Img/industry_black.png); background-repeat:no-repeat; background-position:center"<%
                                        if (this.ViewState["acceso"].ToString() == "Vendedor")
                                        { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                                Locales
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            <a id="A8" runat="server" href="~/SeriesListado"> <input type="button" class= "btn float-xl-none menu_btn" 
                                title="Series"
                                style="background-image:url(Img/configuracion_black.png); background-repeat:no-repeat; background-position:center"<%
                                        if (this.ViewState["acceso"].ToString() == "Vendedor")
                                        { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                                Serie
                            </p>
                        </div>
                        
                    </div>
                    <div class="row"> 
                        <br /> 
                    </div>  
                    <div class="row">
                        <div class="col-md-4" style="text-align:center">
                           <a id="A3" runat="server" href="~/Reporte"><input type="button" class= "btn float-xl-none menu_btn" title="Reporte"
                            style="background-image:url(Img/report_black.png); background-repeat:no-repeat; background-position:center"<%
                                    if (this.ViewState["acceso"].ToString() == "Vendedor")
                                    { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                            Reporte
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            <a id="A4" runat="server" href="~/Dinamico"><input type="button" class= "btn float-xl-none menu_btn" title="Dinámico"
                                style="background-image:url(Img/dinamico_black.png); background-repeat:no-repeat; background-position:center"<%
                                        if (this.ViewState["acceso"].ToString() == "Vendedor")
                                        { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                                Dinámico
                            </p>
                        </div>
                        
                        <div class="col-md-4" style="text-align:center">
                            <a id="A7" runat="server" href="~/Contact"><input type="button" class= "btn float-xl-none menu_btn" title="Contacto"
                                style="background-image:url(Img/contact_black.png); background-repeat:no-repeat; background-position:center" <%
                                        if (this.ViewState["acceso"].ToString() == "Vendedor")
                                        { %> disabled="disabled" <%}%> />
                            </a>
                            <p class="text" style="color:#FFC312">
                            Contacto
                            </p>
                        </div>
                        
                    </div> 
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="text-align:center">
                             <a id="A9" runat="server" href="~/ContrasenaCambio"><input type="button" class= "btn float-xl-none menu_btn" title="Cambio de contraseña"
                                style="background-image:url(Img/contrasena_black.png); background-repeat:no-repeat; background-position:center"/>
                            </a>
                            <p class="text" style="color:#FFC312">
                                Cambio de Clave
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                           <asp:LoginStatus ID="LoginStatus1" runat="server" 
                                LogoutAction="Redirect" 
                                CssClass="btn float-xl-none menu_btn"
                                onclick="return confirm('Esta seguro de salir del sistema?');"
                                LogoutImageUrl="~/Img/salir_black.png" LogoutPageUrl="~/" 
                                OnLoggingOut="Unnamed_LoggingOut" ToolTip="Salir" />
                            <p class="text" style="color:#FFC312">
                                Salir
                            </p>
                        </div>
                        <div class="col-md-4" style="text-align:center">
                            
                        </div>
                        <div class="col-md-4" style="text-align:center">

                        </div>
                    </div> 
                </div>
                <div class="col-md-2"></div>
            </div>
        </div>
    </form>
</body>
</html>
