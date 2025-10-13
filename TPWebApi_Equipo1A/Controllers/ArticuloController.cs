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
        //public IEnumerable<Articulo> Get()
        //{
        //    try
        //    {
        //        articuloNegocio negocio = new articuloNegocio();
        //        List<Articulo> lista = new List<Articulo>();

        //        if (lista != null) { return negocio.listar(); }
        //        else { return null; }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

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


                if (lista != null)
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
                else { return Request.CreateResponse(HttpStatusCode.NoContent, "204"); }
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

                Marca marca = marcasNegocio.listar().Find(x => x.IdMarca == articulo.IdMarca);
                Categoria categoria = categoriasNegocio.listar().Find(x => x.IdCategoria == articulo.IdCategoria);

                if (marca == null) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400"); }
                if (categoria == null) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "400"); }

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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "500");
            }
        }

        // PUT: api/Articulo/5
        public HttpResponseMessage Put(int id, [FromBody] Articulo articulo)
        {
            try
            {
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
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Producto no encontrado.");

                negocio.eliminar(art.CodigoArticulo);
                return Request.CreateResponse(HttpStatusCode.OK, "Producto eliminado.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al eliminar el producto.");
            }
        }

    }
}
