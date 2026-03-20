using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace ChatIa.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        // O ASP.NET injeta automaticamente as configurações e o cliente HTTP
        public ChatController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Perguntar([FromBody] ChatRequest req)
        {
            if (string.IsNullOrEmpty(req.pergunta))
            {
                return BadRequest(new { erro = "A pergunta não pode ser vazia." });
            }

            string? resposta = await EnviarPerguntaIA(req.pergunta, req.especializacao);
            return Ok(new { resposta = resposta });
        }

        private async Task<string?> EnviarPerguntaIA(string? pergunta, string? especializacao)
        {
            // Busca a chave GroqApiKey dos User Secrets ou Variáveis de Ambiente
            string? apiKey = _configuration["GroqApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return "Erro: Chave de API não configurada no servidor.";
            }

            var client = _httpClientFactory.CreateClient();
            string url = "https://api.groq.com/openai/v1/chat/completions";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
    {
        new { role = "system", content = string.IsNullOrWhiteSpace(especializacao) ? "Você é um assistente prestativo." : especializacao },
        new { role = "user", content = pergunta }
    }
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, conteudo);

                if (!response.IsSuccessStatusCode)
                {
                    var erroDetalhe = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\n--- ERRO DA GROQ --- \n{erroDetalhe}\n-------------------\n");
                    return $"Erro na API externa: {response.StatusCode}";
                }

                var resultado = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(resultado);
                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            catch (Exception ex)
            {
                return $"Erro interno: {ex.Message}";
            }
        }
    }

    public class ChatRequest
    {
        public string? pergunta { get; set; }
        public string? especializacao { get; set; }
    }
}

