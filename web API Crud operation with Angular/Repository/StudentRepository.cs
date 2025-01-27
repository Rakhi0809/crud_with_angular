using web_API_Crud_operation_with_Angular.Data;
using web_API_Crud_operation_with_Angular.Model;

namespace web_API_Crud_operation_with_Angular.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }
    }
}
