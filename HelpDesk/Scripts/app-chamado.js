$(document).ready(function () {
    CarregarCategorias();
    $('#btnNovo').hide();
    $('#addAcompanhante').hide();
    $('#addTecnico').hide();
    $('#editCategoria').hide();
    $('#divEncerrar').hide();

    CarregarChamado(document.getElementById('chamadoID').value);

    $(document).on('click', '#btnRemover', function (e) {
        let usuario = $(this).prev().text();
        let chamado = document.getElementById('chamadoID').value;
        let tipo = "";
        if ($(this).hasClass('ACOMPANHAMENTO')) {
            tipo = "ACOMPANHAMENTO";
        } else {
            tipo = "TECNICO";
        }
        removerEnvolvido(usuario, chamado, tipo)
    });

    $(document).on('click', '#btnNovo', function (e) {
        $('#modalNovo').modal('show');
    });

    let form = document.getElementById('formNovo');
    form.addEventListener('submit', function (e) {
        e.preventDefault();
        form.classList.add('was-validated');

        let chamado = document.getElementById('chamadoID').value;
        let descricao = document.getElementById('txtMensagem').value;

        let encerrar = "nao";

        if (document.getElementById('chkEncerrar').checked) {
            encerrar = "sim";
        }

        if (form.checkValidity()) {
            NovaMensagem(descricao, chamado, encerrar);
        }
    });

    $(document).on('click', '#addAcompanhante', function (e) {
        $('#txtTitulo').val('ACOMPANHAMENTO');
        $('#titulo').html('Acompanhando');
        CarregarUsuarios("Todos");
        $('#modalAtribuir').modal('show');
    });

    $(document).on('click', '#addTecnico', function (e) {
        $('#txtTitulo').val('TECNICO');
        $('#titulo').html('Atribuído para');
        CarregarUsuarios("Tecnicos");
        $('#modalAtribuir').modal('show');
    });

    $(document).on('click', '#editCategoria', function (e) {
        $('#modalCategoria').modal('show');
    });

    let attr = document.getElementById('formAtribuir');
    attr.addEventListener('submit', function (e) {
        e.preventDefault();
        attr.classList.add('was-validated');

        let usuarios = Array.prototype.slice.call(document.querySelectorAll('#ddUsuarios option:checked'), 0).map(function (v, i, a) {
            return v.value;
        });

        if (attr.checkValidity()) {
            AtribuirAcompanhamento(usuarios);
        }
    });

    let formCategoria = document.getElementById('formCategoria');
    formCategoria.addEventListener('submit', function (e) {
        e.preventDefault();
        attr.classList.add('was-validated');

        let categoria = Array.prototype.slice.call(document.querySelectorAll('#ddCategoria option:checked'), 0).map(function (v, i, a) {
            return v.value;
        });

        if (formCategoria.checkValidity()) {
            AlterarCategoria(categoria);
        }
    });
});

function CarregarChamado(chamado) {
    $.ajax({
        url: '/chamados/carregarumchamado',
        type: "GET",
        data: { id: chamado },
        success: function (result) {
            $('#chamadoStatus').html(result.STATUS);
            if (result.STATUS != "ENCERRADO") {
                $('#btnNovo').show();
            }
            var usuarioLogado = document.getElementById('usuarioLogado').value;
            var perfil = document.getElementById('perfil').value;

            if (result.CRIADOPOR == usuarioLogado || perfil == "TECNICO" || perfil == "ADMINISTRADOR") {
                $('#addAcompanhante').show();
                $('#editCategoria').show();
            }
            if (perfil == "TECNICO" || perfil == "ADMINISTRADOR") {
                $('#addTecnico').show();
                $('#divEncerrar').show();
            }
            let acompanhamento = result.ACOMPANHAMENTO.split(';');
            let tecnicos = result.TECNICO.split(';');
            let acompanhando = "";
            let tecnico = "";
            for (x = 0; x < acompanhamento.length; x++) {
                if (acompanhamento[x] != "") {
                    if (result.CRIADOPOR == usuarioLogado || perfil == "TECNICO" || perfil == "ADMINISTRADOR") {
                        acompanhando += "<label style='padding-left:10px;' id='" + acompanhamento[x] + "'>" + acompanhamento[x] + "</label> <span class='bi bi-x-circle-fill ACOMPANHAMENTO' style='cursor:pointer;font-size:12px;' id='btnRemover' title='Remover'></span><br/>";
                    } else {
                        acompanhando += "<label style='padding-left:10px;' id='" + acompanhamento[x] + "'>" + acompanhamento[x] + "</label><br/>";
                    }
                }
            }
            for (x = 0; x < tecnicos.length; x++) {
                if (tecnicos[x] != "") {
                    if (perfil == "TECNICO" || perfil == "ADMINISTRADOR") {
                        tecnico += "<label style='padding-left:10px;' id='" + tecnicos[x] + "'>" + tecnicos[x] + "</label> <span class='bi bi-x-circle-fill TECNICO' style='cursor:pointer;font-size:12px;' id='btnRemover' title='Remover'></span><br/>";
                    } else {
                        tecnico += "<label style='padding-left:10px;' id='" + tecnicos[x] + "'>" + tecnicos[x] + "</label><br/>";
                    }

                }
            }

            let categoria = "<label style='padding-left:10px;' id='" + result.CATEGORIA + "'>" + result.CATEGORIA + "</label><br/>"



            $('#requerente').html("<label style='padding-left:10px;'>" + result.CRIADOPOR + "</label>");
            $('#acompanhando').html(acompanhando);
            $('#tecnico').html(tecnico);
            $('#categoria').html(categoria);
            $('#tituloChamado').html("<label class='form-control' style='font-weight:bold;font-size:20px;padding-left:2rem;'>" + result.TITULO + "</label>");
            CarregarHistoricoChamado(result.ID, result.CRIADOPOR)
        },
    });
}

function CarregarHistoricoChamado(chamado, criadopor) {
    $.ajax({
        url: '/chamados/carregarhistoricochamado',
        type: "GET",
        data: { id: chamado },
        success: function (result) {
            var mensagens = "";
            for (x = 0; x < result.length; x++) {
                if (criadopor == result[x].CRIADOPOR) {
                    if (result[x].COMENTARIOS != "" && result[x].COMENTARIOS != null) {
                        if (result[x].ARQUIVOS != "" && result[x].ARQUIVOS != null) {
                            mensagens += "<div class='row chat-left'><div class='requerente'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 left'>" + result[x].COMENTARIOS + "<br /><br /><a href='" + result[x].ARQUIVOS + "' class='file' download>" + result[x].ARQUIVOS + "</a></div></div>";
                        } else {
                            mensagens += "<div class='row chat-left'><div class='requerente'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 left'>" + result[x].COMENTARIOS + "</div></div>";
                        }
                    } else {
                        if (result[x].ARQUIVOS != "" && result[x].ARQUIVOS != null) {
                            mensagens += "<div class='row chat-left'><div class='requerente'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 left'><a href='" + result[x].ARQUIVOS + "' class='file' download>" + result[x].ARQUIVOS + "</a></div></div>";
                        }
                    }
                } else {
                    if (result[x].COMENTARIOS != "" && result[x].COMENTARIOS != null) {
                        if (result[x].ARQUIVOS != "" && result[x].ARQUIVOS != null) {
                            mensagens += "<div class='row chat-right'><div class='acompanhante'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 right'>" + result[x].COMENTARIOS + "<br /><br /><a href='" + result[x].ARQUIVOS + "' class='file' download>" + result[x].ARQUIVOS + "</a></div></div>";
                        } else {
                            mensagens += "<div class='row chat-right'><div class='acompanhante'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 right'>" + result[x].COMENTARIOS + "</div></div>";
                        }
                    } else {
                        if (result[x].ARQUIVOS != "" && result[x].ARQUIVOS != null) {
                            mensagens += "<div class='row chat-right'><div class='acompanhante'>" + result[x].CRIADOPOR + "</div><div class='col-lg-7 right'><a href='" + result[x].ARQUIVOS + "' class='file' download>" + result[x].ARQUIVOS + "</a></div></div>";
                        }
                    }
                }
            }
            $('#historicoChamado').html(mensagens);
        },
    });
}

function removerEnvolvido(usuario, chamado, tipo) {
    $.ajax({
        url: '/chamados/removerusuarioenvolvido',
        type: "DELETE",
        data: { usuario: usuario, chamado: chamado, tipo: tipo },
        success: function (result) {
            window.location.reload();
        },
    });
}


function NovaMensagem(mensagem, id, encerrar) {

    let fileData = new FormData();

    if ($("#inputArquivos").val() != "") {
        let files = $("#inputArquivos").get(0);
        var arquivos = files.files
        for (let n = 0; n < arquivos.length; n++) {
            fileData.append(arquivos[n].name, arquivos[n]);
        }

    }

    fileData.append('mensagem', mensagem);
    fileData.append('id', id);
    fileData.append('encerrar', encerrar);

    $.ajax({
        url: '/chamados/novamensagem',
        type: "POST",
        contentType: false,
        processData: false,
        data: fileData,
        success: function (result) {
            window.location.reload();
        },
        error: function () {
            alert("Ocorreu um erro, tente novamente!");
        }
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
        url: '/gerenciar/carregarcategorias',
        data: {},
        success: function (result) {
            for (let x = 0; x < result.length; x++) {
                let opt = document.createElement('option');
                opt.value = result[x].ID;
                opt.innerHTML = result[x].TITULO;
                document.getElementById('ddCategoria').appendChild(opt);
            }
        },
    });
}

function AtribuirAcompanhamento(usuarios) {
    let chamado = document.getElementById('chamadoID').value;
    let tipo = document.getElementById('txtTitulo').value;

    $.ajax({
        url: '/chamados/atribuiracompanhamento',
        type: "POST",
        data: { usuarios: usuarios, chamado: chamado, tipo: tipo },
        success: function (result) {
            window.location.reload();
        },
    });
}

function AlterarCategoria(categoria) {
    let chamado = document.getElementById('chamadoID').value;
    let usuario = document.getElementById('usuarioLogado').value;

    $.ajax({
        url: '/chamados/alterarcategoria',
        type: "POST",
        data: { chamado: chamado, categoria: categoria, usuario: usuario },
        success: function (result) {
            window.location.reload();
        },
    });
}