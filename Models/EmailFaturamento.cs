using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hubiso.Models
{
    public class EmailFaturamento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O endereço de e-mail é obrigatório.")]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [Display(Name = "E-mail para Faturamento")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // --- A Relação "Um-para-Muitos" ---
        // O E-mail PERTENCE a UM Cliente.

        [Required(ErrorMessage = "É obrigatório associar o e-mail a um cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; } // A Chave Estrangeira

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; } // A Propriedade de Navegação
    }
}