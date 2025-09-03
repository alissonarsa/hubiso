using System.Text.Json.Serialization;

namespace hubiso.Models
{
    // representa os dados que a BrasilAPI nos devolve.
    public class CnpjResponse
    {
        [JsonPropertyName("razao_social")]
        public string? RazaoSocial { get; set; }

        [JsonPropertyName("nome_fantasia")]
        public string? NomeFantasia { get; set; }

        // Adicionarei mais campos aqui no futuro, como o endereço. Por agora, isto é suficiente.
    }
}