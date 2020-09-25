
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Script.Serialization;

namespace Repositorio
{
    public class Reporte : IDisposable
    {
         #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Reporte(IntPtr handle)
        {
            this.handle = handle;
        }

        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use interop to call the method necessary 
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code. 
        // This destructor will run only if the Dispose method 
        // does not get called. 
        // It gives your base class the opportunity to finalize. 
        // Do not provide destructors in types derived from this class.
        ~Reporte()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Reporte()
        { }

        
        public string LoadReporte101(string empresaid, string periodo)
        {
            string str = string.Empty;
            
            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                connection.Open();

                string sql = string.Format("" +
                                   "SELECT MesCodigo, MesNombre as name, CAST( sum(SOLES)  as  decimal(12,0)) as amount  " +
                                   "FROM " +
                                   "( " +
                                   "   select right(left(FECHA_DOCU,6),2) as MesCodigo, " +
                                   "       case right(left(FECHA_DOCU,6),2) " +
                                   "       when '01' then 'Ene' " +
                                   "       when '02' then 'Feb' " +
                                   "       when '03' then 'Mar' " +
                                   "       when '04' then 'Abr' " +
                                   "       when '05' then 'May' " +
                                   "       when '06' then 'Jun' " +
                                   "       when '07' then 'Jul' " +
                                   "       when '08' then 'Ago' " +
                                   "       when '09' then 'Sep' " +
                                   "       when '10' then 'Oct' " +
                                   "       when '11' then 'Nov' " +
                                   "       when '12' then 'Dic' end MesNombre, " +
                                   "       CASE WHEN MONEDA_DOCU='PEN' " + 
                                   "       THEN  " +
                                   "           CASE WHEN TIPO_DOCU='07' " +
                                   "           THEN cast( replace(IMPO_TOTAL,',','') as decimal(12,2))*-1  " +
                                   "           ELSE cast( replace(IMPO_TOTAL,',','') as decimal(12,2))  " +
                                   "           END " +
                                   "       ELSE 0 END as SOLES " +
                                   "  from vDOCUMENTO_REPORTE_CLIENTE " +
                                   "  where EMPRESA_CODIGO={0} and left(FECHA_DOCU,4)={1} and ESTADO_DOCU='ACT' " +
                                   ") a  " +
                                   "group by MesCodigo, MesNombre " +
                                   "order by MesCodigo ", 
                                   empresaid, periodo);

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    List<ListaReporte> list = new List<ListaReporte>();


                    while (reader.Read())
                    {
                        NumberFormatInfo provider = new NumberFormatInfo();
                        provider.NumberDecimalSeparator = ".";
                        ListaReporte item = new ListaReporte();
                        item.name = reader["name"].ToString();
                        item.amount = decimal.Parse(reader["amount"].ToString(), provider);
                        list.Add(item);
                    }

                    str = new JavaScriptSerializer().Serialize(list);
                }
            }
            return str;
        }

        
    }
}

