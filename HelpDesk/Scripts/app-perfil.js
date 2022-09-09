let ID = "";
let NOME = "";
let EMAIL = "";
let LOGIN = "";
let SETOR = "";
let PERFIL = "";

$(document).ready(function () {
    if (document.getElementById('RedefinirSenha').value == "True") {
        $('#txtSenha').val('');
        $('#modalRedefinir').modal('show');
    }

    LOGIN = document.getElementById('Login').value;
    BuscarPerfil(LOGIN);
    RedefinirSenha();
    CarregarSetores();

    $(document).on('click', '#btnRedefinir', function (e) {
        $('#txtSenha').val('');
        $('#modalRedefinir').modal('show');
    });

    $(document).on('click', '#btnResetar', function (e) {
        let senha = document.getElementById('txtSenha').value;
        AlterarSenha(LOGIN, senha);
    });

    $(document).on('click', '#btnEditar', function (e) {
        $("#editNome").val(NOME);
        $("#editEmail").val(EMAIL);
        $("#ddSetor").val(SETOR);
        $('#modalEditar').modal('show');
    });

    let alterar = document.getElementById('formAlterar');
    alterar.addEventListener('submit', function (e) {
        e.preventDefault();
        alterar.classList.add('was-validated');

        let nome = document.getElementById('editNome').value;
        let email = document.getElementById('editEmail').value;
        let setor = document.getElementById('ddSetor').value;

        let usuario = {
            'ID': ID,
            'NOME': nome,
            'EMAIL': email,
            'LOGIN': LOGIN,
            'SETOR': setor,
            'STATUS': true,
            'PERFIL': PERFIL
        }

        if (alterar.checkValidity()) {
            EditarUsuario(usuario);
        }
    });
});

function RedefinirSenha() {
    if ($("#RedefinirSenha").value == "true") {
        $("#btnRedefinir").click;
    }
}

function AlterarSenha(usuario, senha) {
    $.ajax({
        url: '/gerenciar/alterarsenha',
        type: "Post",
        data: { login: usuario, senha: senha },
        success: function (result) {
            alert(result);
            window.location.reload();
        },
    });
}

function BuscarPerfil(login) {
    $.ajax({
        url: '/gerenciar/buscarperfilusuario',
        type: "get",
        data: { login: login},
        success: function (result) {
            ID = result.ID;
            NOME = result.NOME;
            EMAIL = result.EMAIL;
            SETOR = result.SETOR;
            PERFIL = result.PERFIL;

            $("#txtNome").text(NOME);
            $("#txtLogin").text(LOGIN);
            $("#txtEmail").text(EMAIL);
            $("#txtSetor").text(SETOR);
            $("#txtPerfil").text(PERFIL);
        },
    });
}

function CarregarSetores() {
    $.ajax({
        url: '/gerenciar/carregarsetores',
        type: "get",
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

function AlterarUsuario() {
    $.ajax({
        url: '/gerenciar/carregarsetores',
        type: "get",
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

function EditarUsuario(usuario) {
    console.log(usuario);
    $.ajax({
        url: '/gerenciar/update',
        type: "UPDATE",
        data: { usuario: usuario },
        success: function (result) {
            alert(result);
            window.location.reload();
        },
    });
}