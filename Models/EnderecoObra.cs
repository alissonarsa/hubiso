using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hubiso.Models
{
    public class EnderecoObra
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da obra/local é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome/Descrição da Obra")]
        public string DescricaoLocal { get; set; } = string.Empty;

        // --- Campos de Endereço ---
        [StringLength(9)]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [StringLength(200)]
        [Display(Name = "Logradouro")]
        public string? Logradouro { get; set; }

        [StringLength(20)]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [StringLength(100)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [StringLength(100)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(2)]
        [Display(Name = "Estado (UF)")]
        public string? Uf { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // --- A Relação "Um-para-Muitos" ---
        // O Endereço de Obra PERTENCE a UM Cliente.

        [Required(ErrorMessage = "É obrigatório associar o endereço a um cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; } // A Chave Estrangeira

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; } // A Propriedade de Navegação
    }
}