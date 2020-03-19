using MockSchoolManagement.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Models.BlogManagement
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}