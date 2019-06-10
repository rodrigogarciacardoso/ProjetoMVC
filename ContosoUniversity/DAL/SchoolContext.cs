using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Escritorio> Escritorios { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Curso>()
                .HasMany(c => c.Instrutor).WithMany(i => i.Cursos)
                .Map(t => t.MapLeftKey("CursoID")
                    .MapRightKey("InstrutorID")
                    .ToTable("CursoInstrutor"));

            modelBuilder.Entity<Departamento>().MapToStoredProcedures();
        }
    }
}