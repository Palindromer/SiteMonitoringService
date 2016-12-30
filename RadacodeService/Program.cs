using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RadacodeService
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start OWIN host. 
            WebApp.Start<Startup>(url: "http://localhost:9000/");
            
            // Start Service.
            SiteMonitorService.Configure();
        }
    }
}
