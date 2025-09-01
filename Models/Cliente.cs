using System.ComponentModel.DataAnnotations;

namespace hubiso.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18, ErrorMessage = "O CNPJ não pode exceder 18 caracteres.")] // Formato: 00.000.000/0000-00
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Razão Social é obrigatória.")]
        [StringLength(150)]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "Nome Fantasia")]
        public string? NomeFantasia { get; set; } // O '?' indica que o campo não é obrigatório

        [StringLength(20)]
        [Display(Name = "Inscrição Estadual")]
        public string? InscricaoEstadual { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true; // Por defeito, um novo cliente é sempre ativo
    }
}