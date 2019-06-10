using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Instrutor : Pessoa
    {
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Contratação")]
        public DateTime DataContratacao { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }
        public virtual Escritorio OfficeAssignment { get; set; }
    }
}