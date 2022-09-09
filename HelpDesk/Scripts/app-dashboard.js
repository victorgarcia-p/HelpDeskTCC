$(document).ready(function () {
    CarregarDatas();
    CarregarGraficos(document.getElementById('dataIni').value, document.getElementById('dataFim').value);

    $(document).on('click', '#btnFiltrar', function (e) {
        let myChart = Chart.getChart("myChart");
        let myChart2 = Chart.getChart("myChart2");
        let myChart3 = Chart.getChart("myChart3");
        myChart.destroy();
        myChart2.destroy();
        myChart3.destroy();
        CarregarGraficos(document.getElementById('dataIni').value, document.getElementById('dataFim').value);
    });
});

function CarregarGraficos(dataini, datafim) {
    let usuarios = [];

    $.ajax({
        url: '/gerenciar/usuariosdashboard',
        type: "GET",
        data: {},
        success: function (result) {
            if (document.getElementById('perfilUsuario').value == 'ADMINISTRADOR') {
                for (let n = 0; n < result.length; n++) {
                    usuarios.push(result[n].LOGIN);
                }
            } else {
                for (let n = 0; n < result.length; n++) {
                    if (result[n].LOGIN == document.getElementById('idUsuario').value) {
                        usuarios.push(result[n].LOGIN);
                    }
                }
            }
            CarregarChamadosDashboardAlteracao(dataini, datafim, usuarios);
            CarregarChamadosDashboardCriacao(dataini, datafim);
            CarregarChamadosDashboardCategoria(dataini, datafim)
        },
    });
}

function CarregarChamadosDashboardAlteracao(dataini, datafim, usuarios) {
    let chamadosEncerrados = [];
    let chamadosEmAndamento = [];

    $.ajax({
        url: '/gerenciar/chamadosdashboardalteracao',
        type: "GET",
        data: { dataIni: dataini, dataFim: datafim },
        success: function (result) {
            for (let u = 0; u < usuarios.length; u++) {
                let encerrados = 0;
                let emandamento = 0;
                for (let n = 0; n < result.length; n++) {
                    if (result[n].LOGIN == usuarios[u] && result[n].STATUS == 'ENCERRADO' && result[n].TIPO == 'TECNICO') {
                        encerrados++;
                    } else if (result[n].LOGIN == usuarios[u] && result[n].STATUS == 'EM ANDAMENTO' && result[n].TIPO == 'TECNICO') {
                        emandamento++;
                    }
                }
                chamadosEncerrados.push(encerrados);
                chamadosEmAndamento.push(emandamento);
            }

            let dados = {
                labels: usuarios,
                datasets: [{
                    label: 'ENCERRADO',
                    type: 'bar', // (line | bar | radar | doughnut | pie )
                    data: chamadosEncerrados,
                    backgroundColor: [
                        /*'rgba(255, 99, 132, 0.2)',*/
                        'rgba(54, 162, 235, 0.2)'
                        //'rgba(255, 206, 86, 0.2)',
                        //'rgba(75, 192, 192, 0.2)',
                        //'rgba(153, 102, 255, 0.2)',
                        //'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        /*'rgba(255, 99, 132, 1)',*/
                        'rgba(54, 162, 235, 1)'
                        //'rgba(255, 206, 86, 1)',
                        //'rgba(75, 192, 192, 1)',
                        //'rgba(153, 102, 255, 1)',
                        //'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }, {
                    label: 'EM ANDAMENTO',
                    type: 'bar', // (line | bar | radar | doughnut | pie )
                    data: chamadosEmAndamento,
                    backgroundColor: [
                        'rgba(1, 1, 1, 0.2)'
                    ],
                    borderColor: [
                        'rgba(1, 1, 1, 1)'
                    ],
                    borderWidth: 1
                }]
            };
            let options = {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            };

            let config = {
                data: dados,
                options: options
            }
            let myChart = new Chart(document.getElementById('myChart'), config);
        }
    });
}

function CarregarChamadosDashboardCriacao(dataini, datafim) {
    $.ajax({
        url: '/gerenciar/chamadosdashboardcriacao',
        type: "GET",
        data: { dataIni: dataini, dataFim: datafim },
        success: function (result) {
            let encerrados = 0;
            let emandamento = 0;
            let novo = 0;
            for (let n = 0; n < result.length; n++) {
                if (result[n].STATUS == 'ENCERRADO') {
                    encerrados++;
                } else if (result[n].STATUS == 'EM ANDAMENTO') {
                    emandamento++;
                } else {
                    novo++;
                }
            }

            let valores = [encerrados, emandamento, novo];

            let dados = {
                labels: ['ENCERRADO','EM ANDAMENTO','NOVO'],
                datasets: [{
                    label: 'Status',
                    type: 'pie', // (line | bar | radar | doughnut | pie )
                    data: valores,
                    backgroundColor: [
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(1, 1, 1, 0.2)',
                        'rgba(75, 192, 192, 0.2)'
                    ],
                    borderColor: [
                        'rgba(1, 1, 1, 0.2)'
                    ],
                    borderWidth: 1,
                    hoverOffset: 4
                }]
            };
            let options = {
            };

            let config = {
                data: dados,
                options: options
            }
            let myChart2 = new Chart(document.getElementById('myChart2'), config);
        }
    });
}


function CarregarChamadosDashboardCategoria(dataini, datafim) {
    var categorias = [];

    $.ajax({
        url: '/gerenciar/carregarcategorias',
        data: {},
        success: function (result) {
            for (var x = 0; x < result.length; x++) {
                categorias.push(result[x].TITULO);
            }
        },
    });

    $.ajax({
        url: '/gerenciar/chamadosdashboardcriacao',
        type: "GET",
        data: { dataIni: dataini, dataFim: datafim },
        success: function (result) {
            var valores = [];
            

            for (var c = 0; c < categorias.length; c++) {
                var total = 0;
                for (var r = 0; r < result.length; r++) {
                    if (result[r].CATEGORIA == categorias[c]) {
                        total++;
                    }
                }
                valores.push(total);
            }
            console.log(categorias);
            console.log(valores);
            let dados = {
                labels: categorias,
                datasets: [{
                    label: 'Categorias',
                    type: 'pie', // (line | bar | radar | doughnut | pie )
                    data: valores,
                    backgroundColor: [
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(1, 1, 1, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(255, 99, 132, 0.2)'
                    ],
                    borderColor: [
                        'rgba(1, 1, 1, 0.2)'
                    ],
                    borderWidth: 1,
                    hoverOffset: 4
                }]
            };
            let options = {
            };

            let config = {
                data: dados,
                options: options
            }
            let myChart3 = new Chart(document.getElementById('myChart3'), config);
        }
    });
}

function CarregarDatas() {
    let today = new Date();
    let ultimodia = new Date(today.getYear(), today.getMonth() + 1, 0);

    let datainicio = "";
    let datafim = "";

    if (today.getMonth() < 10) {
        datainicio = today.getFullYear() + '-0' + (today.getMonth() + 1) + '-' + '01';
        datafim = today.getFullYear() + '-0' + (today.getMonth() + 1) + '-' + ultimodia.getDate();
    } else {
        datainicio = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + '01';
        datafim = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + ultimodia.getDate();
    }

    $('#dataIni').val(datainicio);
    $('#dataFim').val(datafim);
}