(function () {
    function somenteNumeros(valor) {
        return (valor || "").replace(/\D/g, "");
    }

    function formatarTelefone(valor) {
        let numeros = somenteNumeros(valor).slice(0, 11);

        if (numeros.length <= 2) {
            return numeros;
        }

        if (numeros.length <= 6) {
            return `(${numeros.slice(0, 2)}) ${numeros.slice(2)}`;
        }

        if (numeros.length <= 10) {
            return `(${numeros.slice(0, 2)}) ${numeros.slice(2, 6)}-${numeros.slice(6)}`;
        }

        return `(${numeros.slice(0, 2)}) ${numeros.slice(2, 7)}-${numeros.slice(7)}`;
    }

    function formatarRa(valor) {
        let numeros = somenteNumeros(valor).slice(0, 7);

        if (numeros.length <= 4) {
            return numeros;
        }

        if (numeros.length <= 6) {
            return `${numeros.slice(0, 4)}/${numeros.slice(4)}`;
        }

        return `${numeros.slice(0, 4)}/${numeros.slice(4, 6)}-${numeros.slice(6)}`;
    }

    function aplicarMascara(campo, funcaoFormatacao) {
        campo.value = funcaoFormatacao(campo.value);

        campo.addEventListener("input", function () {
            campo.value = funcaoFormatacao(campo.value);
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll(".mascara-telefone").forEach(function (campo) {
            aplicarMascara(campo, formatarTelefone);
        });

        document.querySelectorAll(".mascara-ra").forEach(function (campo) {
            aplicarMascara(campo, formatarRa);
        });
    });

    // Compatibilidade com eventuais telas antigas que ainda usem onclick.
    // As telas redesenhadas usam os ícones SVG controlados por ui.js.
    window.mostrarSenha = function (idCampo, botao) {
        const campo = document.getElementById(idCampo);
        if (!campo || !botao) return;
        const mostrar = campo.type === "password";
        campo.type = mostrar ? "text" : "password";
        botao.setAttribute("aria-label", mostrar ? "Ocultar senha" : "Mostrar senha");
        botao.textContent = mostrar ? "◉" : "⊘";
    };})();
