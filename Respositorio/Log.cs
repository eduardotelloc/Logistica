using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class Log
    {
        public static void SaveLog(string cadena)
        {
            string pathlog = ConfigurationManager.AppSettings.Get("PathLog");
            string sPath = pathlog + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(cadena);
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }
    }
}
