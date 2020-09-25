using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.Web.Services;
using Newtonsoft.Json;
using Entity;
using RestSharp;

namespace WebVentas
{
    public partial class Operacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
            }




            FormsAuthenticationTicket ticket = null;

            try
            {
                FormsIdentity formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                ticket = formsIdentity.Ticket;
            }
            catch
            {
                Response.Redirect("Default.aspx");
            }

            string empresaid = ticket.UserData.Split('|')[0];
            string ruc = ticket.UserData.Split('|')[1];
            string usuario_acceso = ticket.UserData.Split('|')[3];
            string usuario_serie = ticket.UserData.Split('|')[7];
            string puntosid = ticket.UserData.Split('|')[8];
            string ipcliente = ticket.UserData.Split('|')[9];
            string localcodigo = ticket.UserData.Split('|')[10];
            //Response.Expires = -1;
            Response.ContentType = "application/text";
            string respuesta = "";

            

            if (Request.QueryString["ope"] == "1")
            {
                using (Repositorio.Impresora obj = new Repositorio.Impresora())
                {
                    respuesta = obj.LoadByCodigo(localcodigo);
                }
            }

            if (Request.QueryString["ope"] == "002")
            {
                using (Repositorio.Ubigeo obj = new Repositorio.Ubigeo())
                {
                    respuesta = obj.Load();
                }
            }

            if (Request.QueryString["ope"] == "101")
            {
                string sTipo = Request.QueryString["Tipo"].ToString();
                string sSerie = Request.QueryString["Serie"].ToString();
                string sCorrelativo = Request.QueryString["Correlativo"].ToString();
                string sFechaInicio = string.Empty;
                string sFechaFin = string.Empty;

                if (Request.QueryString["FechaInicio"].ToString() != "")
                    sFechaInicio = Request.QueryString["FechaInicio"].ToString().ConverTo_YYYYMMDD();

                if (Request.QueryString["FechaFin"].ToString() != "")
                    sFechaFin = Request.QueryString["FechaFin"].ToString().ConverTo_YYYYMMDD();

                string sUsuario = Context.User.Identity.Name;

                string sEstado = Request.QueryString["estado"] ?? "";
                string sSunat = Request.QueryString["sunat"] ?? "";

                if (sSerie == "" && usuario_acceso=="Vendedor")
                    sSerie = usuario_serie;

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    respuesta = obj.Load(empresaid, sTipo, sSerie, sCorrelativo, sFechaInicio, sFechaFin, sEstado, sSunat);
                }
            }



            if (Request.QueryString["ope"] == "102")
            {

                string sCliente = Request.QueryString["cliente"].ToString();

                using (Repositorio.Usuario obj = new Repositorio.Usuario())
                {
                    respuesta = obj.GetUsuariosByNombreToJson(empresaid, sCliente);
                }
            }

            if (Request.QueryString["ope"] == "103")
            {
                string sTop = Request.QueryString["Top"] ?? "";
                string sDocumento = Request.QueryString["Documento"].ToString();

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    respuesta = obj.Load(empresaid, sDocumento, sTop);
                }
            }

            if (Request.QueryString["ope"] == "104")
            {
                string empresa = Request.QueryString["empresa"];
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    obj.Imprimir(empresaid, codigo);
                }
            }

            if (Request.QueryString["ope"] == "105")
            {
                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    //respuesta = obj.LoadDocumento(empresaid, ruc);
                }
            }

            if (Request.QueryString["ope"] == "106")
            {
                respuesta = usuario_serie;
                
            }



            if (Request.QueryString["ope"] == "110")
            {
                string idcliente = Request.QueryString["idcli"];
                string idtrabajador = Request.QueryString["idtra"];
                string fecha = Request.QueryString["fecha"];
                string cambio_venta = Request.QueryString["tcambio"];
                string moneda = Request.QueryString["moneda"];
                string tipo = Request.QueryString["tipo"];
                string indicador = Request.QueryString["ind"];
                string serie = Request.QueryString["serie"];
                //string correlativo = string.Empty;
                string correlativo = Request.QueryString["corre"];
                string referencia = Request.QueryString["ref"];
                string valorventa = Request.QueryString["vventa"];
                string igv = Request.QueryString["igv"];
                string total = Request.QueryString["total"];
                string impresora = Request.QueryString["print"];

                string tipopago = Request.QueryString["tipopago"];
                string tipoenvio = Request.QueryString["tipoenvio"];
                string observacion = HttpUtility.UrlDecode(Request.QueryString["observacion"]);


                impresora = ((impresora.ToUpper() == "DEFAULT") ? "" : impresora.ToUpper());

                if (correlativo == "0" || correlativo == "")
                {
                    using (Repositorio.Serie obj = new Repositorio.Serie())
                    {
                        correlativo = obj.LoadSeriesCorrelativo(ruc, tipo, indicador, serie);
                    }
                }

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    respuesta= obj.Agregar(ruc, idcliente, idtrabajador, fecha, cambio_venta, moneda.ToUpper(), 
                        tipo.ToUpper(), 
                        indicador.ToUpper(), serie, correlativo, referencia.ToUpper(), 
                        valorventa, igv, total, ipcliente, impresora,
                        tipopago.ToUpper(), tipoenvio.ToUpper(), observacion.ToUpper());
                }
            }

            if (Request.QueryString["ope"] == "111")
            {
                string json = HttpUtility.UrlDecode(  Request.QueryString["json"]);
               
                List<Detalle> lista = JsonConvert.DeserializeObject<List<Detalle>>(json);

                //string idproducto = Request.QueryString["idproducto"];
                //string producto = Request.QueryString["producto"];
                //string unidad = Request.QueryString["unidad"];
                //string cantidad = Request.QueryString["cantidad"];
                //string preciounit = Request.QueryString["preciounit"];
                //string precioventa = Request.QueryString["precioventa"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    foreach(var item in lista){
                        obj.AgregarDetalle(ruc, item.codigo, item.idproducto, item.producto.Replace('#','"'), item.unidad, item.cantidad, item.preciounit, item.precioventa);
                    }
                }
                
            }

            if (Request.QueryString["ope"] == "112")
            {
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    obj.AgregarTransaccion(ruc, codigo);

                    respuesta = obj.LoadTransaccion(ruc, codigo);
                }


            }

            if (Request.QueryString["ope"] == "113")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    obj.Eliminar(id);
                }
            }


            if (Request.QueryString["ope"] == "114")
            {
                string usuario = Request.QueryString["usuario"];
                string clave = Request.QueryString["clave"];

                using (Repositorio.Usuario obj = new Repositorio.Usuario())
                {
                    respuesta= (obj.ValidateAdminRuc(ruc, usuario, clave)?"1":"0");
                }
            }


            











            if (Request.QueryString["ope"] == "210")
            {
                string idtrabajador = Request.QueryString["idtrabajador"];
                string fecha = Request.QueryString["fecha"];
                string mesa = Request.QueryString["mesa"];
                string valorventa = Request.QueryString["valorventa"];
                string igv = Request.QueryString["igv"];
                string total = Request.QueryString["total"];


                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    respuesta = obj.Agregar(ruc, idtrabajador, fecha,mesa, valorventa, igv, total);
                }
            }

            if (Request.QueryString["ope"] == "211")
            {
                string json = HttpUtility.UrlDecode(Request.QueryString["json"]);

                List<Detalle> lista = JsonConvert.DeserializeObject<List<Detalle>>(json);


                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    foreach (var item in lista)
                    {
                        obj.AgregarDetalle(ruc, item.codigo, item.idproducto, item.producto.Replace('#', '"'), item.unidad, item.cantidad, item.preciounit, item.precioventa);
                    }
                }

            }

            if (Request.QueryString["ope"] == "212")
            {
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    obj.AgregarTransaccion(ruc, codigo);
                }


            }

            if (Request.QueryString["ope"] == "213")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    obj.Eliminar(id);
                }
            }


            if (Request.QueryString["ope"] == "214")
            {

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    respuesta = obj.LoadMesa(ruc);
                }
            }



            if (Request.QueryString["ope"] == "215")
            {
                string mesa = Request.QueryString["mesa"];

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    respuesta = obj.LoadHeader(ruc, mesa);
                }
            }

            if (Request.QueryString["ope"] == "216")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    respuesta = obj.LoadDetail(id);
                }
            }


            if (Request.QueryString["ope"] == "217")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Pedido obj = new Repositorio.Pedido())
                {
                    respuesta = obj.LoadHeader(id);
                }
            }












            if (Request.QueryString["ope"] == "301")
            {
                string nombre = Request.QueryString["nombre"];

                using (Repositorio.Presentacion obj = new Repositorio.Presentacion())
                {
                    respuesta = obj.Load(ruc, nombre);
                }
            }

            if (Request.QueryString["ope"] == "302")
            {
                string id = Request.QueryString["id"];
                string nombre = Request.QueryString["nombre"];
                string descripcion = Request.QueryString["descripcion"];

                using (Repositorio.Presentacion obj = new Repositorio.Presentacion())
                {
                    if (id == "0")
                        obj.Agregar(ruc, nombre, descripcion);
                    else
                        obj.Editar(id, nombre, descripcion);

                }
            }

            if (Request.QueryString["ope"] == "303")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Presentacion obj = new Repositorio.Presentacion())
                {
                    obj.Eliminar(id);
                }
            }







            if (Request.QueryString["ope"] == "401")
            {
                string nombre = Request.QueryString["nombre"];

                using (Repositorio.Categoria obj = new Repositorio.Categoria())
                {
                    respuesta = obj.Load(ruc, nombre);
                }
            }

            if (Request.QueryString["ope"] == "402")
            {
                string id = Request.QueryString["id"];
                string nombre = Request.QueryString["nombre"];
                string descripcion = Request.QueryString["descripcion"];

                using (Repositorio.Categoria obj = new Repositorio.Categoria())
                {
                    if (id == "0")
                        obj.Agregar(ruc, nombre, descripcion);
                    else
                        obj.Editar(id, nombre, descripcion);

                }
            }

            if (Request.QueryString["ope"] == "403")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Categoria obj = new Repositorio.Categoria())
                {
                    obj.Eliminar(id);
                }
            }



            if (Request.QueryString["ope"] == "501")
            {
                using (Repositorio.Puntos obj = new Repositorio.Puntos())
                {
                    respuesta = obj.Load(ruc);
                }
            }

            if (Request.QueryString["ope"] == "502")
            {
                using (Repositorio.Puntos obj = new Repositorio.Puntos())
                {
                    respuesta = obj.LoadMesa(ruc, puntosid);
                }
            }



            if (Request.QueryString["ope"] == "503")
            {
                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    respuesta = obj.LoadByRol(ruc, "Mesero");
                }
            }




            if (Request.QueryString["ope"] == "601")
            {
                string nombre = Request.QueryString["nombre"];
                string sSerie = usuario_serie;

                //string sSerie = string.Empty;

                //if(usuario_acceso == "Vendedor")
                //    sSerie = usuario_serie;

                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    respuesta = obj.Load(ruc, nombre, sSerie);
                }
            }

            if (Request.QueryString["ope"] == "602")
            {

                string id = Request.QueryString["id"];
                string codigo = Request.QueryString["codigo"];
                string serie = usuario_serie;

                
                if(Request.QueryString["serie"]!=null)
                    serie = Request.QueryString["serie"];
                
                string nombre = Request.QueryString["nombre"];
                string descripcion = Request.QueryString["descripcion"];
                string idcategoria = Request.QueryString["idcategoria"];
                string idpresentacion = Request.QueryString["idpresentacion"];
                string stock = Request.QueryString["stock"];
                string precio = Request.QueryString["precio"];
                string total = Request.QueryString["total"];
                string bonificacion = Request.QueryString["bonificacion"];


                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    if (id == "0")
                        obj.Agregar(ruc, codigo, serie, nombre, descripcion, idcategoria, idpresentacion, stock, precio, total, bonificacion);
                    else
                        obj.Editar(id, codigo, serie, nombre, descripcion, idcategoria, idpresentacion, stock, precio, total, bonificacion);

                }
            }

            if (Request.QueryString["ope"] == "603")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    obj.Eliminar(id);
                }
            }

            if (Request.QueryString["ope"] == "604")
            {
                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    respuesta = obj.LoadNombre(ruc, usuario_serie);
                }
            }

            if (Request.QueryString["ope"] == "606")
            {
                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    respuesta = obj.LoadId(ruc, usuario_serie);
                }
            }

            if (Request.QueryString["ope"] == "605")
            {

                string nombre = Request.QueryString["nombre"];
                //nombre = HttpUtility.UrlDecode(nombre);

                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    respuesta = obj.LoadByNombre(ruc, nombre, usuario_serie);
                }
            }


            

            if (Request.QueryString["ope"] == "607")
            {

                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    respuesta = obj.LoadById(ruc, codigo, usuario_serie);
                }
            }


            if (Request.QueryString["ope"] == "700")
            {
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadByCodigo(ruc, codigo);
                }
            }


            if (Request.QueryString["ope"] == "701")
            {

                string nombre = Request.QueryString["nombre"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.Load(ruc, nombre);
                }
            }

            if (Request.QueryString["ope"] == "702")
            {
                bool blnerror = false;
                string id = Request.QueryString["id"];
                string tipo_documento = Request.QueryString["tipo_documento"];
                string num_documento = Request.QueryString["num_documento"];
                string nombre = Request.QueryString["nombre"];


                if (tipo_documento == "DNI") {
                    if (num_documento.Length != 8) {
                        respuesta = "El tipo de documento DNI debe contener 8 caracteres!";
                        blnerror = true;
                    }
                }

                if (tipo_documento == "RUC")
                {
                    if (num_documento.Length != 11)
                    {
                        respuesta = "El tipo de documento RUC debe contener 11 caracteres!";
                        blnerror = true;
                    }
                }

              
                if (nombre.Length< 3)
                {
                    respuesta = "El nombre debe contener mas de 2 caracteres!";
                    blnerror = true;
                }
                

                if (!blnerror)
                {
                    string sexo = Request.QueryString["sexo"];
                    string fecha_nacimiento = Request.QueryString["fecha_nacimiento"];

                    string departamento = Request.QueryString["departamento"];
                    string provincia = Request.QueryString["provincia"];
                    string distrito = Request.QueryString["distrito"];
                    string direccion = Request.QueryString["direccion"];
                    string ubigeo = Request.QueryString["ubigeo"];
                    string telefono = Request.QueryString["telefono"];
                    string email = Request.QueryString["email"];
                    string latitud = Request.QueryString["latitud"];
                    string longitud = Request.QueryString["longitud"];

                    using (Repositorio.Cliente obj = new Repositorio.Cliente())
                    {
                        if (id == "0")
                        {
                            if (obj.ClienteExisteByDocumento(ruc, num_documento))
                                respuesta = "Numero de documento existe!";
                            else
                                obj.Agregar(ruc, nombre, sexo, fecha_nacimiento, tipo_documento, num_documento,
                                        departamento, provincia, distrito, direccion, ubigeo, telefono, email,
                                        latitud, longitud);
                        }
                        else
                            obj.Editar(id, nombre, sexo, fecha_nacimiento, tipo_documento, num_documento,
                                departamento, provincia, distrito, direccion, ubigeo, telefono, email,
                                latitud, longitud);

                    }
                }
            }

            if (Request.QueryString["ope"] == "703")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    obj.Eliminar(id);
                }
            }

            if (Request.QueryString["ope"] == "704")
            {


                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadDocumento(ruc);
                }
            }

            if (Request.QueryString["ope"] == "705")
            {

                string numero_documento = Request.QueryString["numero_documento"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadNombreByDocumento(ruc, numero_documento);
                }
            }

            if (Request.QueryString["ope"] == "706")
            {


                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadNombre(ruc);
                }
            }

            if (Request.QueryString["ope"] == "707")
            {

                string nombre = Request.QueryString["nombre"];
              
                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadDocumentoByNombre(ruc, nombre);
                }
            }


            if (Request.QueryString["ope"] == "708")
            {

                string tipo = Request.QueryString["tipo"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadDocumentoByTipoDocu(ruc, tipo);
                }
            }

            if (Request.QueryString["ope"] == "709")
            {
                string tipo = Request.QueryString["tipo"];

                using (Repositorio.Cliente obj = new Repositorio.Cliente())
                {
                    respuesta = obj.LoadNombreByTipoDocu(ruc, tipo);
                }
            }




            if (Request.QueryString["ope"] == "801")
            {


                using (Repositorio.TipoCambio obj = new Repositorio.TipoCambio())
                {
                    respuesta = obj.Load(ruc);
                }
            }

            if (Request.QueryString["ope"] == "802")
            {

                string id = Request.QueryString["id"];
                string fecha_tipo_cambio = Request.QueryString["fecha_tipo_cambio"];
                string cambio_venta = Request.QueryString["cambio_venta"];
                string cambio_compra = Request.QueryString["cambio_compra"];


                using (Repositorio.TipoCambio obj = new Repositorio.TipoCambio())
                {
                    if (id == "0")
                        obj.Agregar(ruc, fecha_tipo_cambio, cambio_venta, cambio_compra);
                    else
                        obj.Editar(id, fecha_tipo_cambio, cambio_venta, cambio_compra);

                }
            }

            if (Request.QueryString["ope"] == "803")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.TipoCambio obj = new Repositorio.TipoCambio())
                {
                    obj.Eliminar(id);
                }
            }

            if (Request.QueryString["ope"] == "804")
            {


                using (Repositorio.TipoCambio obj = new Repositorio.TipoCambio())
                {
                    respuesta = obj.LoadMontoLast(ruc);
                }
            }


            if (Request.QueryString["ope"] == "900")
            {
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    respuesta = obj.LoadByCodigo(ruc, codigo);
                }
            }


            if (Request.QueryString["ope"] == "901")
            {

                string nombre = Request.QueryString["nombre"];

                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    respuesta = obj.Load(ruc, nombre);
                }
            }

            if (Request.QueryString["ope"] == "902")
            {
                bool blnerror = false;
                string id = Request.QueryString["id"];
                string nombre = Request.QueryString["nombre"];
                string apellidos = Request.QueryString["apellidos"];
                string usuario = Request.QueryString["usuario"];
                string password = Request.QueryString["password"];

                if (nombre.Length < 3)
                {
                    respuesta = "El nombre debe contener mas de 2 caracteres!";
                    blnerror = true;
                }

                if (apellidos.Length < 3)
                {
                    respuesta = "El apellido debe contener mas de 2 caracteres!";
                    blnerror = true;
                }

                if (usuario.Length < 6)
                {
                    respuesta = "El usuario debe contener mas de 6 caracteres!";
                    blnerror = true;
                }

                if (password.Length < 6)
                {
                    respuesta = "La contraseña debe contener mas de 6 caracteres!";
                    blnerror = true;
                }


                if (!blnerror)
                {
                    string sexo = Request.QueryString["sexo"];
                    string fecha_nacimiento = Request.QueryString["fecha_nacimiento"];
                    string tipo_documento = Request.QueryString["tipo_documento"];
                    string num_documento = Request.QueryString["num_documento"];
                    string direccion = Request.QueryString["direccion"];
                    string telefono = Request.QueryString["telefono"];
                    string email = Request.QueryString["email"];
                    string acceso = Request.QueryString["acceso"];
                    string departamento = Request.QueryString["departamento"];
                    string provincia = Request.QueryString["provincia"];
                    string distrito = Request.QueryString["distrito"];
                    string serie = Request.QueryString["serie"];
                    string puntoid = Request.QueryString["puntosid"];

                    using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                    {
                        if (id == "0")
                            obj.Agregar(ruc, nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento,
                                departamento, provincia, distrito, direccion, telefono, email,
                                acceso, usuario, password, serie, puntoid);
                        else
                            obj.Editar(id, nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento,
                                departamento, provincia, distrito, direccion, telefono, email,
                                acceso, usuario, password, serie, puntoid);

                    }
                }
            }

            if (Request.QueryString["ope"] == "903")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    obj.Eliminar(id);
                }
            }

            if (Request.QueryString["ope"] == "904")
            {


                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    respuesta = obj.LoadDocumento(ruc);
                }
            }

            if (Request.QueryString["ope"] == "905")
            {

                string numero_documento = Request.QueryString["numero_documento"];

                using (Repositorio.Trabajador obj = new Repositorio.Trabajador())
                {
                    respuesta = obj.LoadNombreByDocumento(ruc, numero_documento);
                }
            }




            if (Request.QueryString["ope"] == "910")
            {
                string codigo = Request.QueryString["codigo"];

                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    respuesta = obj.LoadByCodigo(ruc, codigo);
                }
            }

            if (Request.QueryString["ope"] == "911")
            {
                string serie = Request.QueryString["serie"];

                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    respuesta = obj.Load(ruc, serie);
                }
            }

            if (Request.QueryString["ope"] == "912")
            {

                string id = Request.QueryString["id"];
                string tipo = Request.QueryString["tipo"];
                string indicador = Request.QueryString["indicador"];
                string serie = Request.QueryString["serie"];
                string correlativo = Request.QueryString["correlativo"];

                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    if (id == "0")
                    {
                        if (obj.SerieExisteByDocumento(ruc, tipo, indicador, serie))
                            respuesta = "Numero de serie existe!";
                        else
                            obj.Agregar(ruc, tipo, indicador, serie, correlativo);
                    }
                    else
                        obj.Editar(id, tipo, indicador, serie, correlativo);

                }
            }

            if (Request.QueryString["ope"] == "913")
            {
                string id = Request.QueryString["id"];

                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    obj.Eliminar(id);
                }
            }

            if (Request.QueryString["ope"] == "914")
            {
                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    string tipo = Request.QueryString["tipo"];
                    string indicador = Request.QueryString["indicador"];
                    respuesta = obj.LoadSeries(ruc,tipo,indicador, usuario_serie);
                }
            }


            if (Request.QueryString["ope"] == "915")
            {
                using (Repositorio.Serie obj = new Repositorio.Serie())
                {
                    respuesta = obj.LoadSelect(ruc);
                }
            }



            if (Request.QueryString["ope"] == "921")
            {
                string nombre = Request.QueryString["nombre"];

                using (Repositorio.Configuracion obj = new Repositorio.Configuracion())
                {
                    respuesta = obj.Load(ruc, nombre);
                }
            }




            if (Request.QueryString["ope"] == "931")
            {
                using (Repositorio.Variable obj = new Repositorio.Variable())
                {
                    respuesta = obj.Load(ruc);
                }
            }


            if (Request.QueryString["ope"] == "932")
            {
                string id = Request.QueryString["id"];
                string valor = Request.QueryString["valor"];
                
                using (Repositorio.Variable obj = new Repositorio.Variable())
                {
                    obj.Editar(id, valor);
                }
            }


            if (Request.QueryString["ope"] == "933")
            {
                using (Repositorio.Variable obj = new Repositorio.Variable())
                {
                    respuesta=obj.LoadGlosa(ruc);
                }
            }


            if (Request.QueryString["ope"] == "981")
            {
                string anio = Request.QueryString["anio"];
                string mes = Request.QueryString["mes"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    respuesta = obj.Resumen(empresaid, anio, mes);
                }
            }


            if (Request.QueryString["ope"] == "2000")
            {
                string rucbusqueda = Request.QueryString["ruc"];

                using (Repositorio.Documento obj = new Repositorio.Documento())
                {
                    respuesta = obj.ConsultaRUC(rucbusqueda);
                }
            }


            if (Request.QueryString["ope"] == "2001")
            {
                string dni = Request.QueryString["dni"];
                string datos="";

                
                var client = new RestClient("http://aplicaciones007.jne.gob.pe/srop_publico/Consulta/Afiliado/GetNombresCiudadano?DNI=" + dni);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Host", "aplicaciones007.jne.gob.pe");
                request.AddHeader("Postman-Token", "cfac1ff1-8ce1-4724-a310-bb31edd424c2,d849f3c1-ca9a-4fc7-bf00-888f677ee133");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
                IRestResponse response = client.Execute(request);
                datos=response.Content;



                respuesta = (datos.Length > 20) ? "" : datos;

            }


            if (base.Request.QueryString["ope"] == "3001")
            {
                string periodo = base.Request.QueryString["periodo"];

                using (Repositorio.Reporte obj = new Repositorio.Reporte())
                {
                    respuesta = obj.LoadReporte101(empresaid,periodo);
                }
            }



            if (base.Request.QueryString["ope"] == "4001")
            {
                string codigo = Request.QueryString["codigo"];
                string nombre = Request.QueryString["nombre"];
                string stock = Request.QueryString["stock"];
                string precio = Request.QueryString["precio"];
                string serie = Request.QueryString["serie"];
                string bonificacion = Request.QueryString["bonificacion"];

                using (Repositorio.Producto obj = new Repositorio.Producto())
                {
                    obj.AgregarMasivo(ruc,codigo,nombre, serie, stock, precio, bonificacion);
                }
            }



            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// CIERRE DE CAJA
            
            if (Request.QueryString["ope"] == "6001")
            {

                string sTop = Request.QueryString["Top"].ToString();
                string sFechaInicio = string.Empty;
                string sFechaFin = string.Empty;

                if (Request.QueryString["FechaInicio"].ToString() != "")
                    sFechaInicio = Request.QueryString["FechaInicio"].ToString().ConverTo_YYYYMMDD();

                if (Request.QueryString["FechaFin"].ToString() != "")
                    sFechaFin = Request.QueryString["FechaFin"].ToString().ConverTo_YYYYMMDD();

                string sUsuario = Context.User.Identity.Name;

                using (Repositorio.CierreCaja obj = new Repositorio.CierreCaja())
                {
                    respuesta = obj.Load(ruc, sFechaInicio, sFechaFin, sTop);
                }
            }


            Response.Write(respuesta);
            Response.ContentEncoding = Encoding.UTF8;
            Response.Flush();
            Response.End();
        }

       
        
    }
}