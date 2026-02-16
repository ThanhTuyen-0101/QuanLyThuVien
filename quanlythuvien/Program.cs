using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        [STAThread]
        static void Main()
        {
            // Yêu cầu Log4net đọc file config 
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log.Info("--- UNG DUNG BAT DAU---");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new dangnhap());
        }
    }
}
