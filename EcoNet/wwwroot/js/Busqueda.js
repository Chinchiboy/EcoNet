function BuscarAnuncios() {
    var texto = document.getElementById("gsearch").value;

    fetch(`/Home/BuscarAnuncios?gsearch=${encodeURIComponent(texto)}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById("AnuncioContainer").innerHTML = html;
        })
        .catch(error => console.error('Error al buscar anuncios:', error));
}