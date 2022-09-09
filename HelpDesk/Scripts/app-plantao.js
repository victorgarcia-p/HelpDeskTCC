$(document).ready(function () {
    BuscarTecnicos();
});

document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prev',
            center: 'title',
            right: 'next',
        },
        locales: 'allLocales',
        locale: 'pt-br',
        navLinks: false, // can click day/week names to navigate views
        businessHours: true, // display business hours
        editable: true,
        selectable: true,
        eventClick: true,
        eventClick: function (info) {
            dataIni = info.event.start.getFullYear() + "-" + String(info.event.start.getMonth() + 1).padStart(2, '0') + "-" + String(info.event.start.getDate()).padStart(2, '0');
            dataFim = info.event.end.getFullYear() + "-" + String(info.event.end.getMonth() + 1).padStart(2, '0') + "-" + String(info.event.end.getDate()).padStart(2, '0');

            editarEventos(dataIni, dataFim, info.event.title, info.event.display, info.event.id);
        },
        select: function (info) {
            dataIni = info.start.getFullYear() + "-" + String(info.start.getMonth() + 1).padStart(2, '0') + "-" + String(info.start.getDate()).padStart(2, '0');
            dataFim = info.end.getFullYear() + "-" + String(info.end.getMonth() + 1).padStart(2, '0') + "-" + String(info.end.getDate()).padStart(2, '0');

            criarEventos(dataIni, dataFim, "Adicionar Plantão");
        },
        eventDrop: function (info) {
            dataIni = info.event.start.getFullYear() + "-" + String(info.event.start.getMonth() + 1).padStart(2, '0') + "-" + String(info.event.start.getDate()).padStart(2, '0');
            dataFim = info.event.end.getFullYear() + "-" + String(info.event.end.getMonth() + 1).padStart(2, '0') + "-" + String(info.event.end.getDate()).padStart(2, '0');
            var usuarioLogado = document.getElementById('usuarioLogado').value;
            $.ajax({
                type: "PUT",
                url: '/Plantao/AlterarPlantao',
                data: { dataIni: dataIni, dataFim: dataFim, usuario: info.event.display, idFolga: info.event.id, usuarioLogado: usuarioLogado },
                dataType: "json",
                success: function (data) {
                    if (data != "Ok") {
                        alert("Erro ao alterar folga" + data);
                    }
                }
            });
        },
        events: function (info, successCallback, failureCallback) {
            let plantoes = [];
            $.ajax({
                type: 'GET',
                url: '/Plantao/BuscarPlantoes',
                dataType: "json",
                success: function (p) {
                    for (var i = 0; i < p.length; i++) {
                        let plantao = {
                            id: p[i].ID,
                            title: p[i].TITULO,
                            start: p[i].DATAINICIO,
                            end: p[i].DATAFIM,
                            display: p[i].USUARIO,
                            color: p[i].COR
                        }
                        plantoes.push(plantao);
                    }
                    successCallback(plantoes);
                }
            });
        }
    });
    calendar.render();

});

function criarEventos(dataIni, dataFim, evento) {
    $("#lblFolga").html(evento);
    $("#dataIni").val(dataIni);
    $("#dataFim").val(dataFim);
    $("#btnExcluir").hide();
    $("#btnAlterar").hide();
    $("#lblFuncionario").hide();
    $("#btnSalvar").show();

    $('#modalCadastro').modal('show');

    var usuarioLogado = document.getElementById('usuarioLogado').value;

    $("#btnSalvar").on('click', function () {
        if ($("#ddFuncionario").val() == 0) {
            alert("Preencha as informações necessárias");
        } else {
            $.ajax({
                type: "POST",
                url: '/Plantao/SalvarPlantao',
                data: { dataIni: $("#dataIni").val(), dataFim: $("#dataFim").val(), usuario: $("#ddFuncionario").val(), usuarioLogado: usuarioLogado },
                dataType: "json",
                success: function (data) {
                    if (data === "Ok") {
                        $('#modalCadastro').modal('hide');
                        alert("Folga salva com sucesso!");
                        window.location.reload();
                    } else {
                        alert("Erro ao salvar folga" + data);
                    }
                }
            });
        }
    });
}

function editarEventos(dataIni, dataFim, nome, usuario, idFolga) {
    $("#lblFolga").html(nome);
    $("#dataIni").val(dataIni);
    $("#dataFim").val(dataFim);
    $("#btnSalvar").hide();
    $("#ddFuncionario").show();
    $("#btnAlterar").show();
    $("#btnExcluir").show();
    $("#lblNome").show();
    $("#lblFuncionario").hide();
    $("#lblFuncionario").html(nome);

    $('#modalCadastro').modal('show');

    $("#ddFuncionario").val(usuario);

    var usuarioLogado = document.getElementById('usuarioLogado').value;

    $("#btnAlterar").on('click', function () {
        if ($("#ddFuncionario").val() == 0) {
            alert("Preencha as informações necessárias");
        } else {
            $.ajax({
                type: "PUT",
                url: '/Plantao/AlterarPlantao',
                data: { dataIni: $("#dataIni").val(), dataFim: $("#dataFim").val(), usuario: $("#ddFuncionario").val(), idFolga: idFolga, usuarioLogado: usuarioLogado },
                dataType: "json",
                success: function (data) {
                    if (data === "Ok") {
                        $('#modalCadastro').modal('hide');
                        alert("Folga alterada com sucesso!");
                        window.location.reload();
                    } else {
                        alert("Erro ao alterar folga" + data);
                    }
                }
            });
        }
    });

    $("#btnExcluir").on('click', function () {
        if (confirm("Deseja realmente remover essa folga?")) {
            $.ajax({
                type: "DELETE",
                url: '/Plantao/ExcluirPlantao',
                data: { idFolga: idFolga },
                dataType: "json",
                success: function (data) {
                    if (data === "Ok") {
                        $('#modalCadastro').modal('hide');
                        alert("Plantão removido!");
                        window.location.reload();
                    } else {
                        alert("Erro ao remover Plantão" + data);
                    }
                }
            });
        }
    });
}

function BuscarTecnicos() {
    $.ajax({
        type: 'GET',
        url: '/Plantao/BuscarTecnicos',
        data: { tipo: 'Tecnicos' },
        dataType: "json",
        success: function (d) {

            let select = document.getElementById('ddFuncionario');

            $('#ddFuncionario').find('option').remove().end().append('<option value="0">Selecione o Funcionário:</option>');
            let config = document.getElementById('configTecnicos');
            let configTec = "";
            for (var i = 0; i < d.length; i++) {
                //configTec = configTec + "<div class='col-lg-12 col-md-12 col-sm-4 col-4'><input class='color-picker' type='color' id='" + d[i].LOGIN + "' value='" + d[i].COR + "'><label style='color: white; background-color: " + d[i].COR + "' class='col-12 label-color-picker'>" + d[i].NOME + "</label></div>"
                configTec = configTec + "<div class='col-lg-12 col-md-12 col-sm-4 col-4' style='margin: 2px 0;'><label style='color: white; background-color: " + d[i].COR + ";' class='col-12 label-color-picker'>" + d[i].NOME + "</label></div>"
                var opt = document.createElement('option');
                opt.value = d[i].LOGIN;
                opt.innerHTML = d[i].NOME;
                select.appendChild(opt);
            }
            config.innerHTML = configTec + "<br />";
        }
    });
}