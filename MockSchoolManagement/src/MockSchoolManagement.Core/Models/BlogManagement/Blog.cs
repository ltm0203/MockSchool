using MockSchoolManagement.Models.BlogManagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockSchoolManagement.Models.Blogs
{
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string BloggerName { get; set; }

        public virtual BlogImage blogImage { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}