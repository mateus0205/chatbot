using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ChatIa.Services; // Ajuste para o seu namespace se for diferente

public class IAService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    // Injetamos o IConfiguration para ler a chave de forma segura
    public IAService(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    public async Task<string?> EnviarPerguntaIA(string pergunta, string especializacao)
    {
        // 1. Busca a chave dos Segredos/Variáveis de Ambiente
        string? apiKey = _configuration["GroqApiKey"];
        string url = "https://api.groq.com/openai/v1/chat/completions";

        if (string.IsNullOrEmpty(apiKey))
        {
            return "Erro: Chave de API não configurada corretamente.";
        }

        // 2. Configura os headers de forma limpa
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // 3. Payload com modelo atualizado (llama-3.3-70b-versatile)
        var payload = new
        {
            model = "llama-3.3-70b-versatile", 
            messages = new[]
            {
                new { role = "system", content = string.IsNullOrWhiteSpace(especializacao) ? "Você é um assistente." : especializacao },
                new { role = "user", content = pergunta }
            }
        };

        try 
        {
            string json = JsonSerializer.Serialize(payload);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await _client.PostAsync(url, conteudo);
            string resultado = await resposta.Content.ReadAsStringAsync();

            if (!resposta.IsSuccessStatusCode)
            {
                // Log para você ver no terminal se algo der errado
                Console.WriteLine($"Erro Groq: {resultado}");
                return $"Erro na API: {resposta.StatusCode}";
            }

            using JsonDocument doc = JsonDocument.Parse(resultado);
            var root = doc.RootElement;

            return root.GetProperty("choices")[0]
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