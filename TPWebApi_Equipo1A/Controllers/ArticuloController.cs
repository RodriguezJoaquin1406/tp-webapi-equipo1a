using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TPWebApi_Equipo1A.Models;
using dominio;
using negocio;

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
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Articulo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Articulo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
