using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class DepartmentsController:Controller
    {
       
       
        public string Index()
        {
            return "我是Departments控制器的Index()操作方法 ";
        }
       
        //[Route("")]//使 List()成为默认路由入口
        public string List()
        {
            return "我是Departments控制器的List()操作方法 ";
        }
         

        public string Details()
        {
            return "我是Departments控制器的Details()操作方法 ";

        }
    }
}
