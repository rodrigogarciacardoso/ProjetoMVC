using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instrutor> Instructors { get; set; }
        public IEnumerable<Curso> Courses { get; set; }
        public IEnumerable<Matricula> Enrollments { get; set; }
    }
}

