import { perguntarIA } from "./api.js";

// Função auxiliar para converter o Markdown (**, *, \n) em tags HTML (<strong>, <em>, <br>)
function formatarTextoIA(texto) {
    if (!texto) return "";
    
    // Converte **texto** para negrito
    let formatado = texto.replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>');
    
    // Converte *texto* para itálico
    formatado = formatado.replace(/\*(.*?)\*/g, '<em>$1</em>');
    
    // Converte quebras de linha reais em tags <br> para manter os parágrafos
    formatado = formatado.replace(/\n/g, '<br>');
    
    return formatado;
}

export async function enviarPergunta() {
    // 1. Pegando os elementos da tela
    const perguntaInput = document.getElementById("pergunta");
    const btnEnviar = document.getElementById("btnPerguntar");
    const especializacao = document.getElementById("especialista").value;
    const chat = document.getElementById("chat");
    const pergunta = perguntaInput.value;

    // 2. Se o campo estiver vazio, não faz nada
    if (!pergunta) return;

    // 3. BLOQUEIA O INPUT E O BOTÃO (Evita cliques duplos)
    perguntaInput.disabled = true;
    btnEnviar.disabled = true;
    btnEnviar.innerText = "Pensando..."; 

    // 4. Cria e exibe a mensagem do usuário
    const userMsg = document.createElement("div");
    userMsg.className = "message user";
    userMsg.innerText = pergunta; // innerText aqui por segurança
    chat.appendChild(userMsg);

    // Limpa o campo de texto
    perguntaInput.value = "";

    // 5. Cria e exibe a mensagem temporária da IA
    const botMsg = document.createElement("div");
    botMsg.className = "message bot";
    botMsg.innerText = "✨ Processando resposta...";
    chat.appendChild(botMsg);

    // Rola o chat para o final
    chat.scrollTop = chat.scrollHeight;

    try {
        // 6. Envia a pergunta para a API e aguarda a resposta
        const data = await perguntarIA(pergunta, especializacao);

        // 7. Atualiza a mensagem temporária com a resposta real formatada
        botMsg.innerHTML = formatarTextoIA(data.resposta);
        
    } catch (error) {
        // Caso dê algum erro na API
        botMsg.innerText = "⚠️ Desculpe, ocorreu um erro ao conectar com a IA.";
        console.error(error);
    }

    // 8. LIBERA O INPUT E O BOTÃO APÓS A RESPOSTA
    perguntaInput.disabled = false;
    btnEnviar.disabled = false;
    btnEnviar.innerText = "Enviar";
    perguntaInput.focus(); // Coloca o cursor de volta no input para continuar digitando

    // Rola o chat para o final novamente para garantir que a resposta toda apareça
    chat.scrollTop = chat.scrollHeight;
}