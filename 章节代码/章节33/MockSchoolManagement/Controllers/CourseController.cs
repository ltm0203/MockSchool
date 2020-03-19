using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Infrastructure;

namespace MockSchoolManagement.Controllers
{
    public class CourseController : Controller
    {

        

      

        // 不填写 [HttpGet]默认为处理GET请求
        public ActionResult Index()
        {
            return View();
        }

    } 
         
    
}