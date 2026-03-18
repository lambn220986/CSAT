using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CSAT
{
    public static class DeviceHelper
    {

        public static string GetDeviceName()
        {
            return Environment.MachineName;
        }

        public static string GetLocalIPv4()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                var ip = host.AddressList
                             .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

                return ip?.ToString() ?? "0.0.0.0";
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        public static (string DeviceName, string IPAddress) GetDeviceInfo()
        {
            return (GetDeviceName(), GetLocalIPv4());
        }
    }
}
