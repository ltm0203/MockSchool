using MockSchoolManagement.Application.Dtos;

namespace MockSchoolManagement.Application.Teachers
{
    public class GetDepartmentInput : PagedSortedAndFilterInput
    {
        public GetDepartmentInput()
        {
            Sorting = "Name";
            MaxResultCount = 3;
        }
    }
}