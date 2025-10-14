using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TPWebApi_Equipo1A.Models;
using dominio;
using negocio;
using Microsoft.Ajax.Utilities;

namespace TPWebApi_Equipo1A.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo


        public HttpResponseMessage Get()
        {
            try
            {
                articuloNegocio negocio = new articuloNegocio();
                List<Articulo> lista = negocio.listar();

                //return Request.CreateResponse(HttpStatusCode.OK, lista);

                if (lista != null) { return Request.CreateResponse(HttpStatusCode.OK, lista); }
                else { return Request.CreateResponse(HttpStatusCode.OK, "200"); }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "500");
            }
        }


        // GET: api/Articulo/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                articuloNegocio negocio = new articuloNegocio();
                List<Articulo> lista = negocio.listar();


                if (lista == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "404");
                }
                else
                {
                    Articulo artBuscado = lista.Find(x => x.IdArticulo == id);
                    if (artBuscado != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, artBuscado);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "404");
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "500");
            }
        }

        // POST: api/Articulo
        public HttpResponseMessage Post([FromBody] ArticuloDto articulo)
        {
            try
            {
                var articuloNegocio = new articuloNegocio();
                var marcasNegocio = new marcaNegocio();
                var categoriasNegocio = new categoriaNegocio();

                // Validar exitencia de marca y categoria
                Marca marca = marcasNegocio.listar().Find(x => x.IdMarca == articulo.IdMarca);
                Categoria categoria = categoriasNegocio.listar().Find(x => x.IdCategoria == articulo.IdCategoria);
                // Validamos que el articulo no exista en base de datos
                List<Articulo> articulosExistentes = articuloNegocio.listar();
                Articulo articuloExistente = articulosExistentes.Find(x => x.CodigoArticulo == articulo.CodigoArticulo);
                // Uso codigo articulo ya q articuloDto viene sin idarticulo no se si es correcto

                if (marca == null) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400"); }
                if (categoria == null) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400"); }
                if (articuloExistente != null) { return Request.CreateErrorResponse(HttpStatusCode.Conflict, "409"); }

                if (!string.IsNullOrEmpty(articulo.CodigoArticulo) && !string.IsNullOrEmpty(articulo.Nombre) && 
                    !string.IsNullOrEmpty(articulo.Descripcion) && articulo.Precio != 0 && articulo.IdMarca != 0 
                    && articulo.IdCategoria != 0)
                {
                    var nuevo = new Articulo
                    {
                        CodigoArticulo = articulo.CodigoArticulo,
                        Nombre = articulo.Nombre,
                        Descripcion = articulo.Descripcion,
                        Precio = articulo.Precio,
                        marca = new Marca { IdMarca = articulo.IdMarca },
                        categoria = new Categoria { IdCategoria = articulo.IdCategoria }
                    };
                    articuloNegocio.agregar(nuevo);
                    List<Articulo> lista = articuloNegocio.listar();
                    Articulo artBuscado = lista.Find(x => x.CodigoArticulo == articulo.CodigoArticulo);
                    if (artBuscado == null) { return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500"); }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, "201");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
            }
        }

        // PUT: api/Articulo/5
        public HttpResponseMessage Put(int id, [FromBody] ArticuloDto articulo)
        {
            try
            {
                // Validación básica
                if (string.IsNullOrWhiteSpace(articulo.CodigoArticulo) ||
                    string.IsNullOrWhiteSpace(articulo.Nombre) ||
                    articulo.Precio <= 0 ||
                    articulo.IdMarca== 0 ||
                    articulo.IdCategoria== 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400");
                }

                articuloNegocio negocio = new articuloNegocio();
                Articulo nuevo = new Articulo();
                nuevo.IdArticulo = id;
                nuevo.CodigoArticulo = articulo.CodigoArticulo;
                nuevo.Nombre = articulo.Nombre;
                nuevo.Descripcion = articulo.Descripcion;
                nuevo.Precio = articulo.Precio;
                nuevo.marca = new Marca { IdMarca = articulo.IdMarca };
                nuevo.categoria = new Categoria { IdCategoria = articulo.IdCategoria };

                negocio.modificar(nuevo);

                //Validar que se hayan modificado los datos
                Articulo validar = negocio.listar().Find(x => x.IdArticulo == id);
                ArticuloDto articuloDto = new ArticuloDto();
                articuloDto.CodigoArticulo = validar.CodigoArticulo;
                articuloDto.Nombre = validar.Nombre;
                articuloDto.Descripcion = validar.Descripcion;
                articuloDto.Precio = validar.Precio;
                articuloDto.IdMarca = validar.marca.IdMarca;
                articuloDto.IdCategoria = validar.categoria.IdCategoria;

                if (articuloDto.CodigoArticulo != articulo.CodigoArticulo ||
                    articuloDto.Nombre != articulo.Nombre ||
                    articuloDto.Descripcion != articulo.Descripcion ||
                    articuloDto.Precio != articulo.Precio ||
                    articuloDto.IdMarca != articulo.IdMarca ||
                    articuloDto.IdCategoria != articulo.IdCategoria)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "200");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
            }
        }


        // DELETE: api/Articulo/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                articuloNegocio negocio = new articuloNegocio();
                Articulo art = negocio.listar().Find(x => x.IdArticulo == id);
                if (art == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "404");

                negocio.eliminar(art.CodigoArticulo);
                // Validar que no exista mas el articulo
                Articulo validar = negocio.listar().Find(x => x.IdArticulo == id);
                if (validar != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "200");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
            }
        }

        [HttpPost]
        [Route("api/Articulo/{id}/imagenes")]
        public HttpResponseMessage AgregarImagenes(int id, [FromBody] List<string> urls)
        {
            try
            {
                articuloNegocio negocio = new articuloNegocio();
                negocio.agregarImagenes(id, urls); // Este método lo podés implementar en tu lógica
                return Request.CreateResponse(HttpStatusCode.OK, "Imágenes agregadas.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al agregar imágenes.");
            }
        }

    }
}
