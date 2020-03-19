using Microsoft.AspNetCore.Mvc;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    public class HomeController:Controller
    {
        private readonly IStudentRepository _studentRepository;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        //返回学生的名字
        public string Index()
        {
            var _studentRepository = new MockStudentRepository();
             
            return _studentRepository.GetStudent(1).Name;
        }


        public ViewResult Details()
        {
            Student model = _studentRepository.GetStudent(1);
            ViewBag.PageTitle = "学生详情";

            return View(model);
        }

        //public ViewResult Details()
        //{
        //    Student model = _studentRepository.GetStudent(1);
        //    //将PageTitle和Student模型对象存储在ViewBag
        //    //我们正在使用动态属性PageTitle和Student
        //    ViewBag.PageTitle = "学生详情";
        //    ViewBag.Student = model;

        //    return View();
        //}


        ///// <summary>
        ///// 返回视图类型
        ///// </summary>
        ///// <returns></returns>
        //public ViewResult Details()
        //{
        //    Student model = _studentRepository.GetStudent(1);
        //    //使用ViewData将PageTitle和Student模型传递给View
        //    ViewData["PageTitle"] = "Student Details";
        //    ViewData["Student"] = model;

        //    return View();
        //}

        //public ObjectResult Details()
        //{
        //    //遵循内容协商
        //    Student model = _studentRepository.GetStudent(1);
        //    return new ObjectResult(model);

        //}

        //public JsonResult Details()
        //{
        //    Student model = _studentRepository.GetStudent(1);
        //    return Json(model);

        //}
    }
}
