using Microsoft.EntityFrameworkCore;
using web_API_Crud_operation_with_Angular.Model;

namespace web_API_Crud_operation_with_Angular.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
         
        {
            
        }
        public DbSet<Student>Students { get; set; }
        public DbSet<Gender>Genders { get; set; }
        public DbSet<Address>Address { get; set; }

    }
}
