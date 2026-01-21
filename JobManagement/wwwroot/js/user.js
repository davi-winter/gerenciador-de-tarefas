// Aplica as máscaras assim que o formulário é carregado
window.onload = function () {
    const inputCpf = document.getElementById('cpf');
    formatCpf({ target: inputCpf });
    const inputPhone = document.getElementById('telephone');
    formatPhone({ target: inputPhone });
};

function formatCpf(event) {
    let input = event.target;
    input.value = formatFieldCpf(input.value);
}

function formatFieldCpf(value) {
    // Remove tudo que não é dígito
    value = value.replace(/\D/g, "");

    // Aplica a máscara para CPF
    if (value.length > 9)
        value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    else if (value.length > 6)
        value = value.replace(/(\d{3})(\d{3})(\d{3})/, "$1.$2.$3");
    else if (value.length > 3)
        value = value.replace(/(\d{3})(\d{3})/, "$1.$2");
    return value;
}

function formatPhone(event) {
    let input = event.target;
    input.value = formatFieldPhone(input.value);
}

function formatFieldPhone(value) {
    // Remove tudo que não é dígito
    value = value.replace(/\D/g, "");

    // Aplica a máscara para telefone/celular (XX) 9XXXX-XXXX
    value = value.replace(/^(\d{2})(\d)/g, "($1) $2");
    value = value.replace(/(\d)(\d{4})$/, "$1-$2");
    return value;
}
