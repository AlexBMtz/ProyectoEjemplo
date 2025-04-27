using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.DTO.OutputDTO;
using System.Data;
using TestMAUIAppApi.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMAUIAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(DataContext dataContext, UserManager<User> userManager) : ControllerBase
    {
        private readonly DataContext dataContext = dataContext;
        private readonly UserManager<User> userManager = userManager;

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<ActionResult<List<Student>>> Get()
        {
            return Ok(await dataContext.Students.Include(u => u.User).ToListAsync());
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<Student>> Post([FromBody] StudentOutputDTO studentDTO)
        {
            // Crear el User
            var user = new User
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                Email = studentDTO.Email,
                UserName = studentDTO.Email
            };

            var createUserResult = await userManager.CreateAsync(user, "Student1234!");

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return BadRequest($"Error creating user: {errors}");
            }

            await userManager.AddToRoleAsync(user, "Student");

            // Crear el Student con Navigation Property
            var student = new Student
            {
                User = user // Aquí asignamos el User directamente
            };

            dataContext.Students.Add(student);
            await dataContext.SaveChangesAsync();

            return Ok(student);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
