using MockSchoolManagement.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Models.BlogManagement
{
    public class BlogImage
    {
        public int BlogImageId { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}