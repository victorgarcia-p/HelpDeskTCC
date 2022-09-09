$(document).ready(function () {
    CarregarTabelaSetores();

    $(document).on('click', '#btnCadastro', function (e) {
        $('#divStatus').hide();
        $('#chkAtivo').attr("checked", true);
        document.getElementById('formCadastro').classList.remove('was-validated');
        $('#titleAlterar').hide();
        $('#titleCadastrar').show();
        $('#btnAlterar').hide();
        $('#btnCadastrar').show();
        $('#idSetor').val("");
        $('#txtDescricao').val("");
        $('#modalCadastro').modal('show');
    });

    $(document).on('click', '#editInfo', function (e) {
        document.getElementById('formCadastro').classList.remove('was-validated');
        let currentRow = $(this).closest("tr");
        $('#idSetor').val(currentRow.find("td:eq(0)").text());
        $('#txtDescricao').val(currentRow.find("td:eq(1)").text());

        if (currentRow.find("td:eq(2)").text() === "ATIVO" || currentRow.find("td:eq(2)").text() === "true") {
            $('#chkAtivo').attr("checked", true);
        } else if (currentRow.find("td:eq(2)").text() === "INATIVO" || currentRow.find("td:eq(2)").text() === "false") {
            $('#chkAtivo').attr("checked", false);
        }

        $('#titleAlterar').show();
        $('#titleCadastrar').hide();
        $('#btnAlterar').show();
        $('#btnCadastrar').hide();
        $('#modalCadastro').modal('show');
    });

    $(document).on('click', '#btnExcluir', function (e) {
        let currentRow = $(this).closest("tr");
        let id = currentRow.find("td:eq(0)").text();
        let titulo = currentRow.find("td:eq(1)").text();
        let status = false;

        if (currentRow.find("td:eq(2)").text() === "true") {
            status = true;
        }

        let setor = {
            'ID': id,
            'TITULO': titulo,
            'STATUS': status
        };

        ExcluirSetor(setor);
    });

    let cadastro = document.getElementById('formCadastro');

    cadastro.addEventListener('submit', function (e) {
        e.preventDefault();
        cadastro.classList.add('was-validated');

        let id = document.getElementById('idSetor').value;
        let titulo = document.getElementById('txtDescricao').value;
        let status = false;

        if ($('#chkAtivo').is(':checked')) {
            status = true;
        }

        let setor = {
            'ID': id,
            'TITULO': titulo,
            'STATUS': status
        };

        if (cadastro.checkValidity()) {
            if ($('#btnAlterar').is(':visible') == true) {
                EditarSetor(setor);
            } else if ($('#btnCadastrar').is(':visible') == true) {
                CadastrarSetor(setor);
            }
        }
    });
});



function CarregarTabelaSetores() {
    $('#tabelaSetores').DataTable({
        ajax: {
            url: '/gerenciar/carregarsetores',
            dataSrc: ''
        },
        columnDefs: [
            {
                render: function (data) {
                    if (data == true) {
                        return 'ATIVO';
                    } else {
                        return 'INATIVO';
                    }
                },
                targets: 2
            }
        ],
        columns: [
            { data: 'ID', width: '5%' },
            { data: 'TITULO', width: '75%' },
            { data: 'STATUS', width: '10%' },
            {
                defaultContent: '<span class="bi bi-pencil-square" id="editInfo" style="cursor:pointer;" title="Editar Informações"></span><span class="bi bi-trash" id="btnExcluir" style="color:red;cursor:pointer;" title="Excluir Setor"></span>', width: '10%'
            }],
        language: {
            lengthMenu: 'Exibir _MENU_ por página',
            zeroRecords: 'Nenhum registro encontrado',
            info: 'Página _PAGE_ de _PAGES_',
            infoEmpty: 'Nenhum registro encontrado',
            infoFiltered: '(filtro entre _MAX_ total de registro)',
            search: 'Pesquisar',
            paginate: {
                next: 'Próximo',
                previous: 'Anterior',
                first: 'Primeiro',
                last: 'Último'
            },
        },
        bDestroy: true
    });
}

function CadastrarSetor(setor) {
    $('#modalCarregando').modal('show');
    $.ajax({
        url: '/gerenciar/cadastrarsetor',
        type: "POST",
        data: { setor: setor },
        success: function (result) {
            $('#modalCadastro').modal('hide');
            alert(result);
            window.location.reload();
        },
    });
}

function EditarSetor(setor) {
    $('#modalCarregando').modal('show');
    $.ajax({
        url: '/gerenciar/atualizarsetor',
        type: "POST",
        data: { setor: setor},
        success: function (result) {
            $('#modalCadastro').modal('hide');
            alert(result);
            window.location.reload();
        },
    });
}

function ExcluirSetor(setor) {
    $('#modalCarregando').modal('show');
    $.ajax({
        url: '/gerenciar/excluirsetor',
        type: "DELETE",
        data: { setor: setor },
        success: function (result) {
            alert(result);
            window.location.reload();
        },
    });
}
