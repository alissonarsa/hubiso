using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hubiso.Models
{
    public class Solicitante
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do solicitante é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(50)]
        [Display(Name = "Departamento")]
        public string? Departamento { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // --- A Relação "Um-para-Muitos" ---
        // O Solicitante PERTENCE a UM Cliente.

        [Required(ErrorMessage = "É obrigatório associar o solicitante a um cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; } // A Chave Estrangeira

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; } // A Propriedade de Navegação
    }
}