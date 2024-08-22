function BuscarAnuncios() {
    var texto = document.getElementById("gsearch").value;
    fetch('/Home/BuscarAnuncios?gsearch=' + encodeURIComponent(texto), {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            actualizarResultados(data);
        })
        .catch(error => console.error('Error al realizar la búsqueda:', error));
}

function actualizarResultados(anuncios) {
    var contenedor = document.querySelector('.row'); 
    contenedor.innerHTML = ''; 
    anuncios.forEach(anuncio => {
        var divAnuncio = document.createElement('div');
        divAnuncio.className = 'col-md-4 col-sm-6 col-12 mb-3';
        divAnuncio.innerHTML = `
            <div onclick="location.href='/Home/Producto/${anuncio.idAnuncio}'" class="VerdePrincipal RecuadroProducto">
                <div><img src="data:image/png;base64,${anuncio.imagenBase64}" width="80%" height="80%" /></div>
                <hr />
                <div class="Titulo">${anuncio.titulo}</div>
                <div class="Precio">${anuncio.precio} €</div>
            </div>`;
        contenedor.appendChild(divAnuncio);
    });
}