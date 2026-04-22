using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSAT
{
    internal static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()));
            var json = args.FirstOrDefault();

            //if (string.IsNullOrWhiteSpace(json))
            //{
            //    var dataTest = new
            //    {
            //        UserId = 5,
            //        UserName = "TEST",
            //        FullName = "NGUYỄN VĂN TEST",
            //        DepartmentId = 10,
            //        DepartmentName = "Phòng TEST"
            //    };

            //    json = JsonConvert.SerializeObject(dataTest);
            //}

            if (string.IsNullOrWhiteSpace(json))
            {
                MessageBox.Show(
                    "Không thể xác định thông tin đăng nhập.\nVui lòng khởi động ứng dụng từ hệ thống HIS.",
                    "Khởi động không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                log.Info($"Base64 Endcode:{json}");
                json = Base64Helper.Decode(json);
                log.Info($"Base64 Decode:{json}");

                ProtectConfig();
                dynamic obj = JsonConvert.DeserializeObject(json);
                CurrentUser.UserId = obj?.UserId ?? 0;
                CurrentUser.UserName = obj?.UserName ?? "";
                CurrentUser.FullName = obj?.FullName ?? "";
                CurrentUser.DepartmentId = obj?.DepartmentId ?? 0;
                CurrentUser.DepartmentName = obj?.DepartmentName ?? "";

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Dữ liệu khởi động không hợp lệ.",
                    "Lỗi tham số",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                log.Error(ex);
            }
        }
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        public static void ProtectConfig()
        {
            IconSizeOptions.Min = int.TryParse(ConfigurationManager.AppSettings["IconSize:Min"], out var minSize) ? minSize : 200;
            IconSizeOptions.Max = int.TryParse(ConfigurationManager.AppSettings["IconSize:Max"], out var maxSize) ? maxSize : 600;

            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                section.SectionInformation.ForceSave = true;
                config.Save();
            }
        }

    }
}
