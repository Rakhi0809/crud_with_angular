using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_API_Crud_operation_with_Angular.Data;
using web_API_Crud_operation_with_Angular.Model;
using web_API_Crud_operation_with_Angular.Repository;

namespace web_API_Crud_operation_with_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CsvController(ApplicationDbContext context)
        {
            _context = context;
        }
        // static Data Generation[HttpGet("generate")]
        [HttpGet]
        public async Task<IActionResult> GenerateCsv()
        {
            var students = await _context.Students.ToArrayAsync(); // corrected variable name
            var csvContent = GenerateCsv(students); // passed the correct variable
            var csvBytes = Encoding.UTF8.GetBytes(csvContent);

            return File(csvBytes, "text/csv", "Students.csv");
        }

        private string GenerateCsv(Student[] students) // Change to accept array
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id,Name,Email,Contact");
            foreach (var student in students)
            {
                sb.AppendLine($"{student.Id},{student.Name},{student.Email},{student.Contact}");
            }
            return sb.ToString();
        }

        // Import CSV
        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var students = new List<Student>();

                string? line;
                int lineNumber = 0;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    lineNumber++;

                    if (lineNumber == 1) continue; // Skip header row

                    var columns = line.Split(',');
                    if (columns.Length < 4) // Ensure there are enough columns
                    {
                        return BadRequest($"Invalid data format on line {lineNumber}.");
                    }

                    // Parse and create a new student
                    students.Add(new Student
                    {
                        Id = int.Parse(columns[0]), // Use 0 for Id if auto-generated
                        Name = columns[1].Trim(),
                        Email = columns[2].Trim(),
                        Contact = columns[3].Trim()
                    });
                }

                // Add parsed students to the database
                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();

                return Ok("CSV file imported successfully.");
            }

        }
    }
}
