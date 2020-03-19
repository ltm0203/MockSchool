using MockSchoolManagement.Application.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MockSchoolManagement.Application.Students {


    public class GetStudentInput : PagedSortedAndFilterInput
    {            

        public GetStudentInput()
        {
            Sorting = "Id";
        }
    }
}