using Microsoft.AspNetCore.Http;
using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class StudentCreateViewModel
    {
         
        [Display(Name = "名字")]
        [Required(ErrorMessage = "请输入名字,它不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 主修科目
        /// </summary>
        [Required(ErrorMessage = "请选择一门科目")]
        [Display(Name = "主修科目")]
        public MajorEnum? Major { get; set; }
        [Display(Name = "电子邮件")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
       ErrorMessage = "邮箱的格式不正确")]
        [Required(ErrorMessage = "请输入邮箱地址,它不能为空")]
        public string Email { get; set; }

        [Display(Name = "头像")]     
        public List<IFormFile> Photos { get; set; }

        /// <summary>
        /// 入学时间
        /// </summary>
        [Display(Name = "入学时间")]
        public DateTime EnrollmentDate { get; set; }

       

    }
}
