using System;
using System.ComponentModel.DataAnnotations;

namespace MockSchoolManagement.Models
{
    public class TodoItem
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}