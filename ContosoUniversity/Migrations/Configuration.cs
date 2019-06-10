namespace ContosoUniversity.Migrations
{
    using ContosoUniversity.DAL;
    using ContosoUniversity.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ContosoUniversity.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SchoolContext context)
        {
            var students = new List<Aluno>
            {
                new Aluno { PrimeiroNome = "Carson",   UltimoNome = "Alexander",
                    DataMatricula = DateTime.Parse("2010-09-01") },
                new Aluno { PrimeiroNome = "Meredith", UltimoNome = "Alonso",
                    DataMatricula = DateTime.Parse("2012-09-01") },
                new Aluno { PrimeiroNome = "Arturo",   UltimoNome = "Anand",
                    DataMatricula = DateTime.Parse("2013-09-01") },
                new Aluno { PrimeiroNome = "Gytis",    UltimoNome = "Barzdukas",
                    DataMatricula = DateTime.Parse("2012-09-01") },
                new Aluno { PrimeiroNome = "Yan",      UltimoNome = "Li",
                    DataMatricula = DateTime.Parse("2012-09-01") },
                new Aluno { PrimeiroNome = "Peggy",    UltimoNome = "Justice",
                    DataMatricula = DateTime.Parse("2011-09-01") },
                new Aluno { PrimeiroNome = "Laura",    UltimoNome = "Norman",
                    DataMatricula = DateTime.Parse("2013-09-01") },
                new Aluno { PrimeiroNome = "Nino",     UltimoNome = "Olivetto",
                    DataMatricula = DateTime.Parse("2005-09-01") }
            };


            students.ForEach(s => context.Alunos.AddOrUpdate(p => p.UltimoNome, s));
            context.SaveChanges();

            var instructors = new List<Instrutor>
            {
                new Instrutor { PrimeiroNome = "Kim",     UltimoNome = "Abercrombie",
                    DataContratacao = DateTime.Parse("1995-03-11") },
                new Instrutor { PrimeiroNome = "Fadi",    UltimoNome = "Fakhouri",
                    DataContratacao = DateTime.Parse("2002-07-06") },
                new Instrutor { PrimeiroNome = "Roger",   UltimoNome = "Harui",
                    DataContratacao = DateTime.Parse("1998-07-01") },
                new Instrutor { PrimeiroNome = "Candace", UltimoNome = "Kapoor",
                    DataContratacao = DateTime.Parse("2001-01-15") },
                new Instrutor { PrimeiroNome = "Roger",   UltimoNome = "Zheng",
                    DataContratacao = DateTime.Parse("2004-02-12") }
            };
            instructors.ForEach(s => context.Instrutores.AddOrUpdate(p => p.UltimoNome, s));
            context.SaveChanges();

            var departments = new List<Departamento>
            {
                new Departamento { Nome = "English",     Despesas = 350000,
                    DataInicio = DateTime.Parse("2007-09-01"),
                    InstrutorID  = instructors.Single( i => i.UltimoNome == "Abercrombie").ID },
                new Departamento { Nome = "Mathematics", Despesas = 100000,
                    DataInicio = DateTime.Parse("2007-09-01"),
                    InstrutorID  = instructors.Single( i => i.UltimoNome == "Fakhouri").ID },
                new Departamento { Nome = "Engineering", Despesas = 350000,
                    DataInicio = DateTime.Parse("2007-09-01"),
                    InstrutorID  = instructors.Single( i => i.UltimoNome == "Harui").ID },
                new Departamento { Nome = "Economics",   Despesas = 100000,
                    DataInicio = DateTime.Parse("2007-09-01"),
                    InstrutorID  = instructors.Single( i => i.UltimoNome == "Kapoor").ID }
            };
            departments.ForEach(s => context.Departamentos.AddOrUpdate(p => p.Nome, s));
            context.SaveChanges();

            var courses = new List<Curso>
            {
                new Curso {CursoID = 1050, Titulo = "Chemistry",      Creditos = 3,
                  DepartamentoID = departments.Single( s => s.Nome == "Engineering").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 4022, Titulo = "Microeconomics", Creditos = 3,
                  DepartamentoID = departments.Single( s => s.Nome == "Economics").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 4041, Titulo = "Macroeconomics", Creditos = 3,
                  DepartamentoID = departments.Single( s => s.Nome == "Economics").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 1045, Titulo = "Calculus",       Creditos = 4,
                  DepartamentoID = departments.Single( s => s.Nome == "Mathematics").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 3141, Titulo = "Trigonometry",   Creditos = 4,
                  DepartamentoID = departments.Single( s => s.Nome == "Mathematics").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 2021, Titulo = "Composition",    Creditos = 3,
                  DepartamentoID = departments.Single( s => s.Nome == "English").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
                new Curso {CursoID = 2042, Titulo = "Literature",     Creditos = 4,
                  DepartamentoID = departments.Single( s => s.Nome == "English").DepartamentoID,
                  Instrutor = new List<Instrutor>()
                },
            };
            courses.ForEach(s => context.Cursos.AddOrUpdate(p => p.CursoID, s));
            context.SaveChanges();

            var officeAssignments = new List<Escritorio>
            {
                new Escritorio {
                    InstrutorID = instructors.Single( i => i.UltimoNome == "Fakhouri").ID,
                    Localizacao = "Smith 17" },
                new Escritorio {
                    InstrutorID = instructors.Single( i => i.UltimoNome == "Harui").ID,
                    Localizacao = "Gowan 27" },
                new Escritorio {
                    InstrutorID = instructors.Single( i => i.UltimoNome == "Kapoor").ID,
                    Localizacao = "Thompson 304" },
            };
            officeAssignments.ForEach(s => context.Escritorios.AddOrUpdate(p => p.InstrutorID, s));
            context.SaveChanges();

            AddOrUpdateInstructor(context, "Chemistry", "Kapoor");
            AddOrUpdateInstructor(context, "Chemistry", "Harui");
            AddOrUpdateInstructor(context, "Microeconomics", "Zheng");
            AddOrUpdateInstructor(context, "Macroeconomics", "Zheng");

            AddOrUpdateInstructor(context, "Calculus", "Fakhouri");
            AddOrUpdateInstructor(context, "Trigonometry", "Harui");
            AddOrUpdateInstructor(context, "Composition", "Abercrombie");
            AddOrUpdateInstructor(context, "Literature", "Abercrombie");

            context.SaveChanges();

            var enrollments = new List<Matricula>
            {
                new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Alexander").ID,
                    CursoID = courses.Single(c => c.Titulo == "Chemistry" ).CursoID,
                    Grade = Grade.A
                },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Alexander").ID,
                    CursoID = courses.Single(c => c.Titulo == "Microeconomics" ).CursoID,
                    Grade = Grade.C
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Alexander").ID,
                    CursoID = courses.Single(c => c.Titulo == "Macroeconomics" ).CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                     AlunoID = students.Single(s => s.UltimoNome == "Alonso").ID,
                    CursoID = courses.Single(c => c.Titulo == "Calculus" ).CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                     AlunoID = students.Single(s => s.UltimoNome == "Alonso").ID,
                    CursoID = courses.Single(c => c.Titulo == "Trigonometry" ).CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Alonso").ID,
                    CursoID = courses.Single(c => c.Titulo == "Composition" ).CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Anand").ID,
                    CursoID = courses.Single(c => c.Titulo == "Chemistry" ).CursoID
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Anand").ID,
                    CursoID = courses.Single(c => c.Titulo == "Microeconomics").CursoID,
                    Grade = Grade.B
                 },
                new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Barzdukas").ID,
                    CursoID = courses.Single(c => c.Titulo == "Chemistry").CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Li").ID,
                    CursoID = courses.Single(c => c.Titulo == "Composition").CursoID,
                    Grade = Grade.B
                 },
                 new Matricula {
                    AlunoID = students.Single(s => s.UltimoNome == "Justice").ID,
                    CursoID = courses.Single(c => c.Titulo == "Literature").CursoID,
                    Grade = Grade.B
                 }
            };

            foreach (Matricula e in enrollments)
            {
                var enrollmentInDataBase = context.Matriculas.Where(
                    s =>
                         s.Aluno.ID == e.AlunoID &&
                         s.Curso.CursoID == e.CursoID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Matriculas.Add(e);
                }
            }
            context.SaveChanges();
        }

        void AddOrUpdateInstructor(SchoolContext context, string courseTitle, string instructorName)
        {
            var crs = context.Cursos.SingleOrDefault(c => c.Titulo == courseTitle);
            var inst = crs.Instrutor.SingleOrDefault(i => i.UltimoNome == instructorName);
            if (inst == null)
                crs.Instrutor.Add(context.Instrutores.Single(i => i.UltimoNome == instructorName));
        }
    }
}
