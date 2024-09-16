using LearnAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // Object instance = new Constructor();
        private static List<Student> students = new List<Student>
        {
            new Student {ID = 1, Name = "John", Age = 18 },
            new Student {ID = 2, Name = "Jane", Age = 19 },
        };

        [HttpGet("/studentName")]
        // Iterator - Iteration - Loop
        // Task is a promise - Asynchronous
        public async Task<IActionResult> getStudent()
        {
            return await Task.FromResult(Ok(students));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> getStudentById(int id)
        {
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return await Task.FromResult(NotFound());
            }

            return await Task.FromResult(Ok(student));
        }
        [HttpPost]
        public async Task<ActionResult<Student>> createNewStudent(Student student)
        {
            student.ID = students.Count + 1;
            students.Add(student);
            return await Task.
                    FromResult(
                CreatedAtAction(
                    nameof(getStudentById),
                    new { id = student.ID }, student)
                );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> updateStudent(int id, Student student)
        {
            var studentToUpdate = students.FirstOrDefault(s => s.ID == id);
            if (studentToUpdate == null)
            {
                return await Task.FromResult(NotFound());
            }

            studentToUpdate.Name = student.Name;
            studentToUpdate.Age = student.Age;

            return await Task.FromResult(Ok(studentToUpdate));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteStudent(int id)
        {
            var studentToDelete = students.FirstOrDefault(s => s.ID == id);
            if (studentToDelete == null)
            {
                return await Task.FromResult(NotFound());
            }

            students.Remove(studentToDelete);
            return await Task.FromResult(NoContent());
        }

    }
}