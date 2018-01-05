
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Snail.Release.Business.Config;
using Snail.Release.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Controllers
{
    [Route("[controller]")]
    public class ReleaseController : Controller
    {
        [HttpGet]
        public ActionResult Release()
        {
            var relaseParams = HttpContext.Items[SystemConfig.Instance.CacheItemParams] as ReleaseParams;
            return base.View(relaseParams.Template);                        
        }       
    }
}
