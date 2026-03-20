export async function perguntarIA(pergunta, especializacao){

    const resposta = await fetch("https://chatbot-com-ia-953v.onrender.com", {
        method: "POST",
        headers:{
            "Content-Type":"application/json"
        },
        body: JSON.stringify({
            pergunta: pergunta,
            especializacao: especializacao
        })
    })

    return await resposta.json()
}