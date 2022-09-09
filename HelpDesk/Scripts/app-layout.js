$(document).ready(function () {
    UsuarioConectado();
});

function UsuarioConectado() {
    let nome = $('#nomeUsuario').val();

    if (nome != "") {
        document.getElementById("Usuario").innerHTML = ("<a href='#' class='nav-link text-white' data-bs-toggle='dropdown' aria-expanded='false'> " +
            "<svg class='bi d-block mx-auto mb-1' width='24' height='24'><use xlink:href='#user' /></svg>" +
            nome +
            "</a>" +
            "<ul class='dropdown-menu text-small shadow' aria-labelledby='dropdownUser2'>" +
            "<li id='Perfil'><a class='dropdown-item' href='/Gerenciar/Perfil'>Perfil</a></li>" +
            "<li><hr class='dropdown-divider''></li>" +
            "<li id='Sair'><a class='dropdown-item' href='/Login/Logout'>Sair</a></li>" +
            "</ul>");
    }
}