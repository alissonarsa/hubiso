using System.ComponentModel.DataAnnotations;

namespace hubiso.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18)]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Razão Social é obrigatória.")]
        [StringLength(150)]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "Nome Fantasia")]
        public string? NomeFantasia { get; set; }

        [StringLength(20)]
        [Display(Name = "Inscrição Estadual")]
        public string? InscricaoEstadual { get; set; }

        // --- CAMPOS DE ENDEREÇO ADICIONADOS ---

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

        // --- FIM DOS CAMPOS DE ENDEREÇO ---

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public ICollection<Solicitante>? Solicitantes { get; set; }
    }
}