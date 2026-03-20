using System.Text;
using System.Text.Json;

public class IAService
{
    private readonly HttpClient _client;

    public IAService(HttpClient client)
    {
        _client = client;
    }

    public async Task<string?> EnviarPerguntaIA(string pergunta, string especializacao)
    {
        string apiKey = "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";
        string url = "https://api.groq.com/openai/v1/chat/completions";

        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        var payload = new
        {
            model = "llama3-70b-8192",
            messages = new[]
            {
                new { role = "system", content = especializacao },
                new { role = "user", content = pergunta }
            }
        };

        string json = JsonSerializer.Serialize(payload);

        var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

        var resposta = await _client.PostAsync(url, conteudo);

        string resultado = await resposta.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(resultado);

        var root = doc.RootElement;

        if (!root.TryGetProperty("choices", out JsonElement choices))
            return "Erro da API";

        return choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}