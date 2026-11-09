/* Interações visuais compartilhadas. Não altera os dados enviados aos controllers. */
(function () {
    "use strict";

    function configurarVisibilidadeSenha(botao) {
        const idCampo = botao.getAttribute("data-password-toggle");
        const campo = document.getElementById(idCampo);
        if (!campo) return;

        const mostrar = campo.type === "password";
        campo.type = mostrar ? "text" : "password";
        botao.setAttribute("aria-pressed", mostrar ? "true" : "false");
        botao.setAttribute("aria-label", mostrar ? "Ocultar senha" : "Mostrar senha");

        const olhoAberto = botao.querySelector("[data-eye-open]");
        const olhoFechado = botao.querySelector("[data-eye-closed]");
        if (olhoAberto) olhoAberto.hidden = mostrar;
        if (olhoFechado) olhoFechado.hidden = !mostrar;
    }

    document.addEventListener("click", function (event) {
        const botao = event.target.closest("[data-password-toggle]");
        if (botao) configurarVisibilidadeSenha(botao);
    });
})();
