using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public abstract class Pessoa
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Último Nome")]
        public string UltimoNome { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Nome não pode ter mais que 50 caracteres.")]
        [Column("FirstName")]
        [Display(Name = "Primeiro Nome")]
        public string PrimeiroNome { get; set; }

        [Display(Name = "Nome Completo")]
        public string NomeCompleto
        {
            get
            {
                return $"{UltimoNome}, {PrimeiroNome}";
            }
        }
    }
}