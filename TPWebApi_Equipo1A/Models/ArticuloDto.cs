using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebApi_Equipo1A.Models
{
    public class ArticuloDto
    {
        public string CodigoArticulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
    }
}