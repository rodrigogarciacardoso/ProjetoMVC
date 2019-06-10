using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Escritorio
    {
        [Key]
        [ForeignKey("Instrutor")]
        public int InstrutorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Localização do Escritório")]
        public string Localizacao { get; set; }

        public virtual Instrutor Instrutor { get; set; }
    }
}