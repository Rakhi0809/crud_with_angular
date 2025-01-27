using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_API_Crud_operation_with_Angular.Repository;

namespace web_API_Crud_operation_with_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studentRepo;

        public StudentsController(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        [Route("[Action]")]
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            return Ok(_studentRepo.GetAllStudents());
        }
    }
}
