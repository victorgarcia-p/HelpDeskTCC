$(document).ready(function () {
    if (document.getElementById('perfilUsuario').value == 'TECNICO') {
        $("#btnCadastro").hide();
    }
    CarregarTabelaUsuarios();
    CarregarPerfis();
    CarregarSetores();

    $(document).on('click', '#btnCadastro', function (e) {
        $('#divStatus').hide();
        $('#chkAtivo').attr("checked", true);
        document.getElementById('formCadastro').classList.remove('was-validated');
        $('#titleAlterar').hide();
        $('#titleCadastrar').show();
        $('#btnAlterar').hide();
        $('#btnCadastrar').show();
        $('#idUsuario').val("");
        $('#txtNome').val("");
        $('#txtLogin').val("");
        $('#txtEmail').val("");
        $('#ddSetor').val("0");
        $('#ddPerfil').val("0");
        $('#modalCadastro').modal('show');
    });

    $(document).on('click', '#editInfo', function (e) {
        $("#txtPerfil").hide();
        $('#divStatus').show();
        document.getElementById('formCadastro').classList.remove('was-validated');
        let currentRow = $(this).closest("tr");
        $('#idUsuario').val(currentRow.find("td:eq(0)").text());
        $('#txtNome').val(currentRow.find("td:eq(2)").text());
        $('#txtLogin').val(currentRow.find("td:eq(1)").text());
        $('#txtEmail').val(currentRow.find("td:eq(3)").text());
        $('#ddSetor').val(currentRow.find("td:eq(4)").text());
        $('#ddPerfil').val(currentRow.find("td:eq(5)").text());
        $('#txtPerfil').html(currentRow.find("td:eq(5)").text());
        if (document.getElementById('perfilUsuario').value == 'TECNICO') {
            $("#ddPerfil").hide();
            $("#txtPerfil").show();
        }
        $('#ativo').attr("checked", false);
        $('#inativo').attr("checked", false);
        if (currentRow.find("td:eq(6)").text() === "ATIVO" || currentRow.find("td:eq(6)").text() === "true") {
            $('#chkAtivo').attr("checked", true);
        } else if (currentRow.find("td:eq(6)").text() === "INATIVO" || currentRow.find("td:eq(6)").text() === "false") {
            $('#chkAtivo').attr("checked", false);
        }
        $('#titleAlterar').show();
        $('#titleCadastrar').hide();
        $('#btnAlterar').show();
        $('#btnCadastrar').hide();
        $('#modalCadastro').modal('show');
    });

    let cadastro = document.getElementById('formCadastro');

    cadastro.addEventListener('submit', function (e) {
        e.preventDefault();
        cadastro.classList.add('was-validated');

        let id = document.getElementById('idUsuario').value;
        let nome = document.getElementById('txtNome').value;
        let login = document.getElementById('txtLogin').value;
        let email = document.getElementById('txtEmail').value;
        let setor = document.getElementById('ddSetor').value;
        let perfil = document.getElementById('ddPerfil').value;
        let status = false;

        if ($('#chkAtivo').is(':checked')) {
            status = true;
        }

        let usuario = {
            'NOME': nome,
            'EMAIL': email,
            'LOGIN': login,
            'SETOR': setor,
            'PERFIL': perfil,
            'STATUS': status
        }

        if (cadastro.checkValidity()) {
            if ($('#btnAlterar').is(':visible') == true) {
                usuario.ID = id;
                EditarUsuario(usuario);
            } else if ($('#btnCadastrar').is(':visible') == true) {
                CadastrarUsuario(usuario);
            }
        }
    });
});



function CarregarTabelaUsuarios() {
    $('#tabelaUsuarios').DataTable({
        ajax: {
            url: 'buscarusuarios',
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
                targets: 6
            }
        ],
        columns: [
            { data: 'ID', width: '5%' },
            { data: 'LOGIN', width: '10%' },
            { data: 'NOME', width: '30%' },
            { data: 'EMAIL', width: '25%' },
            { data: 'SETOR', width: '10%' },
            { data: 'PERFIL', width: '5%' },
            { data: 'STATUS', width: '5%' },
            {
                defaultContent: '<span class="bi bi-pencil-square" id="editInfo" style="cursor:pointer;" title="Editar Informações"></span>', width: '10%'
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

function CadastrarUsuario(usuario) {
    $('#modalCarregando').modal('show');
    $.ajax({
        url: '/gerenciar/post',
        type: "POST",
        data: { usuario: usuario },
        success: function (result) {
            alert(result);
            window.location.reload();
            $('#modalCarregando').modal('hide');
        },
    });
}

function EditarUsuario(usuario) {
    $('#modalCarregando').modal('show');
    $.ajax({
        url: '/gerenciar/update',
        type: "UPDATE",
        data: { usuario: usuario },
        success: function (result) {
            alert(result);
            window.location.reload();
            $('#modalCarregando').modal('hide');
        },
    });
}

function CarregarPerfis() {
    let perfil = $('#perfilUsuario').val();

    let admin = document.createElement('option');
    let tecnico = document.createElement('option');
    let usuario = document.createElement('option');

    if (perfil == 'ADMINISTRADOR') {
        admin.value = 'ADMINISTRADOR';
        admin.innerHTML = 'ADMINISTRADOR';
        document.getElementById('ddPerfil').appendChild(admin);
    }
    tecnico.value = 'TECNICO';
    tecnico.innerHTML = 'TECNICO';
    document.getElementById('ddPerfil').appendChild(tecnico);
    usuario.value = 'USUARIO';
    usuario.innerHTML = 'USUARIO';
    document.getElementById('ddPerfil').appendChild(usuario);
}

function CarregarSetores() {
    $.ajax({
        url: 'carregarsetores',
        success: function (result) {
            for (let x = 0; x < result.length; x++) {
                if (result[x].STATUS == true) {
                    let opt = document.createElement('option');
                    opt.value = result[x].TITULO;
                    opt.innerHTML = result[x].TITULO;
                    document.getElementById('ddSetor').appendChild(opt);
                }
            }
        },
    });
}