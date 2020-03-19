using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class EmailAddressViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name ="邮箱地址:")]
        public string Email { get; set; }
    }
}
