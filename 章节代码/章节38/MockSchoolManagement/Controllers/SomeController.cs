using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class SomeController : Controller
    {
        public string ABC()
        {
            return "我是方法ABP，只要拥有Admin或者User角色即可访问我。";
        }

        [Authorize(Roles = "Admin")]
        public string XYZ()
        {
            return "我是方法XYZ，只有Admi角色才能访问我。";

        }

        [AllowAnonymous]
        public string Anyone()
        {
            return "任何人都可以访问 Anyone()，因为我添加了AllowAnonymous属性。";
        }





    }



  
   
}
