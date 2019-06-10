using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Aluno : Pessoa
    {
        [DataType(DataType.Date)]
        [Display(Name = "Data da Matrícula")]
        public DateTime DataMatricula { get; set; }

        public virtual ICollection<Matricula> Matriculas { get; set; }
    }
}