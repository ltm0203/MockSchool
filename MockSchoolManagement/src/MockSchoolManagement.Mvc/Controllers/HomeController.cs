using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.Application.Students;
using MockSchoolManagement.Application.Students.Dtos;
using MockSchoolManagement.EntityFrameworkCore;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using MockSchoolManagement.Security.CustomTokenProvider;
using MockSchoolManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Student, int> _studentRepository;
        private readonly AppDbContext _dbcontext;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger logger;
        private readonly IStudentService _studentService;

        //IDataProtector提供了Protect() 和 Unprotect() 方法,可以对数据进行加密或者解密。
        private readonly IDataProtector _protector;

        //CreateProtector()方法是IDataProtectionProvider接口提供的,它实例化的名称dataProtectionProvider的方法CreateProtector()需要数据保护字符串
        //所以需要注入我们声明的数据保护用途的链接字符串
        //目前我们只需要保密Student中的id信息。
        public HomeController(IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings, IRepository<Student, int> studentRepository, IStudentService studentService, AppDbContext dbcontext)
        {
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            _studentRepository = studentRepository;
            _protector = dataProtectionProvider.CreateProtector(
                 dataProtectionPurposeStrings.StudentIdRouteValue);
            _studentService = studentService;
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index(GetStudentInput input)
        {
            //获取分页结果
            var dtos = await _studentService.GetPaginatedResult(input);
            dtos.Data = dtos.Data.Select(s =>
            {
                //加密ID值并存储在EncryptedId属性中
                s.EncryptedId = _protector.Protect(s.Id.ToString());
                return s;
            }).ToList();
            return View(dtos);
        }

        // var count = query.Count();

        // query= query.OrderBy(sortBy).AsNoTracking();

        // var students = await PaginatedList<Student>.CreateAsync(query, pageNumber??1, pageSize);

        // return View(model);

        ////查询所有的学生信息
        //List<Student> model = _studentRepository.GetAll().OrderBy(sortBy).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList().Select(s=> {
        //    //加密ID值并存储在EncryptedId属性中
        //    s.EncryptedId = _protector.Protect(s.Id.ToString());
        //    return s;
        //}).ToList();
        ////将学生列表传递到视图
        //return View(model);

        #region 废弃排序

        //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        //var students = _studentRepository.GetAll();
        //switch (sortOrder)
        //{
        //    case "name_desc":
        //        students = students.OrderByDescending(s => s.Name);
        //        break;
        //    case "Date":
        //        students = students.OrderBy(s => s.EnrollmentDate);
        //        break;
        //    case "date_desc":
        //        students = students.OrderByDescending(s => s.EnrollmentDate);
        //        break;
        //    default:
        //        students = students.OrderBy(s => s.Name);
        //        break;
        //}

        //return View(students);

        #endregion 废弃排序

        // Details视图接收加密后的StudentID
        public ViewResult Details(string id)
        {
            var student = DecryptedStudent(id);

            //判断学生信息是否存在
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }
            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "学生详情"
            };
            homeDetailsViewModel.Student.EncryptedId =
                _protector.Protect(student.Id.ToString());

            //将ViewModel对象传递给View()方法
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //封装好了的上传图片代码
                var uniqueFileName = ProcessUploadedFile(model);
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    EnrollmentDate = model.EnrollmentDate,
                    // 将文件名保存在student对象的PhotoPath属性中。
                    //它将保存到数据库 Students的 表中
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Insert(newStudent);

                var encryptedId = _protector.Protect(newStudent.Id.ToString());

                return RedirectToAction("Details", new { id = encryptedId });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(string id)
        {
            var student = DecryptedStudent(id);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = id,
                Name = student.Name,
                Email = student.Email,
                Major = student.Major,
                ExistingPhotoPath = student.PhotoPath,
                EnrollmentDate = student.EnrollmentDate,
            };
            return View(studentEditViewModel);
        }

        //通过模型绑定，作为操作方法的参数
        //StudentEditViewModel 会接收来自Post请求的Edit表单数据
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                var student = DecryptedStudent(model.Id);

                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;
                student.EnrollmentDate = model.EnrollmentDate;

                //如果用户想要更改照片，可以上传新照片它会被模型对象上的Photo属性接收
                //如果用户没有上传照片，那么我们会保留现有的照片信息
                //因为兼容了多图上传所有这里的！=null判断修改判断Photos的总数是否大于0
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    //如果上传了新的照片，则必须显示新的照片信息
                    //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars", model.ExistingPhotoPath);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    //我们将保存新的照片到 wwwroot/images/avatars  文件夹中，并且会更新
                    //Student对象中的PhotoPath属性，然后最终都会将它们保存到数据库中
                    student.PhotoPath = ProcessUploadedFile(model);
                }

                //调用仓储服务中的Update方法，保存studnet对象中的数据，更新数据库表中的信息。
                Student updatedstudent = _studentRepository.Update(student);

                return RedirectToAction("index");
            }

            return View(model);
        }

        #region 删除功能

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(a => a.Id == id);

            if (student == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的学生信息";
                return View("NotFound");
            }

            await _studentRepository.DeleteAsync(a => a.Id == id);
            return RedirectToAction("Index");
        }

        #endregion 删除功能

        //public async Task<ActionResult> About()
        //{
        //    //获取IQueryable类型的Student，然后通过student.EnrollmentDate 进行分组
        //    var data =   from student in _studentRepository.GetAll()
        //             group student by student.EnrollmentDate into dateGroup

        //    select new EnrollmentDateGroupDto()
        //    {
        //        EnrollmentDate = dateGroup.Key,
        //        StudentCount = dateGroup.Count()
        //    };
        //    var dtos = await data.AsNoTracking().ToListAsync();
        //    return View(dtos);
        //}

        public async Task<ActionResult> About()
        {
            List<EnrollmentDateGroupDto> groups = new List<EnrollmentDateGroupDto>();
            //获取数据库的上下文链接
            var conn = _dbcontext.Database.GetDbConnection();
            try
            {    //打开数据库链接
                await conn.OpenAsync();
                //建立链接，因为非委托资源所以需要使用using进行内存资源的释放
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount   FROM Person  WHERE Discriminator = 'Student'  GROUP BY EnrollmentDate";
                    command.CommandText = query; //赋值需要执行的sql命令
                    DbDataReader reader = await command.ExecuteReaderAsync();//执行命令
                    if (reader.HasRows)//判断是否有返回行
                    {       //读取行数据 ，将返回值填充到视图模型中
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroupDto
                            {
                                EnrollmentDate = reader.GetDateTime(0),
                                StudentCount = reader.GetInt32(1)
                            };
                            groups.Add(row);
                        }
                    }
                    //释放使用的所有的资源
                    reader.Dispose();
                }
            }
            finally
            {  //关闭数据库连接。
                conn.Close();
            }
            return View(groups);
        }

        #region 私有方法

        /// <summary>
        /// 解密学生信息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        private Student DecryptedStudent(string id)
        {
            //使用 Unprotect()方法来解析学生id
            string decryptedId = _protector.Unprotect(id);
            int decryptedStudentId = Convert.ToInt32(decryptedId);
            Student student = _studentRepository.FirstOrDefault(s => s.Id == decryptedStudentId);
            return student;
        }

        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns> </returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像上传到wwwroot中的images/avatars文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的webHostEnvironment服务
                    //通过webHostEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars 文件夹
                        photo.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }

        #endregion 私有方法
    }

    //public ViewResult Details()
    //{
    //    Student model = _studentRepository.GetStudent(1);
    //    ViewBag.PageTitle = "学生详情";

    //    return View(model);
    //}

    //public ViewResult Details()
    //{
    //logger.LogTrace("Trace(跟踪) Log");
    //logger.LogDebug("Debug(调试) Log");
    //logger.LogInformation("信息(Information) Log");
    //logger.LogWarning("警告(Warning) Log");
    //logger.LogError("错误(Error) Log");
    //logger.LogCritical("严重(Critical) Log");
    //throw new Exception("在Details视图中抛出异常");

    // Student model = _studentRepository.GetStudent(1); //将PageTitle和Student模型对象存储在ViewBag
    // //我们正在使用动态属性PageTitle和Student ViewBag.PageTitle = "学生详情"; ViewBag.Student = model;

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