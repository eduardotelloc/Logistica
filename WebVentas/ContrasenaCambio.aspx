<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContrasenaCambio.aspx.cs" Inherits="WebVentas.ContrasenaCambio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
	<title>Contrasena</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
   	<script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/jquery.contrasenacambio-1.0.0.js"></script>
	<link href="Content/bootstrap.4.1.3.min.css" rel="stylesheet" />
	<link href="Content/fontawesome.5.3.1.min.css" rel="stylesheet" />
    <link href="defaultstyles.css" rel="stylesheet" />
	<link rel="shortcut icon" type="image/x-icon" href="~/favicon.ico" />
</head>
<body>
	<div class="container">
		<div class="d-flex justify-content-center h-100" >
			<div class="card">
				<div class="card-header">
					<h3>Cambio de Contraseña</h3>
				</div>
				<div class="card-body">
					<form id="formdocument" runat="server">
						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-key"></i></span>
							</div>
							<input type="password" id="CurrentPassword" name="CurrentPassword" onkeypress="return AvoidSpace(event)" class="form-control" placeholder="Clave Actual" runat="server" required="required" pattern=".{6,12}" title="Ingrese entre 6 a 12 caracteres"/>
						</div>
					
						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-key"></i></span>
							</div>
							<input type="password" id="NewPassword" name="NewPassword"  onkeypress="return AvoidSpace(event)" class="form-control" placeholder="Nueva Clave" runat="server" required="required" pattern=".{6,12}" title="Ingrese entre 6 a 12 caracteres" />
						</div>

						<div class="form-group row">
							<div class="col-md-4">
								<input type="button" class="btn float-right login_btn" onclick="window.location.replace('Menu.aspx');" value="Retornar" />
							</div>
							<div class="col-md-4"></div>
							<div class="col-md-4">
								<asp:Button ID="btnCambio" runat="server" CssClass="btn float-right login_btn" OnClick="btnCambio_Click" Text="Aceptar"  />
							</div>
						</div>
						<asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
							<p class="text-danger">
								<asp:Literal runat="server" ID="FailureText" />
							</p>
						</asp:PlaceHolder> 
					</form>
				</div>
				<div class="card-footer">
				</div>
			</div>
        </div>
	</div>
</body>
</html>
