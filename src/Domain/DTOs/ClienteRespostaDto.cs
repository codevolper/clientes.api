using System.Text.Json.Serialization;

namespace Clientes.API.Domain.DTOs
{
    public class ClienteRespostaDto
    {
        [JsonPropertyName("idCliente")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;        

        [JsonPropertyName("detalheErro")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DetalheErro { get; set; }
    }
}
