using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RadacodeService
{
    public class StatusController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public bool CheckStatus()
        {
            return SiteMonitorService.IsRunning;
        }
    }
}
