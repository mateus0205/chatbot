export async function perguntarIA(pergunta, especializacao){

    // ADICIONE O CAMINHO DA API E DO MÉTODO (Ex: /api/chat/enviar)
    const resposta = await fetch("https://chatbot-com-ia-953v.onrender.com/api/chat/enviar", {
        method: "POST",
        headers:{
            "Content-Type":"application/json"
        },
        body: JSON.stringify({
            pergunta: pergunta,
            especializacao: especializacao
        })
    })

    if (!resposta.ok) {
        const erroTexto = await resposta.text();
        console.error("Erro detalhado do servidor:", erroTexto);
        throw new Error("Falha na resposta da IA");
    }

    return await resposta.json();
}