using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Models;
using MockSchoolManagement.ViewModels;

namespace MockSchoolManagement.Controllers {

    public class HomeController : Controller {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController (IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment) {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public ViewResult Index () {
            //查询所有的学生信息
            IEnumerable<Student> model = _studentRepository.GetAllStudents ();
            //将学生列表传递到视图
            return View (model);
        }

        public ViewResult Details (int id) {
            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel () {
                Student = _studentRepository.GetStudentById (id),
                PageTitle = "学生详情"
            };

            //将ViewModel对象传递给View()方法
            return View (homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create () {

            return View ();
        }

        [HttpPost]
        public IActionResult Create (StudentCreateViewModel model) {
            if (ModelState.IsValid) {
                string uniqueFileName = null;

                //如果传入模型对象中的Photo属性不为null,并且Count>0，则表示用户选择至少一个要上传的文件。
                if (model.Photos != null && model.Photos.Count > 0) {
                    //循环每个选定的文件
                    foreach (IFormFile photo in model.Photos) {
                        //必须将图像上传到wwwroot中的images文件夹
                        //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET  Core提供的HostingEnvironment服务
                        //通过HostingEnvironment服务去获取wwwroot文件夹的路径
                        string uploadsFolder = Path.Combine (_webHostEnvironment.WebRootPath, "images", "avatars");
                        //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                        uniqueFileName = Guid.NewGuid ().ToString () + "_" + photo.FileName;
                        string filePath = Path.Combine (uploadsFolder, uniqueFileName);
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars文件夹
                        photo.CopyTo (new FileStream (filePath, FileMode.Create));
                    }
                }
                Student newStudent = new Student {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    // 将文件名保存在student对象的PhotoPath属性中。
                    //它将保存到数据库 Students的 表中
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Insert (newStudent);
                return RedirectToAction ("Details", new { id = newStudent.Id });
            }
            return View ();
        }

        [HttpGet]
        public ViewResult Edit (int id) {
            Student student = _studentRepository.GetStudentById (id);
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Major = student.Major,
                ExistingPhotoPath = student.PhotoPath
            };
            return View (studentEditViewModel);
        }

        //通过模型绑定，作为操作方法的参数
        //StudentEditViewModel 会接收来自Post请求的Edit表单数据
        [HttpPost]
        public IActionResult Edit (StudentEditViewModel model) {

            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid) {
                //从数据库中查询正在编辑的学生信息
                Student student = _studentRepository.GetStudentById (model.Id);
                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;

                //如果用户想要更改照片，可以上传新照片它会被模型对象上的Photo属性接收
                //如果用户没有上传照片，那么我们会保留现有的照片信息
                //因为兼容了多图上传所有这里的！=null判断修改判断Photos的总数是否大于0
                if (model.Photos.Count > 0) {
                    //如果上传了新的照片，则必须显示新的照片信息
                    //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
                    if (model.ExistingPhotoPath != null) {
                        string filePath = Path.Combine (_webHostEnvironment.WebRootPath, "images", "avatars", model.ExistingPhotoPath);
                        System.IO.File.Delete (filePath);
                    }

                    //我们将保存新的照片到 wwwroot/images/avatars  文件夹中，并且会更新
                    //Student对象中的PhotoPath属性，然后最终都会将它们保存到数据库中
                    student.PhotoPath = ProcessUploadedFile (model);
                }

                //调用仓储服务中的Update方法，保存studnet对象中的数据，更新数据库表中的信息。
                Student updatedstudent = _studentRepository.Update (student);

                return RedirectToAction ("index");
            }

            return View (model);
        }

        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile (StudentCreateViewModel model) {
            string uniqueFileName = null;

            if (model.Photos.Count > 0) {
                foreach (var photo in model.Photos) {
                    //必须将图像上传到wwwroot中的images/avatars文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的webHostEnvironment服务
                    //通过webHostEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine (_webHostEnvironment.WebRootPath, "images", "avatars");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    uniqueFileName = Guid.NewGuid ().ToString () + "_" + photo.FileName;
                    string filePath = Path.Combine (uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    var fileStream = new FileStream (filePath, FileMode.Create);

                    //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars 文件夹
                    photo.CopyTo (fileStream);
                }
            }
            return uniqueFileName;
        }

    }
    //public ViewResult Details()
    //{
    //    Student model = _studentRepository.GetStudent(1);
    //    ViewBag.PageTitle = "学生详情";

    //    return View(model);
    //}

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