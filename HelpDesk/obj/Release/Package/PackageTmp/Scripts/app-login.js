$(document).ready(function () {
    $('#carregando').hide();
    CarregarSetores();

    $("#btnEntrar").click(function () {
        $('#formContent').hide();
        $('#carregando').show();
        $.ajax({
            type: "GET",
            url: '/Login/Login',
            contentType: "application/json; charset=utf-8",
            data: { username: $("#usuario").val(), password: $("#senha").val() },
            dataType: "json",
            success: function (data) {
                if (data === "Ok") {
                    document.location.href = "/";
                } else {
                    alert(data);
                    $('#carregando').hide();
                    $('#formContent').show();
                }
            }
        });
    });

    $('#senha').keypress(function (e) {
        if (e.which === 13) {
            $('#btnEntrar').click();
        }
    });

    $(document).on('click', '#btnCadastro', function (e) {
        $('#modalCadastro').modal('show');
    });

    $(document).on('click', '#btnEsqueci', function (e) {
        $('#modalEsqueci').modal('show');
    });

    $(document).on('click', '#btnResetar', function (e) {
        if (confirm("Deseja realmente redefinir a senha?")) {
            $('#modalEsqueci').modal('hide');
            $('#formContent').hide();
            $('#carregando').show();
            let texto = document.getElementById('txtEsqueci').value;
            RedefinirSenha(texto);
        }
    });

    let form = document.getElementById('formCadastro');

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        form.classList.add('was-validated');
        $('#modalCadastro').modal('hide');
        $('#formContent').hide();
        $('#carregando').show();
        let nome = document.getElementById('txtNome').value;
        let login = document.getElementById('txtLogin').value;
        let senha = document.getElementById('txtSenha').value;
        let email = document.getElementById('txtEmail').value;
        let setor = document.getElementById('ddSetor').value;

        let usuario = {
            'NOME': nome,
            'EMAIL': email,
            'LOGIN': login,
            'SENHA': senha,
            'SETOR': setor,
            'STATUS': true,
            'PERFIL': "USUARIO"
        }
        if (form.checkValidity()) {
            CadastrarUsuario(usuario);
        }
    });
});

function CadastrarUsuario(usuario) {
    $.ajax({
        url: '/Login/Post',
        type: "POST",
        data: { usuario: usuario },
        success: function (result) {
            alert(result);
            window.location.reload();
        },
    });
}

function CarregarSetores() {
    $.ajax({
        url: '/Gerenciar/CarregarSetores',
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

function RedefinirSenha(texto) {
    $.ajax({
        url: '/login/esqueceusenha',
        type: "get",
        data: { texto: texto },
        success: function (result) {
            alert(result);
            if (result == "Nova senha enviada para o E-Mail cadastrado") {
                window.location.reload();
            }
        },
    });
}