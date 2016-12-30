using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;


namespace RadacodeService
{
    public class SiteMonitor
    {
        string filePath = Environment.CurrentDirectory + @"/../SiteMonitoring.txt";
        object obj = new object();

        public SiteMonitor()
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "DateTime\t\t\tSiteName\t\t\t\t\tStatus\n");
        }


        public void Start()
        {
            // Set callback method.
            TimerCallback tm = new TimerCallback(OnTimer);
            
            // Google.com timer. Period = every 2 minute 
            Timer googleTr = new Timer(tm, "https://www.google.com", 0, 2000 * 60);
            // Apple.com timer. Period = every 2 minute 
            Timer appleTr = new Timer(tm, "http://www.apple.com", 0, 5000 * 60);
            
            // Set Microsoft.com timer.
            TimeSpan nowTime = DateTime.Now.TimeOfDay;
            TimeSpan dueTime = new TimeSpan(23, 15, 0);

            // Calculate time to next 22:15.
            TimeSpan timeToStart = (nowTime < dueTime) ? (dueTime - nowTime) : (TimeSpan.FromDays(1) - nowTime + dueTime);

            // Periodicity == every 2 days.
            TimeSpan interval = TimeSpan.FromDays(2);
            
            Timer microsoftTr = new Timer(tm, "https://www.microsoft.com", (int) timeToStart.TotalMilliseconds, (int) interval.TotalMilliseconds);
        }

        private void OnTimer(object siteUriObj)
        {
            string siteUri = siteUriObj as string;
            string status = GetSiteStatus(siteUri);
            WriteEntry(DateTime.Now, siteUri, status);
        }

        private string GetSiteStatus(string siteUri)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(siteUri);
                request.Method = "HEAD";
                var response = (HttpWebResponse)request.GetResponse();

                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Writes information about web-site to the file.
        /// </summary>
        private void WriteEntry(DateTime dateTime, string siteName, string status)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(dateTime.ToUniversalTime() + "\t" + siteName + "\t" + status);
                    writer.Flush();
                }
            }
        }
    }
}
