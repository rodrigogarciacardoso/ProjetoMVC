using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Aluno>
            {
            new Aluno{PrimeiroNome="Carson",UltimoNome="Alexander",DataMatricula=DateTime.Parse("2005-09-01")},
            new Aluno{PrimeiroNome="Meredith",UltimoNome="Alonso",DataMatricula=DateTime.Parse("2002-09-01")},
            new Aluno{PrimeiroNome="Arturo",UltimoNome="Anand",DataMatricula=DateTime.Parse("2003-09-01")},
            new Aluno{PrimeiroNome="Gytis",UltimoNome="Barzdukas",DataMatricula=DateTime.Parse("2002-09-01")},
            new Aluno{PrimeiroNome="Yan",UltimoNome="Li",DataMatricula=DateTime.Parse("2002-09-01")},
            new Aluno{PrimeiroNome="Peggy",UltimoNome="Justice",DataMatricula=DateTime.Parse("2001-09-01")},
            new Aluno{PrimeiroNome="Laura",UltimoNome="Norman",DataMatricula=DateTime.Parse("2003-09-01")},
            new Aluno{PrimeiroNome="Nino",UltimoNome="Olivetto",DataMatricula=DateTime.Parse("2005-09-01")}
            };

            students.ForEach(s => context.Alunos.Add(s));
            context.SaveChanges();
            var courses = new List<Curso>
            {
            new Curso{CursoID=1050,Titulo="Chemistry",Creditos=3,},
            new Curso{CursoID=4022,Titulo="Microeconomics",Creditos=3,},
            new Curso{CursoID=4041,Titulo="Macroeconomics",Creditos=3,},
            new Curso{CursoID=1045,Titulo="Calculus",Creditos=4,},
            new Curso{CursoID=3141,Titulo="Trigonometry",Creditos=4,},
            new Curso{CursoID=2021,Titulo="Composition",Creditos=3,},
            new Curso{CursoID=2042,Titulo="Literature",Creditos=4,}
            };
            courses.ForEach(s => context.Cursos.Add(s));
            context.SaveChanges();
            var enrollments = new List<Matricula>
            {
            new Matricula{AlunoID=1,CursoID=1050,Grade=Grade.A},
            new Matricula{AlunoID=1,CursoID=4022,Grade=Grade.C},
            new Matricula{AlunoID=1,CursoID=4041,Grade=Grade.B},
            new Matricula{AlunoID=2,CursoID=1045,Grade=Grade.B},
            new Matricula{AlunoID=2,CursoID=3141,Grade=Grade.F},
            new Matricula{AlunoID=2,CursoID=2021,Grade=Grade.F},
            new Matricula{AlunoID=3,CursoID=1050},
            new Matricula{AlunoID=4,CursoID=1050,},
            new Matricula{AlunoID=4,CursoID=4022,Grade=Grade.F},
            new Matricula{AlunoID=5,CursoID=4041,Grade=Grade.C},
            new Matricula{AlunoID=6,CursoID=1045},
            new Matricula{AlunoID=7,CursoID=3141,Grade=Grade.A},
            };
            enrollments.ForEach(s => context.Matriculas.Add(s));
            context.SaveChanges();
        }
    }
}