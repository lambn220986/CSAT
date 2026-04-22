using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CSAT.Launcher
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

            var json = args.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(json))
            {
                var dataTest = new
                {
                    UserId = 5,
                    UserName = "TEST",
                    FullName = "NGUYỄN VĂN TEST",
                    DepartmentId = 10,
                    DepartmentName = "Phòng TEST"
                };

                json = JsonConvert.SerializeObject(dataTest);

                json = "eyJVc2VySWQiOjUsIlVzZXJOYW1lIjoiVEVTVCIsIkZ1bGxOYW1lIjoiTkdVWeG7hE4gVsOCTiBURVNUIiwiRGVwYXJ0bWVudElkIjoxMCwiRGVwYXJ0bWVudE5hbWUiOiJQaMOibmcgVEVTVCJ9";
            }
            if (string.IsNullOrWhiteSpace(json))
            {
                MessageBox.Show(
                    "Không thể xác định thông tin đăng nhập.\nVui lòng khởi động ứng dụng từ hệ thống HIS.",
                    "Khởi động không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            log.Info(json);
            string processName = "CSAT";
            string exePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "CSAT.exe");
            try
            {
                var running = Process.GetProcessesByName(processName);

                if (running.Length > 0)
                {
                    foreach (var p in running)
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }

                if (File.Exists(exePath))
                {
                    string jsonTest = JsonConvert.SerializeObject(json);
                    var psi = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = $"\"{json.Replace("\"", "\\\"")}\"",
                        UseShellExecute = false
                    };
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy CSAT.exe");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
