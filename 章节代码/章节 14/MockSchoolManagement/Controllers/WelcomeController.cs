using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    public class WelcomeController : Controller
    {
        [Route("WC")]
        [Route("WC/Index")]
        public string Welcome()
        {
            return "我是Welcome控制器中的welcome操作方法";
        }
    }
}
