$(document).ready(function () {
    CarregarChamados(document.getElementById('ddFiltro').value, document.getElementById('ddTipo').value, document.getElementById('txtFiltro').value);

    $(document).on('click', '#btnFiltrar', function (e) {
        CarregarChamados(document.getElementById('ddFiltro').value, document.getElementById('ddTipo').value, document.getElementById('txtFiltro').value);
    });

    $(document).on('click', '#btnNovo', function (e) {
        document.getElementById('formNovo').reset();
        //$('#txtTitulo').val("");
        //$('#txtDescricao').val("");
        //document.querySelectorAll('#ddUsuarios').attr('checked', false);
        CarregarUsuarios("Todos");
        CarregarCategorias();
        $('#modalNovo').modal('show');
    });

    let form = document.getElementById('formNovo');
    form.addEventListener('submit', function (e) {
        e.preventDefault();
        form.classList.add('was-validated');

        let titulo = document.getElementById('txtTitulo').value;
        let descricao = document.getElementById('txtDescricao').value;
        let usuarios = Array.prototype.slice.call(document.querySelectorAll('#ddUsuarios option:checked'), 0).map(function (v, i, a) {
            return v.value;
        });
        let categoria = document.getElementById('ddCategoria').value;

        if (form.checkValidity()) {
            NovoChamado(titulo, usuarios, descricao, categoria);
        }
    });

    $(document).on('click', '#btnExcluir', function (e) {
        let currentRow = $(this).closest("tr");
        let id = currentRow.find("td:eq(0)").text();
        if (confirm("Deseja excluir o chamado, ID: " + id + "?")) {
            ExcluirChamado(id);
        }
    });

});

function CarregarChamados(filtro, tipo, pesquisa) {
    $('#tabelaChamados').DataTable({
        ajax: {
            url: '/chamados/carregarchamados',
            data: { filtro: filtro, tipo: tipo, pesquisa: pesquisa },
            dataSrc: ''
        },
        columnDefs: [
            {
                render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                },
                targets: 2
            },
            {
                render: function (data, type, row, meta) {
                    var nome = document.getElementById('nomeUsuario').value;
                    var perfil = document.getElementById('perfilUsuario').value;
                    if ((nome == data || perfil == "TECNICO" || perfil == "ADMINISTRADOR") && row['STATUS'] != "ENCERRADO") {
                        return '<div style="text-align:center;"><span class="bi bi-trash" style="color:red; cursor:pointer;" id="btnExcluir" title="Excluir Chamado"></span></div>';
                    } else {
                        return '<div style="text-align:center;"><span class="bi bi-trash" style="cursor:not-allowed;opacity:50%"></span></div>';
                    }
                },
                targets: 9
            },
            {
                render: function (data, type, row, meta) {
                    if (row['STATUS'] == "ENCERRADO") {
                        return moment(data).format('DD/MM/YYYY');
                    } else {
                        return '';
                    }
                },
                targets: 8
            },
            {
                render: function (data, type, row, meta) {

                    return '<a style="cursor:pointer; font-size:16px; text-decoration:none;" href="/chamados/chamado/' + row['ID'] + '">' + data + '</a>';
                },
                targets: [0, 1]
            },
        ],
        columns: [
            { data: 'ID', width: '3%' },
            { data: 'TITULO', width: '30%' },
            { data: 'CRIADOEM', width: '8%' },
            { data: 'CRIADOPOR', width: '10.5%' },
            { data: 'ACOMPANHAMENTO', width: '15%' },
            { data: 'TECNICO', width: '9%' },
            { data: 'STATUS', width: '6%' },
            { data: 'CATEGORIA', width: '6%' },
            { data: 'ALTERADOEM', width: '12.5%' },
            { data: 'CRIADOPOR', orderable: false, width: '3%' },
        ],
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
        bDestroy: true,
        order: [[0, 'desc']]
    });
}

function NovoChamado(titulo, acompanhando, mensagem, categoria) {

    let fileData = new FormData();

    if ($("#inputArquivos").val() != "") {
        let files = $("#inputArquivos").get(0);
        var arquivos = files.files
        for (let n = 0; n < arquivos.length; n++) {
            fileData.append(arquivos[n].name, arquivos[n]);
        }
    }

    fileData.append('titulo', titulo);
    fileData.append('acompanhando', acompanhando);
    fileData.append('mensagem', mensagem);
    fileData.append('categoria', categoria);

    $.ajax({
        url: '/chamados/novochamado',
        type: "POST",
        contentType: false,
        processData: false,
        data: fileData,
        success: function () {
            window.location.reload();
        },
    });
}

function ExcluirChamado(chamado) {
    $.ajax({
        url: '/chamados/excluirchamado',
        type: "DELETE",
        data: { id: chamado },
        success: function () {
            window.location.reload();
        },
    });
}

function CarregarUsuarios(tipo) {
    $.ajax({
        url: '/chamados/carregarusuarios',
        type: "Get",
        data: { tipo: tipo },
        success: function (result) {
            for (let x = 0; x < result.length; x++) {
                let opt = document.createElement('option');
                opt.value = result[x].ID;
                opt.innerHTML = result[x].LOGIN;
                document.getElementById('ddUsuarios').appendChild(opt);
            }
        },
    });
}

function CarregarCategorias() {
    $.ajax({
        url: 'gerenciar/carregarcategorias',
        success: function (result) {
            for (let x = 0; x < result.length; x++) {
                if (result[x].STATUS == true) {
                    let opt = document.createElement('option');
                    opt.value = result[x].TITULO;
                    opt.innerHTML = result[x].TITULO;
                    document.getElementById('ddCategoria').appendChild(opt);
                }
            }
        },
    });
}