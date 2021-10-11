using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NemLuuDan
{
    public class WiFiConnection
    {
        public bool testWiFi(string link)
        {
            WebClient client = new WebClient();
            try
            {
                Stream data = client.OpenRead(new Uri(link));
                StreamReader reader = new StreamReader(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
