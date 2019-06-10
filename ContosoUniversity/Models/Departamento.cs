using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Departamento
    {
        public int DepartamentoID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Despesas { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        public int? InstrutorID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Instrutor Administrador { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}