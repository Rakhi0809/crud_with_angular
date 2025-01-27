using web_API_Crud_operation_with_Angular.Model;

namespace web_API_Crud_operation_with_Angular.Repository
{
    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
    }
}
