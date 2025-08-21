using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hubiso.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do material é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome do Material")]
        public string Nome { get; set; } = string.Empty; // <-- CORREÇÃO AQUI: texto vazio

        [StringLength(255)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A unidade de medida é obrigatória.")]
        [StringLength(10)]
        [Display(Name = "Unidade de Medida (Ex: Kg, L, Un)")]
        public string UnidadeMedida { get; set; } = string.Empty; // <-- CORREÇÃO AQUI: texto vazio

        [Required(ErrorMessage = "O preço de venda é obrigatório.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Preço de Venda")]
        public decimal PrecoVenda { get; set; }

        [Required(ErrorMessage = "O peso/volume da embalagem é obrigatório.")]
        [Column(TypeName = "decimal(18, 3)")]
        [Display(Name = "Peso/Volume da Embalagem Padrão")]
        public decimal PesoEmbalagem { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}