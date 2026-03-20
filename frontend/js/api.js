export async function perguntarIA(pergunta, especializacao) {

    // A URL deve terminar em /api/chat (que é o que está no seu Controller)
    const resposta = await fetch("https://chatbot-com-ia-953v.onrender.com/api/chat", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            pergunta: pergunta,
            especializacao: especializacao
        })
    });

    if (!resposta.ok) {
        // Isso ajuda a debugar se o servidor retornar erro 500 ou 404
        const erroTexto = await resposta.text();
        console.error("Erro do Servidor:", erroTexto);
        throw new Error("Erro na rede ou no servidor");
    }

    // Como o seu Controller retorna Ok(new { resposta = resposta })
    // Você precisa pegar a propriedade .resposta do JSON
    // No api.js, mantenha assim:
    const dados = await resposta.json();
    return dados.resposta; // Isso retorna a string "Olá, sou um programador..." 
}