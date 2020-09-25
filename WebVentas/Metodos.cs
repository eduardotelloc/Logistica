using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentas
{
    public static class Metodos
    {
        public static string ConverTo_YYYYMMDD(this string value)
        {
            return string.Format("{0}{1}{2}", value.Substring(6), value.Substring(3, 2), value.Substring(0, 2));
        }


        
        
    }
}