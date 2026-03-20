import { enviarPergunta } from "./chat.js"

document
.getElementById("btnPerguntar")
.addEventListener("click", enviarPergunta)

// Permite enviar apertando a tecla "Enter"
document.getElementById("pergunta").addEventListener("keypress", function(event) {
    if (event.key === "Enter") {
        event.preventDefault(); // Evita quebra de linha no input
        document.getElementById("btnPerguntar").click(); // Simula o clique no botão
    }
});