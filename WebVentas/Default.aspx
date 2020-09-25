<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebVentas.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Default</title>
	
	<script src="Scripts/jquery-3.1.1.js"></script>
	<script src="Scripts/jquery.validate.min.js"></script>
	<script src="Scripts/jquery.default-1.0.0.js"></script>
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
					<h3>Ventas</h3>
					<div class="d-flex justify-content-end social_icon">
						<span><i class="fab fa-facebook-square" onclick="window.open('https://www.facebook.com/twotecnologyperu')"></i></span>
						<span><i class="fab fa-google-plus-square" onclick="window.open('https://www.twotecnology.com')"></i></span>
						<span><i class="fab fa-twitter-square" onclick="window.open('https://www.twotecnology.com')"></i></span>
					</div>
				</div>
				<div class="card-body">
					<form id="formdocument" runat="server">
						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-home"></i></span>
							</div>
							<input type="text" id="Ruc" name="Ruc" class="form-control" placeholder="RUC" runat="server" required="required" maxlength="11" onkeypress="return isNumberKey(event);"/>
						</div>

						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-user"></i></span>
							</div>
							<input type="text" id="Email" name="Email" class="form-control" placeholder="Usuario" runat="server" required="required" maxlength="50"/>
						
						</div>
						<div class="input-group form-group">
							<div class="input-group-prepend">
								<span class="input-group-text"><i class="fas fa-key"></i></span>
							</div>
							<input type="password" id="Password" name="Password" class="form-control" placeholder="Clave" runat="server" required="required" maxlength="12"/>
						</div>
						<div class="row align-items-center remember">
							<input type="checkbox" id="chkrecordar" runat="server" />Recordar datos
						</div>
						<div class="form-group">
							<asp:Button ID="btnLogin" runat="server" CssClass="btn float-right login_btn" OnClick="btnLogin_Click" Text="Acceso"  />
						</div>
						<asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
							<p class="text-danger">
								<asp:Literal runat="server" ID="FailureText" />
							</p>
						</asp:PlaceHolder> 
					</form>
				</div>
				<div class="card-footer">
					<div class="d-flex justify-content-center">
						<a href="#">Recuperar contraseña?</a>
					</div>
				</div>
			</div>
		</div>
	</div>
</body>
</html>
