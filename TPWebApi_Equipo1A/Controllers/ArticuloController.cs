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
        public IEnumerable<Articulo> Get()
        {
            try
            {
                articuloNegocio negocio = new articuloNegocio();
                return negocio.listar();
            }
            catch (Exception ex)
            {

                return null;
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
