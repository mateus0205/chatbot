export async function perguntarIA(pergunta, especializacao){

    const resposta = await fetch("http://localhost:5021/api/chat", {
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