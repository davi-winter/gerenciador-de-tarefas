
// Aguarda o carregamento do DOM
document.addEventListener('DOMContentLoaded', (event) => {

    const checkbox = document.getElementById('enableEditing');
    const table = document.getElementById('tableJobs');
    const lines = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

    // Adiciona um listener para o evento 'change'
    checkbox.addEventListener('change', function () {
        // Percorre as linhas da tabela
        for (var i = 0; i < lines.length; i++) {
            const cells = lines[i].getElementsByTagName('td');
            // Percorre as celulas da tabela
            for (var j = 0; j < cells.length; j++) {
                // Percorre os links da célula com os links de edição
                if (cells[j].id == 'linksEditing') {
                    const links = cells[j].querySelectorAll('a');
                    for (var k = 0; k < links.length; k++) {
                        // Mostra ou esconde os links de acordo com a visibilidade
                        if (this.checked)
                            links[k].style.display = 'inline';
                        else
                            links[k].style.display = 'none';
                    }
                }
            }
        }
    });

    // Remove a classe 'table-no-flicker' da div da tabela (evitar efeito flickering)
    table.classList.remove('table-no-flicker');
});


// Função para salvar o estado no localStorage
function savedState() {
    var checkBox = document.getElementById('enableEditing');
    localStorage.setItem('checkBoxState', checkBox.checked);
}

// Executado quando a página carrega no cliente
window.onload = function () {
    var checkBox = document.getElementById('enableEditing');
    // Recupera o estado salvo ou define como falso se não existir
    var savedState = localStorage.getItem('checkBoxState') == 'true';
    checkBox.checked = savedState;

    // Dispara o evento 'change' do checkbox 
    const evento = new Event('change');
    checkBox.dispatchEvent(evento);
};