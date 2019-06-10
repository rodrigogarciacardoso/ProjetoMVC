using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Matricula
    {
        public int MatriculaID { get; set; }
        public int CursoID { get; set; }
        public int AlunoID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public virtual Curso Curso { get; set; }
        public virtual Aluno Aluno { get; set; }
    }
}

