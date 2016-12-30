using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace RadacodeService
{
    class SiteMonitorService
    {
        const string serviceName = "SiteMonitorService";
        public static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<SiteMonitorService>(service =>
                {
                    service.ConstructUsing(s => new SiteMonitorService());
                    service.WhenStarted(s => s.OnStart());
                    service.WhenStopped(s => s.OnStop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("SiteMonitorService");
                configure.SetDisplayName("SiteMonitorService");
                configure.SetDescription("site monitoring - windows service");
            });
        }


        private static Thread siteMonitorThread = null;
        /// <summary>
        /// Method that runs when the Windows Service starts up.  
        /// </summary>
        public void OnStart()
        {
            siteMonitorThread = new Thread(new ThreadStart(() => new SiteMonitor().Start()));
            siteMonitorThread.Start();
        }


        /// <summary>
        /// Method that runs when the Windows Service stops.  
        /// </summary>
        public void OnStop()
        {
            siteMonitorThread.Abort();
        }


        /// <summary>
        /// Checking of main service-thread status.
        /// </summary>
        public static  bool IsRunning
        {
            get
            {
#if !DEBUG
                return siteMonitorThread?.IsAlive ?? false;
#else
                int count = ServiceController.GetServices().Where(srv => srv.ServiceName == serviceName).Count();                          
                return count != 0;
#endif
            }
        }
    }

}
